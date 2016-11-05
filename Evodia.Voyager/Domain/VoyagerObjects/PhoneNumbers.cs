using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "PhoneNumbers")]
    public class PhoneNumbers
    {
        [XmlElement(ElementName = "Voice")]
        public Voice Voice { get; set; }

        [XmlElement(ElementName = "Fax")]
        public Fax Fax { get; set; }

        [XmlElement(ElementName = "ConsultantMobile")]
        public ConsultantMobile ConsultantMobile { get; set; }
    }
}
