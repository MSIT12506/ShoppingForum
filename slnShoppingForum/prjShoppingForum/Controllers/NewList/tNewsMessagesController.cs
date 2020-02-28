using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjShoppingForum.Models.Entity;

namespace prjShoppingForum.Controllers.AuthList
{
    public class tNewsMessagesController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: tNewsMessages
        public ActionResult Index()
        {
            var tNewsMessages = db.tNewsMessages.Include(t => t.tNew);
            return View(tNewsMessages.ToList());
        }

        // GET: tNewsMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tNewsMessage tNewsMessage = db.tNewsMessages.Find(id);
            if (tNewsMessage == null)
            {
                return HttpNotFound();
            }
            return View(tNewsMessage);
        }

        // GET: tNewsMessages/Create
        public ActionResult Create()
        {
            ViewBag.fNewsId = new SelectList(db.tNews, "fNewsId", "fClass");
            return View();
        }

        // POST: tNewsMessages/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fMessageId,fNewsId,fMessageTime,fMessageArticle,fM_AddUser,fMessageDiscontinue")] tNewsMessage tNewsMessage)
        {
            if (ModelState.IsValid)
            {
                db.tNewsMessages.Add(tNewsMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fNewsId = new SelectList(db.tNews, "fNewsId", "fClass", tNewsMessage.fNewsId);
            return View(tNewsMessage);
        }

        // GET: tNewsMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tNewsMessage tNewsMessage = db.tNewsMessages.Find(id);
            if (tNewsMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.fNewsId = new SelectList(db.tNews, "fNewsId", "fClass", tNewsMessage.fNewsId);
            return View(tNewsMessage);
        }

        // POST: tNewsMessages/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fMessageId,fNewsId,fMessageTime,fMessageArticle,fM_AddUser,fMessageDiscontinue")] tNewsMessage tNewsMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tNewsMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fNewsId = new SelectList(db.tNews, "fNewsId", "fClass", tNewsMessage.fNewsId);
            return View(tNewsMessage);
        }

        // GET: tNewsMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tNewsMessage tNewsMessage = db.tNewsMessages.Find(id);
            if (tNewsMessage == null)
            {
                return HttpNotFound();
            }
            return View(tNewsMessage);
        }

        // POST: tNewsMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tNewsMessage tNewsMessage = db.tNewsMessages.Find(id);
            db.tNewsMessages.Remove(tNewsMessage);
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
