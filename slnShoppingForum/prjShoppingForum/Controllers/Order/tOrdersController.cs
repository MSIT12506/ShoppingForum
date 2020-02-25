using prjShoppingForum.Models.Entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Order.Models;
using static prjShoppingForum.Models.Entity.tOrder;
using tw.com.essentialoil.User.Models;
using prjShoppingForum.ViewModels;
//using prjShoppingForum.ViewModels;

/*
 2020.01.27 vision 1.0 建立tOrderController 
 2020.02.11 vision 1.1 完成訂單成立與換頁顯示訂單資訊，db.oreder能存入，db.orderdetail未能存入
 2020.02.21 vision 1.2 完成UserSession、db.orderdetail多筆存入、OrderList顯示多筆產品、鎖住產生訂單按鍵、購物車到訂單流程完畢、缺購物車勾選商品名單
 2020.02.22 vision 1.3 完成購物車勾選商品名單、新增產品名單Session
 2020.02.23 vision 1.3 完成OrderList顯示產品名稱與產品照片、新增無登入無法進入頁面、新增訂單希望送達日期功能
 2020.02.24 vision 1.4 完成View執行流程、新增清空購物車、商品庫存更新、Session失效判定
*/

namespace tw.com.EssentialOil.Controllers.Order
{
    public class tOrdersController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // 顯示完成訂單資訊
        public ActionResult OrderList()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                if (Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] != null)
                {
                    Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] = null;
                    var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                    var noworderid = db.tOrders.Where(p => p.fId == loginuser).OrderByDescending(p => p.fOrderId).FirstOrDefault();
                    var noworderrow = db.tOrders.Where(p => p.fOrderId == noworderid.fOrderId && p.fId == loginuser).Select(q => q);//找出顯示訂單的資訊
                    var noworderdetail = db.tOrderDetails.Where(p => p.fOrderId == noworderid.fOrderId).OrderBy(p => p.fProductId);//顯示訂單明細的資訊，會有多筆
                    var product = db.tProducts;
                    var list = new COrderViews() { Order = noworderrow, OrderDetail = noworderdetail, Product = product };
                    return View(list);
                }
                return RedirectToAction("viewCart", "ShoppingCart");

            }
            return RedirectToAction("Login","FrontUserProfile");
        }

        //訂單總覽使用
        //public ActionResult OrderList()
        //{
        //    if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
        //    {
        //        var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
        //        var prod = db.tOrders.Where(p => p.fId == loginuser).Select(q => q);
        //        var detail = db.tOrderDetails.Select(q => q);
        //        var list = new OrderView() { Order = prod, OrderDetail = detail };
        //        return View(list);
        //    }
        //    return RedirectToAction("Login", "FrontUserProfile");
        //}

        public ActionResult OrderCreate()
        {

            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                if (Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] != null)
                {
                    return View(new tOrderMetaData());
                }
                return RedirectToAction("viewCart", "ShoppingCart");
            }
            return RedirectToAction("Login", "FrontUserProfile");
        }

        [HttpPost]
        public ActionResult OrderCreate(string ConsigneeName,string ConsigneeCellPhone, string ConsigneeAddress, string OrderCompanyTitle, int OrderTaxIdDNumber, string OrderPostScript, string Payment, DateTime RequirtDate )
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {            
                var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                var l = Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART].ToString();
                var list = l.Split('_').ToList();
                foreach(var w in list)
                {
                    if (w != "")
                    {
                        var z = Convert.ToInt32(w);
                        var shopcartlist = db.tShoppingCarts.FirstOrDefault(p => p.fBasketId == z);
                        var unitinstock = db.tProducts.Where(p => p.fProductID == shopcartlist.fProductID).Select(p => p.fUnitsInStock).FirstOrDefault();
                        var unitinshoppingcart = db.tShoppingCarts.Where(p => p.fBasketId == shopcartlist.fBasketId).Select(p => p.fQuantity).FirstOrDefault();
                        if(unitinshoppingcart> unitinstock)
                        {
                            return RedirectToAction("viewCart", "ShoppingCart");
                        }
                    }
                }
                if (RequirtDate == null)
                {
                    tOrder order = new tOrder()
                    {
                        fId = loginuser,
                        fOrderDate = DateTime.Now,
                        fShippedDate = DateTime.Now.AddDays(1),
                        fConsigneeName = ConsigneeName,
                        fConsigneeCellPhone = ConsigneeCellPhone,
                        fConsigneeAddress = ConsigneeAddress,
                        fOrderCompanyTitle = OrderCompanyTitle,
                        fOrderTaxIdDNumber = OrderTaxIdDNumber,
                        fOrderPostScript = OrderPostScript,
                        fPayment = Payment
                    };
                    db.tOrders.Add(order);
                    db.SaveChanges();
                }
                else
                {
                    tOrder order = new tOrder()
                    {
                        fId = loginuser,
                        fOrderDate = DateTime.Now,
                        fShippedDate = RequirtDate,
                        fRequiredDate = RequirtDate,
                        fConsigneeName = ConsigneeName,
                        fConsigneeCellPhone = ConsigneeCellPhone,
                        fConsigneeAddress = ConsigneeAddress,
                        fOrderCompanyTitle = OrderCompanyTitle,
                        fOrderTaxIdDNumber = OrderTaxIdDNumber,
                        fOrderPostScript = OrderPostScript,
                        fPayment = Payment
                    };
                    db.tOrders.Add(order);
                    db.SaveChanges();
                }
                //OrderDetail Save
                var noworderid = db.tOrders.Where(p => p.fId == loginuser).OrderByDescending(p => p.fOrderId).FirstOrDefault();
                foreach (var x in list)
                {
                    if (x != "")
                    {
                        var z = Convert.ToInt32(x);
                        var pid = (db.tShoppingCarts.Where(p => p.fBasketId == z).Select(p => p)).ToList();
                        
                        foreach (var items in pid)
                        {
                            tOrderDetail orderDetail = new tOrderDetail();
                            orderDetail.fOrderId = noworderid.fOrderId;
                            orderDetail.fProductId = items.fProductID;
                            orderDetail.fOrderQuantity = items.fQuantity;
                            orderDetail.fUnitPrice = items.fQuantity * db.tProducts.Where(p => p.fProductID == items.fProductID).Select(q => q.fUnitPrice).FirstOrDefault();
                            db.tOrderDetails.Add(orderDetail);
                            db.SaveChanges();
                        }
                    }
                }
                //Clear ShoppingCart
                foreach (var y in list)
                {
                    if (y != "")
                    {
                        var z = Convert.ToInt32(y);
                        var pid = db.tShoppingCarts.Where(p => p.fBasketId == z).FirstOrDefault();
                        db.tShoppingCarts.Remove(pid);
                        db.SaveChanges();
                    }
                }
                //sub product UnitInStock
                var noworderdetail = db.tOrderDetails.Where(p => p.fOrderId == noworderid.fOrderId).ToList();
                foreach (var items in noworderdetail)
                {
                    tProduct product = db.tProducts.Where(p => p.fProductID == items.fProductId).FirstOrDefault();
                    product.fUnitsInStock -= items.fOrderQuantity;
                    db.SaveChanges();
                }
                return RedirectToAction("OrderList");
            }
            return RedirectToAction("Login", "FrontUserProfile");
        }
    }
}
