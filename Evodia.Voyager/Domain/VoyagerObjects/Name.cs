using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Name")]
    public class Name
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "First")]
        public string First { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Last")]
        public string Last { get; set; }
    }
}
