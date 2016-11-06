using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "ConsultantMobile")]
    public class ConsultantMobile
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "AreaCode")]
        public string AreaCode { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "TelNumber")]
        public string TelNumber { get; set; }
    }
}
