using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Models;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers
{
    public class ProductBrowseHistoryController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        int userId;

        // GET: ProductBrowseHistory
        public ActionResult Creat(int productId)
        {
            //有登入過寫入資料庫
            if (CStaticMethod.isLogin(Session, "ProductBrowseHistory", "Creat"))
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;

                tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);
                if (product != null)
                {
                    tUserBrowseHistory browseHistory = db.tUserBrowseHistories.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductId == productId);

                    if (browseHistory == null)
                    {
                        tUserBrowseHistory browseHistoryNew = new tUserBrowseHistory();
                        browseHistoryNew.fId = userId;
                        browseHistoryNew.fProductId = productId;
                        browseHistoryNew.fBrowseTime = DateTime.Now;
                        db.tUserBrowseHistories.Add(browseHistoryNew);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ProductSinglePage", "ProductFront", new { productId = productId });
        }
    }
}