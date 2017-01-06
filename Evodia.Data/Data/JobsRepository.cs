using System.Collections.Generic;
using System.Linq;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Umbraco.Web;

namespace Evodia.Data.Data
{
    public static class JobsRepository
    {
        public static IEnumerable<VacancyModel> AllJobs(UmbracoHelper umbraco)
        {
            var root = VacanciesModel.JobsRoot(umbraco);
            var desc = root.PublishedContent.Descendants(VacancyModel.TypeAlias).OrderByDescending(v => v.CreateDate);

            return desc.Select(e => e.As<VacancyModel>());
        }

        public static SortedSet<string> GetTypes()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobTypes = new SortedSet<string>();
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

        public static SortedSet<string> GetSectors()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobSectors = new SortedSet<string>();
            var jobs = AllJobs(umbracoHelper);

            foreach (var job in jobs)
            {
                var sector = job.PublishedContent.GetPropertyValue<string>("class1");

                if (!string.IsNullOrWhiteSpace(sector))
                {
                    jobSectors.Add(sector);
                }
            }

            return jobSectors;
        }

        public static SortedSet<string> GetSecurityClearances()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobSecurityClearances = new SortedSet<string>();
            var jobs = AllJobs(umbracoHelper);

            foreach (var job in jobs)
            {
                var sector = job.PublishedContent.GetPropertyValue<string>("class3");

                if (!string.IsNullOrWhiteSpace(sector))
                {
                    jobSecurityClearances.Add(sector);
                }
            }

            return jobSecurityClearances;
        }

        public static SortedSet<string> GetLocations()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var jobLocations = new SortedSet<string>();
            var jobs = AllJobs(umbracoHelper);

            foreach (var job in jobs)
            {
                var location = job.PublishedContent.GetPropertyValue<string>("class2");

                if (!string.IsNullOrWhiteSpace(location))
                {
                    jobLocations.Add(location);
                }
            }

            return jobLocations;
        }
    }
}
