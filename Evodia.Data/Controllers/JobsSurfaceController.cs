using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Umbraco.Web.Mvc;

namespace Evodia.Data.Controllers
{
    public class JobsSurfaceController : SurfaceController
    {
        public ActionResult GetFilteredJobs(string keywords, int offset, int size, bool titleOnly = false, string location = "", string sector = "", string salary = "")
        {
            List<VacancyModel> allJobs;
            if (titleOnly)
            {
                allJobs = SearchJobsByTitleOnly(keywords);
            }
            else
            {
                allJobs = SearchJobsByKeywords(keywords);

                SearchJobsByLocation(location);
                SearchJobsBySector(sector);
                SearchJobsBySalary(salary);
            }

            var pagedJobs = allJobs.Skip(offset * size).Take(size);

            return View(pagedJobs);
        }

        private void SearchJobsBySalary(string salary)
        {
            throw new NotImplementedException();
        }

        private void SearchJobsBySector(string sector)
        {
            throw new NotImplementedException();
        }

        private void SearchJobsByLocation(string location)
        {
            throw new NotImplementedException();
        }

        private List<VacancyModel> SearchJobsByKeywords(string keywords)
        {
            return new List<VacancyModel>();
        }

        private List<VacancyModel> SearchJobsByTitleOnly(string keywords)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection["JobsSearchSearcher"];

            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            var query = searchCriteria.Field("nodeName", keywords.Boost(3)).Or().Field("jobDescription", keywords.Fuzzy());
            var searchResults = searcher.Search(query.Compile()).OrderByDescending(x => x.Score).TakeWhile(x => x.Score > 0.05f);

            var foundJobs = new List<VacancyModel>();

            foreach (var result in searchResults)
            {
                var vacancy = Umbraco.TypedContent(result.Id).As<VacancyModel>();

                foundJobs.Add(vacancy);
            }

            return foundJobs;
        }
    }
}
