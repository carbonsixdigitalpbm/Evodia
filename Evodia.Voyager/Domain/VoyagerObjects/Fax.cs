using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Fax")]
    public class Fax
    {
        [XmlElement(ElementName = "AreaCode")]
        public string AreaCode { get; set; }

        [XmlElement(ElementName = "TelNumber")]
        public string TelNumber { get; set; }
    }
}
