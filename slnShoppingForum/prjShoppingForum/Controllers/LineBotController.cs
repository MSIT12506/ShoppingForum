using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers.LineBot
{
    public class LineBotController : Controller
    {
        //由LineBot導引進入的葉面
        public ActionResult link(string linkToken)
        {      
            ViewBag.linkToken = linkToken;
            return PartialView();
        }

        public ActionResult checkLogin(string account, string password, string linkToken)
        {
            //TODO - 錯誤處理

            CUser user = new CUser();
            tUserProfile cust = user.checkLineLogin(account, password);
            string lineNonce = "";
            object result = null;

            if (cust != null)
            {
                if (user.lineBotGetBase64String(cust, ref lineNonce))
                {
                    result = new { result = "True", linkToken = linkToken, lineNonce = lineNonce };
                }
                else
                {
                    result = new { result = "False", linkToken = linkToken, lineNonce = lineNonce };
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
    }
}