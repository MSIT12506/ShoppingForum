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
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers.FrontUser
{
    public class FrontUserProfileController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: tUserProfile

        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(tUserProfile c)
        {
            db.tUserProfiles.Add(c);
            db.SaveChanges();
            return RedirectToAction("Home");
        }
        public ActionResult UserEdit(string g)
        {
            if(Session[UserDictionary.S_CURRENT_LOGINED_USER]!=null)
            g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
            tUserProfile cust = (new dbShoppingForumEntities()).tUserProfiles.FirstOrDefault(c => c.fUserId == g);
            if (cust == null)
                return RedirectToAction("Home");
            return View(cust);
        }
        [HttpPost]
        public ActionResult UserEdit(string fName, string fTel, string fPhone,string fCity,string fAddress)
        {
            var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
            tUserProfile cust = db.tUserProfiles.FirstOrDefault(t => t.fUserId == g);
            if (cust != null)
            {
                cust.fName = fName;
                cust.fTel = fTel;
                cust.fPhone = fPhone;
                cust.fCity = fCity;
                cust.fAddress = fAddress;
                db.SaveChanges();
            }
            return RedirectToAction("Home");
        }

        //LogIn
        Random r = new Random();
        public async Task<ActionResult> Index()
        {
            var tUserProfile = db.tUserProfiles.Include(t => t.tForumAuth).Include(t => t.tScore).Include(t => t.tUserLog);
            return View(await tUserProfile.ToListAsync());
        }

        public ActionResult Home()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            else
            {
                var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                var detail = db.tUserProfiles.Where(p => p.fName == g).Select(q=>q);
                return View(detail);
            }
        }
        public ActionResult Login(CLoginData data)
        {
            if (Session[UserDictionary.S_AUTHENTICATED_CODE] == null)
            {
                Session[UserDictionary.S_AUTHENTICATED_CODE] =
                    r.Next(0, 10).ToString() +
                    r.Next(0, 10).ToString() +
                    r.Next(0, 10).ToString() +
                    r.Next(0, 10).ToString();
            }
            if (data != null)
            {
                var cust = from d in db.tUserProfiles
                           where d.fUserId == data.fUserId
                           select d.fPassword;
                if (cust != null)
                {
                    if (cust.Contains(data.fPassword))
                    {
                        var Ufid = db.tUserProfiles.FirstOrDefault(u => u.fUserId == data.fUserId).fId;
                        //var UfId = from d in db.tUserProfile
                        //              where d.fUserId == data.fUserId
                        //              select d.fId;
                        if (data.fVeriCode.Equals(Session[UserDictionary.S_AUTHENTICATED_CODE].ToString()) && Session[UserDictionary.S_AUTHENTICATED_CODE] != null)
                        {
                            Session[UserDictionary.S_CURRENT_LOGINED_USER] = data.fUserId;
                            Session[UserDictionary.S_CURRENT_LOGINED_USERfid] = Ufid;
                            return RedirectToAction("Home");
                        }
                        else
                            ViewBag.ErrorMessage = "驗證碼不符";
                    }
                    else ViewBag.ErrorMessage = "密碼不正確";
                }
            }
            return View();
        }

























        //自動產生
        // GET: tUserProfiles/Details/5
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

        // GET: tUserProfiles/Create
        public ActionResult Create()
        {
            ViewBag.fId = new SelectList(db.tForumAuths, "fId", "fAuthBlackList");
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId");
            ViewBag.fId = new SelectList(db.tUserLogs, "fId", "fUserIP");
            return View();
        }

        // POST: tUserProfiles/Create
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

        // GET: tUserProfiles/Edit/5
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

        // POST: tUserProfiles/Edit/5
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

        // GET: tUserProfiles/Delete/5
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

        // POST: tUserProfiles/Delete/5
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
