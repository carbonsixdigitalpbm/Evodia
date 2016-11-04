using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using UmbracoStarterKit.Data;
using UmbracoStarterKit.Models;

namespace UmbracoStarterKit.Controllers
{
    public class BlogController : RenderMvcController
    {
        public ActionResult Blog(RenderModel model)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var blogRepository = new BlogRepository(umbracoHelper);
            var newModel = new Blog(model.Content)
            {
                AllBlogPosts = blogRepository.GetAllPosts()
            };

            return Index(newModel);
        }
    }
}