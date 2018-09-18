using System.IO;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Evodia.Core.Events
{
    public class UmbracoEvents : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Trashed += ContentService_Trashed;
        }

        private static void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var node in e.MoveInfoCollection)
            {
                var cvFileDoctype = node.Entity.ContentType.Alias == Constants.GenericCvFormAlias ||
                                node.Entity.ContentType.Alias == Constants.JobCvFormAlias;
                if (!cvFileDoctype)
                {
                    continue;
                }

                var filePath = node.Entity.GetValue<string>("filePath");

                if (string.IsNullOrEmpty(filePath))
                {
                    continue;
                }

                var dirPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(dirPath))
                {
                    continue;
                }

                var di = new DirectoryInfo(dirPath);

                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (var dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                Directory.Delete(dirPath);
            }
        }
    }
}
