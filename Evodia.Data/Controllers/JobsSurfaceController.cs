using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Evodia.Data.Data;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;

namespace Evodia.Data.Controllers
{
    public class JobsSurfaceController : SurfaceController
    {
        public ActionResult GetFilteredJobs(int offset, int size, string keywords = "", bool titleOnly = false, string location = "", string sector = "", string salary = "")
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
                
                SearchJobsByLocation(allJobs, location);
                SearchJobsBySector(allJobs, sector);
                SearchJobsBySalary(allJobs, salary);
            }

            var pagedJobs = allJobs.Skip(offset * size).Take(size);

            return View(pagedJobs);
        }

        private List<VacancyModel> SearchJobsBySalary(List<VacancyModel> jobs, string salary)
        {
            return jobs;
        }

        private List<VacancyModel> SearchJobsBySector(List<VacancyModel> jobs, string sector)
        {
            return jobs;
        }

        private List<VacancyModel> SearchJobsByLocation(List<VacancyModel> jobs, string location)
        {
            return jobs;
        }

        private List<VacancyModel> SearchJobsByKeywords(string keywords)
        {
            return new List<VacancyModel>();
        }

        private List<VacancyModel> SearchJobsByTitleOnly(string keywords)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            var query = searchCriteria.Field("nodeName", keywords.Boost(2)).Or().Field("nodeName", keywords.Fuzzy());

            foreach (var keyword in keywords.Split(' '))
            {
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
