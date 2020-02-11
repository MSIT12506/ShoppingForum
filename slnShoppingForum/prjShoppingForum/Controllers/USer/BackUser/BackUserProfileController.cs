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

namespace tw.com.essentialoil.Controllers.User.BackUser
{
    public class BackUserProfileController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: BackUserProfile
        public async Task<ActionResult> Index()
        {
            var tUserProfile = db.tUserProfiles.Include(t => t.tForumAuth).Include(t => t.tScore).Include(t => t.tUserLog);
            return View(await tUserProfile.ToListAsync());
        }

        //List與自動產生的Index功能一樣，先保留
        public ActionResult List()
        {
            var table = from c in db.tUserProfiles
                        select c;
            return View(table);
            //CUser user = new CUser();
            //return View(user);
        }

        public ActionResult BUserEdit(string fUserId)
        {
            tUserProfile cust = (new dbShoppingForumEntities()).tUserProfiles.FirstOrDefault(c => c.fUserId == fUserId);
            if (cust == null)
                return RedirectToAction("List");
            return View(cust);
        }
        [HttpPost]
        public ActionResult BUserEdit(tUserProfile c)
        {
            tUserProfile cust = db.tUserProfiles.FirstOrDefault(t => t.fUserId == c.fUserId);
            if (cust != null)
            {
                cust.fName = c.fName;
                cust.fTel = c.fTel;
                cust.fPhone = c.fPhone;
                cust.fCity = c.fCity;
                cust.fAddress = c.fAddress;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        //會員不使用刪除功能
        //TODO:會員隱藏




















        //自動產生
        // GET: BackUserProfile/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tUserProfile tUserProfile = await db.tUserProfiles.FindAsync(id);
            if (tUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(tUserProfile);
        }

        // GET: BackUserProfile/Create
        public ActionResult Create()
        {
            ViewBag.fId = new SelectList(db.tForumAuths, "fId", "fAuthBlackList");
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId");
            ViewBag.fId = new SelectList(db.tUserLogs, "fId", "fUserIP");
            return View();
        }

        // POST: BackUserProfile/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "fId,fUserId,fPassword,fPasswordSalt,fName,fGender,fBirthday,fTel,fPhone,fCity,fAddress,fPhoto,fCreateDate,fScore,fAuth,fAuthPost,fAuthReply")] tUserProfile tUserProfile)
        {
            if (ModelState.IsValid)
            {
                db.tUserProfiles.Add(tUserProfile);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.fId = new SelectList(db.tForumAuths, "fId", "fAuthBlackList", tUserProfile.fId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tUserProfile.fId);
            ViewBag.fId = new SelectList(db.tUserLogs, "fId", "fUserIP", tUserProfile.fId);
            return View(tUserProfile);
        }

        // GET: BackUserProfile/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tUserProfile tUserProfile = await db.tUserProfiles.FindAsync(id);
            if (tUserProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.fId = new SelectList(db.tForumAuths, "fId", "fAuthBlackList", tUserProfile.fId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tUserProfile.fId);
            ViewBag.fId = new SelectList(db.tUserLogs, "fId", "fUserIP", tUserProfile.fId);
            return View(tUserProfile);
        }

        // POST: BackUserProfile/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "fId,fUserId,fPassword,fPasswordSalt,fName,fGender,fBirthday,fTel,fPhone,fCity,fAddress,fPhoto,fCreateDate,fScore,fAuth,fAuthPost,fAuthReply")] tUserProfile tUserProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tUserProfile).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.fId = new SelectList(db.tForumAuths, "fId", "fAuthBlackList", tUserProfile.fId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tUserProfile.fId);
            ViewBag.fId = new SelectList(db.tUserLogs, "fId", "fUserIP", tUserProfile.fId);
            return View(tUserProfile);
        }

        // GET: BackUserProfile/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tUserProfile tUserProfile = await db.tUserProfiles.FindAsync(id);
            if (tUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(tUserProfile);
        }

        // POST: BackUserProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tUserProfile tUserProfile = await db.tUserProfiles.FindAsync(id);
            db.tUserProfiles.Remove(tUserProfile);
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
