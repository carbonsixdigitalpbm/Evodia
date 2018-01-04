using System;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;

namespace Evodia.Core.Controllers
{
    public class ContactConsultingFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        public ActionResult RenderContactConsultingForm(string legend = "")
        {
            if (!string.IsNullOrWhiteSpace(legend))
            {
                ViewData["legend"] = legend;
            }

            return PartialView("~/Views/Partials/Forms/ContactConsultingFormView.cshtml", new ContactConsultingForm());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFormSubmission(ContactConsultingForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ContactConsultingFormValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["ContactConsultingFormValidationPasses"] = "The form has been validated successfully.";
            TempData["ContactConsultingFormFormFolderId"] = Constants.ContactConsultingFormFolderId;

            SaveContactConsultingFormSubmission(model);
            SendEmailNotifications(model);

            if (Umbraco.TypedContent(Constants.ContactConsultingFormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.ContactConsultingFormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveContactConsultingFormSubmission(ContactConsultingForm model)
        {
            try
            {
                var contentService = Services.ContentService;
                var formSubmission = contentService.CreateContent(model.Name + ", " + model.Email + " - " + DateTime.Now.ToShortDateString(), Constants.ContactConsultingFormFolderId, Constants.ContactConsultingFormAlias);

                formSubmission.SetValue("name", model.Name);
                formSubmission.SetValue("emailAddress", model.Email);
                formSubmission.SetValue("message", model.Message);

                contentService.SaveAndPublishWithStatus(formSubmission);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Contact form saving failed with the exception: " + ex.Message);
            }

        }

        private void SendEmailNotifications(object model)
        {
            var formFolder = Umbraco.TypedContent(Constants.ContactConsultingFormFolderId);

            if (!string.IsNullOrEmpty(formFolder.Name))
            {
                _mailHelper.CreateAndSendNotifications(model, formFolder);
            }
            else
            {
                LogHelper.Warn(GetType(), "Couldn't get the form folder with the id: " + Constants.ContactConsultingFormFolderId);
            }
        }
    }
}