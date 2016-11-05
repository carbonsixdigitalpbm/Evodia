using System.Collections.Generic;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Attributes")]
    public class Attributes
    {
        [XmlElement(ElementName = "Attribute")]
        public List<Attribute> Attribute { get; set; }
    }
}
