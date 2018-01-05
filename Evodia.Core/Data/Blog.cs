using System;
using Umbraco.Core.Models;
using Umbraco.Web;
using Evodia.Core.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Evodia.Core.Data
{
    public static class Blog
    {
        public static IEnumerable<IPublishedContent> AllPosts(IPublishedContent root)
        {
            var posts = new List<IPublishedContent>();

            if (root == null)
            {
                return posts;
            }

            posts = root.Children().ToList();

            return posts;
        }

        public static IEnumerable<IPublishedContent> AllOrderedPosts(IPublishedContent root)
        {
            return AllPosts(root).OrderByDescending(x => x.GetPropertyValue<DateTime>("releaseDate"));
        }

        public static bool StringInList(string stringInQuestion, string stringList)
        {
            return stringList.Split(',').Any(x => x.Equals(stringInQuestion, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<IPublishedContent> FilterSelection(IEnumerable<IPublishedContent> source, string author, string category, string month, string year)
        {
            var filterByCategory = !string.IsNullOrWhiteSpace(category);
            var filterByAuthor = !string.IsNullOrWhiteSpace(author);

            if (filterByAuthor && filterByCategory)
            {
                var postsInCategory = Helpers.FilterByPrevalueName(source, "category", category);
                var postsByAuthor = Helpers.FilterByPrevalueName(source, "author", author);

                source = postsInCategory.Intersect(postsByAuthor).ToList();
            }
            else
            {
                if (filterByAuthor)
                {
                    source = Helpers.FilterByPrevalueName(source, "author", author);
                }

                if (filterByCategory)
                {
                    source = Helpers.FilterByPrevalueName(source, "category", category);
                }
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                source = Helpers.FilterByYearAndMonth(source, month, year, "releaseDate");
            }

            return source;
        }
        
    }
}