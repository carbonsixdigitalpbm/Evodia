using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "NotificationDetails")]
    public class NotificationDetails
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "EmailAddress")]
        public string EmailAddress { get; set; }

        [DefaultValue("")]
        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }
    }
}
