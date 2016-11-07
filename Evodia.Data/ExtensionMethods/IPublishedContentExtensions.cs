using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Evodia.Data.Models;
using Umbraco.Core.Models;

namespace Evodia.Data.ExtensionMethods
{
    public static class IPublishedContentExtensions
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

            Type modelType = null;
            T modelObject;

            // If this object is actually the right document type then use it's type.
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

            // Let's try and create the object of the type we want.
            //try
            //{
            modelObject = (T)Activator.CreateInstance(modelType ?? typeof(T), new object[] { iPublishedContent });
            //}
            //catch
            //{
            //    modelObject = default(T);
            //}

            return modelObject;
        }

    }
}
