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
using tw.com.essentialoil.Models;
using tw.com.essentialoil.Order.Models;

namespace tw.com.essentialoil.Controllers.FrontUser
{
    public class FrontUserProfileController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: tUserProfile
        //新增使用者
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(tUserProfile c)
        {
            c.fCreateDate = DateTime.Now;
            c.fAuth = "1";
            c.fAuthPost = true;
            c.fAuthReply = true;
            db.tUserProfiles.Add(c);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        //新增使用者前，先判斷該帳號是否已經存在
        public ActionResult NewUserAjax(string fUserId, string fPassword, string fVeriCode)
        {
            var loginUser = db.tUserProfiles.FirstOrDefault(u => u.fUserId == fUserId);
            string message = "此帳號尚未註冊";
            if (loginUser != null)
            {
                message = "帳號已註冊";
            }
            return Content(message);
        }


        //使用者登入後修改個人資料
        public ActionResult UserEdit()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
            {
                var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                tUserProfile cust = (new dbShoppingForumEntities()).tUserProfiles.FirstOrDefault(c => c.fUserId == g);
                return PartialView(cust);
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult UserEdit(string fName, string fTel, string fPhone, string fCity, string fAddress)
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
            return RedirectToAction("MemberCenter");
        }

        //使用者登入
        Random r = new Random();
        public async Task<ActionResult> Index()
        {
            var tUserProfile = db.tUserProfiles.Include(t => t.tForumAuth).Include(t => t.tScore).Include(t => t.tUserLog);
            return View(await tUserProfile.ToListAsync());
        }

        //Home:user登入狀態，之後會修改，目前先不要使用
        public ActionResult Home()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            else
            {
                var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                var detail = db.tUserProfiles.FirstOrDefault(c => c.fUserId == g);
                return View(detail);
            }
        }

        
        public ActionResult Login(CLoginData data)
        {
            //產生驗證碼
            if (Session[UserDictionary.S_AUTHENTICATED_CODE] == null)
            {
                Session[UserDictionary.S_AUTHENTICATED_CODE] =
                    r.Next(0, 10).ToString() +
                    r.Next(0, 10).ToString() +
                    r.Next(0, 10).ToString() +
                    r.Next(0, 10).ToString();
            }
            //使用者輸入資料
            if (data != null)
            {
                tUserProfile cust = (from d in db.tUserProfiles
                                     where d.fUserId == data.fUserId
                                     select d).FirstOrDefault();

                if (cust != null )
                {
                    if (cust.fPassword==data.fPassword)
                    {
                        if (data.fVeriCode.Equals(Session[UserDictionary.S_AUTHENTICATED_CODE].ToString()) && Session[UserDictionary.S_AUTHENTICATED_CODE] != null)
                        {
                            //使用下面Session的值判斷是否使用者已登入
                            Session[UserDictionary.S_CURRENT_LOGINED_USER] = cust.fUserId;//存fUserId
                            Session[UserDictionary.S_CURRENT_LOGINED_USERFID] = cust.fId;//存fid
                            Session[CDictionary.UserLoginInfo] = new UserLoginInfo() { user_fid = cust.fId, user_name = cust.fName, user_userid = cust.fUserId };//討論區有用到，可參考其用法
                            Session[UserDictionary.S_AUTHENTICATED_CODE] = null;//清掉當前驗證碼
                            return RedirectToAction("MemberCenter");//等首頁命名完成再修改導至首頁
                        }return Content("驗證碼有誤");//ViewBag
                    }return Content("使用者密碼有誤");
                }return View();//TODO:會再修改，目前尚不影響流程
            }return View();
        }

        //判斷帳號是否已存在，目前此段為測試用，將刪除(不會影響其他頁面)
        public ActionResult LoginAjax(string fUserId, string fPassword, string fVeriCode)
        {
            var loginUser = db.tUserProfiles.FirstOrDefault(u => u.fUserId == fUserId);
            string message = "無此帳號";
            if (loginUser != null)
            {
                message = "帳號確認";
            }

            return Content(message);
        }
        //會員中心
        public ActionResult MemberCenter()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            return View();
        }
        //給會員中心使用的數個PartialView
        //會員個人資料
        public ActionResult MemberDetail()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            else
            {
                var q = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                //var q = "CindyHu@test.con";
                var detail = db.tUserProfiles.FirstOrDefault(c => c.fUserId == q);
                return PartialView(detail);
            }
        }
        //會員個人密碼修改
        public ActionResult MemberPasswordEdit()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            return PartialView();
        }

        [HttpPost]
        public ActionResult MemberPasswordEdit(string fPassword, string newPassword, string checkPassword)
        {

            var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
            tUserProfile cust = db.tUserProfiles.FirstOrDefault(t => t.fUserId == g);
            if (cust.fPassword == fPassword)
            {
                if (newPassword == checkPassword)
                {
                    cust.fPassword = newPassword;
                    db.SaveChanges();
                    Session.Clear();
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        //會員個人歷史訂單
        public ActionResult MyOrderList()
        {
            var q = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
            //var id = db.tOrders.FirstOrDefault(p => p.fId == q);
            var prod = db.tOrders.Where(p => p.fId == q).Select(p => p);
            var detail = db.tOrderDetails.Select(p => p);
            var list = new OrderView() { Order = prod, OrderDetail = detail };
            return PartialView(list);
        }
        //會員個人商品追蹤/收藏清單

        //會員個人收藏文章

        //會員個人積分
        public ActionResult MyScore()
        {
            var q = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
            var score = (from i in db.tScores
                         where i.fId == 1      //TODO
                         select i).FirstOrDefault();
            return PartialView(score);
        }
    }
}
