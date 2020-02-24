using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.User.Models;

namespace prjShoppingForum.Controllers.LineBot
{
    public class LineBotController : Controller
    {
        //由LineBot導引進入的葉面
        public ActionResult link(string linkToken)
        {
            ViewBag.linkToken = linkToken;
            return PartialView();
        }

        public ActionResult checkLogin(string account, string password)
        {
            CUser user = new CUser();
            tUserProfile cust = user.checkLogin(account, password);

            if (cust != null)
            {
                user.lineBotGetBase64String(cust);
            }



            return PartialView();
        }

        
    }
}