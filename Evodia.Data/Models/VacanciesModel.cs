using System.Linq;
using Evodia.Data.ExtensionMethods;
using Evodia.Data.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Evodia.Data.Data
{
    public class VacanciesModel : ObjectModelContent
    {
        public const string TypeAlias = "jobs";

        public VacanciesModel(IPublishedContent content) : base(content)
        {
        }

        public static VacanciesModel JobsRoot(UmbracoHelper umbraco)
        {
            return umbraco.TypedContentAtRoot().DescendantsOrSelf(TypeAlias).First().As<VacanciesModel>();
        }
    }
}