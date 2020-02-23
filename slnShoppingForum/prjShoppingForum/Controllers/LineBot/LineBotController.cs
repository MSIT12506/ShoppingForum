using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjShoppingForum.Controllers.LineBot
{
    public class LineBotController : Controller
    {
        // GET: LineBot
        public ActionResult link(string linkToken)
        {
            var stream = linkToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;



            return View();
        }
    }
}