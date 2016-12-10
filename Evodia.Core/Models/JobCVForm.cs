using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Evodia.Core.Models
{
    public class JobCvForm
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
        public string Email { get; set; }

        [DisplayName("CV attachment")]
        [Required(ErrorMessage = "Please attach your CV")]
        [Utility.Attachment]
        public HttpPostedFileBase CvAttachment { get; set; }
    }
}
