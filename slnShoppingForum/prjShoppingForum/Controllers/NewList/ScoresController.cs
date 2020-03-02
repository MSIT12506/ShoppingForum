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
    public class ScoresController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: Scores
        public ActionResult Index()
        {
            var tScores = db.tScores.Include(t => t.tUserProfile);
            return View(tScores.ToList());
        }

        // GET: Scores/Details/5
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

        // GET: Scores/Create
        public ActionResult Create()
        {
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId");
            return View();
        }

        // POST: Scores/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fScoreId,fId,fScore,fActiveScore,fQuestionScore,fScoreDate,fScoreDiscontinue")] tScore tScore)
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

        // GET: Scores/Edit/5
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

        // POST: Scores/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fScoreId,fId,fScore,fActiveScore,fQuestionScore,fScoreDate,fScoreDiscontinue")] tScore tScore)
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

        // GET: Scores/Delete/5
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

        // POST: Scores/Delete/5
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
