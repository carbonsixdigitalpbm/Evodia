using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Evodia.Core.Models
{
    public class ExpoForm
    {
        [DisplayName("Page name")]
        public string PageName { get; set; }

        [DisplayName("First name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [DisplayName("Surname")]
        [Required(ErrorMessage = "Please enter your surname")]
        public string Surname { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [DisplayName("Contact number")]
        [Required(ErrorMessage = "Please enter your contact number")]
        public string ContactNumber { get; set; }

        [DisplayName("Service leave date (if applicable)")]
        public string ServiceLeaveDate { get; set; }

        [DisplayName("Notice Period (if applicable)")]
        public string NoticePeriod { get; set; }

        [DisplayName("Current Contract end date (if applicable)")]
        public string CurrentContractEndDate { get; set; }

        [DisplayName("Security clearance level")]
        public string SecurityClearanceLevel { get; set; }

        [DisplayName("Job type preference")]
        public string JobTypePreference { get; set; }

        [DisplayName("Current location")]
        [Required(ErrorMessage = "Please enter your current location")]
        public string CurrentLocation { get; set; }

        [DisplayName("Location preference")]
        public List<Location> LocationPreference { get; set; }

    }

    public class Location
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public Location()
        {
        }
    }

}