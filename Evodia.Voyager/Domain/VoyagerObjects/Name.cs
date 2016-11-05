using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Name")]
    public class Name
    {
        [XmlElement(ElementName = "First")]
        public string First { get; set; }

        [XmlElement(ElementName = "Last")]
        public string Last { get; set; }
    }
}
