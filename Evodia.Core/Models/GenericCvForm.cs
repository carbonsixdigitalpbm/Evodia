﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Evodia.Core.Models
{
    public class GenericCvForm
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

        [DisplayName("Security clearance level")]
        public string SecurityClearanceLevel { get; set; }

        [DisplayName("Job preference")]
        public List<SelectListItem> JobPreference { get; set; }

        [DisplayName("Job preference")]
        public string SelectedJobPreference { get; set; }

        [DisplayName("Availability")]
        public string Availability { get; set; }

        [DisplayName("CV attachment")]
        [Required(ErrorMessage = "Please attach your file")]
        [Utility.Attachment]
        public HttpPostedFileBase Attachment { get; set; }
    }
}