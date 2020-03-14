using PagedList;
using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Discount.Models;
using tw.com.essentialoil.Discount.ViewModels;
using tw.com.essentialoil.Forum.Models;
using tw.com.essentialoil.User.Models;
using tw.com.essentialoil.AdminLTE.Models;
using tw.com.essentialoil.News.Models;
using tw.com.essentialoil.Product.Models;
using tw.com.essentialoil.Questions.Models;
using tw.com.essentialoil.Tests.Models;
using tw.com.essentialoil.Score.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;
using tw.com.essentialoil.ViewModels;
using System.Text;

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
        //後台Admin登入
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
        //管理員登入登出狀態切換
        public ActionResult AfterAdminLogin()
        {
            return PartialView();
        }

        //數據圖表
        //商品銷售排行
        public ActionResult getTopProductData(int topNumber = 5)
        {
            var data = (from o in db.tOrderDetails
                        join p in db.tProducts on o.fProductId equals p.fProductID
                        group o by o.tProduct.fProductChName into g
                        select new
                        {
                            name = g.Key,                        //該項商品Id
                            y = g.Sum(p => p.fOrderQuantity)     //該項商品營銷總數量
                        }).OrderByDescending(q => q.y).Take(topNumber).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        //營業額
        public ActionResult getWeekRevenueData()
        {
            CAdminLTE LTE = new CAdminLTE();

            return Json(new { y = LTE.revenue() },JsonRequestBehavior.AllowGet);
        }

        //後台管理員登出
        public ActionResult AdminManagerLogOut()
        {
            if (Session[CAdminData.S_CURRENT_LOGINED_ADMIN] != null)
            {
                Session.RemoveAll();
                return RedirectToAction("AdminManagerLogin");
            }
            return RedirectToAction("AdminManagerLogin");
        }
        //=======================================後台=======================================

        //=======================================會員=======================================

        //會員管理、會員查詢
        int pagesize = 6;

        public ActionResult MemberList(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            IPagedList<tUserProfile> pageresult =  null;
            var data = db.tUserProfiles.OrderBy(p => p.fId);
            pageresult = data.ToPagedList(currentPage, pagesize);
            return View(pageresult);
        }

        [HttpPost]
        public ActionResult MemberList(string searchUserId, string searchName, string searchCity, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            IQueryable<tUserProfile> UserId = null;
            IQueryable<tUserProfile> Name = null;
            IQueryable<tUserProfile> City = null;
            if(searchUserId != "" && searchUserId !=null)
                UserId = db.tUserProfiles.Where(p => p.fUserId.Contains(searchUserId)).OrderBy(p => p.fId);
            if (searchName != "" && searchName != null)
                Name = db.tUserProfiles.Where(p => p.fName.Contains(searchName)).OrderBy(p => p.fId);//尚待調整
            if (searchCity != "" && searchCity != null)
                City = db.tUserProfiles.Where(p => p.fCity.Contains(searchCity)).OrderBy(p => p.fId);//尚待調整
            
            if (UserId !=null)
            {
                var finduserid = UserId.ToPagedList(currentPage, pagesize);
                return View(finduserid);
            }
            if (Name != null)
            {
                var findname = Name.ToPagedList(currentPage, pagesize);
                return View(findname);
            }
            if (City != null)
            {
                var findcity = City.ToPagedList(currentPage, pagesize);
                return View(findcity);
            }

            return View();
        }

        //會員編輯
        public ActionResult MemberEdit(int id)
        {
            var prod = db.tUserProfiles.Where(m => m.fId == id).FirstOrDefault();
            return View(prod);
        }
        [HttpPost]
        public ActionResult MemberEdit(tUserProfile m)
        {
            var member = db.tUserProfiles.FirstOrDefault(p => p.fId == m.fId);
            member.fName = m.fName;
            member.fPhone = m.fPhone;
            member.fTel = m.fTel;
            member.fCity = m.fCity;
            member.fAddress = m.fAddress;
            member.fScore = m.fScore;
            member.fAuth = m.fAuth;
            member.fAuthPost = m.fAuthPost;
            member.fAuthReply = m.fAuthReply;
            db.SaveChanges();
            return RedirectToAction("MemberList");
        }
       
        //會員停權
        public ActionResult MemberAuth()
        {
            var list = db.tUserProfiles;
            return View(list);
        }

        [HttpPost]
        public ActionResult MemberAuth(int? fId)
        {
            if (fId != 0)
            {
                var auth = db.tUserProfiles.Where(p => p.fId == fId).Select(p => p.fAuth).FirstOrDefault();
                auth = "0";
                db.SaveChanges();
            }
            return View();
        }
        //=======================================會員=======================================

        //======================================討論區======================================

        //後台討論區 - 文章停權/恢復權限列表
        public ActionResult PostListAll(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageCount = 10;

            CForum forum = new CForum();
            return View(forum.queryAllPostContainDisable(currentPage, pageCount));
        }

        //後台討論區 - 置頂功能
        public ActionResult SetForumPostTop(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageCount = 15;

            CForum forum = new CForum();
            return View(forum.queryAllEnableNoTopPost(currentPage, pageCount));
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

        //======================================優惠卷======================================

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
        //======================================優惠卷======================================

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
        public ActionResult ProductCreate(tProduct prod, HttpPostedFileBase prodImg, HttpPostedFileBase produraImg)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            productRepository.InsertProduct(prod, prodImg,produraImg,Server);

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
        public ActionResult ProductEdit(tProduct prod, HttpPostedFileBase prodImg, HttpPostedFileBase produraImg)
        {
            productRepository.UpdateProduct(prod, prodImg, Server,produraImg);
            tProductImage productImage = new tProductImage();
            return RedirectToAction("ProductPage");
        }

        //=======================================商品=======================================

        //=======================================消息=======================================

        //消息清單,沒有剔除不顯示因為是後台
        public ActionResult NewsList(int page = 1)
        {
            int pagesize = 10;
            int currentPage = pagesize < 1 ? 1 : page;
            //var Newspage = db.tNews.OrderBy(p => p.fNewsId).ToList();
            var News = from a in db.tNews
                       where a.fNewsDiscontinue != true
                       orderby a.fNewsId ascending
                       orderby a.fApproved descending
                       select a;
            var Newspage = News.ToList();
            var result = Newspage.ToPagedList(currentPage, pagesize);
            return View(result);
        }
        //消息搜索,沒有剔除不顯示因為是後台
        [HttpPost]
        public ActionResult NewsList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IPagedList<tNew> NewsList = _NewsRepository.GetBackNewstitle(searchKey).ToPagedList(1,10);
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
                if (tNew.fApproved == null)
                {
                    tNew.fApproved = "N";
                    db.SaveChanges();
                }
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

        //直接刪除
        public ActionResult NewsRemove(int Id)
        {
            _NewsRepository.RemoveNews(Id);
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
        public ActionResult QuizList(int page = 1)
        {
            int currentPage = pagesize < 1 ? 1 : page;
            var Quizpage = db.tQuestions.OrderBy(p => p.fQuestionId).ToList();
            var result = Quizpage.ToPagedList(currentPage, pagesize);
            return View(result);
        }

        [HttpPost]
        public ActionResult QuizList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IPagedList<tQuestion> QuizList =_QuizRepository.GetQuizName(searchKey).ToPagedList(1, 10);
            return View(QuizList);
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
        public ActionResult QuizCreate(tQuestion tQuestion)
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
        public ActionResult QuizEdit(tQuestion tQuestion)
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
        public ActionResult TestList(int page = 1)
        {
            int pagesize = 10;
            int currentPage = pagesize < 1 ? 1 : page;
            var Testpage = db.tTests.OrderBy(p => p.fTestId).ToList();
            var result = Testpage.ToPagedList(currentPage, pagesize);
            return View(result);
        }

        [HttpPost]
        //測驗搜尋功能
        public ActionResult TestList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IPagedList<tTest> TestList = _TestRepository.GetTestAccount(searchKey).ToPagedList(1, 10);
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
        public ActionResult ScoreList(int page = 1)
        {
            int pagesize = 10;
            int currentPage = pagesize < 1 ? 1 : page;
            var Scorepage = db.tScores.OrderBy(p => p.fId).ToList();
            var result = Scorepage.ToPagedList(currentPage, pagesize);
            return View(result);
        }

        [HttpPost]
        //分數搜尋
        public ActionResult ScoreList(string searchKey)
        {
            ViewBag.Message = searchKey;
            IPagedList<tScore> ScoreList = _ScoreRepository.GetScoreAccount(searchKey).ToPagedList(1, 10);
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
       
            
        




        public ActionResult Index(int page = 1)
        {
            int pagesize = 10;
            int currentPage = pagesize < 1 ? 1 : page;
            var Orders = db.tOrders.Include(t => t.tUserProfile).OrderByDescending(p => p.fOrderId);
            var result = Orders.ToPagedList(currentPage, pagesize);
            return View(result);
        }

        [HttpPost]
        public ActionResult Index(string Search, string minprice, string maxprice)
        {
            int page = 1;
            int pagesize = 10;
            int currentPage = pagesize < 1 ? 1 : page;
            var Orders = db.tOrders.Include(t => t.tUserProfile).OrderByDescending(p => p.fOrderId);
            if (!string.IsNullOrEmpty(Search))
            {
                Orders = db.tOrders.Include(t => t.tUserProfile).Where(p => p.fOrderId.ToString().Contains(Search) || p.fId.ToString().Contains(Search)  || p.fConsigneeName.ToString().Contains(Search)).OrderByDescending(p => p.fOrderId);
            }
            //if (!string.IsNullOrEmpty(minprice) && !string.IsNullOrEmpty(maxprice))
            //{
            //    var tot = db.tOrderDetails.GroupBy(p=>p.fOrderId).Sum(p => p.FirstOrDefault().fUnitPrice * p.FirstOrDefault().fOrderQuantity);
            //    if(Convert.ToInt32(tot) > Convert.ToInt32(minprice))
            //    {
            //        var total = db.tOrders.Include(t => t.tUserProfile).FirstOrDefault().tOrderDetails.GroupBy(p=>p.fOrderId).Sum(p => p.FirstOrDefault().fUnitPrice * p.FirstOrDefault().fOrderQuantity);
            //    }
                
            //    Orders = db.tOrders.Include(t => t.tUserProfile).FirstOrDefault().tOrderDetails.Sum(p=>p.fUnitPrice*p.fOrderQuantity).OrderByDescending(p => p.fOrderId);
            //}
            var result = Orders.ToPagedList(currentPage, pagesize);
            return View(result);
        }

        //[HttpPost]
        //public ActionResult MemberList(string searchUserId, string searchName, string searchCity, int page = 1)
        //{
        //    int currentPage = page < 1 ? 1 : page;
        //    IQueryable<tUserProfile> UserId = null;
        //    IQueryable<tUserProfile> Name = null;
        //    IQueryable<tUserProfile> City = null;
        //    if (searchUserId != "" && searchUserId != null)
        //        UserId = db.tUserProfiles.Where(p => p.fUserId.Contains(searchUserId)).OrderBy(p => p.fId);
        //    if (searchName != "" && searchName != null)
        //        Name = db.tUserProfiles.Where(p => p.fName.Contains(searchName)).OrderBy(p => p.fId);//尚待調整
        //    if (searchCity != "" && searchCity != null)
        //        City = db.tUserProfiles.Where(p => p.fCity.Contains(searchCity)).OrderBy(p => p.fId);//尚待調整

        //    if (UserId != null)
        //    {
        //        var finduserid = UserId.ToPagedList(currentPage, pagesize);
        //        return View(finduserid);
        //    }
        //    if (Name != null)
        //    {
        //        var findname = Name.ToPagedList(currentPage, pagesize);
        //        return View(findname);
        //    }
        //    if (City != null)
        //    {
        //        var findcity = City.ToPagedList(currentPage, pagesize);
        //        return View(findcity);
        //    }

        //    return View();
        //}


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tOrder tOrder = await db.tOrders.FindAsync(id);
            if (tOrder == null)
            {
                return HttpNotFound();
            }
            return View(tOrder);
        }

        //存出檔案(未完成)
        public ActionResult ExportFile(int? id)
        {
            var query = db.tOrders.Where(p => p.fOrderId == id);
            var items = db.tOrderDetails.Where(p => p.fOrderId == id);
            var customer = db.tUserProfiles.Where(p => p.fId == query.FirstOrDefault().fId);
            COrderViews views = new COrderViews() { Order = query, OrderDetail = items, UserProfile = customer };
            string fileName = string.Format("Order-{0}.csv", DateTime.UtcNow.AddHours(8).ToString("yyyyMMdd"));
            StringBuilder sb = new StringBuilder();
            sb.Append(";fOrderId,fOrderDate,fShippedDate,fRequiredDate,fConsigneeName,fConsigneeCellPhone,fConsigneeAddress," +
                "fOrderCompanyTitle,fOrderTaxIdDNumber,fOrderPostScript");
            foreach (var row in views.Order.ToList())
            {
                sb.AppendFormat("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                    row.fOrderId, row.fOrderDate, row.fShippedDate, row.fRequiredDate, row.fConsigneeName, row.fConsigneeCellPhone,
                    row.fConsigneeAddress, row.fOrderCompanyTitle, row.fOrderTaxIdDNumber, row.fOrderPostScript);
            }

            byte[] OutputContent = new UTF8Encoding().GetBytes(sb.ToString());

            return File(OutputContent, "text/csv", fileName);
        }

        //GET: BacktOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tOrder tOrder = await db.tOrders.FindAsync(id);
            if (tOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tOrder.fId);
            return View(tOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "fOrderId,fId,fOrderDate,fShippedDate,fRequiredDate,fScore,fConsigneeName,fConsigneeTelephone,fConsigneeCellPhone,fConsigneeAddress,fOrderCompanyTitle,fOrderTaxIdDNumber,fOrderPostScript,fDiscountCode,fPayment")] tOrder tOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tOrder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tOrder.fId);
            return View(tOrder);
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
