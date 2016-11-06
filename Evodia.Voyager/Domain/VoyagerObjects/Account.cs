using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Account")]
    public class Account
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "User")]
        public string User { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }
    }
}
