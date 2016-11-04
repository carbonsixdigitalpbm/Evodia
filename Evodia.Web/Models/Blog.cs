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
    public class Blog : RenderModel
    {
        public Blog()
            : this(new UmbracoHelper(UmbracoContext.Current).TypedContent(UmbracoContext.Current.PageId))
        { }

        public Blog(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        { }

        public Blog(IPublishedContent content)
            : base(content)
        { }

        public string Headline
        {
            get { return Content.HasValue("headline") ? Content.GetPropertyValue<string>("headline") : Content.Name; }
        }

        public IEnumerable<Post> AllBlogPosts { get; internal set; }

    }
}