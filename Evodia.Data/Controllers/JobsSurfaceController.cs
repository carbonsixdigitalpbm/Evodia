using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Evodia.Data.Data;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Umbraco.Core;
using Umbraco.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace Evodia.Data.Controllers
{
    public class JobsSurfaceController : SurfaceController
    {
        public ActionResult GetFilteredJobs(int offset, int size, string keywords = "", bool titleOnly = false, string location = "", string sector = "", string salary = "", string type = "")
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

                allJobs = SearchJobsByType(allJobs, type);
                allJobs = SearchJobsByLocation(allJobs, location);
                allJobs = SearchJobsBySector(allJobs, sector);
                allJobs = SearchJobsBySalary(allJobs, salary);
            }

            var pagedJobs = allJobs.Skip(offset * size).Take(size);

            return View(pagedJobs);
        }

        private static List<VacancyModel> SearchJobsByType(List<VacancyModel> jobs, string type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                var types = type.Split(',');


                jobs = jobs.Where(j => j.JobType.ContainsAny(types, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return jobs;
        }

        private static List<VacancyModel> SearchJobsBySalary(List<VacancyModel> jobs, string salaryString)
        {
            double salary;
            var isValidNumber = double.TryParse(salaryString, NumberStyles.Number, null, out salary);

            if (isValidNumber)
            {
                jobs = jobs.Where(j => j.Salary > salary).ToList();
            }

            return jobs;
        }

        private static List<VacancyModel> SearchJobsBySector(List<VacancyModel> jobs, string sector)
        {
            if (!string.IsNullOrEmpty(sector))
            {
                jobs = jobs.Where(j => j.Sector.Equals(sector, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return jobs;
        }

        private static List<VacancyModel> SearchJobsByLocation(List<VacancyModel> jobs, string location)
        {
            if (!string.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j => j.Sector.Equals(location, StringComparison.OrdinalIgnoreCase)).ToList();
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

                LogHelper.Info(GetType(), vacancy.Name + " has a score of " + result.Score);

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

                LogHelper.Info(GetType(), vacancy.Name + " has a score of " + result.Score);

                foundJobs.Add(vacancy);
            }

            return foundJobs;

            // METHOD 1
            //var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];
            //var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            //var term = keywords;
            //var luceneString = "nodeName:";
            //luceneString += "(+" + term.Replace(" ", " +") + ")^5 ";
            //luceneString += "nodeName:" + term;
            //var query = searchCriteria.RawQuery(luceneString);
            //var searchResults = searcher.Search(query).OrderByDescending(x => x.Score);

            // METHOD 2
            //var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];
            //var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            //var query = searchCriteria.Field("nodeName", keywords.Boost(3)).Or().Field("nodeName", keywords.Fuzzy());
            //var searchResults = searcher.Search(query.Compile()).OrderByDescending(x => x.Score).TakeWhile(x => x.Score > 0.05f);

            // METHOD 3
            //var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];
            //var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            //var query = searchCriteria.Field("nodeName", keywords.Boost(5)).Or().Field("nodeName", keywords.Fuzzy());
            //var searchResults = searcher.Search(query.Compile()).OrderByDescending(x => x.Score).TakeWhile(x => x.Score > 0.05f);
        }
    }
}
