using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;

namespace UmbracoStarterKit.Models
{
    public class Post : RenderModel
    {
        public Post()
            : this(new UmbracoHelper(UmbracoContext.Current).TypedContent(UmbracoContext.Current.PageId))
        { }

        public Post(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        { }

        public Post(IPublishedContent content)
            : base(content)
        { }

        public string Headline
        {
            get { return Content.HasValue("headline") ? Content.GetPropertyValue<string>("headline") : Content.Name; }
        }

        public string Excerpt
        {
            get { return Content.GetPropertyValue<string>("excerpt"); }
        }

        public DateTime ReleaseDate
        {
            get { return Content.GetPropertyValue<DateTime>("releaseDate"); }
        }

        public bool HasAuthor
        {
            get { return Content.HasValue("author"); }
        }

        public int AuthorId
        {
            get { return Content.GetPropertyValue<int>("author"); }
        }

        public string AuthorName
        {
            get
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var authorPage = umbracoHelper.TypedContent(AuthorId);

                return authorPage.Name;
            }
        }

        public string AuthorUrl
        {
            get
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var authorPage = umbracoHelper.TypedContent(AuthorId);

                return authorPage.Url;
            }
        }

        public bool HasCategory
        {
            get { return Content.HasValue("category"); }
        }

        public int CategoryId
        {
            get { return Content.GetPropertyValue<int>("category"); }
        }

        public string CategoryName
        {
            get
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var categoryPage = umbracoHelper.TypedContent(CategoryId);

                return categoryPage.Name;
            }
        }

        public string CategoryUrl
        {
            get
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var categoryPage = umbracoHelper.TypedContent(AuthorId);

                return categoryPage.Url;
            }
        }
    }
}