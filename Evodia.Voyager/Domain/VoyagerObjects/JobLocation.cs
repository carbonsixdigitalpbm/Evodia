using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "JobLocation")]
    public class JobLocation
    {
        [XmlElement(ElementName = "Location")]
        public string Location { get; set; }

        [XmlElement(ElementName = "AddressLine1")]
        public string AddressLine1 { get; set; }

        [XmlElement(ElementName = "AddressLine2")]
        public string AddressLine2 { get; set; }

        [XmlElement(ElementName = "AddressLine3")]
        public string AddressLine3 { get; set; }

        [XmlElement(ElementName = "Town")]
        public string Town { get; set; }

        [XmlElement(ElementName = "County")]
        public string County { get; set; }

        [XmlElement(ElementName = "Postcode")]
        public string Postcode { get; set; }

        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }

        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }
    }
}
