using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Forum.ViewModels;

namespace tw.com.essentialoil.Forum.Models
{
    public class CForum
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        //Create New Post
        public void newPost(int userId, string title, string content)
        {
            tForum newForumRecord = new tForum();
            newForumRecord.fId = userId;
            newForumRecord.fPostTitle = title;
            newForumRecord.fPostContent = content;
            newForumRecord.fIsPost = true;
            newForumRecord.fCreateTime = DateTime.Now;
            newForumRecord.fUpdateTime = DateTime.Now;
            newForumRecord.fEnableFlag = true;
            newForumRecord.fTopSeq = 999;
            newForumRecord.fTotalReplyCount = 0;
            newForumRecord.fTotalViewCount = 0;

            db.tForums.Add(newForumRecord);
            
            db.SaveChanges();
        }

        //Select All Post
        public IQueryable<tForum> queryAllPost()
        {
            IQueryable<tForum> result = from i in db.tForums
                                        where i.fEnableFlag == true       //刪除的不要被select出來
                                        select i;

            return result;
        }

        //Select Post by Id
        public tForum queryPostById(int fPostId)
        {
            //TODO - 補上權限控制
            tForum result = (from i in db.tForums
                          where i.fPostId == fPostId
                          select i).FirstOrDefault();

            return result;
        }

        //Select Post By Time
        public List<tForum> queryPostByTime(DateTime prevDateTime) {
            var results = from p in db.tForums
                          where (p.fUpdateTime > prevDateTime) && (p.fEnableFlag == true)
                          select p;

            return results.ToList();
        }

        //Select 【Disable】 Post By Time
        public List<tForum> queryPostByDelTime(DateTime prevDateTime)
        {
            var results = from p in db.tForums
                          where (p.fDisableTime > prevDateTime) && (p.fEnableFlag == false)
                          select p;

            return results.ToList();
        }

        //Update Post By Id
        public void updatePostById(object fPostId, CForumCreate data)
        {
            int fId = Convert.ToInt32(fPostId);

            tForum result = (from i in db.tForums
                             where i.fPostId == fId
                             select i).FirstOrDefault();

            if (result!=null)
            {
                result.fPostTitle = data.postTitle;
                result.fPostContent = data.tmpContent;
                result.fUpdateTime = DateTime.Now;

                db.SaveChanges();
            }

        }

        //Delete Post By Id
        public void deletePostById(int fPostId)
        {
            tForum result = (from i in db.tForums
                             where i.fPostId == fPostId && i.fEnableFlag == true
                             select i).FirstOrDefault();

            if (result != null)
            {
                result.fEnableFlag = false;
                result.fEnableUserId = 1;    //TODO - 要動態產生
                result.fDisableTime = DateTime.Now;

                db.SaveChanges();
                
            }

        }

    }
}