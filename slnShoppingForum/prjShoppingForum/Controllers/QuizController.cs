using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult Index()
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            ViewBag.Title = "第一題";
            return View();
        }

        public ActionResult HandleQ1(string Q1)
        {
            //dbShoppingForumEntities db = new dbShoppingForumEntities();

            ViewData["Q1"] = Q1;
            ViewBag.Title = "第二題";
            return View("Q2");
        }
        public ActionResult HandleQ2(string Q1,string Q2)
        {
            //dbShoppingForumEntities db = new dbShoppingForumEntities();

            ViewData["Q1"] = Q1;
            ViewData["Q2"] = Q2;
            ViewBag.Title = "計分結果";
            return View("Result");

        }
        
    }
}