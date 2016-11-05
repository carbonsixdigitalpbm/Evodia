using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "SalaryRange")]
    public class SalaryRange
    {
        [XmlElement(ElementName = "From")]
        public string From { get; set; }

        [XmlElement(ElementName = "To")]
        public string To { get; set; }

        [XmlElement(ElementName = "PackageMin")]
        public string PackageMin { get; set; }

        [XmlElement(ElementName = "PackageMax")]
        public string PackageMax { get; set; }

        [XmlElement(ElementName = "ISOCurrency")]
        public string IsoCurrency { get; set; }

        [XmlElement(ElementName = "Period")]
        public string Period { get; set; }
    }
}
