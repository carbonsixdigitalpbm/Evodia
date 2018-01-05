using System.Collections.Generic;

namespace Evodia.Core.Models
{
    public class LinkListViewModel
    {
        public string Id { get; set; }

        public List<Link> Links { get; set; }

        public string ModifierClass { get; set; }

        public LinkListViewModel(List<Link> links = null)
        {
            Links = links;
        }
    }
}
