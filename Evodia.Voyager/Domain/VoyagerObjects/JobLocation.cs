using System.ComponentModel;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "JobLocation")]
    public class JobLocation
    {
        [DefaultValue("")]
        [XmlElement(ElementName = "Location")]
        public string Location { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "AddressLine1")]
        public string AddressLine1 { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "AddressLine2")]
        public string AddressLine2 { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "AddressLine3")]
        public string AddressLine3 { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Town")]
        public string Town { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "County")]
        public string County { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Postcode")]
        public string Postcode { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }

        [DefaultValue("")]
        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }
    }
}
