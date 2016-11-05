using System;

namespace UmbracoStarterKit.ExtensionMethods
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Uses reflection to get the value of a public constant 'TypeAlias' of the type supplied
        /// </summary>
        /// <param name="type">type to identify a specific Model</param>
        /// <returns>the string value of the Type.Name unless a TypeAlias constant is avaiable</returns>
        public static string GetTypeAlias(this Type type)
        {
            var typeAlias = type.Name;

            var typeAliasFieldInfo = type.GetField("TypeAlias");

            if (typeAliasFieldInfo != null)
            {
                typeAlias = typeAliasFieldInfo.GetValue(null).ToString();
            }

            return typeAlias;
        }
    }
}