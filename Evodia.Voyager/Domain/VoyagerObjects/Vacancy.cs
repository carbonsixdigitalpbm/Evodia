using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "Vacancy")]
    public class Vacancy
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "AdvertStatus")]
        public string AdvertStatus { get; set; }

        [XmlElement(ElementName = "Attributes")]
        public Attributes Attributes { get; set; }

        [XmlElement(ElementName = "Compensation")]
        public Compensation Compensation { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "ClientJobTitle")]
        public string ClientJobTitle { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "ClientRef")]
        public string ClientRef { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "JobReference")]
        public string JobReference { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Company")]
        public string Company { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Contact")]
        public string Contact { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Class1")]
        public string Class1 { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Class2")]
        public string Class2 { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Class3")]
        public string Class3 { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "JobDescription")]
        public string JobDescription { get; set; }

        [XmlElement(ElementName = "EndDate")]
        public EndDate EndDate { get; set; }

        [XmlElement(ElementName = "DisplayTo")]
        public DisplayTo DisplayTo { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Hours")]
        public string Hours { get; set; }

        [XmlElement(ElementName = "JobLocation")]
        public JobLocation JobLocation { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "JobTitle")]
        public string JobTitle { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "JobType")]
        public string JobType { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "OtherContacts")]
        public string OtherContacts { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "ProjectCode")]
        public string ProjectCode { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "QualsCerts")]
        public string QualsCerts { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "ReportsTo")]
        public string ReportsTo { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Sector")]
        public string Sector { get; set; }

        [XmlElement(ElementName = "StartDate")]
        public StartDate StartDate { get; set; }
    }
}


