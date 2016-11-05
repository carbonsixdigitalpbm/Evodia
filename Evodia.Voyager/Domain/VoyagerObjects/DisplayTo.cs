using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Evodia.Voyager.Domain.VoyagerObjects
{
    [XmlRoot(ElementName = "DisplayTo")]
    public class DisplayTo
    {
        [XmlElement(ElementName = "Date")]
        public Date Date { get; set; }
    }
}
