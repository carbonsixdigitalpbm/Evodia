using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "TechSupport")]
    public class TechSupport
    {
        [XmlElement(ElementName = "NotificationDetails")]
        public NotificationDetails NotificationDetails { get; set; }
    }
}
