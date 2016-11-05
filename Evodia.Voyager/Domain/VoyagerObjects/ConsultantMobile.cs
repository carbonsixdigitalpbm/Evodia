using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "ConsultantMobile")]
    public class ConsultantMobile
    {
        [XmlElement(ElementName = "AreaCode")]
        public string AreaCode { get; set; }

        [XmlElement(ElementName = "TelNumber")]
        public string TelNumber { get; set; }
    }
}
