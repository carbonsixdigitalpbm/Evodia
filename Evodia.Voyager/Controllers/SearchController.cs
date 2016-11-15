using System.Collections.Generic;
using System.Web.Mvc;
using Evodia.Data.Data;
using Evodia.Voyager.Domain.Models;
using Umbraco.Web.Mvc;

namespace Evodia.Voyager.Controllers
{
    public class SearchFormController : SurfaceController
    {
        public ActionResult RenderBasicSearchForm()
        {
            return PartialView("~/Views/Partials/Forms/BasicSearchFormView.cshtml", new BasicSearchForm());
        }
        public ActionResult RenderFullSearchForm()
        {
            var model = new FullSearchForm
            {
                JobTypes = GetJobTypesFromAvailableJobs(),
                Locations = GetJobLocationsFromAvailableJobs(),
                Sectors = GetJobSectorsFromAvailableJobs(),
                MinimumSalary = GetMinimumSalaryList()
            };

            var keywords = TempData["Keywords"];
            var titleOnly = TempData["TitleOnly"];

            if (keywords != null)
            {
                model.Keywords = (string) keywords;
            }

            if (titleOnly != null)
            {
                model.TitleOnly = (bool) titleOnly;
            }

            return PartialView("~/Views/Partials/Forms/FullSearchFormView.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessBasicFormSubmission(BasicSearchForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["Keywords"] = model.Keywords;
            TempData["TitleOnly"] = model.TitleOnly;

            return RedirectToUmbracoPage(1186);
        }

        private static List<JobType> GetJobTypesFromAvailableJobs()
        {
            var jobTypesList = new List<JobType>();
            var jobTypes = JobsRepository.GetTypes();

            foreach (var jobType in jobTypes)
            {
                jobTypesList.Add(new JobType
                {
                    Name = jobType,
                    IsSelected = false
                });
            }

            return jobTypesList;
        }

        private List<Sector> GetJobSectorsFromAvailableJobs()
        {
            var sectorsList = new List<Sector>();
            var sectors = JobsRepository.GetSectors();

            foreach (var sector in sectors)
            {
                sectorsList.Add(new Sector
                {
                    Name = sector,
                    IsSelected = false
                });
            }
           
            return sectorsList;
        }

        private static List<SelectListItem> GetJobLocationsFromAvailableJobs()
        {
            var locationsList = new List<SelectListItem>();
            var locations = JobsRepository.GetLocations();

            foreach (var location in locations)
            {
                locationsList.Add(new SelectListItem
                {
                    Text = location,
                    Value = location,
                    Selected = false
                });
            }

            return locationsList;
        }

        private static List<SelectListItem> GetMinimumSalaryList()
        {
            var locations = new List<SelectListItem>();

            locations.Add(new SelectListItem
            {
                Text = "£10,000",
                Value = "10000",
                Selected = false
            });

            var salarySteps = 50;

            for (var i = 11; i <= salarySteps; i+=2)
            {
                var increaseRange = 0;

                if (i > 20)
                {
                    increaseRange = 1;
                }

                locations.Add(new SelectListItem
                {
                    Text = "£" + (i + increaseRange) + ",000",
                    Value = (i + increaseRange) + "000",
                    Selected = false
                });

                if (i >= salarySteps - 1)
                {
                    locations.Add(new SelectListItem
                    {
                        Text = "£" + (i + increaseRange * 2) + ",000+",
                        Value = (i + increaseRange * 2) + "000",
                        Selected = false
                    });
                }
            }

            return locations;
        }
    }
}
