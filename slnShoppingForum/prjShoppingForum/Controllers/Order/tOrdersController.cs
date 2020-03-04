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
using System.Net.Mail;
using tw.com.essentialoil.ViewModels;
//using prjShoppingForum.ViewModels;

/*
 2020.01.27 vision 1.0 建立tOrderController 
 2020.02.11 vision 1.1 完成訂單成立與換頁顯示訂單資訊，db.oreder能存入，db.orderdetail未能存入
 2020.02.21 vision 1.2 完成UserSession、db.orderdetail多筆存入、OrderList顯示多筆產品、鎖住產生訂單按鍵、購物車到訂單流程完畢、缺購物車勾選商品名單
 2020.02.22 vision 1.3 完成購物車勾選商品名單、新增產品名單Session
 2020.02.23 vision 1.3 完成OrderList顯示產品名稱與產品照片、新增無登入無法進入頁面、新增訂單希望送達日期功能
 2020.02.24 vision 1.4 完成View執行流程、新增清空購物車、商品庫存更新、Session失效判定
 2020.02.27 vision 1.5 完成Email寄送
 2020.02.28 vision 1.6 完成訂單名稱顯示訂單明細
 2020.02.29 vision 1.7 UI初步優化
 2020.03.01 vision 1.8 預計完成Payment Radio Button
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
                    //var loginuser = 1;
                    Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] = null;
                    var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                    var loginuserid = Session[UserDictionary.S_CURRENT_LOGINED_USER].ToString();
                    var loginusername = db.tUserProfiles.Where(p => p.fId == loginuser).Select(p => p.fName).FirstOrDefault().ToString();
                    var noworderid = db.tOrders.Where(p => p.fId == loginuser).OrderByDescending(p => p.fOrderId).FirstOrDefault();
                    var orderid = noworderid.fOrderDate.ToString("yyyyMMdd") + noworderid.fOrderId.ToString();
                    var noworderrow = db.tOrders.Where(p => p.fOrderId == noworderid.fOrderId && p.fId == loginuser).Select(q => q);//找出顯示訂單的資訊
                    var noworderdetail = db.tOrderDetails.Where(p => p.fOrderId == noworderid.fOrderId).OrderBy(p => p.fProductId);//顯示訂單明細的資訊，會有多筆
                    var product = db.tProducts;
                    var list = new COrderViews() { Order = noworderrow, OrderDetail = noworderdetail, Product = product };
                    //寄送Email給購買者
                    var senderEmail = new MailAddress("isgoldAoil@gmail.com", "ESSENCE SHOP");//管理員寄email所用的信箱，若要測試請填自己可用的email
                    var receiverEmail = new MailAddress(loginuserid, loginusername);
                    var password = "vnmuhcmaxieewtbi";//管理員寄email所用的信箱密碼，測試時請自行填入
                    var sub = "訂單發貨通知信";
                    var body = "親愛的 " + loginusername + " 妳好:\n" + "您的訂單 " + orderid + " 已按照您預定的配送方式給您發貨了\n" + "再次感謝您對我們的支持, 歡迎您的再次光臨 !";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View(list);
                }
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Login","FrontUserProfile");
        }

        public ActionResult OrderCreate()
        {

            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                if (Session[UserDictionary.S_CURRENT_LOGINED_USERSHOPCART] != null)
                {
                    var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                    var userinfo = db.tUserProfiles.Where(p => p.fId == loginuser).FirstOrDefault();
                    COrderView view = new COrderView() { tUserProfiles = userinfo };
                    return View(view);
                }
                return RedirectToAction("viewCart", "ShoppingCart");
            }
            return RedirectToAction("Login", "FrontUserProfile");
        }

        [HttpPost]
        public ActionResult OrderCreate(string ConsigneeName,string ConsigneeCellPhone, string ConsigneeAddress, string OrderCompanyTitle, int? OrderTaxIdDNumber, string OrderPostScript, string Payment, DateTime? RequirtDate )
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
                        fOrderDate = DateTime.UtcNow.AddHours(8),
                        fShippedDate = DateTime.UtcNow.AddHours(8).AddDays(1),
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
                        fOrderDate = DateTime.UtcNow.AddHours(8),
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
