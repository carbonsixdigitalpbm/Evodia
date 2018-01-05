using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Evodia.Core.Models;

namespace Evodia.Core.Utility
{
    public static class Helpers
    {
        public static int GetMonthNumber(string monthName)
        {
            try
            {
                return DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
            }
            catch
            {
                return 0;
            }
        }

        public static string MakeValidFileName(this string name)
        {
            var invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public static string GetDaySuffix(string day)
        {
            switch (day)
            {
                case "1":
                case "21":
                case "31":
                    return "st";
                case "2":
                case "22":
                    return "nd";
                case "3":
                case "23":
                    return "rd";
                default:
                    return "th";
            }
        }

        public static bool IsPrimarySite(int id = 0)
        {
            const int PrimarySiteRootId = 1153;

            return PrimarySiteRootId == id;
        }

        /// <summary>
        /// Takes a list of nodes and checks if the passed in urlSegment(string)
        /// matches any of the names in the list
        /// </summary>
        /// <param name="source">Selection of nodes</param>
        /// <param name="propertyAlias">Property alias for MNTP to check url segment against</param>
        /// <param name="name">Name to be used as url segment for comparison</param>
        /// <returns>True if any match else false</returns>
        public static IEnumerable<IPublishedContent> FilterByPrevalueName(IEnumerable<IPublishedContent> source, string propertyAlias, string name)
        {
            var urlFriendlyName = name.ToUrlSegment();
            return source.Where(x => IsUrlSegmentInList(x.GetPropertyValue<IEnumerable<IPublishedContent>>(propertyAlias),urlFriendlyName)).ToList();
        }

        /// <summary>
        /// Takes a list of nodes and checks if the passed in urlSegment(string)
        /// matches any of the names in the list
        /// </summary>
        /// <param name="source">Selection of nodes</param>
        /// <param name="urlSegment">Specified url segment to compare</param>
        /// <returns>True if any match else false</returns>
        public static bool IsUrlSegmentInList(IEnumerable<IPublishedContent> source, string urlSegment) {
            if (source == null)
            {
                return false;
            }
            return source.Any(x => x.Name.ToUrlSegment().InvariantEquals(urlSegment));
        }

        public static IEnumerable<IPublishedContent> FilterByDocumentTypeAlias(IEnumerable<IPublishedContent> source, string documentType)
        {
            return source.Where(x => x.DocumentTypeAlias.InvariantEquals(documentType)).ToList();
        }

        public static IEnumerable<IPublishedContent> FilterBySelectedPrevaluePage(IEnumerable<IPublishedContent> source, string propertyAlias, IPublishedContent page)
        {
            //return source.Where(x => x.GetPropertyValue<int>(propertyAlias) == page.Id).ToList();
            return source.Where(x => x == page ).ToList();
        }

        public static IEnumerable<IPublishedContent> FilterByYearAndMonth(IEnumerable<IPublishedContent> source, string month, string year, string propertyAlias)
        {
            int yearInt;
            var monthInt = GetMonthNumber(month);
            var isValidYear = int.TryParse(year, out yearInt);

            if (!isValidYear) return source;

            source = source.Where(x => x.GetPropertyValue<DateTime>(propertyAlias).Year == yearInt);

            if (monthInt > 0)
            {
                source = source.Where(x => x.GetPropertyValue<DateTime>(propertyAlias).Month == monthInt);
            }

            return source;
        }

        public static List<Link> GetCategoryLinks(IPublishedContent root, string baseUrl = "", string qCat = "")
        {
            if (root == null)
            {
                return null;
            }
            var categories = root.Children().ToList();

            if (!categories.Any())
            {
                return null;
            }

            var categoryLinks = new List<Link> {
                new Link{
                    Name = "All",
                    Url = baseUrl,
                    IsActive = String.IsNullOrWhiteSpace(qCat)
                }
            };

            foreach (var item in categories)
            {
                var linkName = item.Name;
                var urlFriendlyName = linkName.ToUrlSegment();
                var linkUrl = baseUrl + "?category=" + urlFriendlyName;
                var isActive = urlFriendlyName.InvariantEquals(qCat);
                categoryLinks.Add(
                    new Link
                    {
                        Name = linkName,
                        Url = linkUrl,
                        IsActive = isActive
                    });
            }
            return categoryLinks;
        }

        public static List<Link> GetAuthorLinks(IPublishedContent root, string baseUrl = "", string qAuthor = "")
        {
            if (root == null)
            {
                return null;
            }
            var authors = root.Children().ToList();

            if (!authors.Any())
            {
                return null;
            }

            var authorLinks = new List<Link> {
                new Link{
                    Name = "All",
                    Url = baseUrl,
                    IsActive = String.IsNullOrWhiteSpace(qAuthor)
                }
            };

            foreach (var item in authors)
            {
                var linkName = item.Name;
                var urlFriendlyName = linkName.ToUrlSegment();
                var linkUrl = baseUrl + "?author=" + urlFriendlyName;
                var isActive = urlFriendlyName.InvariantEquals(qAuthor);
                authorLinks.Add(
                    new Link
                    {
                        Name = linkName,
                        Url = linkUrl,
                        IsActive = isActive
                    });
            }
            return authorLinks;
        }

        public static List<Link> GetDatesFromSelection(List<IPublishedContent> selection = null, string baseUrl = "", string qMonth = "", string qYear = "")
        {
            if (selection == null || !selection.Any())
            {
                return null;
            }

            var dateList = selection.Where(i => i.HasValue("releaseDate"))
                .Select(d => new DateTime(d.GetPropertyValue<DateTime>("releaseDate").Year, d.GetPropertyValue<DateTime>("releaseDate").Month, 1))
                .Distinct()
                .ToList();

            var dateLinks = new List<Link> {
                    new Link{
                        Name = "All dates",
                        Url = baseUrl,
                        IsActive = String.IsNullOrWhiteSpace(qYear) && String.IsNullOrWhiteSpace(qMonth)
                    }
                };

            foreach (var date in dateList)
            {
                var linkName = date.ToString("MMMM yyyy");
                var urlMonth = date.ToString("MMMM").ToLower();
                var urlYear = date.ToString("yyyy");
                var linkUrl = baseUrl + "?month=" + urlMonth + "&year=" + urlYear;
                var isActive = urlMonth.InvariantEquals(qMonth) && urlYear.InvariantEquals(qYear);
                dateLinks.Add(
                    new Link
                    {
                        Name = linkName,
                        Url = linkUrl,
                        IsActive = isActive
                    });
            }
            return dateLinks;
        }

    }
}