using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class TestController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: Test
        // 測驗後台
        public ActionResult List()
        {
            var t = from i in db.tTests
                    select i;

            List<tTest> tt = t.ToList();

            return View(t);
        }
        //新增題目
        public ActionResult New()
        {
            return View(new tTest());
        }
        [HttpPost]
        public ActionResult New(tTest p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            db.tTests.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //刪除題目
        public ActionResult Delete(int fId)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tTest q = db.tTests.FirstOrDefault(p => p.fTestId == fId);
            db.tTests.Remove(q);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //編輯題目
        public ActionResult Edit(int fId)
        {
            tTest prod = db.tTests.FirstOrDefault(p => p.fTestId == fId);
            if (prod == null)
                return RedirectToAction("List");
            return View(new tTest());
        }
        [HttpPost]
        public ActionResult Edit(tTest p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tTest prod = db.tTests.FirstOrDefault(t => t.fTestId == p.fTestId);
            if (prod != null)
            {
                prod.fTestId = p.fTestId;
                prod.fQuestionId = p.fQuestionId;
                prod.fId = p.fId;
                //prod.fQuestionCount = p.fQuestionCount;
                //prod.fCorrectCount = p.fCorrectCount;
                prod.fQuestionScore = p.fQuestionScore;
                prod.fScoreDate = p.fScoreDate;
                //prod.fTestStar = p.fTestStar;
                //prod.fTestEnd = p.fTestEnd;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}