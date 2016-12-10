using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Evodia.Core.Models
{
    public class VacancyForm
    {
        [DisplayName("Contact name")]
        [Required(ErrorMessage = "Please enter your contact name")]
        public string ContactName { get; set; }

        [DisplayName("Company name")]
        public string CompanyName { get; set; }

        [DisplayName("Telephone")]
        [Required(ErrorMessage = "Please enter your telephone number")]
        public string Telephone { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [DisplayName("Job title")]
        [Required(ErrorMessage = "Please enter job title")]
        public string JobTitle { get; set; }

        [DisplayName("Salary/Rates")]
        public string SalaryRates { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Brief job description")]
        public string BriefJobDescription { get; set; }

        [DisplayName("Jobs specs")]
        [Required(ErrorMessage = "Please attach your job specs ")]
        [Utility.Attachment]
        public HttpPostedFileBase JobsSpecs { get; set; }
    }
}
