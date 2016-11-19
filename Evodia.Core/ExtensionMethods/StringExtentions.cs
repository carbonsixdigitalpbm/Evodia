using System;

namespace Evodia.Core.ExtensionMethods
{
    public static class StringExtentions
    {
        public static bool IsValidUrl(this string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
