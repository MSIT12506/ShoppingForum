﻿using PagedList;
using prjShoppingForum.Models.Entity;
using prjShoppingForum.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Discount.Models;
using tw.com.essentialoil.Discount.ViewModels;
using tw.com.essentialoil.Forum.Models;
using tw.com.essentialoil.News.Models;
using tw.com.essentialoil.Product.Models;
using tw.com.essentialoil.Questions.Models;
using tw.com.essentialoil.Score.Models;
using tw.com.essentialoil.Tests.Models;

namespace tw.com.essentialoil.Controllers
{
    public class AdminLTEController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();
        private NewsRepository _NewsRepository;
        ProductRepository productRepository = new ProductRepository();
        DropDownList DropDownList = new DropDownList();
        private QuizRepository _QuizRepository;
        private TestRepository _TestRepository;
        private ScoreRepository _ScoreRepository;

        //建構子
        public AdminLTEController()
        {
            _QuizRepository = new QuizRepository();
            _TestRepository = new TestRepository();
            _NewsRepository = new NewsRepository();
            _ScoreRepository = new ScoreRepository();

        }




        //=======================================後台=======================================
        //後台首頁 - 儀表板
        public ActionResult Dashboard()
        {
            return View();
        }
        //後台Admin登入，缺加密
        public ActionResult AdminManagerLogin(CAdminData data)
        {
            if (data != null)
            {
                tAdminManager cust = (from d in db.tAdminManagers
                                      where d.ManagerId == data.ManagerId
                                      select d).FirstOrDefault();
                if (cust != null)
                {
                    var backpwd = data.ManagerPassword;
                    if (cust.ManagerPassword == backpwd)
                    {
                        //使用下面Session的值判斷是否使用者已登入
                        Session[CAdminData.S_CURRENT_LOGINED_ADMIN] = cust.ManagerId;//存fUserId
                        return RedirectToAction("Dashboard");//登入成功進到儀表板頁面
                    }
                    return View();
                }
                return View();
            }
            return View();
        }


        public ActionResult AdminManagerLogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("AdminManagerLogin");
        }
        //=======================================後台=======================================

        //=======================================商品=======================================
        // 檢視全部商品
        public ActionResult ProductPage(int page = 1)
        {
            IPagedList<tProduct> pageresult = productRepository.ProdPageList(page);
            return View(pageresult);
        }

        //新增商品
        public ActionResult ProductCreate()
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            return View();
        }
        [HttpPost]
        public ActionResult ProductCreate(tProduct prod, HttpPostedFileBase prodImg)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            productRepository.InsertProduct(prod, prodImg, Server);

            return RedirectToAction("ProductPage");
        }

        //刪除商品
        public ActionResult ProductDelete(int prodId)
        {
            productRepository.deleteProd(prodId);
            return RedirectToAction("ProductPage");
        }

        //修改商品
        public ActionResult ProductEdit(int prodId)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            var prod = db.tProducts.Where(m => m.fProductID == prodId).FirstOrDefault();
            return View(prod);
        }
        [HttpPost]
        public ActionResult ProductEdit(tProduct prod, HttpPostedFileBase prodImg)
        {
            productRepository.UpdateProduct(prod, prodImg, Server);
            tProductImage productImage = new tProductImage();
            return RedirectToAction("ProductPage");
        }

        //=======================================商品=======================================

        //=======================================會員=======================================
        //會員管理、會員編輯、停權、會員查詢ajax
        int pagesize = 10;
        public ActionResult MemberList(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            var users = db.tUserProfiles.ToList();
            var pageresult = users.ToPagedList(currentPage, pagesize);
            return View(pageresult);
        }

        public ActionResult MemberEdit(int id)
        {
            var prod = db.tUserProfiles.Where(m => m.fId == id).FirstOrDefault();
            return View(prod);
        }
        [HttpPost]
        public ActionResult MemberEdit(tUserProfile prod)
        {
            db.SaveChanges();
            return RedirectToAction("MemberList");
        }
        //=======================================會員=======================================

        //======================================討論區======================================
        //後台討論區 - 文章停權/恢復權限列表
        public ActionResult PostListAll()
        {
            CForum forum = new CForum();
            return View(forum.queryAllPostContainDisable());
        }

        //後台討論區 - 置頂功能
        public ActionResult SetForumPostTop()
        {
            CForum forum = new CForum();

            return View(forum.queryAllEnableNoTopPost());
        }

        //後台討論區的置頂列表 - 獨立寫，不使用前台的 partial view
        public ActionResult ShowForumTopList()
        {
            CForum forum = new CForum();
            return PartialView(forum.queryTopPost());
        }

        //--------------------Ajax--------------------
        //文章權限設定
        public ActionResult ActionToEnableOrNot(string pid, string actionName)
        {
            //TODO - 後台登入與前台登入是兩套，待後台這邊的session功能完成，才能一併更新是哪個管理員把文章停權
            CForum forum = new CForum();
            int postid = Convert.ToInt16(pid);
            forum.updatePostByStatus(postid, actionName);
            return Content("");
        }

        //文章置頂設定
        public ActionResult ResetTopList(string postList)
        {
            string[] results = postList.Split('|');
            CForum forum = new CForum();
            forum.resetTopList(results);


            return Content("");
        }

        //======================================討論區======================================

        //======================================優惠眷======================================

        //後台優惠券清單
        public ActionResult DiscountList()
        {
            CDiscount discount = new CDiscount();
            return View(discount.queryAllDiscount().ToList());
        }

        public ActionResult DisableDiscount()
        {
            CDiscount discount = new CDiscount();
            return PartialView(discount.queryDisableDiscount().ToList());
        }

        //後台新增優惠券
        public ActionResult DiscountCreate()
        {
            CDiscount discount = new CDiscount();
            return View();
        }
        //新增優惠券
        public ActionResult DiscountCreatePost(CDiscountCreate[] datas)
        {
            CDiscount discount = new CDiscount();
            discount.craeteDiscount(datas);

            return Content("");
        }

        //優惠券下架
        public ActionResult DiscountToDisable(string discountCode)
        {
            CDiscount discount = new CDiscount();
            bool result = discount.disableCode(discountCode);

            return Content(result.ToString());
        }

        //優惠券上架
        public ActionResult DiscountToEnable(string discountCode)
        {
            CDiscount discount = new CDiscount();
            bool result = discount.enableCode(discountCode);

            return Content(result.ToString());
        }

        //======================================優惠眷======================================

        //=======================================消息=======================================

        //消息清單
        public ActionResult NewsList()
        {
            var t = from i in db.tNews
                    select i;
            var tt = t.Where(p => p.fNewsDiscontinue != true).ToList();
            return View(tt);
        }

        //消息搜索
        [HttpPost]
        public ActionResult NewsList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IEnumerable<tNew> NewsList = _NewsRepository.GetNewstitle(searchKey);
            return View("NewsList", NewsList);
        }

        //消息明細
        public ActionResult NewsDetails(int Id)
        {
            var News = db.tNews.FirstOrDefault(p => p.fNewsId == Id);
            return View(News);
        }

        //新增消息
        public ActionResult NewsCreate()
        {
            ViewBag.fAddUser = new SelectList(db.tAdminManagers, "ManagerEmail", "ManagerId");
            return View();
        }

        //確認新增
        [HttpPost]
        public ActionResult NewsCreate(tNew tNew)
        {

            if (ModelState.IsValid)
            {
                _NewsRepository.InsertNews(tNew);
                db.SaveChanges();
                return RedirectToAction("NewsList");
            }
            return View(tNew);
        }

        // 編輯消息
        public ActionResult NewsEdit(int Id)
        {
            ViewBag.fChangUser = new SelectList(db.tAdminManagers, "ManagerEmail", "ManagerId");
            var News = _NewsRepository.GetNews(Id);
            return View(News);

        }

        // 點擊編輯
        [HttpPost]
        public ActionResult NewsEdit(tNew tNew)
        {
            if (ModelState.IsValid)
            {
                _NewsRepository.UpdateNews(tNew);
                return RedirectToAction("NewsDetails", new { Id = tNew.fNewsId });
            }
            return View(tNew);
        }

        // 刪除使用不顯示方式
        public ActionResult NewsDelete(int Id)
        {
            _NewsRepository.EditNewsToDiscontinue(Id);
            return RedirectToAction("NewsList");
        }

        //日曆

        public ActionResult NewsCalendar()
        {
            return View();
        }


        //=======================================消息=======================================

        //=======================================問題=======================================

        // 後臺_題目
        public ActionResult QuizList()
        {
            return View(db.tQuestions.ToList());
        }

        // 後台_題目明細
        public ActionResult QuizDetails(int? id)
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
        public ActionResult QuizCreate()
        {
            return View();
        }

        //後台_新增存資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuizCreate([Bind(Include = "fQuestionId,fQuestionName,fQuestion,fAnswer,fItemA,fItemB,fItemC,fItemD,fItemE")] tQuestion tQuestion)
        {
            if (ModelState.IsValid)
            {
                db.tQuestions.Add(tQuestion);
                db.SaveChanges();
                return RedirectToAction("QuizList");
            }

            return View(tQuestion);
        }

        // 後台_編輯題目
        public ActionResult QuizEdit(int Id)
        {
            var Quiz = _QuizRepository.GetQuiz(Id);
            return View(Quiz);
        }

        //後台_編輯題目存資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuizEdit([Bind(Include = "fQuestionId,fQuestionName,fQuestion,fAnswer,fItemA,fItemB,fItemC,fItemD,fItemE")] tQuestion tQuestion)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuizList");
            }
            return View(tQuestion);
        }

        // 後台_刪除題目
        public ActionResult QuizDelete(int? id)
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
        [HttpPost, ActionName("QuizDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult QuizDeleteConfirmed(int id)
        {
            tQuestion tQuestion = db.tQuestions.Find(id);
            db.tQuestions.Remove(tQuestion);
            db.SaveChanges();
            return RedirectToAction("QuizList");
        }

        //=======================================問題=======================================

        //=======================================測驗=======================================

        // 後台_測驗
        public ActionResult TestList()
        {
            var t = _TestRepository.AllTest();
            return View(t);
        }

        [HttpPost]
        //測驗搜尋功能
        public ActionResult TestList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IEnumerable<tTest> TestList = _TestRepository.GetTestAccount(searchKey);
            return View(TestList);
        }

        // 後台_測驗明細
        public ActionResult TestDetails(int Id)
        {
            var testList = _TestRepository.GetTest(Id);
            return View(testList);
        }

        // 後台_新增測驗
        public ActionResult TestCreate()
        {
            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName");
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fId");
            return View();
        }

        // 後台_新增測驗到資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestCreate(tTest tTest)
        {
            if (ModelState.IsValid)
            {
                tTest.fScoreDate = DateTime.UtcNow.AddHours(8);
                tTest.fTestDiscontinue = false;
                db.tTests.Add(tTest);
                db.SaveChanges();
                return RedirectToAction("TestList");
            }

            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName", tTest.fQuestionId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tTest.fId);
            return View(tTest);
        }

        // 後台_編輯測驗
        public ActionResult TestEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tTest tTest = db.tTests.Find(id);
            if (tTest == null)
            {
                return HttpNotFound();
            }
            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName", tTest.fQuestionId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tTest.fId);
            return View(tTest);
        }

        //後台_編輯測驗到資料庫
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestEdit(tTest tTest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tTest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TestList");
            }
            ViewBag.fQuestionId = new SelectList(db.tQuestions, "fQuestionId", "fQuestionName", tTest.fQuestionId);
            ViewBag.fId = new SelectList(db.tScores, "fId", "fId", tTest.fId);
            return View(tTest);
        }

        // 後台刪除測驗紀錄
        public ActionResult TestDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tTest tTest = db.tTests.Find(id);
            if (tTest == null)
            {
                return HttpNotFound();
            }
            return View(tTest);
        }

        // 後台_刪除測驗到資料庫
        [HttpPost, ActionName("TestDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tTest tTest = db.tTests.Find(id);
            tTest.fTestDiscontinue = true;
            db.tTests.Remove(tTest);
            db.SaveChanges();
            return RedirectToAction("TestList");
        }

        //=======================================測驗=======================================

        //=======================================分數=======================================

        //分數清單
        public ActionResult ScoreList()
        {
            var tScores = db.tScores.Include(t => t.tUserProfile).Where(p => p.fScoreDiscontinue != true);
            return View(tScores.ToList());
        }

        [HttpPost]
        //分數搜尋
        public ActionResult ScoreList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IEnumerable<tScore> ScoreList = _ScoreRepository.GetScoreAccount(searchKey);
            return View(ScoreList);
        }

        // GET: Scores/Details/5
        public ActionResult ScoreDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tScore tScore = db.tScores.Find(id);
            if (tScore == null)
            {
                return HttpNotFound();
            }
            return View(tScore);
        }

        // 新增積分
        public ActionResult ScoreCreate()
        {
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScoreCreate(tScore tScore)
        {
            if (ModelState.IsValid)
            {
                _ScoreRepository.InsertScore(tScore);
                db.SaveChanges();
                return RedirectToAction("ScoreList");
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tScore.fId);
            return View(tScore);
        }

        //編輯分數
        public ActionResult ScoreEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tScore tScore = db.tScores.Find(id);
            if (tScore == null)
            {
                return HttpNotFound();
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tScore.fId);
            return View(tScore);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScoreEdit(tScore tScore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tScore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ScoreList");
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tScore.fId);
            return View(tScore);
        }

        // 不顯示
        public ActionResult ScoreDelete(int Id)
        {
            _ScoreRepository.EditScoreToAuth(Id);
            return RedirectToAction("ScoreList");
        }


        //=======================================分數=======================================

        //=======================================訂單=======================================

        public async Task<ActionResult> Index()
        {
            var tOrders = db.tOrders.Include(t => t.tUserProfile);
            return View(await tOrders.ToListAsync());
        }

        //=======================================訂單=======================================


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