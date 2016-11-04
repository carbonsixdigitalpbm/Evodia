using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using UmbracoStarterKit.ExtensionMethods;
using UmbracoStarterKit.Models;
using UmbracoStarterKit.Utility;

namespace UmbracoStarterKit.Data
{
    public class BlogRepository
    {
        private readonly UmbracoHelper _umbracoHelper;
        
        public BlogRepository(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var blogRoot = _umbracoHelper.TypedContentAtRoot().FirstOrDefault().Descendants("blog").First();
            var allPosts = blogRoot.Descendants("post");

            allPosts = FilterByQueryParameters(allPosts);

            return allPosts.Select(p => p.As<Post>()).OrderByDescending(p => p.ReleaseDate);
        }

        private static IEnumerable<IPublishedContent> FilterByQueryParameters(IEnumerable<IPublishedContent> allPosts)
        {
            var qYear = HttpContext.Current.Request["year"];
            var qMonth = HttpContext.Current.Request["month"];

            int temp;

            if (int.TryParse(qYear, out temp))
            {
                var year = new DateTime(temp, 1, 1);

                allPosts = allPosts.Where(x => x.GetPropertyValue<DateTime>("releaseDate").Year.Equals(year.Year)).ToList();

                if (!string.IsNullOrEmpty(qMonth) && Helpers.GetMonthNumber(qMonth) > 0)
                {
                    var month = new DateTime(1, Helpers.GetMonthNumber(qMonth), 1);

                    allPosts = allPosts.Where(x => x.GetPropertyValue<DateTime>("releaseDate").Month.Equals(month.Month)).ToList();
                }
            }

            return allPosts;
        }
    }
}