using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evodia.Data.Data;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Umbraco.Core;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using Examine;
using Examine.SearchCriteria;
using Examine.LuceneEngine.SearchCriteria;

namespace Evodia.Data.Controllers
{
    public class JobsSurfaceController : SurfaceController
    {
        public ActionResult GetFilteredJobs(int offset, int size, string keywords = "", bool titleOnly = false, string location = "", string sector = "", string security = "", string type = "")
        {
            var allJobs = GetJobsBySearch(keywords, titleOnly, location, sector, security, type);
            var queryStringValues = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                queryStringValues.Add("keywords", keywords);
            }

            if (titleOnly)
            {
                queryStringValues.Add("titleonly", "true");
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                queryStringValues.Add("location", location);
            }
            
            if (!string.IsNullOrWhiteSpace(sector))
            {
                queryStringValues.Add("sector", sector);
            }

            if (!string.IsNullOrWhiteSpace(security))
            {
                queryStringValues.Add("salary", security);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                queryStringValues.Add("type", type);
            }

            if (offset > 0)
            {
                offset = offset - 1;
            }

            var activePage = offset + 1;

            var pagedJobs = allJobs.Skip(offset * size).Take(size);

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    status = "OK",
                    count = allJobs.Count,
                    jobs = RenderRazorViewToString("GetFilteredJobs", pagedJobs, 0, ""),
                    navigation = RenderRazorViewToString("GetFilteredJobsNavigation", allJobs, activePage, ToQueryString(queryStringValues))
                });
            }

            return View(pagedJobs);
        }

        public List<VacancyModel> GetJobsBySearch(string keywords = "", bool titleOnly = false, string location = "", string sector = "", string security = "", string type = "")
        {
            List<VacancyModel> allJobs;

            if (titleOnly && !string.IsNullOrEmpty(keywords))
            {
                allJobs = SearchJobsByTitleOnly(keywords);
            }
            else
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    allJobs = SearchJobsByKeywords(keywords);
                }
                else
                {
                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

                    allJobs = JobsRepository.AllJobs(umbracoHelper).ToList();
                }
            }

            allJobs = SearchJobsByType(allJobs, type);
            allJobs = SearchJobsByLocation(allJobs, location);
            allJobs = SearchJobsBySector(allJobs, sector);
            allJobs = SearchJobsBySecurity(allJobs, security);
            //allJobs = SearchJobsBySalary(allJobs, salary);

            return allJobs;
        }

        private static string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array).ToLower();
        }

        private static List<VacancyModel> SearchJobsByType(List<VacancyModel> jobs, string type)
        {
            if (string.IsNullOrEmpty(type)) return jobs;

            var types = type.ToLower().Split(',');
                
            jobs = jobs.Where(j => j.JobType.ToLower().ContainsAny(types, StringComparison.OrdinalIgnoreCase)).ToList();

            return jobs;
        }

        //private static List<VacancyModel> SearchJobsBySalary(List<VacancyModel> jobs, string salaryString)
        //{
        //    double salary;
        //    var isValidNumber = double.TryParse(salaryString, NumberStyles.Number, null, out salary);

        //    if (isValidNumber)
        //    {
        //        jobs = jobs.Where(j => j.Salary > salary).ToList();
        //    }

        //    return jobs;
        //}

        private static List<VacancyModel> SearchJobsBySecurity(List<VacancyModel> jobs, string security)
        {
            if (string.IsNullOrEmpty(security)) return jobs;

            var securityClearances = security.ToLower().Split(',');

            jobs = jobs.Where(j => j.SecurityClearanceLevel.ToLower().ContainsAny(securityClearances, StringComparison.OrdinalIgnoreCase)).ToList();

            return jobs;
        }

        private static List<VacancyModel> SearchJobsBySector(List<VacancyModel> jobs, string sector)
        {
            if (string.IsNullOrEmpty(sector) || sector.Contains("All")) return jobs;

            var sectors = sector.ToLower().Split(',');

            jobs = jobs.Where(j => j.Sector.ToLower().ContainsAny(sectors, StringComparison.OrdinalIgnoreCase)).ToList();

            return jobs;
        }

        private static List<VacancyModel> SearchJobsByLocation(List<VacancyModel> jobs, string location)
        {
            if (!string.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j => j.Location.ToLower().Equals(location.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return jobs;
        }

        private List<VacancyModel> SearchJobsByKeywords(string keywords)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            var query = searchCriteria.Field("nodeName", keywords.Boost(4)).Or()
                .Field("nodeName", keywords.Fuzzy()).Or()
                .Field("jobDescription", keywords.Boost(3)).Or()
                .Field("jobDescription", keywords.Fuzzy());

            foreach (var keyword in keywords.Split(' '))
            {
                query.Or().Field("nodeName", keyword.Boost(3));
                query.Or().Field("nodeName", keyword.Fuzzy());

                query.Or().Field("jobDescription", keyword.Boost(2));
                query.Or().Field("jobDescription", keyword.Fuzzy());
            }

            var searchResults = searcher.Search(query.Compile()).OrderByDescending(x => x.Score).TakeWhile(x => x.Score > 0.035f);
            var foundJobs = new List<VacancyModel>();

            foreach (var result in searchResults)
            {
                var vacancy = Umbraco.TypedContent(result.Id).As<VacancyModel>();

                foundJobs.Add(vacancy);
            }

            return foundJobs;
        }

        private List<VacancyModel> SearchJobsByTitleOnly(string keywords)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            var query = searchCriteria.Field("nodeName", keywords.Boost(3)).Or().Field("nodeName", keywords.Fuzzy());

            foreach (var keyword in keywords.Split(' '))
            {
                query.Or().Field("nodeName", keyword.Boost(1));
                query.Or().Field("nodeName", keyword.Fuzzy());
            }

            var searchResults = searcher.Search(query.Compile()).OrderByDescending(x => x.Score).TakeWhile(x => x.Score > 0.05f);

            var foundJobs = new List<VacancyModel>();

            foreach (var result in searchResults)
            {
                var vacancy = Umbraco.TypedContent(result.Id).As<VacancyModel>();

                foundJobs.Add(vacancy);
            }

            return foundJobs;
        }

        public string RenderRazorViewToString(string viewName, object model, int activePage, string query)
        {
            ViewData.Model = model;
            ViewData["page"] = activePage;
            ViewData["query"] = query;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
