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
    public class GenericCvFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();
        private readonly IContentService _contentService = ApplicationContext.Current.Services.ContentService;
        private readonly UmbracoHelper _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        private const int FormFolderId = 1155;

        public ActionResult RenderGenericCvForm()
        {
            return PartialView("~/Views/Partials/Forms/GenericCvForm.cshtml", new GenericCvForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFormSubmission(GenericCvForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["ValidationPasses"] = "The form has been validated successfully.";
            TempData["FormFolderId"] = FormFolderId;

            //SaveContactFormSubmission(model);
            //SendEmailNotifications(model);

            if (_umbracoHelper.TypedContent(FormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(_umbracoHelper.TypedContent(FormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        //private void SaveContactFormSubmission(GenericCvForm model)
        //{
        //    try
        //    {
        //        var formSubmission = _contentService.CreateContent(model.Name + ", " + model.Email + " - " + DateTime.Now.ToShortDateString(), FormFolderId, "contactForm");

        //        formSubmission.SetValue("name", model.Name);
        //        formSubmission.SetValue("emailAddress", model.Email);
        //        formSubmission.SetValue("message", model.Message);
        //        _contentService.SaveAndPublishWithStatus(formSubmission);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Warn(GetType(), "Contact form saving failed with the exception: " + ex.Message);
        //    }

        //}

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
