using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Evodia.Voyager.Domain.Models
{
    public class FullSearchForm
    {
        [DisplayName("Job title")]
        public string Keywords { get; set; }

        [DisplayName("Search job title only")]
        public bool TitleOnly { get; set; }

        [DisplayName("Job type")]
        public List<JobType> JobTypes { get; set; }

        [DisplayName("Location")]
        public List<SelectListItem> Locations { get; set; }

        public string SelectedLocation { get; set; }

        [DisplayName("Sectors")]
        public List<Sector> Sectors { get; set; }

        [DisplayName("Salary")]
        public List<SelectListItem> MinimumSalary { get; set; }

        public string SelectedSalary { get; set; }
    }


    public class JobType
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public JobType()
        {
        }
    }

    public class Sector
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public Sector()
        {
        }
    }
}