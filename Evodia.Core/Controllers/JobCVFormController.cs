using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models.ContentEditing;
using Evodia.Core.Models;
using System.IO;
using Umbraco.Core.Configuration;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using Evodia.Core.Utility;

namespace Evodia.Core.Controllers
{
    public class JobCVFormController : SurfaceController
    {
        public ActionResult RenderUploadCVForm()
        {
            return PartialView("~/Views/Partials/Forms/JobCVFormView.cshtml", new JobCVForm()
            {
                JobRole = UmbracoContext.Current.PublishedContentRequest.PublishedContent.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSubmittedCV(JobCVForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ValidationFail"] = "TempData: Validation fails";

                return CurrentUmbracoPage();
            }

            SaveUploadedFile(model);

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveUploadedFile(JobCVForm model)
        {
            try
            {
                string pattern = @"[^0-9a-zA-Z\.]+";
                Regex rgx = new Regex(pattern);
                var fileName = rgx.Replace(model.Attachment.FileName, "");
                var ext = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

                var mediaType = Constants.Conventions.MediaTypes.File;

                const int uploadFolderId = 1343;
                var mediaService = ApplicationContext.Current.Services.MediaService;
                var uploadedCVFile = mediaService.CreateMedia(fileName, uploadFolderId, mediaType);
                uploadedCVFile.SetValue("umbracoFile", model.Attachment);
                mediaService.Save(uploadedCVFile);

                TempData["ValidationPasses"] = "TempData: Validation passes";

                SendEmailWithSubmittedCV(model, uploadedCVFile);
                SaveCVFormSubmission(model, uploadedCVFile);

            }
            catch (Exception ex)
            {
                TempData["ExceptionSave"] = ex.Message;

                throw;
            }
        }

        private string SaveCVFormSubmission(JobCVForm model, IMedia cv)
        {
            try
            {
                var contentService = ApplicationContext.Current.Services.ContentService;

                var formSubmission = contentService.CreateContent("CV uploaded by: " + model.FirstName + " " + model.SecondName + ", " + model.JobRole, 1342, "uploadedCVForm");

                formSubmission.SetValue("name", model.FirstName);
                formSubmission.SetValue("surname", model.SecondName);
                formSubmission.SetValue("emailAddress", model.EmailAddress);
                formSubmission.SetValue("jobRole", model.JobRole);
                formSubmission.SetValue("cV", cv.Id);
                // formSubmission.SetValue("officeToContact", Umbraco.TypedContent(model.OfficeToContactId).Name);

                contentService.Save(formSubmission);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void SendEmailWithSubmittedCV(JobCVForm model, IMedia cv)
        {
            var formFolder = Umbraco.TypedContent(1342);

            if(formFolder == null)
            {
                return;
            }

            var to = formFolder.GetPropertyValue<string>("internalNotificationAddress");

            if(string.IsNullOrEmpty(to))
            {
                return;
            }

            var emailCcAddresses = formFolder.GetPropertyValue<string>("internalNotificationCc");

            var subj = "Evodia CV form submitted";

            var mail = new MailMessage();

            foreach (var emailAddress in emailCcAddresses.Split(','))
            {
                if (IsValidEmail(emailAddress))
                {
                    mail.CC.Add(emailAddress);
                }
            }

            string emailBody = string.Empty;
            string emailInnerBody = "<p><strong>Name: </strong>{{Name}}</p><p><strong>Surname: </strong>{{Surname}}</p><p><strong>Email address: </strong>{{EmailAddress}}</p><p><strong>Job role: </strong>{{JobRole}}</p><p><strong>Link to CV: </strong><a href='{{CVLink}}'>{{Name}} CV</a></p>";
            using (StreamReader reader = new StreamReader(Server.MapPath("~/App_Code/EmailTemplate.html")))
            {

                emailBody = reader.ReadToEnd();

            }

            emailInnerBody = emailInnerBody.Replace("{{Name}}", model.FirstName);
            emailInnerBody = emailInnerBody.Replace("{{Surname}}", model.SecondName);
            emailInnerBody = emailInnerBody.Replace("{{EmailAddress}}", model.EmailAddress);
            emailInnerBody = emailInnerBody.Replace("{{JobRole}}", model.JobRole);
            emailInnerBody = emailInnerBody.Replace("{{CVLink}}", "http://" + System.Web.HttpContext.Current.Request.Url.Host + Umbraco.TypedMedia(cv.Id).Url);

            emailBody = emailBody.Replace("{{EmailTitle}}", "Job CV form submitted");
            emailBody = emailBody.Replace("{{Content}}", emailInnerBody);

            mail.From = new MailAddress("noreply@evodia.co.uk", "Evodia website");
            mail.To.Add(to);
            mail.Subject = subj;
            mail.Body = emailBody;
            mail.IsBodyHtml = true;

            MailHelper mailHelper = new MailHelper();
//            mailHelper.SendMailMessage(mail);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email.Trim());
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AttachmentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            // The file is required.
            if (file == null)
            {
                return new ValidationResult("Please upload a file!");
            }

            if (file.FileName == "")
            {
                return new ValidationResult("The file name is invalid.");
            }

            // The meximum allowed file size is 10MB.
            if (file.ContentLength > 10 * 1024 * 1024)
            {
                return new ValidationResult("This file is too big!");
            }

            // Only PDF can be uploaded.
            string ext = Path.GetExtension(file.FileName);
            string allowedExtentions = ".pdf,.doc,.docx";
            if (String.IsNullOrEmpty(ext) || !allowedExtentions.Contains(ext) )
            {
                return new ValidationResult("Please upload a PDF or a Word file");
            }

            // Everything OK.
            return ValidationResult.Success;
        }
    }
}
