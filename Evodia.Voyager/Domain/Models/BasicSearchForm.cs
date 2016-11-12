using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Evodia.Voyager.Domain.Models
{
    public class BasicSearchForm
    {
        [DisplayName("Job title")]
        public string Keywords { get; set; }

        [DisplayName("Search job title only")]
        public bool TitleOnly { get; set; }
    }
}