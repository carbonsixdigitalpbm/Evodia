using Umbraco.Core.Models;
using Umbraco.Web;

namespace Evodia.Data.Models
{
    /// <summary>
    /// This is the master object for our strong type model. All others inherit from here.
    /// </summary>
    public abstract class ObjectModelContent
    {
        /// <summary>
        /// Always take the current content into the object on creation. That way we always have
        /// access to the original object if needed.
        /// </summary>
        /// <param name="content"></param>
        protected ObjectModelContent(IPublishedContent content)
        {
            PublishedContent = content;
        }

        public IPublishedContent PublishedContent { get; set; }

        /// <summary>
        /// Expose the commonly used properties from the content.
        /// </summary>
        public int Id
        {
            get
            {
                return PublishedContent.Id;
            }
        }
        public string Name
        {
            get
            {
                return PublishedContent.HasValue("clientJobTitle") ? PublishedContent.GetPropertyValue<string>("clientJobTitle") : PublishedContent.Name;
            }
        }
        public string Url
        {
            get
            {
                switch (DocumentTypeAlias)
                {
                    default:
                        return PublishedContent.Url;
                }
            }
        }

        public string DocumentTypeAlias
        {
            get
            {
                return PublishedContent.DocumentTypeAlias;
            }
        }

        protected T GetProperty<T>(string propertyAlias)
        {
            return PublishedContent.GetPropertyValue<T>(propertyAlias);
        }

    }
}
