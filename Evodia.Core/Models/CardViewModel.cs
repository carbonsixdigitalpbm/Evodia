using Umbraco.Core.Models;

namespace Evodia.Core.Models
{
    public class CardViewModel
    {
        public string Headline { get; set; }

        public IPublishedContent Image { get; set; }

        public string Description { get; set; }
        
        public string Url { get; set; }

        public string Target { get; set; }

        public string CtaName { get; set; }

        public string ModifierClass { get; set; }
    
        public CardViewModel()
        {
            CtaName = "Read more";
        }
    }

}