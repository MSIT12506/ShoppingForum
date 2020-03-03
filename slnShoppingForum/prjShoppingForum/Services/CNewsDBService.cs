using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Services
{
    public class CNewsDBService
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();


        public List<tNew> GetDataList(ForPaging Paging, string Search, string Account)
        {
            List<tNew> DataList = new List<tNew>();
            if (!string.IsNullOrWhiteSpace(Search))
            {
                //有搜尋條件時
                SetMaxPaging(Paging, Search, Account);
                DataList = GetAllDataList(Paging, Search, Account);
            }
            else
            {
                //無搜尋條件時
                SetMaxPaging(Paging, Account);
                DataList = GetAllDataList(Paging, Account);
            }
            return DataList;
        }
        //無搜尋最大頁數
        public void SetMaxPaging(ForPaging Paging, string Account)
        {
            int Row = 0;
            var q = from i in db.tNews
                    where i.fAddUser == Account
                    select i.ToString();
            Row = q.Count();
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Row) / Paging.ItemNum));
            Paging.SetRightPage();
        }
        //有搜尋最大頁數
        public void SetMaxPaging(ForPaging Paging,string Search , string Account)
        {
            int Row = 0;
            var q = from i in db.tNews
                    where i.fAddUser == Account
                    select i.ToString();
            Row = q.Count();
            Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Row) / Paging.ItemNum));
            Paging.SetRightPage();

        }
        //無搜尋條件時方法
        public List<tNew> GetAllDataList(ForPaging paging,string Account)
        {
            //宣告回傳資料為DBtNews
            List<tNew> DataList = new List<tNew>();
            var q = from i in db.tNews
                    select i;
            return DataList;
        }
        public List<tNew> GetAllDataList(ForPaging Paging,string Search,string Account)
        {
            List<tNew> DataList = new List<tNew>();
            var q = from i in db.tNews
                    select i;
            return DataList;
        }

        //新增資料
        //public void updatePostById(object fNewsId, CNewsArticle newData)
        //{
        //    int fId = Convert.ToInt32(fNewsId);
        //    dbShoppingForumEntities db = new dbShoppingForumEntities();
        //    newData.Account = LastfNewsIdFinder();
        //    tNews result = (from i in db.tNews
        //                     where i.fNewsId == fId
        //                     select i).FirstOrDefault();

        //    if (result != null)
        //    {

        //        result.fNewsStart = DateTime.UtcNow.AddHours(8);

        //        db.SaveChanges();
        //    }

        //}

        //private string LastfNewsIdFinder()
        //{
        //    throw new NotImplementedException();
        //}
    }
}