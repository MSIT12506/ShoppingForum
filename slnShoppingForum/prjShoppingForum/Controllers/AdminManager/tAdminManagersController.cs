using prjShoppingForum.Models.Entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class tAdminManagersController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: tAdminManagers
        public async Task<ActionResult> Index()
        {
            return View(await db.tAdminManagers.ToListAsync());
        }

        // GET: tAdminManagers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tAdminManager tAdminManager = await db.tAdminManagers.FindAsync(id);
            if (tAdminManager == null)
            {
                return HttpNotFound();
            }
            return View(tAdminManager);
        }

        // GET: tAdminManagers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tAdminManagers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ManagerId,ManagerPassword,ManagerPasswordSalt,ManagerEmail,ManagerAuth")] tAdminManager tAdminManager)
        {
            if (ModelState.IsValid)
            {
                db.tAdminManagers.Add(tAdminManager);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tAdminManager);
        }

        // GET: tAdminManagers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tAdminManager tAdminManager = await db.tAdminManagers.FindAsync(id);
            if (tAdminManager == null)
            {
                return HttpNotFound();
            }
            return View(tAdminManager);
        }

        // POST: tAdminManagers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ManagerId,ManagerPassword,ManagerPasswordSalt,ManagerEmail,ManagerAuth")] tAdminManager tAdminManager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAdminManager).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tAdminManager);
        }

        // GET: tAdminManagers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tAdminManager tAdminManager = await db.tAdminManagers.FindAsync(id);
            if (tAdminManager == null)
            {
                return HttpNotFound();
            }
            return View(tAdminManager);
        }

        // POST: tAdminManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tAdminManager tAdminManager = await db.tAdminManagers.FindAsync(id);
            db.tAdminManagers.Remove(tAdminManager);
            await db.SaveChangesAsync();
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
