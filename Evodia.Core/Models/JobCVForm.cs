using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Evodia.Core.Models
{
    public class JobCvForm
    {
        [DisplayName("Job title")]
        public string JobTitle { get; set; }

        [DoNotInclude]
        public int JobPageId { get; set; }

        [DisplayName("Job reference")]
        public string JobReference { get; set; }

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
        [Required(ErrorMessage = "Please attach your file")]
        [Utility.Attachment]
        public HttpPostedFileBase Attachment { get; set; }
    }
    public class DoNotIncludeAttribute : Attribute
    {
    }

    public static class ExtensionsOfPropertyInfo
    {
        public static IEnumerable<T> GetAttributes<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetCustomAttributes(typeof(T), true).Cast<T>();
        }
        public static bool IsMarkedWith<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetAttributes<T>().Any();
        }
    }
}

