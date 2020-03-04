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
using tw.com.essentialoil.Tests.Models;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers
{
    public class QuestionsController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        private QuizRepository _QuizRepository;

        private TestRepository _TestRepository;

        public QuestionsController()
        {
            _QuizRepository = new QuizRepository();

            _TestRepository = new TestRepository();

        }

        //前台題目
        public ActionResult QuizList()
        {
            //判斷是否登入,沒登入就回到login畫面,有登入跳到題目畫面
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] == null)
            {
                return RedirectToAction("Login", "FrontUserProfile");
            }
            return RedirectToAction("Content", "Questions");
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


        //前台題目畫面
        public ActionResult Content()
        {
            var AllQuiz = db.tQuestions.ToList();
            int quiznum = AllQuiz.Count;
            //todo沒題目
            var rnd = new Random();
            //qq=題目Id
            int qq = rnd.Next(0, quiznum - 1);
            var dd = DateTime.UtcNow.AddHours(8);
            string userDateString = dd.ToString("yyyy-MM-dd");

            var userId = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
            var testList = db.tTests.Where(p => p.fId == userId).FirstOrDefault();
            //true 不行做 ; false 可以做
            bool Testflag = true;

            if (testList == null)
            {

                //新增一筆到ttest
                var tTest = new tTest();
                tTest.fId = userId;
                tTest.fQuestionId = qq+1;
                tTest.fScoreDate = dd;
                tTest.fTestDiscontinue = false;
                db.tTests.Add(tTest);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
                Testflag = false;
            }
            else
            {
                var testmax = db.tTests.Where(p => p.fId == userId && p.fQuestionScore != null).Select(p => p.fScoreDate).ToList();
                if (testmax.Count > 0) { }
                DateTime testTime =testmax.Max();
                string userTestDateString = testTime.ToString("yyyy-MM-dd");
                //取出scoreDate的值減掉今天>1
                DateTime dt1 = Convert.ToDateTime(userTestDateString);
                DateTime dt2 = Convert.ToDateTime(userDateString);
                TimeSpan span = dt2 - dt1;
                double days = span.TotalDays;
                int diff = Convert.ToInt32(days);
                if (diff >= 1)
                {
                    //新增一筆到ttest
                    var tTest = new tTest();
                    tTest.fId = userId;
                    tTest.fQuestionId = qq + 1;
                    tTest.fScoreDate = dd;
                    tTest.fTestDiscontinue = false;
                    db.tTests.Add(tTest);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    Testflag = false;
                }
                else
                {
                    //true=做過=不行做
                    var nullscore = db.tTests.Where(p => p.fId == userId && p.fScoreDate == testTime).FirstOrDefault();
                    if (nullscore.fQuestionScore ==null)
                    {
                        Testflag = false;
                    }
                    Testflag = true;
                }
            }
            if (quiznum > 0  && Testflag == false)  //&& 要寫一個flag)
            {
                return View(AllQuiz[qq]);
            }
            
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        //只接受頁面post
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Content(int fQuestionId,string fAnswer,string RadioButton1)
        {
            var userId = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
            var QAns = fAnswer;
            var UserAns = RadioButton1;
            var dd = DateTime.UtcNow.AddHours(8);
            var userTest = db.tTests.Where(p => p.fQuestionId == fQuestionId && p.fId==userId && p.fQuestionScore == null).FirstOrDefault();
            var userdetail = db.tUserProfiles.Where(p => p.fId == userId).FirstOrDefault();
            int userTotalScore = Convert.ToInt32(userdetail.fScore);


            if (QAns == UserAns)
            {
                //更新測驗分數
                userTest.fQuestionScore = 100;
                userTest.fScoreDate = dd;
                try
                {
                    db.SaveChanges();
                    Session["Quizstatus"] = "done";
                }
                catch (Exception e)
                {
                    throw;
                }
                //新增分數異動
                var tScore = new tScore();
                tScore.fId = userId;
                tScore.fQuestionScore = Convert.ToInt32(userTest.fQuestionScore);
                tScore.fActiveScore = 0;
                tScore.fScoreDate = dd;
                tScore.fScore = userTotalScore + (tScore.fActiveScore) + (tScore.fQuestionScore);
                db.tScores.Add(tScore);
                //更新會員積分
                userdetail.fScore = tScore.fScore;
                try
                {
                    db.SaveChanges();
                    Session["Quizstatus"] = "done";
                }
                catch (Exception ee)
                {
                    throw;
                }
            }
            //答錯
            else
            {
                //更新測驗
                userTest.fQuestionScore = 0;
                userTest.fScoreDate = dd;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            //判斷答題對錯來加score 
            //一律更新scoreDate
            //return null;
            return RedirectToAction("MyScore", "FrontUserProfile");
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
