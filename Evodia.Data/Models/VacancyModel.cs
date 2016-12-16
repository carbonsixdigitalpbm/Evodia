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
                return GetProperty<string>("jobType");
                
            }
        }

        public string Location {
            get
            {
                return GetProperty<string>("class2");
                
            }
        }

        public string Sector
        {
            get
            {
                return GetProperty<string>("class1");

            }
        }

        public string SecurityClearanceLevel
        {
            get
            {
                return GetProperty<string>("class3");
            }
        }

        public double Salary
        {
            get
            {
                var salaryString = GetProperty<string>("from");
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
