using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "SalaryRange")]
    public class SalaryRange
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "From")]
        public string From { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "To")]
        public string To { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "PackageMin")]
        public string PackageMin { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "PackageMax")]
        public string PackageMax { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "ISOCurrency")]
        public string IsoCurrency { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Period")]
        public string Period { get; set; }
    }
}
