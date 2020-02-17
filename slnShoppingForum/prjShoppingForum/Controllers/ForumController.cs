//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//------------------------------------------//

using tw.com.essentialoil.Forum.Models;
using tw.com.essentialoil.Forum.ViewModels;
using tw.com.essentialoil.Models;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers
{
    public class ForumController : Controller
    {
        //所有文章列表
        public ActionResult List()
        {
            //user - fid
            int id = 0;

            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session,"Forum","List"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                id = userLoginInfo.user_fid;
            }

            //一進入Action就先取出當下時間
            ViewBag.DateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //紀錄session-id
            ViewBag.sessionId = id;
            CForum forum = new CForum();
            return View(forum.queryAllPost(id));
        }

        //置頂文章列表
        public ActionResult TopList()
        {
            CForum forum = new CForum();

            return PartialView(forum.queryTopPost());
        }

        //呈現文章的內容
        public ActionResult PostView(int fPostId) {

            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "Forum", "List"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }

            //一進入Action就先取出當下時間
            ViewBag.DateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            CForum forum = new CForum();
            tForum tForum = forum.queryPostById(fPostId);

            CReply reply = new CReply();
            List<List<tForumReply>> replys = reply.getReplysById(fPostId);

            CPostView postview = new CPostView { forum = tForum, reply = replys };
            
            if (postview.forum != null)
            {
                string test = postview.forum.fPostContent;
                return View(postview);
            }

            
            return RedirectToAction("List");
        }

        //修改文章內容
        public ActionResult Edit(int fPostId)
        {
            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "Forum", "List"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }

            CForum forum = new CForum();
            tForum tForum = forum.queryPostById(fPostId);

            if (tForum != null) return View(tForum);

            return RedirectToAction("List");
        }

        //修改文章內容[POST]
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(string postId, string title, string content)
        {
            CForum forum = new CForum();
            forum.updatePostById(postId, title, content);

            return RedirectToAction("List");

        }

        //收藏/隱藏文章列表
        public ActionResult SelfList()
        {
            //user - fid
            int id = 0;

            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "Forum", "SelfList"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                id = userLoginInfo.user_fid;
            }

            //一進入Action就先取出當下時間
            ViewBag.DateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //紀錄session-id
            ViewBag.sessionId = id;
            CForum forum = new CForum();
            return View(forum.querySelfPost(id));
        }

        //刪除文章
        public ActionResult Delete(int fPostId)
        {
            CForum forum = new CForum();
            forum.deletePostById(fPostId);

            return Content("success");
        }

        //搜尋文章功能
        public ActionResult SearchPost(string searchText)
        {
            CForum forum = new CForum();
            if (searchText != null)
            {
                List<tForum> forums = forum.searchString(searchText.Trim());
                return View(forums);
            }

            return RedirectToAction("List");
        }


        //----------------------------Ajax----------------------------
        //新增文章
        public ActionResult Create(string title, string content)
        {
            //從Session讀取資料
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;

            string status = "error";
            if (!String.IsNullOrWhiteSpace(title))
            {
                CForum forum = new CForum();
                forum.newPost(userLoginInfo.user_fid, title, content);
                status = "success";
            }

            //回傳狀態
            return Content(status);
        }

        //定時更新文章List
        public ActionResult RefreshList(int lastPostId, string prevDtaetime) {

            //一進入Action就先取出當下時間
            string newTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //撈出更新時間在prevDateTime之後的所有文章
            DateTime targetTime = DateTime.ParseExact(prevDtaetime, "yyyyMMddHHmmssfff", CultureInfo.CurrentCulture);

            CForum forum = new CForum();
            List<tForum> forums = forum.queryPostByTime(targetTime);
            List<tForum> delForums = forum.queryPostByDelTime(targetTime);

            List<object> newForums = new List<object>();
            List<object> updateForums = new List<object>();
            List<object> deleteForums = new List<object>();

            //取得session資訊
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;

            //利用postIdList區分是更新的文章還是新增的文章
            foreach (tForum post in forums)
            {
                if ( post.fPostId > lastPostId )
                {
                    var newPost = new
                    {
                        title = post.fPostTitle,      //文章標題
                        postId = post.fPostId,         //文章編號
                        editable = (post.tUserProfile.fId == userLoginInfo.user_fid).ToString()
                    };

                    newForums.Add(newPost);
                }
                else
                {
                    var updatePost = new
                    {
                        title = post.fPostTitle,      //文章標題
                        postId = post.fPostId         //文章編號
                    };

                    updateForums.Add(updatePost);
                }
            }

            //取得所有刪除的文章編號
            foreach (tForum post in delForums)
            {
                var deletePost = new
                {
                    postId = post.fPostId             //文章編號
                };

                deleteForums.Add(deletePost);
            }

            //定義回傳json
            if (forums.Count > 0)
            {
                return Json(
                    new
                    {
                        newTime = newTime,
                        newForums = newForums,
                        updateForums = updateForums,
                        deleteForums = deleteForums
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(
                    new
                    {
                        newTime = newTime,
                        newForums = newForums,
                        updateForums = updateForums,
                        deleteForums = deleteForums
                    }, JsonRequestBehavior.AllowGet);
            }

        }

        //回覆文章 / 回覆回覆
        public ActionResult Reply(CNewReplyCreate replyInfo) {
            string status = "";

            //從Session讀取資料
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;

            CReply reply = new CReply();
            if (replyInfo.targetType == "POST") reply.NewCommentForPost(replyInfo, userLoginInfo.user_fid);
            if (replyInfo.targetType == "COMMENT") reply.NewCommentForComment(replyInfo, userLoginInfo.user_fid);

            return Content(status);
        }

        //定時更新留言List
        public ActionResult RefreshReplyList(int lastPostId, string prevDtaetime)
        {

            //一進入Action就先取出當下時間
            string newTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //撈出更新時間在prevDateTime之後的所有留言
            DateTime targetTime = DateTime.ParseExact(prevDtaetime, "yyyyMMddHHmmssfff",CultureInfo.CurrentCulture);

            CReply reply = new CReply();
            List<tForumReply> replys = reply.getNewReplysByTime(lastPostId, targetTime);

            List<object> newReplyList = new List<object>();

            if (replys.Count > 0)
            {
                foreach (var item in replys)
                {
                    var newReply = new
                    {
                        replyId = item.fReplyId,               //自己的ID
                        replyTargetId = item.fReplyTargetId,   //回覆對象的ID
                        replySeqNo = item.fReplySeqNo,
                        replyContent = item.fContent,
                        replyName = item.tUserProfile.fName,
                        replyDate = item.fReplyTime.ToLongDateString(),
                        replyTime = item.fReplyTime.ToLongTimeString()
                    };

                    newReplyList.Add(newReply);
                }
            }

            return Json(
                new
                {
                    newTime = newTime,
                    newReplyList = newReplyList
                }, JsonRequestBehavior.AllowGet);

        }

        //新增文章到收藏清單
        public ActionResult FavirotePost(string pid)
        {
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;

            CForum forum = new CForum();
            int postId = Convert.ToInt32(pid);
            forum.addFavirotePost(userLoginInfo.user_fid, postId);

            return Content("");
        }

        //新增文章到隱藏清單
        public ActionResult HidePost(string pid)
        {
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;

            CForum forum = new CForum();
            int postId = Convert.ToInt32(pid);
            forum.addHidePost(userLoginInfo.user_fid, postId);

            return Content("");
        }

        //移除收藏/隱藏清單的文章
        public ActionResult RemovePost(string pid)
        {
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;

            CForum forum = new CForum();
            int postId = Convert.ToInt32(pid);
            forum.removePost(userLoginInfo.user_fid, postId);

            return Content("");
        }


    }
}