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

namespace tw.com.essentialoil.Controllers.FrontUser
{
    public class FrontUserProfileController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();
        Random r = new Random();
        // GET: tUserProfile
        //新增使用者
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(tUserProfile c)
        {
            var x = c.fPassword;
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var salt = new StringBuilder();
            for (var i = 0; i < 16; i++)
            {
                salt.Append(str[r.Next(0, str.Length)]);
            }
            var addsalt =x+ salt.ToString();
            //進行SHA256加密
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(addsalt);
            byte[] crypto = sha256.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);

            c.fPassword = result;
            c.fPasswordSalt = salt.ToString();
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

        //會員註冊Email帳號驗證: 最近會更新完成
        //public ActionResult UserIdVerify()
        //{

        //}

       
        //使用者登入後，修改個人資料
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
        public async Task<ActionResult> Index()
        {
            var tUserProfile = db.tUserProfiles.Include(t => t.tForumAuth).Include(t => t.tScore).Include(t => t.tUserLogs);
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
            //登入後需統一導回首頁。
            if (data != null)
            {
                tUserProfile cust = (from d in db.tUserProfiles
                                     where d.fUserId == data.fUserId
                                     select d).FirstOrDefault();
                var message = "";
                if (cust != null )
                {
                    var backpwd = data.fPassword + cust.fPasswordSalt;
                    SHA256 sha256 = new SHA256CryptoServiceProvider();
                    byte[] source = Encoding.Default.GetBytes(backpwd);
                    byte[] crypto = sha256.ComputeHash(source);
                    string result = Convert.ToBase64String(crypto);
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
                            return RedirectToAction("Index","Home");//導回首頁
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

        //忘記密碼，送出時間
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string UserName)
        {
            //找出資料庫有此使用者，將他的密碼更換，然後寄密碼給他
            var userforgot = db.tUserProfiles.Where(p => p.fUserId == UserName).Select(p => p).FirstOrDefault();
            if (userforgot != null)
            {
                var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                var newrdn = new StringBuilder();
                for (var i = 0; i < 8; i++)
                {
                    newrdn.Append(str[r.Next(0, str.Length)]);
                }

                var newsalt = new StringBuilder();
                for (var i = 0; i < 16; i++)
                {
                    newsalt.Append(str[r.Next(0, str.Length)]);
                }
                var newpwd = newrdn.ToString() + newsalt.ToString();
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] source = Encoding.Default.GetBytes(newpwd);
                byte[] crypto = sha256.ComputeHash(source);
                string result = Convert.ToBase64String(crypto);

                userforgot.fPasswordSalt = newsalt.ToString();
                userforgot.fPassword = result;
                db.SaveChanges();
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("", "ESSENCE SHOP");//管理員寄email所用的信箱，若要測試請填自己可用的email
                        var receiverEmail = new MailAddress(UserName.ToString(), "Receiver");
                        var password = "";//管理員寄email所用的信箱密碼，測試時請自行填入
                        var sub = "更換密碼通知";
                        var body = "您好，已收到您的忘記密碼請求，請用下面的新密碼重新登入:"+ newrdn.ToString()+" "+"!";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = sub,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }
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
        public ActionResult MemberCenter()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
                return View();
            return RedirectToAction("Login");
        }

        //給會員中心使用的數個PartialView
        //會員個人資料
        public ActionResult MemberDetail()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
            {
                var q = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                //var q = "CindyHu@test.con";
                var detail = db.tUserProfiles.FirstOrDefault(c => c.fUserId == q);
                return PartialView(detail);
            }
            return RedirectToAction("Login");
        }

        //會員個人密碼修改
        public ActionResult MemberPasswordEdit()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USER] != null)
                return PartialView();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult MemberPasswordEdit(string fPassword, string newPassword, string checkPassword)
        {

            var g = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
            tUserProfile cust = db.tUserProfiles.FirstOrDefault(t => t.fUserId == g);
            var pwdsalt = fPassword + cust.fPasswordSalt;


            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(pwdsalt);
            byte[] crypto = sha256.ComputeHash(source);
            string pwd = Convert.ToBase64String(crypto);
            if (cust.fPassword == pwd)
            {
                if (newPassword == checkPassword)
                {
                    var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    var newsalt = new StringBuilder();
                    for (var i = 0; i < 16; i++)
                    {
                        newsalt.Append(str[r.Next(0, str.Length)]);
                    }
                    var realpwd = newPassword + newsalt.ToString();
                    byte[] source2 = Encoding.Default.GetBytes(realpwd);
                    byte[] crypto2 = sha256.ComputeHash(source2);
                    string newpwd = Convert.ToBase64String(crypto2);


                    cust.fPasswordSalt = newsalt.ToString();
                    cust.fPassword = newpwd;
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
        //會員個人商品追蹤/收藏清單，購物車端表示目前不做

        //會員個人收藏文章，討論區將提供


        //會員積分
        public ActionResult MyScore()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                var q = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                var tscore = db.tScores.Where(p => p.fId == q).FirstOrDefault();
                if (tscore != null)
                    {
                        var score = (from i in db.tScores
                                     where i.fId == q      
                                     select i).FirstOrDefault();
                        return PartialView(score);
                    }
                return PartialView();
            }
            return RedirectToAction("Login");
        }

        //使用者登出
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
