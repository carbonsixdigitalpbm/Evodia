using System.Configuration;

namespace Evodia.Voyager.Domain
{
    public class Configuration
    {
        public static string VoyagerPath
        {
            get { return ConfigurationManager.AppSettings["voyager.relative.path"]; }
        }
    }
}
