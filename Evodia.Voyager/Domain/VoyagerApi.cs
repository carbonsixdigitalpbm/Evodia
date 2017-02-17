using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Evodia.Voyager.Domain.Models;
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

        public List<SyncFile> GetXmlSyncFiles()
        {
            try
            {
                var directoryPath = System.Web.HttpContext.Current.Server.MapPath(Configuration.VoyagerPath);
                var voyagerDirectory = new DirectoryInfo(directoryPath);

                var syncedFiles = new List<SyncFile>();

                foreach (var file in voyagerDirectory.GetFiles("*.xml"))
                {
                    syncedFiles.Add(new SyncFile
                    {
                        FileLocation = file.FullName,
                        FileName = file.Name,
                        FileUpDateTime = GetJobExportTime(file.Name),
                        JobReferenceNumber = GetJobReferenceNumber(file.Name)
                    });

                }

                return syncedFiles; 
            }
            catch (DirectoryNotFoundException ex)
            {
                LogHelper.Info(GetType(), "Failed getting XML file paths:" + ex.Message);

                return null;
            }
        }

        private static DateTime GetJobExportTime(string fileName)
        {
            const string pattern = "yyyyMddHHmmss";

            var ci = new CultureInfo("en-GB");
            
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

            var splitFileName = fileName.Split('_');
            var dateString = splitFileName[1].Trim();
            var jobPostDate = DateTime.ParseExact(dateString, pattern, ci);

            return jobPostDate;
        }

        private static string GetJobReferenceNumber(string fileName)
        {
            var splitFileName = fileName.Split('_');
            var jobRef = splitFileName[2].Replace(".xml", "");
            
            return jobRef;
        }

        public void DeleteXmlFiles(IEnumerable<SyncFile> filesToDelete )
        {
            try
            {
                foreach (var file in filesToDelete)
                {
                    File.Delete(file.FileLocation);

                    LogHelper.Info(GetType(), "Deleted XML fle:" + file.FileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(GetType(), "Failed deleting files:" + ex.Message);
            }
        }
    }
}