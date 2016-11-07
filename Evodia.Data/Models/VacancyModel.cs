using Umbraco.Core.Models;

namespace Evodia.Data.Models
{
    public class VacancyModel : ObjectModelContent
    {
        public const string TypeAlias = "job";

        public VacancyModel(IPublishedContent content) : base(content)
        {
        }
    }
}
