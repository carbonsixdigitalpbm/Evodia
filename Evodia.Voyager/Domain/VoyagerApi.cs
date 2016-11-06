using System;
using System.IO;
using Umbraco.Core.Logging;

namespace Evodia.Voyager.Domain
{
    internal class VoyagerApi
    {
        private static VoyagerApi _instance;

        public static VoyagerApi Instance()
        {
            return _instance ?? (_instance = new VoyagerApi());
        }

        public string[] GetXmlFilePaths()
        {
            try
            {
                var directory = System.Web.HttpContext.Current.Server.MapPath(Configuration.VoyagerPath);
                var fileNames = Directory.GetFiles(directory, "*.xml", SearchOption.TopDirectoryOnly);

                return fileNames;
            }
            catch (DirectoryNotFoundException ex)
            {
                LogHelper.Info(GetType(), "Failed getting XML file paths:" + ex.Message);

                return null;
            }
        }

        public void DeleteXmlFiles()
        {
            try
            {
                foreach (var filePath in GetXmlFilePaths())
                {
                    File.Delete(filePath);
                    LogHelper.Info(GetType(), "Deleted XML fle:" + filePath);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(), "Failed deleting files:" + ex.Message);
            }
        }
    }
}