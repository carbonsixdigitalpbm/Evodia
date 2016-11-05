using System.IO;

namespace Evodia.Voyager.Domain
{
    internal class VoyagerApi
    {
        private static VoyagerApi _instance;

        public static VoyagerApi Instance()
        {
            return _instance ?? (_instance = new VoyagerApi());
        }

        public string[] GetXmlFileNames()
        {
            try
            {
                var path = System.Web.HttpContext.Current.Server.MapPath(Configuration.VoyagerPath);
                var fileNames = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);

                return fileNames;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }
    }
}