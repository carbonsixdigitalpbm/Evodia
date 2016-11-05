using System;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Evodia.Core.Models;
using Evodia.Core.Utility;
using Umbraco.Core.Services;

namespace Evodia.Core.Controllers
{
    public class ContactFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();
        private readonly IContentService _contentService = ApplicationContext.Current.Services.ContentService;
        private readonly UmbracoHelper _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        private const int FormFolderId = 1155;

        public ActionResult RenderContactForm()
        {
            return PartialView("~/Views/Partials/Forms/ContactFormView.cshtml", new ContactForm());
        }

        public ActionResult RenderAjaxContactForm()
        {
            TempData["FormFolderId"] = FormFolderId;

            return PartialView("~/Views/Partials/Forms/AjaxContactFormView.cshtml", new ContactForm());
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
            TempData["FormFolderId"] = FormFolderId;

            SaveContactFormSubmission(model);
            SendEmailNotifications(model);

            if (_umbracoHelper.TypedContent(FormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(_umbracoHelper.TypedContent(FormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ProcessAjaxFormSubmission(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            SaveContactFormSubmission(model);
            SendEmailNotifications(model);

            return Json(new { success = true });
        }

        private void SaveContactFormSubmission(ContactForm model)
        {
            try
            {
                var formSubmission = _contentService.CreateContent(model.Name + ", " + model.Email + " - " + DateTime.Now.ToShortDateString(), FormFolderId, "contactForm");

                formSubmission.SetValue("name", model.Name);
                formSubmission.SetValue("emailAddress", model.Email);
                formSubmission.SetValue("message", model.Message);
                _contentService.SaveAndPublishWithStatus(formSubmission);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Contact form saving failed with the exception: " + ex.Message);
            }

        }

        private void SendEmailNotifications(ContactForm model)
        {
            var formFolder = _umbracoHelper.TypedContent(FormFolderId);

            if (!string.IsNullOrEmpty(formFolder.Name))
            {
                _mailHelper.CreateAndSendNotifications(model, formFolder);
            }
            else
            {
                LogHelper.Warn(GetType(), "Couldn't get the form folder with the id: " + FormFolderId);
            }
        }
    }
}
