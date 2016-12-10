using System;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;

namespace Evodia.Core.Controllers
{
    public class ContactFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        public ActionResult RenderContactForm()
        {
            return PartialView("~/Views/Partials/Forms/ContactFormView.cshtml", new ContactForm());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFormSubmission(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["ValidationPasses"] = "The form has been validated successfully.";
            TempData["FormFolderId"] = Constants.ContactFormForlderId;

            SaveContactFormSubmission(model);
            SendEmailNotifications(model);

            if (Umbraco.TypedContent(Constants.ContactFormForlderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.ContactFormForlderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveContactFormSubmission(ContactForm model)
        {
            try
            {
                var contentService = Services.ContentService;
                var formSubmission = contentService.CreateContent(model.Name + ", " + model.Email + " - " + DateTime.Now.ToShortDateString(), Constants.ContactFormForlderId, Constants.ContactFormAlias);

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
            var formFolder = Umbraco.TypedContent(Constants.ContactFormForlderId);

            if (!string.IsNullOrEmpty(formFolder.Name))
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
