using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "StartDate")]
    public class StartDate
    {
        [XmlElement(ElementName = "Date")]
        public Date Date { get; set; }
    }
}
