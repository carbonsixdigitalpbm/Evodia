using Umbraco.Core.Models;

namespace Evodia.Data.Models
{
    public class VacancyModel : ObjectModelContent
    {
        public const string TypeAlias = "job";

        public VacancyModel(IPublishedContent content) : base(content)
        {
        }

        public string Location {
            get
            {
                return "";
                
            }
        }

        public string Sector
        {
            get
            {
                return "";

            }
        }

        public double Salary
        {
            get
            {
                return 0;
                
            }
        }
    }
}
