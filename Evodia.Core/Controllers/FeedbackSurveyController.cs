using System;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;
using Umbraco.Core.Logging;

namespace Evodia.Core.Controllers
{
    public class FeedbckSurveyController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        public ActionResult RenderFeedbackSurvey()
        {
            var feedbackSurvey = new FeedbackSurvey
            {
                Name = "Paulius",
                Email = "paulius@tgdh.co.uk"
            };

            return PartialView("~/Views/Partials/Forms/FeedbackSurveyView.cshtml", feedbackSurvey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFeedbackSurveySubmission(FeedbackSurvey model)
        {
            if (!ModelState.IsValid)
            {
                TempData["FeedbackSurveyValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["FeedbackSurveyValidationPasses"] = "The form has been validated successfully.";
            TempData["FeedbackSurveyFolderId"] = Constants.FeedbackSurveyFolderId;

            SaveJobCvFormSubmission(model);
            SendEmailNotifications(model);

            if (Umbraco.TypedContent(Constants.FeedbackSurveyFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.FeedbackSurveyFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveJobCvFormSubmission(FeedbackSurvey model)
        {
            try
            {
                var contentService = Services.ContentService;
                var feedbackSurvey = contentService.CreateContent(model.Name + " " + model.Email + ", " + model.Email + " - " + DateTime.Now.ToString("d"), Constants.FeedbackSurveyFolderId, Constants.FeedbackSurveyAlias);

                feedbackSurvey.SetValue("name", model.Name);
                feedbackSurvey.SetValue("email", model.Email);
                feedbackSurvey.SetValue("telephone", model.Telephone);
                feedbackSurvey.SetValue("companyName", model.CompanyName);
                feedbackSurvey.SetValue("message", model.Message);

                contentService.SaveAndPublishWithStatus(feedbackSurvey);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Feedback survey saving failed with the exception: " + ex.Message);
            }
        }

        private void SendEmailNotifications(object model)
        {
            var formFolder = Umbraco.TypedContent(Constants.FeedbackSurveyFolderId);

            if (formFolder != null)
            {
                _mailHelper.CreateAndSendNotifications(model, formFolder);
            }
            else
            {
                LogHelper.Warn(GetType(), "Couldn't get the form folder with the id: " + Constants.FeedbackSurveyFolderId);
            }
        }
    }
}
