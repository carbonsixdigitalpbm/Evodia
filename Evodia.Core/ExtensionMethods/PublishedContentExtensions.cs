using System;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace Evodia.Core.ExtensionMethods
{
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// This extension allows any Umbraco Published Content to be converted to a strong-typed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T As<T>(this IPublishedContent content) where T : RenderModel
        {
            Type modelType;
            T modelObject;

            if (typeof (T).GetTypeAlias() == content.DocumentTypeAlias)
            {
                modelType = typeof (T);
            }
            else
            {
                modelType = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .SingleOrDefault(x =>
                        x.IsSubclassOf(typeof (T))
                        && x.GetTypeAlias() == content.DocumentTypeAlias);
            }

            try
            {
                modelObject = (T) Activator.CreateInstance(modelType ?? typeof (T), content);
            }
            catch
            {
                modelObject = default(T);
            }

            return modelObject;
        }
    }
}
