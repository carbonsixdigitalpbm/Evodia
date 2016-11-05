using System.Collections.Generic;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Consultants")]
    public class Consultants
    {
        [XmlElement(ElementName = "Consultant")]
        public List<Consultant> Consultant { get; set; }
    }
}
