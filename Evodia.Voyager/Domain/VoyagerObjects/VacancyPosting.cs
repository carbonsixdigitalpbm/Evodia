using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "VacancyPosting")]
    public class VacancyPosting
    {
        [XmlElement(ElementName = "Vacancy")]
        public Vacancy Vacancy { get; set; }

        [XmlElement(ElementName = "Consultants")]
        public Consultants Consultants { get; set; }
    }
}
