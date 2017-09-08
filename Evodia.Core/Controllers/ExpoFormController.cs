using System;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;
using Evodia.Core.Utility;
using System.Collections.Generic;
using Umbraco.Core;

namespace Evodia.Core.Controllers
{
    public class ExpoFormController : SurfaceController
    {
        private readonly MailHelper _mailHelper = new MailHelper();

        public ActionResult RenderExpoForm(string legend = "", string pageName = "")
        {
            if (!string.IsNullOrWhiteSpace(legend))
            {
                ViewData["legend"] = legend;
            }

            if (!string.IsNullOrWhiteSpace(pageName))
            {
                ViewData["pageName"] = pageName;
            }

            return PartialView("~/Views/Partials/Forms/ExpoFormView.cshtml", new ExpoForm{
                LocationPreference = GetLocationOptions()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFormSubmission(ExpoForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ExpoFormValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["ExpoFormValidationPasses"] = "The form has been validated successfully.";
            TempData["ExpoFormFormFolderId"] = Constants.ExpoFormForlderId;

            SaveExpoFormSubmission(model);
            SendEmailNotifications(model);

            if (Umbraco.TypedContent(Constants.ExpoFormForlderId).HasValue("redirectPage"))
            {
                return RedirectToUmbracoPage(Umbraco.TypedContent(Constants.ExpoFormForlderId).GetPropertyValue<int>("redirectPage"));
            }

            return RedirectToCurrentUmbracoPage();
        }

        private void SaveExpoFormSubmission(ExpoForm model)
        {
            try
            {
                var contentService = Services.ContentService;
                var formSubmission = contentService.CreateContent(model.FirstName + ", " + model.Email + " - " + DateTime.Now.ToShortDateString(), Constants.ExpoFormForlderId, Constants.ExpoFormAlias);

                formSubmission.SetValue("firstName", model.FirstName);
                formSubmission.SetValue("pageName", model.PageName);
                formSubmission.SetValue("surname", model.Surname);
                formSubmission.SetValue("emailAddress", model.Email);
                formSubmission.SetValue("contactNumber", model.ContactNumber);
                formSubmission.SetValue("serviceLeaveDate", model.ServiceLeaveDate);
                formSubmission.SetValue("currentContractEndDate", model.CurrentContractEndDate);
                formSubmission.SetValue("noticePeriod", model.NoticePeriod);
                formSubmission.SetValue("securityClearanceLevel", model.SecurityClearanceLevel);
                formSubmission.SetValue("jobTypePreference", model.JobTypePreference);
                formSubmission.SetValue("currentLocation", model.CurrentLocation);
                formSubmission.SetValue("locationPreference", GetResultsFromList(model.LocationPreference));


                contentService.SaveAndPublishWithStatus(formSubmission);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(GetType(), "Expo form saving failed with the exception: " + ex.Message);
            }

        }

        private void SendEmailNotifications(object model)
        {
            var formFolder = Umbraco.TypedContent(Constants.ExpoFormForlderId);

            if (!string.IsNullOrEmpty(formFolder.Name))
            {
                _mailHelper.CreateAndSendNotifications(model, formFolder);
            }
            else
            {
                LogHelper.Warn(GetType(), "Couldn't get the form folder with the id: " + Constants.ExpoFormForlderId);
            }
        }
        
        public List<Location> GetLocationOptions()
        {
            var locationsList = new List<Location>();
            var locations = Constants.ExpoFormLocations;

            foreach (var location in locations.Split(','))
            {
                var isSelected = false;

                locationsList.Add(new Location
                {
                    Name = location,
                    IsSelected = isSelected
                });
            }

            return locationsList;
        }

        public string GetResultsFromList(List<Location> selection)
        {
            if (selection == null)
            {
                return "N/A";
            }

            var stringToReturn = "";

            foreach(var item in selection)
            {
                if( item.IsSelected )
                {
                    if(String.IsNullOrWhiteSpace(stringToReturn)) {
                        stringToReturn = item.Name + ", ";
                    } else {
                        stringToReturn = stringToReturn + System.Environment.NewLine + item.Name + ", ";
                    }
                }
            }

            if(String.IsNullOrWhiteSpace(stringToReturn)) {
                stringToReturn = "N/A";
            }

            return stringToReturn.TrimEnd(", ");
        }
    }
}
