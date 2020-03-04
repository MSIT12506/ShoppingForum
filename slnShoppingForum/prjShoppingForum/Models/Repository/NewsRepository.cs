using prjShoppingForum.Models.Entity;
using prjShoppingForum.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.News.Models
{
    public class NewsRepository
    {
        private dbShoppingForumEntities db;

        public NewsRepository()
        {
            db = new dbShoppingForumEntities();
        }

        //標題搜尋
        public IEnumerable<tNew> GetNewstitle(string searchKey)
        {
            var tNewsList = db.tNews.Where(p => p.fNewsTitle.Contains(searchKey) && p.fNewsDiscontinue != true);
            return tNewsList;
        }

        //消息清單
        public tNew GetNews(int Id)
        {
            var News = db.tNews.FirstOrDefault(p => p.fNewsId == Id);
            return News;
        }


        public void UpdateNews(tNew tNew)
        {
            var tNewsFromDb = GetNews(tNew.fNewsId);

            tNewsFromDb.fClass = tNew.fClass;
            tNewsFromDb.fNewsTitle = tNew.fNewsTitle;
            tNewsFromDb.fNewsArticle = tNew.fNewsArticle;
            tNewsFromDb.fNewsDesc = tNew.fNewsDesc;
            tNewsFromDb.fNewsStart = tNew.fNewsStart;
            tNewsFromDb.fNewsEnd = tNew.fNewsEnd;
            tNewsFromDb.fChangUser = tNew.fChangUser;
            tNewsFromDb.fApproved = tNew.fApproved;
            tNewsFromDb.fNewsDiscontinue = tNew.fNewsDiscontinue;

 //         fNewsId,,fAddUser,fDeleteUser,fApproved,fNewsTag,fGet_No
            db.SaveChanges();
        }

        //直接刪
        public void RemoveNews(int Id)
        {
            var News = GetNews(Id);
            db.tNews.Remove(News);
            db.SaveChanges();
        }

        public void EditNewsToDiscontinue(int Id)
        {
            var News = GetNews(Id);
            News.fNewsDiscontinue = true;
            db.SaveChanges();
        }

        public void InsertNews(tNew tNew)
        {

            tNew.fNewsDiscontinue = false;
            tNew.fNewsTag = 0;
            tNew.fGet_No = 0;
            tNew.fChangUser = "No";
            tNew.fDeleteUser = "No";
            db.tNews.Add(tNew);
            try
            {
                db.SaveChanges();

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public tUserProfile GetUser(int fId)
        {
            var Account = db.tUserProfiles.FirstOrDefault(p => p.fId == fId);
            return Account ;
        }


        //點閱 tag
        //public void Tag(int fNewsId, int addnum, int fId)
        //{
        //    var News = GetNews(fNewsId);
        //    var Account = db.tUserProfiles.FirstOrDefault(p => p.fId == fId);

        //    if (!db.tNews.Any(p => p.Accound == fId.ToString() && p.fNewsId == fNewsId))
        //    {
        //        var newsLog = new tNewsTag() { Account = fId.ToString(), fNewsId = fNewsId };
        //        News.fNewsTag = News.fNewsTag + addnum;
        //        db.tNewsTags.Add(newsLog);
        //        db.SaveChanges();
        //    }
        //}


    }
}