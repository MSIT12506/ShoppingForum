using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Models;
using tw.com.essentialoil.ProductFavorite.ViewModels;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers
{
    public class ProductFavoriteController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        int userId;

        // GET: ProductFavorite

        //清單列表
        public ActionResult List()
        {
            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "ProductFavorite", "List"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;
            }

            var tableProductFavorite = from f in db.tUserProductFavorites
                                       where f.fId == userId
                                       select f;

            var tableProduct = from p in db.tProducts
                               select p;

            CProductFavorite favorite = new CProductFavorite() { ProductFavorite = tableProductFavorite, Product = tableProduct };

            return View(favorite);
        }

        // 加入到收藏清單
        public ActionResult Creat(int productId)
        {
            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);

            userId = (Session[CDictionary.UserLoginInfo] as UserLoginInfo).user_fid;

            if (product != null)
            {
                tUserProductFavorite favorite = db.tUserProductFavorites.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductId == productId);

                if (favorite != null)
                {
                    return JavaScript("alert('該商品已經加入到收藏清單內')");
                }
                else
                {
                    tUserProductFavorite favoriteNew = new tUserProductFavorite();
                    favoriteNew.fId = userId;
                    favoriteNew.fProductId = product.fProductID;
                    favoriteNew.fAddTime = DateTime.Now;
                    db.tUserProductFavorites.Add(favoriteNew);
                    db.SaveChanges();
                }
                return RedirectToAction("viewCart", "ShoppingCart");
            }
            else
            {
                return JavaScript("alert('沒有該項商品');");
            }
        }

        public ActionResult Delete(int fFavoriteId)
        {
            tUserProductFavorite favorite = db.tUserProductFavorites.FirstOrDefault(p => p.fFavoriteId == fFavoriteId);
            if (favorite != null)
            {
                db.tUserProductFavorites.Remove(favorite);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}