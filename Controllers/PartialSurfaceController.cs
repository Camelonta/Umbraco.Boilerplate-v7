using System;
using Camelonta.Boilerplate.Classes;
using Camelonta.Boilerplate.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Examine;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Camelonta.Boilerplate.Controllers
{
    public class PartialSurfaceController : SurfaceController
    {
        [HttpPost]
        public PartialViewResult CookieWarning(string nodeId)
        {
            var currentSite = Umbraco.TypedContent(nodeId).AncestorOrSelf(1);
            return PartialView("~/Views/Partials/_CookieWarning.cshtml", currentSite);
        }
    }
}