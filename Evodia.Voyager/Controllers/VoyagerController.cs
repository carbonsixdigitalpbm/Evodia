using System;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace Evodia.Voyager.Controllers
{
    public class VoyagerController : SurfaceController
    {
        [HttpPost]
        public JsonResult Sync()
        {
            LogHelper.Info(GetType(), "Sync method has just been called.");

            try
            {
                return Json(new {
                    status = "OK",
                    data = "",
                    message = "Sync has been succesful."
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
