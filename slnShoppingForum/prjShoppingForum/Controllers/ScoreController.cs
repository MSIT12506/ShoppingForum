using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class ScoreController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: Score
        // 積分後台介面
        public ActionResult List()
        {

            var t = (from i in db.tScores
                    where i.fId == 1      //TODO
                    select i).FirstOrDefault();

            //List<tScore> tt = t.ToList();

            return View(t);
        }
        //新增題目
        public ActionResult New()
        {
            return View(new tScore());
        }
        [HttpPost]
        public ActionResult New(tScore p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            db.tScores.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //刪除積分
        public ActionResult Delete(int fId)
        {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        tScore q = db.tScores.FirstOrDefault(p => p.fId == fId);
        db.tScores.Remove(q);
                db.SaveChanges();
            return RedirectToAction("List");
    }

    //編輯題目
    public ActionResult Edit(int fId)
        {
            tScore prod = db.tScores.FirstOrDefault(p => p.fId == fId);
            if (prod == null)
                return RedirectToAction("List");
            return View(new tScore());
        }
        [HttpPost]
        public ActionResult Edit(tScore p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tScore prod = db.tScores.FirstOrDefault(t => t.fId == p.fId);
            if (prod != null)
            {
                prod.fId = p.fId;
                prod.fScore = p.fScore;
                prod.fActiveScore = p.fActiveScore;
                prod.fQuestionScore = p.fQuestionScore;
                prod.fScoreDate = p.fScoreDate;
                prod.fAuthTestFlag = p.fAuthTestFlag;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}