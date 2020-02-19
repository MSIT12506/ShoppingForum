using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjShoppingForum.Controllers
{
    public class AdminLTEController : Controller
    {
        // GET: AdminLTE
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}