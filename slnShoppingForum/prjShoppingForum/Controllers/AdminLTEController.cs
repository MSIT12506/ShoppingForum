using prjShoppingForum.Models.Entity;
using prjShoppingForum.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                    var backpwd = data.ManagerPassword /*+ cust.ManagerPasswordSalt*/;
                    //SHA256 sha256 = new SHA256CryptoServiceProvider();
                    //byte[] source = Encoding.Default.GetBytes(backpwd);
                    //byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                    //string result = Convert.ToBase64String(crypto);
                    if (cust.ManagerPassword == backpwd)
                    {
                            //使用下面Session的值判斷是否使用者已登入
                            Session[CAdminData.S_CURRENT_LOGINED_ADMIN] = cust.ManagerId;//存fUserId
                            return RedirectToAction("Dashboard");//登入成功進到儀表板頁面
                    }
                    return View();
                }
                return View();//TODO:會再修改，目前尚不影響流程
            }
            return View();
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



    }
}