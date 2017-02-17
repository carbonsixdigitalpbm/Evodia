using System.Linq;
using Evodia.Data.ExtensionMethods;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Evodia.Data.Models
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