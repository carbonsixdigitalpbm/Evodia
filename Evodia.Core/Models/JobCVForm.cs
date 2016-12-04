using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Evodia.Core.Models
{
    public class JobCVForm
    {
        [DisplayName("First name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [DisplayName("Second name")]
        [Required(ErrorMessage = "Please enter your second name")]
        public string SecondName { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

        [DisplayName("Job role")]
        [Required(ErrorMessage = "Please enter job role your interested")]
        public string JobRole { get; set; }

        [DisplayName("Your CV (Pdf or Word file)")]
        [Required(ErrorMessage = "Please attache your CV")]
        [Evodia.Core.Controllers.Attachment]
        public HttpPostedFileBase Attachment { get; set; }
    }
}
