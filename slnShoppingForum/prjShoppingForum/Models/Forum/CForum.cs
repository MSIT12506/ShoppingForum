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
        //Create New Post
        public void newPost(CForumCreate c)
        {
            //Html Encode
            //string afterEncoding = HttpUtility.HtmlEncode(c.tmpContent);

            dbShoppingForumEntities db = new dbShoppingForumEntities();
            
            tForum newForumRecord = new tForum();
            newForumRecord.fId = 1;                    //TODO-要動態取得
            newForumRecord.fIsPost = true;
            newForumRecord.fCreateTime = DateTime.Now;
            newForumRecord.fUpdateTime = DateTime.Now;
            newForumRecord.fEnableFlag = true;
            newForumRecord.fTopSeq = 999;
            newForumRecord.fTotalReplyCount = 0;
            newForumRecord.fTotalViewCount = 0;
            newForumRecord.fPostContent = c.tmpContent;
            newForumRecord.fPostTitle = c.postTitle;

            db.tForums.Add(newForumRecord);
            
            db.SaveChanges();
        }

        //Select All Post
        public IQueryable<tForum> queryAllPost()
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();

            IQueryable<tForum> result = from i in db.tForums
                                        where i.fEnableFlag == true       //刪除的不要被select出來
                                        select i;

            return result;
        }

        //Select Post by Id
        public tForum queryPostById(int fPostId)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();

            tForum result = (from i in db.tForums
                          where i.fPostId == fPostId
                          select i).FirstOrDefault();

            return result;
        }

        public void updatePostById(object fPostId, CForumCreate data)
        {
            int fId = Convert.ToInt32(fPostId);
            dbShoppingForumEntities db = new dbShoppingForumEntities();

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

        public void deletePostById(int fPostId)
        {
            dbShoppingForumEntities db = new dbShoppingForumEntities();

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