using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Vacancy")]
    public class Vacancy
    {
        [XmlElement(ElementName = "AdvertStatus")]
        public string AdvertStatus { get; set; }

        [XmlElement(ElementName = "Attributes")]
        public Attributes Attributes { get; set; }

        [XmlElement(ElementName = "Compensation")]
        public Compensation Compensation { get; set; }

        [XmlElement(ElementName = "ClientJobTitle")]
        public string ClientJobTitle { get; set; }

        [XmlElement(ElementName = "ClientRef")]
        public string ClientRef { get; set; }

        [XmlElement(ElementName = "JobReference")]
        public string JobReference { get; set; }

        [XmlElement(ElementName = "Company")]
        public string Company { get; set; }

        [XmlElement(ElementName = "Contact")]
        public string Contact { get; set; }

        [XmlElement(ElementName = "Class1")]
        public string Class1 { get; set; }

        [XmlElement(ElementName = "Class2")]
        public string Class2 { get; set; }

        [XmlElement(ElementName = "Class3")]
        public string Class3 { get; set; }

        [XmlElement(ElementName = "JobDescription")]
        public string JobDescription { get; set; }

        [XmlElement(ElementName = "EndDate")]
        public EndDate EndDate { get; set; }

        [XmlElement(ElementName = "DisplayTo")]
        public DisplayTo DisplayTo { get; set; }

        [XmlElement(ElementName = "Hours")]
        public string Hours { get; set; }

        [XmlElement(ElementName = "JobLocation")]
        public JobLocation JobLocation { get; set; }

        [XmlElement(ElementName = "JobTitle")]
        public string JobTitle { get; set; }

        [XmlElement(ElementName = "JobType")]
        public string JobType { get; set; }

        [XmlElement(ElementName = "OtherContacts")]
        public string OtherContacts { get; set; }

        [XmlElement(ElementName = "ProjectCode")]
        public string ProjectCode { get; set; }

        [XmlElement(ElementName = "QualsCerts")]
        public string QualsCerts { get; set; }

        [XmlElement(ElementName = "ReportsTo")]
        public string ReportsTo { get; set; }

        [XmlElement(ElementName = "Sector")]
        public string Sector { get; set; }

        [XmlElement(ElementName = "StartDate")]
        public StartDate StartDate { get; set; }
    }
}


