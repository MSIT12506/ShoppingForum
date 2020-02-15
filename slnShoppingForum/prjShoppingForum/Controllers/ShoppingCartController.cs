//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.ShoppingCart.ViewModels;

namespace tw.com.essentialoil.Controllers
{
    public class ShoppingCartController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        int userId = 1;

        // GET: ShoppingCart

        public ActionResult addCart(int productId)
        {
            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);
            if (product != null)
            {
                tShoppingCart cart = new tShoppingCart();
                cart.fId = userId;
                cart.fProductID = product.fProductID;
                cart.fQuantity = 1;     //TODO  數量也要由商品來
                cart.fAddTime = DateTime.Now;
                db.tShoppingCarts.Add(cart);
                db.SaveChanges();
            }
            return RedirectToAction("ProductFrontPage", "ProductFront");
        }

        public ActionResult viewCart()
        {
            var tableCart = from c in db.tShoppingCarts
                            where c.fId == userId
                            select c;
            var tableProduct = from p in db.tProducts
                               select p;
            CShoppingCart cart = new CShoppingCart() { ShoppingCart = tableCart, Product = tableProduct };
            return View(cart);
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

        public ActionResult addFavorite(int productId)
        {
            tProduct product = db.tProducts.FirstOrDefault(p => p.fProductID == productId);
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
    }
}