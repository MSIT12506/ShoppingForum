using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjShoppingForum.Models.WebAPI
{
    public class CLineBot
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        public bool? setAccountLink(string lineNonce, string lineUserId)
        {
            tLineBotAccountLink record = db.tLineBotAccountLinks.Where(r => r.fLineNonce == lineNonce).FirstOrDefault();
            tLineBotAccountLink existRecord = db.tLineBotAccountLinks.Where(r => r.fLineUserId == lineUserId).FirstOrDefault();

            //lineuserid已經綁定過
            if (existRecord !=null)
            {
                return false;
            }

            if (record != null)
            {
                record.fLineUserId = lineUserId;
                record.fAccountLinkDatetime = DateTime.Now;
                db.SaveChanges();
                return true;
            }

            return null;
        }

        public bool CheckLineUserId(string lineUserId)
        {
            tLineBotAccountLink record = db.tLineBotAccountLinks.Where(r => r.fLineUserId == lineUserId).FirstOrDefault();

            return (record == null) ? false : true;
            
            //false：表示沒有綁定過
            //true：表示有綁定過
        }

        public string checkLoginDayByDay(string lineUserId, ref int score)
        {
            tLineBotAccountLink record = db.tLineBotAccountLinks.Where(r => r.fLineUserId == lineUserId).FirstOrDefault();

            if (record==null)
            {
                return "N";   //帳號尚未綁定
            }

            tScore scoreRecord = db.tScores.Where(p => p.fId == record.fId).FirstOrDefault();

            //沒有記錄直接新增並且獲得積分
            if (scoreRecord==null)
            {
                tScore newScore = new tScore();
                newScore.fId = record.fId;
                newScore.fScore = 10;             //獲得10點積分
                score = 10;
                db.tScores.Add(newScore);
                record.fAccountLinkDatetime = DateTime.Now;    //更新時間
                db.SaveChanges();
                return "Y";
            }

            //超過一天
            //月份不一樣，或是日期不一樣，就是相差一天以上
            if (record.fAccountLinkDatetime.Value.Month != DateTime.Now.Month || record.fAccountLinkDatetime.Value.Day != DateTime.Now.Day)
            {
                scoreRecord.fScore += 10;
                score = (Int16)scoreRecord.fScore;
                record.fAccountLinkDatetime = DateTime.Now;
                db.SaveChanges();

                return "Y";
            }

            return "D";
        }
    }
}