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

namespace tw.com.essentialoil.Controllers
{
    public class AdminLTEController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();
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
        public ActionResult getReportData(int topNumber = 5)
        {
            var data = (from o in db.tOrderDetails
                        join p in db.tProducts on o.fProductId equals p.fProductID
                        group o by o.tProduct.fProductChName into g
                        select new
                        {
                            name = g.Key,                        //該項商品Id
                            y = g.Sum(p => p.fOrderQuantity)     //該項商品營銷總數量
                        }).OrderByDescending(q => q.y).Take(topNumber).ToList();

            return Json(data);
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

        //會員管理、會員查詢

        int pagesize = 10;
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

        //後台討論區 - 文章停權/恢復權限列表
        public ActionResult PostListAll(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageCount = 15;

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
    }
}