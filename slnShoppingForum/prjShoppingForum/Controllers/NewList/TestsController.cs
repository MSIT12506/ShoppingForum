using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjShoppingForum.Models.Entity;
using tw.com.essentialoil.Tests.Models;

namespace tw.com.essentialoil.Controllers
{
    public class TestsController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();
        private TestRepository _TestRepository;


        public TestsController()
        {
            _TestRepository = new TestRepository();
        }


        // 後台_測驗
        public ActionResult Index()
        {
            var t = _TestRepository.AllTest();
            return View(t);
        }

        [HttpPost]
        //後台搜尋功能
        public ActionResult Index(string searchKey)
        {
            ViewBag.Message = searchKey;
            IEnumerable<tTest> TestList = _TestRepository.GetTestAccount(searchKey);
            return View(TestList);
        }


        // 後台_測驗明細
        public ActionResult Details(int Id)
        {
            var testList = _TestRepository.GetTest(Id);
            return View(testList);
        }

        // 後台_新增測驗
        public ActionResult Create()
        {
            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName");
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fId");
            return View();
        }

        // 後台_新增測驗到資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fTestId,fScoreDate,fQuestionScore,fTestDiscontinue")] tTest tTest)
        {
            if (ModelState.IsValid)
            {
                tTest.fScoreDate = DateTime.UtcNow.AddHours(8);
                tTest.fTestDiscontinue = false;
                db.tTests.Add(tTest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName", tTest.fQuestionId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tTest.fId);
            return View(tTest);
        }

        // 後台_編輯測驗
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tTest tTest = db.tTests.Find(id);
            if (tTest == null)
            {
                return HttpNotFound();
            }
            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName", tTest.fQuestionId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tTest.fId);
            return View(tTest);
        }

        //後台_編輯測驗到資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fTestId,fId,fQuestionId,fScoreDate,fQuestionScore,fTestDiscontinue")] tTest tTest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tTest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName", tTest.fQuestionId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tTest.fId);
            return View(tTest);
        }

        // 後台刪除測驗紀錄
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tTest tTest = db.tTests.Find(id);
            if (tTest == null)
            {
                return HttpNotFound();
            }
            return View(tTest);
        }

        // 後台_刪除測驗到資料庫
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tTest tTest = db.tTests.Find(id);
            tTest.fTestDiscontinue = true;
            db.tTests.Remove(tTest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
