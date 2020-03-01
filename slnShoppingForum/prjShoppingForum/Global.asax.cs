//using prjShoppingForum.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace prjShoppingForum
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));
            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //if (Request.IsAuthenticated)
            //{
            //    // 先取得該使用者的 FormsIdentity
            //    FormsIdentity ID = (FormsIdentity)User.Identity;
            //    // 再取出使用者的 FormsAuthenticationTicket
            //    FormsAuthenticationTicket ticket = ID.Ticket;
            //    // 將儲存在 FormsAuthenticationTicket 中的角色定義取出，並轉成字串陣列
            //    //string userData = ticket.UserData;
            //    string[] roles = ticket.UserData.Split(new char[] { ',' });
            //    // 指派角色到目前這個 HttpContext 的 User 物件去
            //    //剛剛在創立表單的時候，你的UserData 放使用者名稱就是取名稱，我放的是群組代號，所以取出來就是群組代號
            //    //然後會把這個資料放到Context.User內
            //    Context.User = new GenericPrincipal(Context.User.Identity, roles);
            //}
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}