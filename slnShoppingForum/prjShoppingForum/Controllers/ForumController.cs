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

namespace tw.com.essentialoil.Controllers
{
    public class ForumController : Controller
    {
        //所有文章列表
        public ActionResult List()
        {
            //一進入Action就先取出當下時間
            ViewBag.DateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            CForum forum = new CForum();
            
            return View(forum.queryAllPost());
        }

        //呈現文章的內容
        public ActionResult PostView(int fPostId) {
            
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
            Session[CDictionary.UPDATE_FORUM_ID] = fPostId;

            CForum forum = new CForum();
            tForum tForum = forum.queryPostById(fPostId);

            if (tForum != null) return View(tForum);

            return RedirectToAction("List");
        }

        //修改文章內容[POST]
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(CForumCreate vm)
        {
            if (Session[CDictionary.UPDATE_FORUM_ID] != null)
            {
                CForum forum = new CForum();
                forum.updatePostById(Session[CDictionary.UPDATE_FORUM_ID], vm);
            }

            return RedirectToAction("List");

        }

        //刪除文章
        public ActionResult Delete(int fPostId)
        {
            CForum forum = new CForum();
            forum.deletePostById(fPostId);
            
            return RedirectToAction("List");
        }


        //----------------------------Ajax----------------------------
        //新增文章
        public ActionResult Create(string title, string content)
        {
            //TODO
            //從Session讀取資料
            //判斷是否有登入，如果有登入，取得該會員的fId

            string status = "error";
            if (!String.IsNullOrWhiteSpace(title))
            {
                CForum forum = new CForum();
                forum.newPost(5, title, content);
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

            //利用postIdList區分是更新的文章還是新增的文章
            foreach (tForum post in forums)
            {
                if ( post.fPostId > lastPostId )
                {
                    var newPost = new
                    {
                        title = post.fPostTitle,      //文章標題
                        postId = post.fPostId         //文章編號
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

            //TODO
            //從Session讀取資料
            //判斷是否有登入，如果有登入，取得該會員的fId

            CReply reply = new CReply();
            if (replyInfo.targetType == "POST") reply.NewCommentForPost(replyInfo, 5);
            if (replyInfo.targetType == "COMMENT") reply.NewCommentForComment(replyInfo, 5);

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
                        replyContent = item.fContent
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

    }
}