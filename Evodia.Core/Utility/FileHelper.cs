using System;
using System.IO;
using System.Web;

namespace Evodia.Core.Utility
{
    public class FileHelper
    {
        public void SaveFormAttachmentToServer(FileHelperSettings fileSavingOptions, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0) return;

            var rootPath = Directory.GetParent(HttpContext.Current.Server.MapPath("/"));
            var uploadFolder = Path.Combine(rootPath.FullName, "Uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var parentFolderPath = Path.Combine(uploadFolder, fileSavingOptions.ParentFolderName);
            if (!Directory.Exists(parentFolderPath))
            {
                Directory.CreateDirectory(parentFolderPath);
            }

            var filePath = Path.Combine(parentFolderPath, file.FileName.MakeValidFileName());

            file.SaveAs(filePath);
        }
    }

    public class FileHelperSettings
    {
        public string Directory { get; set; }

        public string ParentFolderName { get; set; }
    }
}
