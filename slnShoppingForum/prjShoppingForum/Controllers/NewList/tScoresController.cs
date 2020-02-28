using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjShoppingForum.Models.Entity;

namespace prjShoppingForum.Controllers.NewList
{
    public class tScoresController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // 後台會員分數
        public ActionResult Index()
        {
            var tScores = db.tScores.Include(t => t.tUserProfile);
            return View(tScores.ToList());
        }

        // 後台會員分數明細
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tScore tScore = db.tScores.Find(id);
            if (tScore == null)
            {
                return HttpNotFound();
            }
            return View(tScore);
        }

        // 後台分數新增
        public ActionResult Create()
        {
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId");
            return View();
        }

        // 後台分數新增資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fId,fScore,fActiveScore,fQuestionScore,fScoreDate,fScoreDiscontinue")] tScore tScore)
        {
            if (ModelState.IsValid)
            {
                db.tScores.Add(tScore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tScore.fId);
            return View(tScore);
        }

        // 後台編輯分數
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tScore tScore = db.tScores.Find(id);
            if (tScore == null)
            {
                return HttpNotFound();
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tScore.fId);
            return View(tScore);
        }

        // 後台編輯分數資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fId,fScore,fActiveScore,fQuestionScore,fScoreDate,fScoreDiscontinue")] tScore tScore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tScore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tScore.fId);
            return View(tScore);
        }

        // 後台刪除分數
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tScore tScore = db.tScores.Find(id);
            if (tScore == null)
            {
                return HttpNotFound();
            }
            return View(tScore);
        }

        // 後台刪除分數資料庫
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tScore tScore = db.tScores.Find(id);
            db.tScores.Remove(tScore);
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
