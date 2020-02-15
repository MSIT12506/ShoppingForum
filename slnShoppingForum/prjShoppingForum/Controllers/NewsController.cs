using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class NewsController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        // GET: News
        //消息後台
        public ActionResult List()
        {
            var t = from i in db.tNews
                    select i;
            List<tNew> tt = t.ToList();

            return View(t);
        }

        //新增文章
        public ActionResult New()
        {
            return View(new tNew());
        }
        [HttpPost]
        public ActionResult New(string tmp)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            //db.tNews.Add(tmp);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //刪除題目
        public ActionResult Delete(int fId)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tNew q = db.tNews.FirstOrDefault(p => p.fNewsId == fId);
            db.tNews.Remove(q);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //編輯題目
        public ActionResult Edit(int fId)
        {
            tNew prod = db.tNews.FirstOrDefault(p => p.fNewsId == fId);
            if (prod == null)
                return RedirectToAction("List");
            return View(new tNew());
        }
        [HttpPost]
        public ActionResult Edit(tNew p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tNew prod = db.tNews.FirstOrDefault(t => t.fNewsId == p.fNewsId);
            if (prod != null)
            {
                prod.fClass = p.fClass;
                prod.fNewsTitle = p.fNewsTitle;
                prod.fNewsDesc = p.fNewsDesc;
                prod.fNewsArticle = p.fNewsArticle;
                prod.fNewsTag = p.fNewsTag;
                prod.fGet_No = p.fGet_No;
                prod.fAddUser = p.fAddUser;
                prod.fChangUser = p.fChangUser;
                prod.fDeleteUser = p.fDeleteUser;
                prod.fApproved = p.fApproved;
                prod.fNewsStart = p.fNewsStart;
                prod.fNewsEnd = p.fNewsEnd;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }


    }
}