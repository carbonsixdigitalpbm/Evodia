using System.IO;
using System.Web;

namespace Evodia.Core.Utility
{
    public class FileHelper
    {
        public void SaveFormAttachmentToServer(FileHelperSettings fileSavingOptions, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0) return;

            var mainPath = HttpContext.Current.Server.MapPath("~/" + fileSavingOptions.Directory);
            if (!Directory.Exists(mainPath))
            {
                Directory.CreateDirectory(mainPath);
            }

            var parentFolderPath = Path.Combine(mainPath, fileSavingOptions.ParentFolderName);
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
