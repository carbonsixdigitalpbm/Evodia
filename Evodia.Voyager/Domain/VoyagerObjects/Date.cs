using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Date")]
    public class Date
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "Day")]
        public string Day { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Month")]
        public string Month { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Year")]
        public string Year { get; set; }
    }
}
