using Nikcio.UHeadless.ContentItems;

namespace Nikcio.UHeadless.Defaults.ContentItems;

public partial class ContentItem
{
    public new class CreateCommand : ContentItemBase.CreateCommand
    {
        public required int StatusCode { get; init; }

        public required RedirectObject? Redirect { get; init; }

        public class RedirectObject
        {
            public required string? RedirectUrl { get; init; }

            public required bool IsPermanent { get; init; }
        }
    }
}
