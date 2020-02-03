using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.Discount.Models
{
    public class CDiscount
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        //get all discount code
        public List<tDiscount> queryAllDiscount()
        {
            var all = from d in db.tDiscounts
                      select d;

            return all.ToList();
        }

        //Create Discount Code
        public void craeteDiscount(tDiscount data)
        {
            tDiscount newdiscount = new tDiscount();
            newdiscount.fDiscountCode      = Guid.NewGuid().ToString();
            newdiscount.fDiscountName      = data.fDiscountName;
            newdiscount.fDiscountCategory  = data.fDiscountCategory;
            newdiscount.fDiscountMoneyRule = data.fDiscountMoneyRule;
            newdiscount.fMoneyLimit        = data.fMoneyLimit;
            newdiscount.fDiscountContent   = data.fDiscountContent;
            newdiscount.fStartdate         = data.fStartdate;
            newdiscount.fEndDate           = data.fEndDate;
            newdiscount.fEnable            = data.fEnable;

            db.tDiscounts.Add(newdiscount);
            db.SaveChanges();

        }

        //TODO
        //Calculate the Order Price By Discount Code
        //1. products - 由購物車或是訂單傳過來的商品清單
        //2. counts  - 由購物車或訂單傳過來的商品數量清單
        //3. userid  - 由購物車或訂單傳過來的訂購會員id
        //4. discode - 由購物車或訂單傳過來的優惠代碼

        //這4個參數可以透過建立一個新的model來取代，為了測試方便，先把有參數的拿掉
        //回傳金額，如果 <0，表示沒有符合條件
        //public decimal? calculatePriceByDiscountCode(List<tProduct> products, List<int> counts ,string userid, string discode)
        public decimal? calculatePriceByDiscountCode()
        {
            decimal? result = -1;  //預設不符合條件

            //假裝訂單有傳資料給這個method，且 products & counts，順序是可以正確mapping的
            //執行完測試資料匯入，寫死測試資料
            tProduct product1 = db.tProducts.Where(p => p.fProductID == 1).ToList()[0];
            tProduct product2 = db.tProducts.Where(p => p.fProductID == 2).ToList()[0];
            tProduct product3 = db.tProducts.Where(p => p.fProductID == 3).ToList()[0];
            tProduct product4 = db.tProducts.Where(p => p.fProductID == 4).ToList()[0];
            tProduct product5 = db.tProducts.Where(p => p.fProductID == 5).ToList()[0];
            tProduct product6 = db.tProducts.Where(p => p.fProductID == 6).ToList()[0];

            int cnt1 = 1;
            int cnt2 = 2;
            int cnt3 = 1;
            int cnt4 = 2;
            int cnt5 = 1;
            int cnt6 = 2;

            //測試先寫死兩種優惠種類(這邊要依照不同環境，先執行sql產生這兩組discode)
            tDiscount discode1 = db.tDiscounts.Where(p => p.fDiscountCode == "dfacd486-6652-4e4b-9588-b7f176b8f45f").ToList()[0];
            tDiscount discode2 = db.tDiscounts.Where(p => p.fDiscountCode == "059879aa-285c-41f1-baa9-cf0c85286bff").ToList()[0];

            //檢查這個userid有這個discode(數量>0) -> 假設檢查成功(tUserDiscountList)
            // ....................

            //檢查這個discode有生效 -> 假設檢查成功(fEnable)
            // ....................

            //檢查這個今天有在discode的起訖日範圍內 -> 假設檢查成功(fStartdate, fEndDate)
            // ....................

            //檢查這個discode有沒有金額限制的使用條件(例如:500元以上) -> 判斷 fDiscountMoneyRule TRUE or FALSE
            // ....................

            //計算回傳優惠後的金額
            //1.總金額
            int? totalPrice = product1.fUnitPrice * cnt1 +
                              product2.fUnitPrice * cnt2 +
                              product3.fUnitPrice * cnt3 +
                              product4.fUnitPrice * cnt4 +
                              product5.fUnitPrice * cnt5 +
                              product6.fUnitPrice * cnt6;

            //如果這個discode有金額限制的使用條件 -> 判斷總金額有沒有高過該discode的設定值(fMoneyLimit)
            // ....................

            //2.判斷是打折還是折固定金額(選一個discode測試)
            switch (discode2.fDiscountCategory)
            {
                case "P":             //打折
                    result = totalPrice * discode2.fDiscountContent;
                    break;
                case "C":             //折現金
                    result = totalPrice - discode2.fDiscountContent;
                    break;
            }

            return result;

        }
    }
}