using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Discount.ViewModels;

namespace tw.com.essentialoil.Discount.Models
{
    public class CDiscount
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        //取得所有優惠代碼
        public List<tDiscount> queryAllDiscount()
        {
            var all = from d in db.tDiscounts
                      select d;

            return all.ToList();
        }

        //新增優惠代碼
        public void craeteDiscount(CDiscountCreate[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                int moneyLimit = Convert.ToInt32(datas[i].moneyLimit);

                tDiscount newDiscount          = new tDiscount();
                newDiscount.fDiscountCode      = Guid.NewGuid().ToString();
                newDiscount.fDiscountName      = datas[i].discountName;
                newDiscount.fDiscountCategory  = datas[i].discountCategory;
                newDiscount.fDiscountMoneyRule = false;  //不再使用這個欄位
                newDiscount.fMoneyLimit        = (moneyLimit > 0) ? moneyLimit : 0;
                newDiscount.fDiscountContent   = (decimal) ((datas[i].discountCategory == "P") ? (Convert.ToInt32(datas[i].discountContent) / 100.00) : Convert.ToInt32(datas[i].discountContent));
                newDiscount.fStartdate         = DateTime.Parse(datas[i].startDate);
                newDiscount.fEndDate           = DateTime.Parse(datas[i].endDate);
                newDiscount.fEnable            = true;
                newDiscount.fCount             = Convert.ToInt32(datas[i].count);

                db.tDiscounts.Add(newDiscount);
            }

            db.SaveChanges();
        }

        //計算優惠後金額
        public string calculatePriceByDiscountCode(int userId, string discountCode, int totalMoney, ref decimal resultNum)
        {
            //檢查是否有這個優惠券代碼
            //檢查這個discode有生效 
            //檢查這個今天有在discode的起訖日範圍內 -> 假設檢查成功(fStartdate, fEndDate)
            tDiscount targetDiscount = db.tDiscounts.Where(d => d.fDiscountCode == discountCode && d.fEnable == true && d.fStartdate <= DateTime.Now && d.fEndDate >= DateTime.Now).FirstOrDefault();

            if (targetDiscount==null)
            {
                return "該優惠券目前無法使用，請至會員中心重新確認優惠券的詳細內容。";
            }

            //檢查這個userid有這個discode(數量>0)
            tUserDiscountList userDiscount = db.tUserDiscountLists.Where(d => d.fId == userId && d.fDiscountCode == discountCode).FirstOrDefault();

            if (userDiscount == null || userDiscount.fCount == 0)
            {
                return "該優惠券目前無法使用，請至會員中心重新確認優惠券的詳細內容。";
            }


            //檢查這個discode有沒有金額限制的使用條件(例如:500元以上) -> 判斷 判斷限制金額有無>0，0表示沒有限制(fMoneyLimit)
            //判斷總金額有沒有高過該discode的設定值(fMoneyLimit)
            int moneyLimit = targetDiscount.fMoneyLimit;

            if (totalMoney < moneyLimit)
            {
                return "未符合優惠券使用條件，請至會員中心確認詳細資料。";
            }

            //判斷是打折還是折固定金額(選一個discode測試)
            switch (targetDiscount.fDiscountCategory)
            {
                case "P":             //打折
                    resultNum = totalMoney * targetDiscount.fDiscountContent;
                    break;
                case "C":             //折現金
                    resultNum = (totalMoney - targetDiscount.fDiscountContent) < 0 ? 0 : totalMoney - targetDiscount.fDiscountContent;
                    break;
            }

            return "已成功使用優惠券";

        }

    }
}