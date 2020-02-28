 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjShoppingForum.Models.Entity;
using tw.com.essentialoil.Questions.Models;
using tw.com.essentialoil.User.Models;

namespace prjShoppingForum.Controllers.AuthList
{
    public class tQuestionsController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        private QuizRepository _QuizRepository;

        public tQuestionsController()
        {
            _QuizRepository = new QuizRepository();
        }

        //前台題目
        public ActionResult QuizList()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] == null)
            {
                return RedirectToAction("Login", "FrontUserProfile");

            }
            return RedirectToAction("Content", "tQuestions");


        }

        //    //var list = new List<T>();
        //    //fillList(list);
        //    //var randomizedList = new List<T>();
        //    //var rnd = new Random();
        //    //while (list.Count != 0)
        //    //{
        //    //    var index = rnd.Next(0, list.Count);
        //    //    randomizedList.Add(list[index]);
        //    //    list.RemoveAt(index);
        //    //}
        //    //var list = new List<T>();
        //    //var randomizedList = new List<T>();


        //前台題目內容
        public ActionResult Content()
        {
            var AllQuiz = db.tQuestions.ToList();
            int quiznum = AllQuiz.Count;
            //todo沒題目
            var rnd = new Random();
            int qq = rnd.Next(0, quiznum - 1);
            var dd = DateTime.Now;

            var userId = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
            var testTime = db.tTests.Where(p => p.fId == userId).FirstOrDefault();
            //

            if (testTime == null)
            {
                //新增一筆到ttest
            }
            else
            {
                //取出scoreDate的值檢調今天>1
            }


            if (quiznum>0)  //&& 要寫一個flag)
            {
                return View(AllQuiz[qq]);
                
            }

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        //只接受頁面post
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Content(int Id, string RadioButton1)
        {
            //判斷答題對錯來加score 
            //一律更新scoreDate


            //return null;
            return RedirectToAction("","");
        }


        // 後臺_題目
        public ActionResult Index()
        {
            return View(db.tQuestions.ToList());
        }

        // 後台_題目明細
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tQuestion tQuestion = db.tQuestions.Find(id);
            if (tQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tQuestion);
        }

        // 後台_新增題目
        public ActionResult Create()
        {
            return View();
        }

        //後台_新增存資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fQuestionId,fQuestionName,fQuestion,fAnswer,fItemA,fItemB,fItemC,fItemD,fItemE")] tQuestion tQuestion)
        {
            if (ModelState.IsValid)
            {
                db.tQuestions.Add(tQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tQuestion);
        }

        // 後台_編輯題目
        public ActionResult Edit(int Id)
        {
            var Quiz = _QuizRepository.GetQuiz(Id);
            return View(Quiz);
        }

        //後台_編輯題目存資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fQuestionId,fQuestionName,fQuestion,fAnswer,fItemA,fItemB,fItemC,fItemD,fItemE")] tQuestion tQuestion)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tQuestion);
        }

        // 後台_刪除題目
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tQuestion tQuestion = db.tQuestions.Find(id);
            if (tQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tQuestion);
        }

        //後台_刪除題目存資料庫
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tQuestion tQuestion = db.tQuestions.Find(id);
            db.tQuestions.Remove(tQuestion);
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
