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
            CShoppingCart cart = new CShoppingCart() { ShoppingCart = tableCart, Product = tableProduct, BrowseHistory = tableBrowseHistory };
            return View(cart);
        }

        [HttpPost]
        public ActionResult viewCart(string url)
        {
            Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] = url;
            return RedirectToAction("OrderCreate", "tOrders", new { totalBasketId = url });
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
                    cartNew.fAddTime = DateTime.Now;
                    db.tShoppingCarts.Add(cartNew);
                }
                db.SaveChanges();
            }
            return View();
        }

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
                tShoppingCart cart = db.tShoppingCarts.Where(p => p.fId == userId).FirstOrDefault(p => p.fProductID == productId);
                if (cart != null)
                {
                    if ( (cart.fQuantity + (short)selectQuantity) <= product.fUnitsInStock)
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


        [HttpPost]
        //ajax - 取得優惠後的金額
        public ActionResult getDiscountValue(string discountCode, int totalMoney)
        {
            decimal resultNum = 0;

            UserLoginInfo userLoginInfo = Session[CDictionary.UserLoginInfo] as UserLoginInfo;
            //userId = userLoginInfo.user_fid;
            userId = 4;

            CDiscount discount = new CDiscount();
            string result = discount.calculatePriceByDiscountCode(userId, discountCode, totalMoney, ref resultNum);

            return Json(new { message = result, returnNum = resultNum });

        }
    }
}