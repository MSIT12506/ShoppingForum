using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.LineChatBot.ViewModels;

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
                newScore.fScoreDate = DateTime.Now;
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

        public List<CRandomProduct> getRandomProduct()
        {
            tProduct[] products = db.tProducts.ToArray();
            int count = products.Length;
            List<CRandomProduct> results = new List<CRandomProduct>();

            Random rn = new Random();

            for (int i = 0; i < 3; i++)
            {
                int num = rn.Next(0, count - 1);

                if (i > 0 && num == results[i - 1].productId) { i--; continue; }   //避免重複的productId

                CRandomProduct newOne = new CRandomProduct()
                {
                    productId = products[num].fProductID,
                    productDesc = products[num].fProductDesc,
                    productName = products[num].fProductChName,
                    productImgUrl = $"{num+1}.jpg"
                };

                results.Add(newOne);
            }

            return results;
        }

        public string addProductToFavorite(CLineProductAdd p)
        {
            tLineBotAccountLink record = db.tLineBotAccountLinks.Where(r => r.fLineUserId == p.lineUserId).FirstOrDefault();

            if (record == null)
            {
                return "N";   //帳號尚未綁定
            }

            //確認收藏清單是否有該商品
            int productId = Convert.ToInt16(p.productId);
            int user_fid = record.fId;
            tUserProductFavorite favRecord = db.tUserProductFavorites.Where(s => s.fId == user_fid && s.fProductId == productId).FirstOrDefault();

            //沒有記錄直接新增
            if (favRecord == null)
            {
                tUserProductFavorite newFavRecord = new tUserProductFavorite();
                newFavRecord.fId = user_fid;
                newFavRecord.fProductId = productId;
                newFavRecord.fAddTime = DateTime.Now;

                db.tUserProductFavorites.Add(newFavRecord);
                db.SaveChanges();

                return "Y";
            }

            return "D";
        }

        public string addProductToShopCart(CLineProductAdd p)
        {
            tLineBotAccountLink record = db.tLineBotAccountLinks.Where(r => r.fLineUserId == p.lineUserId).FirstOrDefault();

            if (record == null)
            {
                return "N";   //帳號尚未綁定
            }

            //確認購物車是否有該商品
            int productId = Convert.ToInt16(p.productId);
            int user_fid = record.fId;
            tShoppingCart shopRecord = db.tShoppingCarts.Where(s => s.fId == user_fid && s.fProductID == productId).FirstOrDefault();

            //沒有記錄直接新增
            if (shopRecord == null)
            {
                tShoppingCart newShopRecord = new tShoppingCart();
                newShopRecord.fId = user_fid;
                newShopRecord.fProductID = productId;
                newShopRecord.fQuantity = 1;
                newShopRecord.fAddTime = DateTime.Now;

                db.tShoppingCarts.Add(newShopRecord);
                db.SaveChanges();

                return "Y";
            }

            return "D";
        }

        public bool? getDiscountCode(string lineUserId)
        {
            bool getFlag = false;    //是否有拿到優惠券

            tLineBotAccountLink record = db.tLineBotAccountLinks.Where(r => r.fLineUserId == lineUserId).FirstOrDefault();

            if (record == null)
            {
                return null;   //帳號尚未綁定
            }

            //取得會員fid
            int user_fid = record.fId;

            //確認是否有領過
            List<tDiscount> discountList = db.tDiscounts.
                                           Where(d => d.fEnable == true && d.fStartdate <= DateTime.Now && d.fEndDate > DateTime.Now && d.fCount > 0).
                                           ToList();

            //已經有的不能領取，只能領符合時間內，且未領取過的
            foreach (tDiscount item in discountList)
            {
                tUserDiscountList tUserDiscount = db.tUserDiscountLists.Where(u => u.fId == user_fid && u.fDiscountCode == item.fDiscountCode).FirstOrDefault();
                if (tUserDiscount==null)
                {
                    tUserDiscount = new tUserDiscountList() { fId = user_fid, fDiscountCode = item.fDiscountCode, fCount = 1 };
                    db.tUserDiscountLists.Add(tUserDiscount);
                    item.fCount -= 1;
                    getFlag = true;
                }
            }

            db.SaveChanges();

            return getFlag;
        }
    }
}