//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Linq;
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
            CForum forum = new CForum();
            
            return View(forum.queryAllPost());
        }

        //選取文章的內容
        public ActionResult PostView(int fPostId) {
            CForum forum = new CForum();
            tForum tForum = forum.queryPostById(fPostId);

            CReply reply = new CReply();
            List<List<tForumReply>> replys = reply.getReplysById(fPostId);

            CPostView postview = new CPostView { forum = tForum, reply = replys };

            if (postview.forum != null) return View(postview);

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

        //新增文章
        public ActionResult Create()
        {
            return View();
        }

        //新增文章[POST]
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Create(CForumCreate vm)
        {
            CForum forum = new CForum();
            forum.newPost(vm);
            return RedirectToAction("List");
        }

        //刪除文章
        public ActionResult Delete(int fPostId)
        {
            CForum forum = new CForum();
            forum.deletePostById(fPostId);
            
            return RedirectToAction("List");
        }

        //--------------------------Reply--------------------------
        //回覆 文章/回覆
        [HttpPost]
        public ActionResult PostView(CNewReplyCreate vm)
        {
            CReply reply = new CReply();
            if (vm.tmpReplyType == "POST") reply.NewCommentForPost(vm);
            if (vm.tmpReplyType == "COMMENT") reply.NewCommentForComment(vm);

            //return RedirectToAction("PostView", new RouteValueDictionary(new { controller = "Forum", action = "PostView", fPostId = fPostId }));
            return RedirectToAction("List");
        }
    }
}