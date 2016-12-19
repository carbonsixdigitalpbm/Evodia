using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace Evodia.Core.Utility
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AttachmentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;

            if (file == null)
            {
                return new ValidationResult("Please upload a file!");
            }

            if (file.FileName == "")
            {
                return new ValidationResult("The file name is invalid.");
            }

            if (file.ContentLength > 10 * 1024 * 1024)
            {
                return new ValidationResult("This file is too big!");
            }

            var ext = Path.GetExtension(file.FileName);
            const string allowedExtentions = ".pdf,.doc,.docx";

            if (string.IsNullOrEmpty(ext) || !allowedExtentions.Contains(ext))
            {
                return new ValidationResult("Please upload a PDF or a Word file");
            }

            return ValidationResult.Success;
        }
    }
}