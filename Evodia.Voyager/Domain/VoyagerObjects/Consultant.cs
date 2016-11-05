using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Consultant")]
    public class Consultant
    {
        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "EmailAddress")]
        public string EmailAddress { get; set; }

        [XmlElement(ElementName = "PhoneNumbers")]
        public PhoneNumbers PhoneNumbers { get; set; }

        [XmlElement(ElementName = "ConsultantDepartment")]
        public string ConsultantDepartment { get; set; }
    }
}
