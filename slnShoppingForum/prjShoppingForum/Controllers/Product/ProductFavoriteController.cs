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

            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);

            if (product != null)
            {
                tUserProductFavorite favorite = db.tUserProductFavorites.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductId == productId);

                string blueHeart = "/Images/Product/like_blue.png";              //藍色愛心
                string redHeart = "/Images/Product/like_red.png";                //紅色愛心

                if (favorite != null)
                {
                    db.tUserProductFavorites.Remove(favorite);
                    db.SaveChanges();

                    return Json(new { pic = blueHeart, mess = "商品已從收藏清單移除" });
                }
                else
                {
                    tUserProductFavorite favoriteNew = new tUserProductFavorite();
                    favoriteNew.fId = userId;
                    favoriteNew.fProductId = product.fProductID;
                    favoriteNew.fAddTime = DateTime.UtcNow.AddHours(8);
                    db.tUserProductFavorites.Add(favoriteNew);
                    db.SaveChanges();
                    return Json(new { pic = redHeart, mess = "商品已加入收藏清單" });
                }
            }
            else
            {
                return JavaScript("alert('沒有該項商品');");
            }
        }

        // 從商品加入到收藏清單
        public ActionResult CreatFromProduct(int productId)
        {
            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "ProductFavorite", "List"))
            {
                return JavaScript("location.href = `/FrontUserProfile/Login`");
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;
            }

            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);

            if (product != null)
            {
                tUserProductFavorite favorite = db.tUserProductFavorites.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductId == productId);

                string blueHeart = "/Images/Product/like_blue.png";            //藍色愛心
                string redHeart = "/Images/Product/like_red.png";              //紅色愛心

                if (favorite != null)
                {
                    db.tUserProductFavorites.Remove(favorite);
                    db.SaveChanges();

                    return Json(new { pic = blueHeart, mess = "商品已從收藏清單移除" });
                }
                else
                {
                    tUserProductFavorite favoriteNew = new tUserProductFavorite();
                    favoriteNew.fId = userId;
                    favoriteNew.fProductId = product.fProductID;
                    favoriteNew.fAddTime = DateTime.UtcNow.AddHours(8);
                    db.tUserProductFavorites.Add(favoriteNew);
                    db.SaveChanges();
                    return Json(new { pic = redHeart, mess = "商品已加入收藏清單"});
                }
            }
            else
            {
                return Content("<script >alert('沒有該項商品');</script >", "text/html");
            }
        }
        //刪除收藏
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
        //判斷商品是否已經在使用者收藏清單
        public ActionResult favoriteJudge(int productId)
        {
            string redHeart = "/Images/Product/like_red.png";
            string blueHeart = "/Images/Product/like_blue.png";

            //沒有登入傳藍色的愛心圖
            if (!CStaticMethod.isLogin(Session, "ProductFavorite", "List"))
            {
                return Json(new { pic = blueHeart });
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;

                tUserProductFavorite favorite = db.tUserProductFavorites.Where(u => u.fId == userId).FirstOrDefault(p => p.fProductId == productId);
                if (favorite != null)
                {
                    return Json(new { pic = redHeart });
                }
                else
                {
                    return Json(new { pic = blueHeart });
                }
            }
        }
    }
}