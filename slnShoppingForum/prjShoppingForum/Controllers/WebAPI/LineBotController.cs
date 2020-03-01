using prjShoppingForum.Models.Entity;
using prjShoppingForum.Models.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using tw.com.essentialoil.LineChatBot.ViewModels;

namespace tw.com.essentialoil.Controllers.WebAPI
{
    public class LineBotController : ApiController
    {
        [HttpPost]
        //帳號綁定
        public object AccountConnect(CLineAccountLink link)
        {
            //將LineUserId寫入資料庫
            CLineBot lineBot = new CLineBot();
            bool? result = lineBot.setAccountLink(link.lineNonce, link.lineUserId);

            return new { result };
        }

        [HttpPost]
        //確認是否有綁定過
        public bool CheckLineUserIdConnectedOrNot(CLineUserId t)
        {
            //false：表示沒有綁定過
            //true：表示有綁定過
            CLineBot lineBot = new CLineBot();
            return (lineBot.CheckLineUserId(t.lineUserId));
        }

        [HttpPost]
        //每日報到
        public object CheckLoginDayByDay(CLineUserId t)
        {
            CLineBot lineBot = new CLineBot();
            int score = 0;
            string result = lineBot.checkLoginDayByDay(t.lineUserId, ref score);

            switch (result)
            {
                case "N":
                    return new { returnValue = "帳號尚未綁定，會員綁定後才可進行每日報到!" };
                case "Y":
                    return new { returnValue = $"您今日已完成報到，目前累積積分為 {score} 點!" };
                case "D":
                    return new { returnValue = "每天只能報到一次啦!!!!" };
                default:
                    return new { returnValue = "發生異常!!! 發生異常!!!" };
            }
            
        }

        [HttpPost]
        //獲得優惠券
        public object GetDiscountCode(CLineUserId t)
        {
            CLineBot lineBot = new CLineBot();
            bool? result = lineBot.getDiscountCode(t.lineUserId);

            switch (result)
            {
                case null:
                    return new { returnValue = "帳號尚未綁定，會員綁定後才可進行優惠券領取!" };
                case true:
                    return new { returnValue = "您已領取優惠券，請至會員中心查看" };
                case false:
                    return new { returnValue = "目前無可領取的優惠券喔" };
                default:
                    return new { returnValue = "發生異常!!! 發生異常!!!" };
            }

        }

        [HttpGet]
        //隨機推薦商品
        public List<CRandomProduct> RandomProduct()
        {
            CLineBot lineBot = new CLineBot();
            List<CRandomProduct> results =  lineBot.getRandomProduct();
            return results;
        }

        [HttpPost]
        //加入購物車
        public object ProductToShopCart(CLineProductAdd p)
        {
            CLineBot lineBot = new CLineBot();
            string result = lineBot.addProductToShopCart(p);

            switch (result)
            {
                case "N":
                    return new { returnValue = "帳號尚未綁定，會員綁定後才可進行每日報到!" };
                case "Y":
                    return new { returnValue = "您已將該商品加入購物車，請趕快至網站結帳付錢!!!!" };
                case "D":
                    return new { returnValue = "該商品目前已經在購物車內囉" };
                default:
                    return new { returnValue = "發生異常!!! 發生異常!!!" };
            }
        }

        [HttpPost]
        //加入收藏清單
        public object ProductToFavorite(CLineProductAdd p)
        {
            CLineBot lineBot = new CLineBot();
            string result = lineBot.addProductToFavorite(p);

            switch (result)
            {
                case "N":
                    return new { returnValue = "帳號尚未綁定，會員綁定後才可進行每日報到!" };
                case "Y":
                    return new { returnValue = "您已將該商品加入收藏清單!" };
                case "D":
                    return new { returnValue = "該商品目前已經在收藏清單內囉" };
                default:
                    return new { returnValue = "發生異常!!! 發生異常!!!" };
            }
        }
    }
}