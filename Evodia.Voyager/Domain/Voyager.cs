using System.IO;
using System.Xml.Serialization;
using Evodia.Voyager.Domain.VoyagerObjects;

namespace Evodia.Voyager.Domain
{
    public class Voyager
    {
        public void ReadAndDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(VacancyFeed));
            var path = System.Web.HttpContext.Current.Server.MapPath("/Voyager/InfinityJobXML_20161020114241_JO0000000605.xml");

            using (var file = File.OpenText(path))
            {
                var data = (VacancyFeed)serializer.Deserialize(file);

                var isNotNull = data != null;
            }
        }
    }
}
