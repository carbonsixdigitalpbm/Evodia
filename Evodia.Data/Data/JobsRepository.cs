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
            var desc = root.PublishedContent.Descendants(VacancyModel.TypeAlias);

            return desc.Select(e => e.As<VacancyModel>());
        }
    }
}
