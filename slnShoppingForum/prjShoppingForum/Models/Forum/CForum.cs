﻿using prjShoppingForum.Models.Entity;
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
        public List<CForumList> queryAllPost(int id)
        {
            //確認是否有收藏/隱藏的文章，如果有則不顯示(by user)
            List<int> hates = (from p in db.tUserFavorites
                               where p.fId == id
                               select p.fPostId).ToList();

            List<CForumList> result = (from i in db.tForums
                                       where i.fEnableFlag == true       //刪除的不要被select出來
                                       && i.fTopSeq >= 999               //置頂文章不要呈現
                                       orderby i.fCreateTime descending  //新到舊作為預設排序(文章建立時間)
                                       select new CForumList() { postId = i.fPostId, postTitle = i.fPostTitle, userFid = i.tUserProfile.fId, likeOrHate = null }).ToList();

            foreach (int item in hates)
            {
                result.RemoveAll(r => r.postId == item);
            }

            //判斷是否有喜歡或是討厭的紀錄
            foreach (var item in result)
            {
                bool? isLikeHate = db.tForumAnalysis.Where(p => p.fPostId == item.postId && p.fId == item.userFid).Select(p => p.fLikeHate).FirstOrDefault();

                if (isLikeHate != null)
                {
                    item.likeOrHate = isLikeHate.ToString();
                }
            }

            return result;
        }

        public List<tForum> queryTopPost()
        {
            List<tForum> topLists = (from f in db.tForums
                                     where f.fEnableFlag == true
                                     &&    f.fTopSeq < 999
                                     orderby f.fTopSeq ascending
                                     select f).ToList();

            return topLists;
        }

        //Select Post by Id
        public tForum queryPostById(int fPostId)
        {
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
        public void updatePostById(string postId, string title, string content)
        {
            int fId = Convert.ToInt32(postId);

            tForum result = (from i in db.tForums
                             where i.fPostId == fId
                             select i).FirstOrDefault();

            if (result!=null)
            {
                result.fPostTitle = title;
                result.fPostContent = content;
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

        public object querySelfPost(int id)
        {
            //確認是否有收藏/隱藏的文章，全部顯示(by user)
            List<int> hates = (from p in db.tUserFavorites
                               where p.fId == id && p.fIsFavorite == false
                               select p.fPostId).ToList();

            List<int> favs = (from p in db.tUserFavorites
                               where p.fId == id && p.fIsFavorite == true
                               select p.fPostId).ToList();

            List<List<tForum>> results = new List<List<tForum>>();
            results.Add(new List<tForum>());   //hate
            results.Add(new List<tForum>());   //fav

            foreach (int item in hates)
            {
                tForum record = db.tForums.Where(f => f.fPostId == item).FirstOrDefault();
                results[0].Add(record);
            }

            foreach (int item in favs)
            {
                tForum record = db.tForums.Where(f => f.fPostId == item).FirstOrDefault();
                results[1].Add(record);
            }

            return results;
        }

        public void addFavirotePost(int userId, int postId)
        {
            //檢查是否有把該文章加到隱藏，或是已經加入收藏的紀錄
            tUserFavorite checkRecord = db.tUserFavorites.Where(uf => uf.fId == userId && uf.fPostId == postId).FirstOrDefault();

            if (checkRecord == null)
            {
                tUserFavorite userFavorite = new tUserFavorite();
                userFavorite.fId = userId;
                userFavorite.fPostId = postId;
                userFavorite.fIsFavorite = true;
                db.tUserFavorites.Add(userFavorite);
                db.SaveChanges();
            }
            else
            {
                checkRecord.fIsFavorite = true;
                db.SaveChanges();
            }
        }

        public void addHidePost(int userId, int postId)
        {
            //檢查是否有把該文章加到收藏，或是已經加入隱藏的紀錄
            tUserFavorite checkRecord = db.tUserFavorites.Where(uf => uf.fId == userId && uf.fPostId == postId).FirstOrDefault();

            if (checkRecord == null)
            {
                tUserFavorite userFavorite = new tUserFavorite();
                userFavorite.fId = userId;
                userFavorite.fPostId = postId;
                userFavorite.fIsFavorite = false;
                db.tUserFavorites.Add(userFavorite);
                db.SaveChanges();
            }
            else
            {
                checkRecord.fIsFavorite = false;
                db.SaveChanges();
            }
        }

        public void removePost(int userId, int postId)
        {
            tUserFavorite checkRecord = db.tUserFavorites.Where(uf => uf.fId == userId && uf.fPostId == postId).FirstOrDefault();

            if (checkRecord != null)
            {
                db.tUserFavorites.Remove(checkRecord);
                db.SaveChanges();
            }
            
        }

        public List<tForum> searchString(string searchText)
        {
            List<tForum> results = db.tForums.Where(f => f.fPostTitle.Contains(searchText)).ToList();

            return results;
        }

        public void addLikeOrHateCount(int userId, int postId, bool result)
        {
            //檢查是否已經有討厭或是喜歡的紀錄
            tForumAnalysi checkRecord = db.tForumAnalysis.Where(p => p.fId == userId && p.fPostId == postId ).FirstOrDefault();

            if (checkRecord == null)
            {
                tForumAnalysi addLikeRecord = new tForumAnalysi();
                addLikeRecord.fId = userId;
                addLikeRecord.fPostId = postId;
                addLikeRecord.fLikeHate = result;
                db.tForumAnalysis.Add(addLikeRecord);
                db.SaveChanges();
            }
            else
            {
                checkRecord.fLikeHate = result;
                db.SaveChanges();
            }


        }
    }
}