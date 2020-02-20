//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Models;
using tw.com.essentialoil.ShoppingCart.ViewModels;
using tw.com.essentialoil.User.Models;

namespace tw.com.essentialoil.Controllers
{
    public class ShoppingCartController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        int userId;

        // GET: ShoppingCart

        public ActionResult viewCart()
        {
            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "ShoppingCart", "viewCart"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;
            }
            
            var tableCart = from c in db.tShoppingCarts
                            where c.fId == userId
                            select c;
            var tableProduct = from p in db.tProducts
                               select p;
            CShoppingCart cart = new CShoppingCart() { ShoppingCart = tableCart, Product = tableProduct };
            return View(cart);
        }

        public ActionResult addCart(int productId)
        {
            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);

            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
            userId = userLoginInfo.user_fid;

            if (product != null)
            {

                tShoppingCart cart = db.tShoppingCarts.Where(p => p.fId == userId).FirstOrDefault(p=>p.fProductID == productId);

                if (cart != null)
                {
                    if (cart.fQuantity <= (product.fUnitsInStock - 1))
                        cart.fQuantity += 1;
                    else
                        return JavaScript("alert('購物車內該商品數量已超過庫存');");
                }
                else
                {
                    tShoppingCart cartNew = new tShoppingCart();
                    cartNew.fId = userId;
                    cartNew.fProductID = product.fProductID;
                    cartNew.fQuantity = 1;     //TODO  數量也要由商品來
                    cartNew.fAddTime = DateTime.Now;
                    db.tShoppingCarts.Add(cartNew);
                }
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult editCart(int basketId, int quantity)
        {
            tShoppingCart cart = db.tShoppingCarts.FirstOrDefault(p => p.fBasketId == basketId);
            if (cart != null)
            {
                cart.fQuantity = (short)quantity;
                db.SaveChanges();
            }

            return RedirectToAction("viewCart");
        }

        public ActionResult delete(int basket)
        {
            tShoppingCart cart = db.tShoppingCarts.FirstOrDefault(p => p.fBasketId == basket);
            if (cart != null)
            {
                db.tShoppingCarts.Remove(cart);
                db.SaveChanges();
            }
            return RedirectToAction("viewCart");
        }

        public ActionResult deleteAll()
        {
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
            userId = userLoginInfo.user_fid;

            foreach (var cart in db.tShoppingCarts.Where(p => p.fId == userId))
            {
                if (cart != null)
                {
                    db.tShoppingCarts.Remove(cart);
                }
            }
            db.SaveChanges();
            return RedirectToAction("viewCart");
        }

        public ActionResult cartTotalQuantity()
        {
            //沒有登入回傳空值
            if (!CStaticMethod.isLogin(Session, "ShoppingCart", "viewCart"))
            {
                return Content("");
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;
            }

            var tableCart = from c in db.tShoppingCarts
                            where c.fId == userId
                            select c;

            short totalQuantity = 0;
            foreach (var item in tableCart)
            {
                totalQuantity += item.fQuantity;
            }


            return Content(totalQuantity.ToString());
        }


        public ActionResult addFavorite(int productId)
        {
            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);
            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
            userId = userLoginInfo.user_fid;

            if (product != null)
            {
                tUserProductFavorite favorite = new tUserProductFavorite();
                favorite.fId = userId;
                favorite.fProductId = product.fProductID;
                favorite.fAddTime = DateTime.Now;
                db.tUserProductFavorites.Add(favorite);
                db.SaveChanges();
            }
            return RedirectToAction("viewCart");
        }

        public ActionResult orderSend(string totalBasketId)
        {
            return RedirectToAction("viewCart");
        }
        
    }
}