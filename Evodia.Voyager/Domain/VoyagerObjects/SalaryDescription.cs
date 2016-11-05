using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "SalaryDescription")]
    public class SalaryDescription
    {
        [XmlElement(ElementName = "SalaryRange")]
        public SalaryRange SalaryRange { get; set; }
    }
}
