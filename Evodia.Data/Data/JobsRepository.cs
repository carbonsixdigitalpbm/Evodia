using System.Collections.Generic;
using System.Linq;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Umbraco.Core;
using Umbraco.Web;

namespace Evodia.Data.Data
{
    public static class JobsRepository
    {
        public static IEnumerable<VacancyModel> AllJobs(UmbracoHelper umbraco)
        {
            var root = VacanciesModel.JobsRoot(umbraco);
            var desc = root.PublishedContent.Descendants(VacancyModel.TypeAlias);

            return desc.Select(e => e.As<VacancyModel>());
        }

        public static HashSet<string> GetTypes()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobTypes = new HashSet<string>();
            var jobs = AllJobs(umbracoHelper);

            foreach (var job in jobs)
            {
                var jobType = job.PublishedContent.GetPropertyValue<string>("jobType");

                if (!string.IsNullOrWhiteSpace(jobType))
                {
                    jobTypes.Add(jobType);
                }
            }

            return jobTypes;
        }

        public static HashSet<string> GetSectors()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobSectors = new HashSet<string>();
            var jobs = AllJobs(umbracoHelper);

            foreach (var job in jobs)
            {
                var sector = job.PublishedContent.GetPropertyValue<string>("sector");

                if (!string.IsNullOrWhiteSpace(sector))
                {
                    jobSectors.Add(sector);
                }
            }

            return jobSectors;
        }

        public static HashSet<string> GetLocations()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobLocations = new HashSet<string>();
            var jobs = AllJobs(umbracoHelper);

            foreach (var job in jobs)
            {
                var location = job.PublishedContent.GetPropertyValue<string>("county");

                if (!string.IsNullOrWhiteSpace(location))
                {
                    jobLocations.Add(location);
                }
            }

            return jobLocations;
        }
    }
}
