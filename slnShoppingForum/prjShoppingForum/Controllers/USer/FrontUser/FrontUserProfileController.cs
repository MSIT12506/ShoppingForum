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
using System.Text;
using System.Net.Mail;
using System.Security.Cryptography;
using prjShoppingForum.Models.User;
using System.Web.Security;
using System.Web.Configuration;
using tw.com.essentialoil.Services;

namespace tw.com.essentialoil.Controllers.FrontUser
{
    public class FrontUserProfileController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();
        private readonly CUsersDBService membersService = new CUsersDBService();
        private readonly CUserMailService mailService = new CUserMailService();
        CUserDataMethod userinfo = new CUserDataMethod();
        Random r = new Random();
        // GET: tUserProfile
        //新增使用者
        public ActionResult New()
        {
            //if (User.Identity.IsAuthenticated)
            //    return RedirectToAction("Index", "Item");
            return View();
        }
        [HttpPost]
        public ActionResult New(tUserProfile c)
        {
            if (ModelState.IsValid)
            {               
                //信箱驗證碼
                string AuthCode = mailService.GetNewPassword();
                c.fAuthCode = AuthCode;
                membersService.New(c);
            
                return RedirectToAction("Login");
            }        
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EmailCheck(string Account, string AuthCode)
        {           
            ViewData["EmailCheck"] = membersService.EmailCheck(Account, AuthCode);
            return View();
        }

        //新增使用者前，先判斷該帳號是否已經存在
        public ActionResult NewUserAjax(string fUserId)
        {
            var loginUser = db.tUserProfiles.FirstOrDefault(u => u.fUserId == fUserId);
            string message = "此帳號尚未註冊";
            if (loginUser != null)
            {
                message = "帳號已註冊";
            }
            return Content(message);
        }

        //使用者登入後，修改個人資料
        //[Authorize]
        public ActionResult UserEdit()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
            {
                var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                tUserProfile cust = membersService.GetByUserId(g);
                return PartialView(cust);
            }
            return RedirectToAction("Login");
        }

        //[Authorize]
        [HttpPost]
        public ActionResult UserEdit(string fName, string fTel, string fPhone, string fCity, string fAddress)
        {
            var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
            tUserProfile cust = membersService.GetByUserId(g);
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

        public async Task<ActionResult> Index()
        {
            var tUserProfile = db.tUserProfiles.Include(t => t.tForumAuth).Include(t => t.tScores).Include(t => t.tUserLogs);
            return View(await tUserProfile.ToListAsync());
        }

        //Home:user登入狀態，之後會修改，目前先不要使用
        public ActionResult Home()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
            {
                var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                var detail = db.tUserProfiles.FirstOrDefault(c => c.fUserId == g);
                return View(detail);
            }
            return RedirectToAction("Login");
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
            
            //使用者輸入資料           
            if (data != null)
            {
                tUserProfile cust = userinfo.GetUserProfile(data.fUserId);
         
                var message = "";
                if (cust != null )
                {                    
                    string result = membersService.HashPassword(data.fPassword, cust.fPasswordSalt);
                    if (cust.fPassword== result)
                    {
                        if (data.fVeriCode.Equals(Session[UserDictionary.S_AUTHENTICATED_CODE].ToString()) && Session[UserDictionary.S_AUTHENTICATED_CODE] != null)
                        {
                            //使用下面Session的值判斷是否使用者已登入
                            Session[UserDictionary.S_CURRENT_LOGINED_USER] = cust.fUserId;//存fUserId
                            Session[UserDictionary.S_CURRENT_LOGINED_USERFID] = cust.fId;//存fid
                            Session[CDictionary.UserLoginInfo] = new UserLoginInfo() { user_fid = cust.fId, user_name = cust.fName, user_userid = cust.fUserId };//討論區有用到，可參考其用法
                            Session[UserDictionary.S_AUTHENTICATED_CODE] = null;//清掉當前驗證碼
                            tUserLog signin = new tUserLog();
                            signin.fId = cust.fId;
                            signin.fLoginTime = DateTime.Now;
                            db.tUserLogs.Add(signin);
                            db.SaveChanges();

                            //導回前一頁的功能:由於購物車與Forum的session設定需微調，所以此處先註解，但此功能在討論區可用，已測試過。(2020/02/24)
                            //1.登入後需統一導回首頁。
                            //2.但在登入後>又進入Forum頁面後，可在討論區頁面裡進行回到前頁功能。
                            //加入此段目前產生問題:登入後直接導入購物車畫面，這個需修改才能使用此段。
                            //LoginPageInfo loginPageInfo = Session[CDictionary.LoginPageInfo] as LoginPageInfo;
                            //if (loginPageInfo != null)
                            //{
                            //    return RedirectToRoute(new
                            //    {
                            //        controller = loginPageInfo.controllerName,
                            //        action = loginPageInfo.actionName
                            //    });
                            //}

                            string RoleData = membersService.GetRole(cust.fUserId);
                            string userData = "";

                            //Cookie ticket
                            //var ticket = new FormsAuthenticationTicket(
                            //    1,
                            //    name: cust.fName,
                            //    DateTime.UtcNow,
                            //    DateTime.UtcNow.AddMinutes(20),
                            //    true,
                            //    userData,
                            //    FormsAuthentication.FormsCookiePath);

                            //// Encrypt the ticket.
                            //string encryptTicket = FormsAuthentication.Encrypt(ticket);
                            //// Create the cookie.
                            //var ticketCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
                            //// 加入cookie
                            //Response.Cookies.Add(ticketCookie);

                            return RedirectToAction("Index","Home");
                        }
                        message = "驗證碼有誤";
                        return View();
                    }
                    message = "使用者密碼有誤";
                    return View();
                }
                message = "尚未輸入使用者帳號"; 
                return View();
            }return View();
        }

        //判斷帳號是否已存在，目前此段提供為測試用，將刪除(不會影響其他頁面)
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

        //忘記密碼
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]//寄信成功 未更改密碼 待修正
        public ActionResult ForgetPassword(string UserName)
        {
            //找出資料庫有此使用者，將他的密碼更換，然後寄密碼給他
            var userforgot = userinfo.GetUserProfile(UserName);
            if (userforgot != null)
            {    
                try
                {
                    string newpwd = mailService.GetNewPassword();
                    string newrdn = mailService.GetSalt();                   
                    string result = membersService.HashPassword(newpwd, newrdn); 
                

                    userforgot.fPasswordSalt = newrdn;
                    userforgot.fPassword = result;
                    db.SaveChanges();
            
                    if (ModelState.IsValid)
                    {
                        mailService.SendNewMail(UserName, newpwd);                       
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Some Error";
                }
                return RedirectToAction("Login");
            }return RedirectToAction("Login");
        }

        //會員中心
        //[Authorize]
        public ActionResult MemberCenter()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
                return View();
            return RedirectToAction("Login");
        }

        //會員中心PartialView
        //會員個人資料
        ////[Authorize]
        public ActionResult MemberDetail()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
            {
                var q = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();              
                var detail = db.tUserProfiles.FirstOrDefault(c => c.fUserId == q);
                return PartialView(detail);
            }
            return RedirectToAction("Login");
        }

        //會員個人密碼修改
        //[Authorize]
        public ActionResult MemberPasswordEdit()
        {
            //if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
                return PartialView();
        }

        //[Authorize]
        [HttpPost]
        public ActionResult MemberPasswordEdit(string fPassword, string newPassword, string checkPassword)
        {

            var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
            tUserProfile cust = db.tUserProfiles.FirstOrDefault(t => t.fUserId == g);           
            string pwd = membersService.HashPassword(fPassword, cust.fPasswordSalt);
            if (cust.fPassword == pwd)
            {
                if (newPassword == checkPassword)
                {                   
                    var newsalt = mailService.GetSalt();                   
                    string newpwd = membersService.HashPassword(newPassword, newsalt);
                    cust.fPasswordSalt = newsalt.ToString();
                    cust.fPassword = newpwd;
                    db.SaveChanges();
                    Session.Clear();
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("MemberCenter");
        }
        //會員個人歷史訂單
        //[Authorize]
        public ActionResult MyOrderList()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                var q = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                var prod = db.tOrders.Where(p => p.fId == q).Select(p => p);
                var detail = db.tOrderDetails.Select(p => p);
                var list = new OrderView() { Order = prod, OrderDetail = detail };
                return PartialView(list);
            }
            return RedirectToAction("Login");
        }


        //會員積分
        //[Authorize]
        public ActionResult MyScore()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                var q = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                var tscore = db.tUserProfiles.FirstOrDefault(p => p.fId == q);
                if (tscore != null)
                    {
                        return PartialView(tscore);
                    }
                return PartialView();
            }
            return RedirectToAction("Login");
        }

        //使用者登出
        //[Authorize]
        public ActionResult LogOut()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                var q = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                var logs = db.tUserLogs.Where(p => p.fId == q).OrderByDescending(p=>p.fLoginTime).FirstOrDefault();
                if (logs.fLogoutTime == null)
                {
                    //logs.fLogoutTime = DateTime.Now;
                    //db.SaveChanges();
                    Session.RemoveAll();
                }return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
