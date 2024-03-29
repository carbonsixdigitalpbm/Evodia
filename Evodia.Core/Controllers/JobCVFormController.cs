using System;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;
using Umbraco.Core.Logging;

namespace Evodia.Core.Controllers
{
    public class JobCvController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        private readonly FileHelper _fileHelper = new FileHelper();

        public ActionResult RenderJobCvForm(int jobId, string legend = "")
        {
            var jobPage = Umbraco.TypedContent(jobId);

            var jobCvForm = new JobCvForm();

            if (jobPage != null)
            {
                jobCvForm.JobPageId = jobPage.Id;
                jobCvForm.JobTitle = jobPage.HasValue("clientJobTitle") ? jobPage.GetPropertyValue<string>("clientJobTitle") : jobPage.Name;
                jobCvForm.JobReference = jobPage.HasValue("jobReference") ? jobPage.GetPropertyValue<string>("jobReference") : "Not specified";
            }

            if (!string.IsNullOrWhiteSpace(legend))
            {
                ViewData["legend"] = legend;
            }

            return PartialView("~/Views/Partials/Forms/JobCvFormView.cshtml", jobCvForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessJobCvFormSubmission(JobCvForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["JobCvFormValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["JobCvFormValidationPasses"] = "The form has been validated successfully.";
            TempData["JobCvFormFolderId"] = Constants.JobCvFormFolderId;

            
            SendEmailNotifications(model);

            var fileSavingOptions = new FileHelperSettings
            {
                Directory = "Job_CV",
                ParentFolderName = model.FirstName.MakeValidFileName() + " " + model.SecondName.MakeValidFileName() + " " + model.JobReference + " - " + DateTime.Now.ToString("F"),
                FilePrefix = model.JobReference
            };

            var filePath = _fileHelper.SaveFormAttachmentToServer(fileSavingOptions, model.Attachment);

            SaveJobCvFormSubmission(model, filePath);

            if (Umbraco.TypedContent(Constants.JobCvFormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.JobCvFormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveJobCvFormSubmission(JobCvForm model, string filePath)
        {
            try
            {
                var contentService = Services.ContentService;
                var jobCvForm = contentService.CreateContent(model.FirstName + " " + model.SecondName + ", " + model.Email + " - " + DateTime.Now.ToString("d"), Constants.JobCvFormFolderId, Constants.JobCvFormAlias);

                jobCvForm.SetValue("jobTitle", model.JobTitle);
                jobCvForm.SetValue("jobReference", model.JobReference);
                jobCvForm.SetValue("firstName", model.FirstName);
                jobCvForm.SetValue("secondName", model.SecondName);
                jobCvForm.SetValue("email", model.Email);
                jobCvForm.SetValue("filePath", filePath);

                contentService.SaveAndPublishWithStatus(jobCvForm);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Job CV form saving failed with the exception: " + ex.Message);
            }
        }

        private void SendEmailNotifications(JobCvForm model)
        {
            var formFolder = Umbraco.TypedContent(Constants.JobCvFormFolderId);
            var jobPage = Umbraco.TypedContent(model.JobPageId);

            if (formFolder != null)
            {
                //_mailHelper.CreateAndSendNotifications(model, formFolder);

                if (jobPage != null)
                {
                    _mailHelper.CreateAndSendConsultantNotifications(model, formFolder, jobPage);
                }

            }
            else
            {
                LogHelper.Warn(GetType(), "Couldn't get the form folder with the id: " + Constants.ContactFormForlderId);
            }


        }
    }
}
