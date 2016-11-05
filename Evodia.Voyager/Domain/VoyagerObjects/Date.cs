using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Date")]
    public class Date
    {
        [XmlElement(ElementName = "Day")]
        public string Day { get; set; }

        [XmlElement(ElementName = "Month")]
        public string Month { get; set; }

        [XmlElement(ElementName = "Year")]
        public string Year { get; set; }
    }
}
