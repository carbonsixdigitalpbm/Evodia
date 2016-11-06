using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "VacancyFeed")]
    public class VacancyFeed
    {
        [XmlElement(ElementName = "TechSupport")]
        public TechSupport TechSupport { get; set; }

        [XmlElement(ElementName = "Account")]
        public Account Account { get; set; }

        [XmlElement(ElementName = "VacancyPosting")]
        public VacancyPosting VacancyPosting { get; set; }

        [XmlIgnore]
        public string FingerPrint { get; set; }

        [XmlIgnore]
        public bool New { get; set; }

        //[XmlIgnore]
        //public bool? Published { get; set; }

        [XmlIgnore]
        public bool Dirty { get; set; }

        //[XmlIgnore]
        //public int? UmbracoEventId { get; set; }

        [XmlIgnore]
        public bool Delete { get; set; }
    }
}
