using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;
using Umbraco.Core.Logging;

namespace Evodia.Core.Controllers
{
    public class GenericCvFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        private readonly FileHelper _fileHelper = new FileHelper();

        public ActionResult RenderGenericCvForm(string legend)
        {
            var genericCvForm = new GenericCvForm
            {
                JobPreference = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Contract",
                        Value = "Contract"
                    },
                    new SelectListItem
                    {
                        Text = "Permanent",
                        Value = "Permanent"
                    }
                }
            };

            if (!string.IsNullOrWhiteSpace(legend))
            {
                ViewData["legend"] = legend;
            }

            return PartialView("~/Views/Partials/Forms/GenericCvFormView.cshtml", genericCvForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessGenericCvFormSubmission(GenericCvForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["GenericCvFormValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["GenericCvFormValidationPasses"] = "The form has been validated successfully.";
            TempData["GenericCvFormFolderId"] = Constants.GenericCvFormFolderId;

            SendEmailNotifications(model);

            var fileSavingOptions = new FileHelperSettings
            {
                Directory = "Generic_CV",
                ParentFolderName = model.FirstName.MakeValidFileName() + " " + model.SecondName.MakeValidFileName() + " - " + DateTime.Now.ToString("F")
            };

            var filePath = _fileHelper.SaveFormAttachmentToServer(fileSavingOptions, model.Attachment);

            SaveGenericCvFormSubmission(model, filePath);

            if (Umbraco.TypedContent(Constants.GenericCvFormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.GenericCvFormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveGenericCvFormSubmission(GenericCvForm model, string filePath)
        {
            try
            {
                var contentService = Services.ContentService;
                var genericCvForm = contentService.CreateContent(model.FirstName + " " + model.SecondName + ", " + model.Email + " - " + DateTime.Now.ToString("d"), Constants.GenericCvFormFolderId, Constants.GenericCvFormAlias);

                genericCvForm.SetValue("firstName", model.FirstName);
                genericCvForm.SetValue("secondName", model.SecondName);
                genericCvForm.SetValue("emailAddress", model.Email);
                genericCvForm.SetValue("securityClearanceLevel", model.SecurityClearanceLevel);
                genericCvForm.SetValue("jobPreference", model.SelectedJobPreference);
                genericCvForm.SetValue("availability", model.Availability);
                genericCvForm.SetValue("filePath", filePath);

                contentService.SaveAndPublishWithStatus(genericCvForm);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Generic CV form saving failed with the exception: " + ex.Message);
            }
        }

        private void SendEmailNotifications(object model)
        {
            var formFolder = Umbraco.TypedContent(Constants.GenericCvFormFolderId);

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
