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

        public ActionResult RenderJobCvForm()
        {
            var jobCvForm = new JobCvForm
            {
                //FirstName = "Paulius",
                //SecondName = "Putna",
                //Email = "paulius@tgdh.co.uk"
            };

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

            SaveJobCvFormSubmission(model);
            SendEmailNotifications(model);

            var fileSavingOptions = new FileHelperSettings
            {
                Directory = "Job CV",
                ParentFolderName = model.FirstName.MakeValidFileName() + " " + model.SecondName.MakeValidFileName() + " - " + DateTime.Now.ToString("F")
            };

            _fileHelper.SaveFormAttachmentToServer(fileSavingOptions, model.CvAttachment);

            if (Umbraco.TypedContent(Constants.JobCvFormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.JobCvFormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveJobCvFormSubmission(JobCvForm model)
        {
            try
            {
                var contentService = Services.ContentService;
                var jobCvForm = contentService.CreateContent(model.FirstName + " " + model.SecondName + ", " + model.Email + " - " + DateTime.Now.ToString("d"), Constants.JobCvFormFolderId, Constants.JobCvFormAlias);

                jobCvForm.SetValue("firstName", model.FirstName);
                jobCvForm.SetValue("secondName", model.SecondName);
                jobCvForm.SetValue("email", model.Email);

                contentService.SaveAndPublishWithStatus(jobCvForm);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Job CV form saving failed with the exception: " + ex.Message);
            }
        }

        private void SendEmailNotifications(object model)
        {
            var formFolder = Umbraco.TypedContent(Constants.JobCvFormFolderId);

            if (formFolder != null)
            {
                _mailHelper.CreateAndSendNotifications(model, formFolder);
            }
            else
            {
                LogHelper.Warn(GetType(), "Couldn't get the form folder with the id: " + Constants.ContactFormForlderId);
            }
        }
    }
}
