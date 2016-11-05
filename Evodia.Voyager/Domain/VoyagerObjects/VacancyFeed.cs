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
    }
}
