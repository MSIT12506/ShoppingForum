//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Discount.Models;
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
            var tableBrowseHistory = from b in db.tUserBrowseHistories
                                     where b.fId == userId
                                     select b;
            var tableUser = from s in db.tUserProfiles
                             where s.fId == userId
                             select s;
            var tableDiscount = from d in db.tUserDiscountLists
                                where d.fId == userId
                                select d;
            CShoppingCart cart = new CShoppingCart() { ShoppingCart = tableCart, Product = tableProduct, BrowseHistory = tableBrowseHistory, UserProfiles = tableUser, UserDiscountLists= tableDiscount };
            return View(cart);
        }

        [HttpPost]
        public ActionResult viewCart(string url, int score, string coupon)
        {
            string urls = url + "," + score.ToString() + "," + coupon;
            Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] = urls;
            return RedirectToAction("OrderCreate", "tOrders", new { totalBasketId = url });
        }

        public ActionResult addCart(int productId)
        {
            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);

            if (!CStaticMethod.isLogin(Session, "ShoppingCart", "viewCart"))
            {
                return RedirectToRoute(new CRedirectToLogin());
            }
            else
            {
                UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
                userId = userLoginInfo.user_fid;
            }

            if (product != null)
            {
                if (product.fUnitsInStock != 0)
                {
                    tShoppingCart cart = db.tShoppingCarts.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductID == productId);
                    if (cart != null)
                    {
                        if (cart.fQuantity < product.fUnitsInStock)
                            cart.fQuantity += 1;
                        else
                            return JavaScript("alert('購物車內該商品數量已超過庫存');");
                    }
                    else
                    {
                        tShoppingCart cartNew = new tShoppingCart();
                        cartNew.fId = userId;
                        cartNew.fProductID = product.fProductID;
                        cartNew.fQuantity = 1;
                        cartNew.fAddTime = DateTime.UtcNow.AddHours(8);
                        db.tShoppingCarts.Add(cartNew);
                    }
                    db.SaveChanges();
                }
                else
                {
                    return JavaScript("alert('該商品已無庫存');");
                }
            }
            return View();
        }
        //從商品單項清單加入購物車(會有一個以上的數量)
        public ActionResult addToCartfromProduct(int productId, int selectQuantity)
        {
            //沒有登入過不能進來
            if (!CStaticMethod.isLogin(Session, "ShoppingCart", "viewCart"))
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
                if (product.fUnitsInStock != 0)
                {
                    tShoppingCart cart = db.tShoppingCarts.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductID == productId);
                    if (cart != null)
                    {
                        if ((cart.fQuantity + (short)selectQuantity) <= product.fUnitsInStock)
                            cart.fQuantity += (short)selectQuantity;
                        else
                            return JavaScript("alert('購物車內該商品數量已超過庫存');");
                    }
                    else
                    {
                        tShoppingCart cartNew = new tShoppingCart();
                        cartNew.fId = userId;
                        cartNew.fProductID = product.fProductID;
                        cartNew.fQuantity = (short)selectQuantity;
                        cartNew.fAddTime = DateTime.UtcNow.AddHours(8);
                        db.tShoppingCarts.Add(cartNew);
                    }
                    db.SaveChanges();
                }
                else
                {
                    return JavaScript("alert('該商品已無庫存');");
                }
            }

            return View();

        }
        //更改購物車DB數量
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
        //刪除單項購物車
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
        //清空購物車
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
        //回傳使用者的購物車總數量
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

            return Json(new { totalQuantity = totalQuantity },JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDiscountValue(string discountCode, int totalMoney)
        {
            decimal resultNum = (decimal)totalMoney;

            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
            userId = userLoginInfo.user_fid;

            CDiscount discount = new CDiscount();
            string result = discount.calculatePriceByDiscountCode(userId, discountCode, totalMoney, ref resultNum);

            return Json(new { message = result, returnNum = resultNum });

        }
    }
}