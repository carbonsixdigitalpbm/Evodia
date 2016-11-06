using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Compensation")]
    public class Compensation
    {
        [XmlElement(ElementName = "SalaryDescription")]
        public SalaryDescription SalaryDescription { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Benefits")]
        public string Benefits { get; set; }
    }
}
