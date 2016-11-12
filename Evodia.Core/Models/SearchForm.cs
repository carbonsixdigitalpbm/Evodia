using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Evodia.Core.Models
{
    public class SearchForm
    {
        [DisplayName("Job title")]
        public string Keywords { get; set; }

        [DisplayName("Search job title only")]
        public bool TitleOnly { get; set; }
    }
}