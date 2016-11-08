using System.Globalization;
using Umbraco.Core.Models;

namespace Evodia.Data.Models
{
    public class VacancyModel : ObjectModelContent
    {
        public const string TypeAlias = "job";

        public VacancyModel(IPublishedContent content) : base(content)
        {
        }

        public string JobType
        {
            get
            {
                return this.GetProperty<string>("jobType");
                
            }
        }

        public string Location {
            get
            {
                return this.GetProperty<string>("county");
                
            }
        }

        public string Sector
        {
            get
            {
                return this.GetProperty<string>("sector");

            }
        }

        public double Salary
        {
            get
            {
                var salaryString = this.GetProperty<string>("from");
                double salary;
                var isValidNumber = double.TryParse(salaryString, NumberStyles.Number, null, out salary);

                if (isValidNumber)
                {
                    return salary;
                }

                return 0;
                
                
            }
        }
    }
}
