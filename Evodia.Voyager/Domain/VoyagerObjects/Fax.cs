using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Fax")]
    public class Fax
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "AreaCode")]
        public string AreaCode { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "TelNumber")]
        public string TelNumber { get; set; }
    }
}
