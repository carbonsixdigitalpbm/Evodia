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
        [Required(ErrorMessage = "Please enter your desired salary/rates")]
        public string SalaryRates { get; set; }

        [DisplayName("Location")]
        [Required(ErrorMessage = "Please enter job location")]
        public string Location { get; set; }

        [DisplayName("How we can help?")]
        public string BriefJobDescription { get; set; }

        [DisplayName("Jobs specs")]
        [Required(ErrorMessage = "Please attach your file")]
        [Utility.Attachment]
        public HttpPostedFileBase Attachment { get; set; }
    }
}
