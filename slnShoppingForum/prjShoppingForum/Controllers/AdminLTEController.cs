using PagedList;
using prjShoppingForum.Models.Entity;
using prjShoppingForum.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Discount.Models;
using tw.com.essentialoil.Discount.ViewModels;
using tw.com.essentialoil.Forum.Models;

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