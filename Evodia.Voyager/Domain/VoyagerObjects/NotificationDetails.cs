using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "NotificationDetails")]
    public class NotificationDetails
    {
        [XmlElement(ElementName = "EmailAddress")]
        public string EmailAddress { get; set; }

        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }
    }
}
