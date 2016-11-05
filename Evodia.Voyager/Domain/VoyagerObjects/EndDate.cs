using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "EndDate")]
    public class EndDate
    {
        [XmlElement(ElementName = "Date")]
        public Date Date { get; set; }
    }
}
