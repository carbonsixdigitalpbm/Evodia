using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Compensation")]
    public class Compensation
    {
        [XmlElement(ElementName = "SalaryDescription")]
        public SalaryDescription SalaryDescription { get; set; }

        [XmlElement(ElementName = "Benefits")]
        public string Benefits { get; set; }
    }
}
