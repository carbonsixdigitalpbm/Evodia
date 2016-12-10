using System;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;
using Umbraco.Core.Logging;

namespace Evodia.Core.Controllers
{
    public class VacancyFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        private readonly FileHelper _fileHelper = new FileHelper();

        public ActionResult RenderVacancyForm()
        {
            var vacancyForm = new VacancyForm
            {
                ContactName = "Paulius Putna",
                CompanyName = "TGDH",
                Email = "paulius@tgdh.co.uk",
                JobTitle = "Web developer",
                Telephone = "07528 118833",
                BriefJobDescription = "I want to make websites and stuff",
                SalaryRates = "Over 9000",
                Location = "Hampshire"
            };

            return PartialView("~/Views/Partials/Forms/VacancyFormView.cshtml", vacancyForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessVacancyFormSubmission(VacancyForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["VacancyFormValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["VacancyFormValidationPasses"] = "The form has been validated successfully.";
            TempData["VacancyFormFolderId"] = Constants.VacancyFormFolderId;

            SaveVacancyFormSubmission(model);
            SendEmailNotifications(model);

            var fileSavingOptions = new FileHelperSettings
            {
                Directory = "Vacancies",
                ParentFolderName = model.ContactName.MakeValidFileName()
            };

            _fileHelper.SaveFormAttachmentToServer(fileSavingOptions, model.JobsSpecs);

            if (Umbraco.TypedContent(Constants.VacancyFormFolderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.VacancyFormFolderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveVacancyFormSubmission(VacancyForm model)
        {
            try
            {
                var contentService = Services.ContentService;
                var vacancyForm = contentService.CreateContent(model.ContactName + ", " + model.Email + " - " + DateTime.Now.ToString("d"), Constants.VacancyFormFolderId, Constants.VacancyFormAlias);

                vacancyForm.SetValue("contactName", model.ContactName);
                vacancyForm.SetValue("companyName", model.CompanyName);
                vacancyForm.SetValue("telephone", model.Telephone);
                vacancyForm.SetValue("email", model.Email);
                vacancyForm.SetValue("jobTitle", model.JobTitle);
                vacancyForm.SetValue("salaryRates", model.SalaryRates);
                vacancyForm.SetValue("location", model.Location);
                vacancyForm.SetValue("briefJobDescription", model.BriefJobDescription);

                contentService.SaveAndPublishWithStatus(vacancyForm);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Vacancy form saving failed with the exception: " + ex.Message);
            }
        }

        private void SendEmailNotifications(object model)
        {
            var formFolder = Umbraco.TypedContent(Constants.VacancyFormFolderId);

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
