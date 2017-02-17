using System;
using System.Linq;
using System.Reflection;
using Evodia.Data.Models;
using Umbraco.Core.Models;

namespace Evodia.Data.ExtensionMethods
{
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// This extension allows any Umbraco Published Content to be converted to a strong-typed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iPublishedContent"></param>
        /// <returns></returns>
        public static T As<T>(this IPublishedContent iPublishedContent) where T : ObjectModelContent
        {
            if (iPublishedContent == null)
            {
                return null;
            }

            Type modelType;

            if (typeof(T).GetTypeAlias() == iPublishedContent.DocumentTypeAlias)
            {
                modelType = typeof(T);
            }
            else
            {
                modelType = Assembly.GetExecutingAssembly()
                                    .GetTypes()
                                    .SingleOrDefault(x =>
                                        x.IsSubclassOf(typeof(T))// ensure the class can be cast to the model type requested
                                        && x.GetTypeAlias() == iPublishedContent.DocumentTypeAlias);
            }

            var modelObject = (T)Activator.CreateInstance(modelType ?? typeof(T), new object[] { iPublishedContent });

            return modelObject;
        }

    }
}
