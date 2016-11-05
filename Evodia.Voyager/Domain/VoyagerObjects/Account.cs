using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Account")]
    public class Account
    {
        [XmlElement(ElementName = "User")]
        public string User { get; set; }

        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }
    }
}
