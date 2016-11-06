using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Attribute")]
    public class Attribute
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Essential")]
        public string Essential { get; set; }
    }
}
