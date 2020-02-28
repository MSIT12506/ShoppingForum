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

        //[HttpPost]
        //public object RandomProduct(CLineUserId t)
        //{
        //    CLineBot lineBot = new CLineBot;
        //    lineBot.
        //}










        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}