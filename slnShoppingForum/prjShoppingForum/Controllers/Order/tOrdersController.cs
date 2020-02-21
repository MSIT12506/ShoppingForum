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

namespace tw.com.EssentialOil.Controllers.Order
{
    public class tOrdersController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // 目前僅顯示最後一筆已完成訂單
        public ActionResult OrderList()
        {
            if (Session[UserDictionary.S_CURRENT_LOGINED_USERFID] != null)
            {
                var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
                var noworderid = db.tOrders.Where(p => p.fId == loginuser).OrderByDescending(p => p.fOrderId).FirstOrDefault();
                var prod = db.tOrders.Where(p => p.fId == loginuser&& p.fOrderId == noworderid.fOrderId).Select(q => q);
                var detail = db.tOrderDetails.Where(p=>p.fOrderId==noworderid.fOrderId).Select(q => q);
                var list = new OrderView() { Order = prod, OrderDetail = detail };
                return View(list);
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
            return View(new tOrderMetaData());
        }


        //剩將totalBasketId加入商品判斷條件式
        [HttpPost]
        public ActionResult OrderCreate(string ConsigneeName,string ConsigneeCellPhone, string ConsigneeAddress, string OrderCompanyTitle, int OrderTaxIdDNumber, string OrderPostScript, string Payment, string totalBasketId)
        {
            var loginuser = Convert.ToInt32(Session[UserDictionary.S_CURRENT_LOGINED_USERFID]);
            tOrder order = new tOrder()
            {
                fId = loginuser,
                fOrderDate = DateTime.Now,
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
            
            var fromshopcart = db.tShoppingCarts.Where(p => p.fId == loginuser).Select(p=>p).ToList();
            var noworderid = db.tOrders.Where(p=>p.fId== loginuser).OrderByDescending(p=>p.fOrderId).FirstOrDefault();
            foreach (var items in fromshopcart)
            {
                tOrderDetail orderDetail = new tOrderDetail();
                orderDetail.fOrderId = noworderid.fOrderId;
                orderDetail.fProductId = items.fProductID;
                orderDetail.fOrderQuantity = items.fQuantity;
                orderDetail.fUnitPrice = items.fQuantity * db.tProducts.Where(p => p.fProductID == items.fProductID).Select(q => q.fUnitPrice).FirstOrDefault();
                db.tOrderDetails.Add(orderDetail);
                db.SaveChanges();
            }
            return RedirectToAction("OrderList");
        }
    }
}
