using System.Reflection;
using System.Runtime.CompilerServices;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;
using Nikcio.UHeadless.Common.Properties;
using static Nikcio.UHeadless.Common.Directives.DirectiveUtils;

namespace Nikcio.UHeadless.Common.Directives;

public sealed class UseDirectivesAttribute : ObjectFieldDescriptorAttribute
{
    public UseDirectivesAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }

    protected override void OnConfigure(
        IDescriptorContext context,
        IObjectFieldDescriptor descriptor,
        MemberInfo member)
    {
        descriptor.Use<Middleware>();
    }

    internal class Middleware
    {
        private readonly FieldDelegate _next;

        public Middleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            InvokeDirectives(context);

            await _next(context).ConfigureAwait(false);
        }

        public static void InvokeDirectives(IResolverContext context)
        {
            foreach (DirectiveNode directive in context.Selection.SyntaxNode.Directives)
            {
                switch (directive.Name.Value)
                {
                    case ContextDirective.DirectiveName:
                        ContextDirective.Invoke(context, new ContextDirective.InvokeArguments
                        {
                            Culture = ResolveArgumentValue<string>(directive, ArgumentName(nameof(ContextDirective.InvokeArguments.Culture)), context.Variables),
                            IncludePreview = ResolveArgumentValue<bool>(directive, ArgumentName(nameof(ContextDirective.InvokeArguments.IncludePreview)), context.Variables)
                        });
                        break;

                    case SegmentDirective.DirectiveName:
                        SegmentDirective.Invoke(context, new SegmentDirective.InvokeArguments
                        {
                            Segment = ResolveArgumentValue<string>(directive, ArgumentName(nameof(SegmentDirective.InvokeArguments.Segment)), context.Variables)
                        });
                        break;

                    case FallbackDirective.DirectiveName:
                        FallbackDirective.Invoke(context, new FallbackDirective.InvokeArguments
                        {
                            Fallbacks = ResolveArgumentValue<List<PropertyFallback>>(directive, ArgumentName(nameof(FallbackDirective.InvokeArguments.Fallbacks)), context.Variables)
                        });
                        break;
                }
            }
        }
    }

    private static T? ResolveArgumentValue<T>(DirectiveNode directive, string argumentName, IVariableValueCollection variables)
    {
        ArgumentNode? argument = directive.Arguments.FirstOrDefault(x => x.Name.Value == argumentName);

        if (argument?.Value.Kind == SyntaxKind.Variable)
        {
            return (T?) (variables.FirstOrDefault(x => x.Name == ((VariableNode) argument.Value).Name.Value).Value?.Value ?? default(T));
        }

        return (T?) (argument?.Value?.Value ?? default(T));
    }
}
