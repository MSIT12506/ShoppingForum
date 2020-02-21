using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Forum.Models;

namespace tw.com.essentialoil.Controllers
{
    public class AdminLTEController : Controller
    {
        // GET: AdminLTE
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult PostListAll()
        {
            CForum forum = new CForum();
            return View(forum.queryAllPostContainDisable());
        }



        //討論區
        //置頂功能
        public ActionResult SetForumPostTop()
        {
            CForum forum = new CForum();

            return PartialView(forum.queryTopPost());
        }
    }
}