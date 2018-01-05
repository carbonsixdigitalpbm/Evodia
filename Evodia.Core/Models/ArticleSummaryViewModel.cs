using Umbraco.Core.Models;

namespace Evodia.Core.Models
{
    public class ArticleSummaryViewModel
    {
        public IPublishedContent Image { get; set; }

        public string Url { get; set; }

        public string UrlTarget { get; set; }

        public string Headline { get; set; }

        public string Subtitle { get; set; }

        public string Label { get; set; }

        public string Copy { get; set; }

        public string ModifierClass { get; set; }

        public ArticleSummaryViewModel()
        {

        }
    }
}
