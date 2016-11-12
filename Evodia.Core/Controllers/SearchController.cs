using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Evodia.Core.Models;

namespace Evodia.Core.Controllers
{
    public class SearchFormController : SurfaceController
    {
        public ActionResult RenderSearchForm()
        {
            return PartialView("~/Views/Partials/Forms/SearchFormView.cshtml", new SearchForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessFormSubmission(SearchForm model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ValidationFailed"] = "The form validation could not pass. Please check your input.";

                return CurrentUmbracoPage();
            }

            TempData["Keywords"] = model.Keywords;
            TempData["TitleOnly"] = model.TitleOnly;

            return RedirectToUmbracoPage(1186);
        }
    }
}
