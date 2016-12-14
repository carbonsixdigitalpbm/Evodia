using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
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
                SecurityClearances = GetJobSecurityClearancesFromAvailableJobs()
                //,MinimumSalary = GetMinimumSalaryList()
            };

            var keywords = Request.QueryString["keywords"];
            var titleOnly = Request.QueryString["titleonly"];

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                model.Keywords = keywords;
            }

            if (!string.IsNullOrWhiteSpace(titleOnly))
            {
                model.TitleOnly = Convert.ToBoolean(titleOnly);
            }

            return PartialView("~/Views/Partials/Forms/FullSearchFormView.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessBasicFormSubmission(BasicSearchForm model)
        {
            var queryString = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(model.Keywords))
            {
                queryString["keywords"] = HttpUtility.HtmlEncode(model.Keywords);
            }

            if (model.TitleOnly)
            {
                queryString["titleonly"] = model.TitleOnly.ToString().ToLower();
            }

            return RedirectToUmbracoPage(1186, queryString);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFullFormSubmission(FullSearchForm model)
        {
            var queryString = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(model.Keywords))
            {
                queryString["keywords"] = HttpUtility.HtmlEncode(model.Keywords);
            }

            if (model.TitleOnly)
            {
                queryString["titleonly"] = model.TitleOnly.ToString().ToLower();
            }

            if (model.JobTypes.Any(t => t.IsSelected))
            {
                queryString["type"] = string.Join(",", model.JobTypes.Where(t => t.IsSelected).Select(i => i.Name.ToLower()));
            }

            if (model.Sectors.Any(t => t.IsSelected))
            {
                queryString["sector"] = string.Join(",", model.Sectors.Where(t => t.IsSelected).Select(i => i.Name.ToLower()));
            }

            //if (!string.IsNullOrWhiteSpace(model.SelectedSalary))
            //{
            //    queryString["salary"] = model.SelectedSalary;
            //}

            if (!string.IsNullOrWhiteSpace(model.SelectedLocation))
            {
                queryString["location"] = model.SelectedLocation.ToLower();
            }

            return RedirectToUmbracoPage(1186, queryString);
        }

        private List<JobType> GetJobTypesFromAvailableJobs()
        {
            var jobTypesList = new List<JobType>();
            var jobTypes = JobsRepository.GetTypes();

            foreach (var jobType in jobTypes)
            {
                var isSelected = false;

                var queryStringTypes = Request.QueryString["type"];

                if (!string.IsNullOrWhiteSpace(queryStringTypes))
                {
                    isSelected = queryStringTypes.ToLower().Contains(jobType.ToLower());
                }

                jobTypesList.Add(new JobType
                {
                    Name = jobType,
                    IsSelected = isSelected
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
                var isSelected = false;

                var queryStringSectors = Request.QueryString["sector"];

                if (!string.IsNullOrWhiteSpace(queryStringSectors))
                {
                    isSelected = queryStringSectors.ToLower().Contains(sector.ToLower());
                }

                sectorsList.Add(new Sector
                {
                    Name = sector,
                    IsSelected = isSelected
                });
            }
           
            return sectorsList;
        }

        private List<SecurityClearance> GetJobSecurityClearancesFromAvailableJobs()
        {
            var securityClearancesList = new List<SecurityClearance>();
            var securityClearances = JobsRepository.GetSecurityClearances();

            foreach (var sector in securityClearances)
            {
                var isSelected = false;

                var queryStringSectors = Request.QueryString["security"];

                if (!string.IsNullOrWhiteSpace(queryStringSectors))
                {
                    isSelected = queryStringSectors.ToLower().Contains(sector.ToLower());
                }

                securityClearancesList.Add(new SecurityClearance()
                {
                    Name = sector,
                    IsSelected = isSelected
                });
            }

            return securityClearancesList;
        }

        private List<SelectListItem> GetJobLocationsFromAvailableJobs()
        {
            var locationsList = new List<SelectListItem>();
            var locations = JobsRepository.GetLocations();

            foreach (var location in locations)
            {
                var isSelected = false;

                var queryStringLocation = Request.QueryString["location"];

                if (!string.IsNullOrWhiteSpace(queryStringLocation))
                {
                    isSelected = queryStringLocation.ToLower().Contains(location.ToLower());
                }

                locationsList.Add(new SelectListItem
                {
                    Text = location,
                    Value = location,
                    Selected = isSelected
                });
            }

            return locationsList;
        }

        //private List<SelectListItem> GetMinimumSalaryList()
        //{
        //    const int salarySteps = 50;
        //    var salaries = new List<SelectListItem>();
        //    var isDefaultSelected = false;
        //    var queryStringSalary = Request.QueryString["salary"];

        //    if (!string.IsNullOrWhiteSpace(queryStringSalary))
        //    {
        //        isDefaultSelected = queryStringSalary.Equals("10000");
        //    }

        //    salaries.Add(new SelectListItem
        //    {
        //        Text = "£10,000",
        //        Value = "10000",
        //        Selected = isDefaultSelected
        //    });

        //    for (var i = 11; i <= salarySteps; i+=2)
        //    {
        //        var increaseRange = 0;

        //        if (i > 20)
        //        {
        //            increaseRange = 1;
        //        }

        //        var isSelected = false;

        //        if (!string.IsNullOrWhiteSpace(queryStringSalary))
        //        {
        //            isSelected = queryStringSalary.Equals(i + increaseRange + "000");
        //        }

        //        salaries.Add(new SelectListItem
        //        {
        //            Text = "£" + (i + increaseRange) + ",000",
        //            Value = i + increaseRange + "000",
        //            Selected = isSelected
        //        });

        //        if (i >= salarySteps - 1)
        //        {
        //            salaries.Add(new SelectListItem
        //            {
        //                Text = "£" + (i + increaseRange * 2) + ",000+",
        //                Value = i + increaseRange * 2 + "000",
        //                Selected = isSelected
        //            });
        //        }
        //    }

        //    return salaries;
        //}
    }
}
