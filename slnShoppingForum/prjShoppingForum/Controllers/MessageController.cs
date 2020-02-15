using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class MessageController : Controller
    {
    
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            // GET: 留言
            //留言後台
            public ActionResult List()
            {
                var t = from i in db.tNewsMessages
                        select i;
                List<tNewsMessage> tt = t.ToList();

                return View(t);
            }

            //新增文章留言
            public ActionResult New()
            {
            return View(new tNewsMessage());
            }

            [HttpPost]
            public ActionResult New(tNewsMessage p)
            {
                dbShoppingForumEntities db = new dbShoppingForumEntities();
                db.tNewsMessages.Add(p);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            //刪除留言
            public ActionResult Delete(int fId)
            {
                dbShoppingForumEntities db = new dbShoppingForumEntities();
            tNewsMessage q = db.tNewsMessages.FirstOrDefault(p => p.fMessageId == fId);
                db.tNewsMessages.Remove(q);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            //編輯留言
            public ActionResult Edit(int fId)
            {
            tNewsMessage prod = db.tNewsMessages.FirstOrDefault(p => p.fMessageId == fId);
                if (prod == null)
                    return RedirectToAction("List");
                return View(new tNewsMessage());
            }
            [HttpPost]
            public ActionResult Edit(tNewsMessage p)
            {
                dbShoppingForumEntities db = new dbShoppingForumEntities();
            tNewsMessage prod = db.tNewsMessages.FirstOrDefault(t => t.fMessageId == p.fMessageId);
                if (prod != null)
                {
                    prod.fM_AddUser = p.fM_AddUser;
                    prod.fMessageTime = p.fMessageTime;
                    prod.fMessageArticle = p.fMessageArticle;

                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }


        }
    }