using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tw.com.essentialoil.Controllers
{
    public class QuestionController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: Question
        //每日一答後台介面
        public ActionResult List()
        {
            //var q1 = db.tQuestion.OrderByDescending(q => q.fQuestionId).ToList();

            var t = from i in db.tQuestions
                    select i;

            List<tQuestion> tt = t.ToList();

            return View(t);
        }
        //新增題目
        public ActionResult New()
        {
            return View(new tQuestion());
        }
        [HttpPost]
        public ActionResult New(tQuestion p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            db.tQuestions.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //刪除題目
        public ActionResult Delete(int fQuizId)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tQuestion q = db.tQuestions.FirstOrDefault(p => p.fQuestionId == fQuizId);
                db.tQuestions.Remove(q);
                db.SaveChanges();
            return RedirectToAction("List");
        }

        //編輯題目
        public ActionResult Edit(int fQuizId)
        {
            tQuestion prod = db.tQuestions.FirstOrDefault(p => p.fQuestionId == fQuizId);
            if (prod == null)
                return RedirectToAction("List");
            return View(new tQuestion());
        }
        [HttpPost]
        public ActionResult Edit(tQuestion p)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();
            tQuestion prod = db.tQuestions.FirstOrDefault(t => t.fQuestionId == p.fQuestionId);
            if (prod != null)
            {
                prod.fQuestionName = p.fQuestionName;
                prod.fQuestion = p.fQuestion;
                prod.fAnswer = p.fAnswer;
                prod.fItemA = p.fItemA;
                prod.fItemB = p.fItemB;
                prod.fItemC = p.fItemC;
                prod.fItemD = p.fItemD;
                prod.fItemE = p.fItemE;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }


    }
}