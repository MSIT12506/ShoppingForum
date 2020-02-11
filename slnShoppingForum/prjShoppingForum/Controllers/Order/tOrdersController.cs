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

namespace tw.com.EssentialOil.Controllers.Order
{
    public class tOrdersController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        // GET: tOrders
        public ActionResult OrderList()
        {
            var prod = db.tOrders./*Where(p => p.fOrderId == 1003).*/Select(q => q);
            var detail = db.tOrderDetails./*Where(p => p.fOrderId == 1003).*/Select(q => q);
            var list = new OrderView() { Order = prod, OrderDetail=detail };
            return View(list);
        }

        public ActionResult OrderCreate()
        {
            return View(new tOrderMetaData());
        }

        [HttpPost]
        public ActionResult OrderCreate(string ConsigneeName,string ConsigneeCellPhone, string ConsigneeAddress, string OrderCompanyTitle, int OrderTaxIdDNumber, string OrderPostScript, string Payment)
        {
            var jConsigneeName = ConsigneeName;
            var jConsigneeCellPhone = ConsigneeCellPhone;
            var jConsigneeAddress = ConsigneeAddress;
            var jOrderCompanyTitle = OrderCompanyTitle;
            var jOrderTaxIdDNumber = OrderTaxIdDNumber;
            var jOrderPostScript = OrderPostScript;
            var jPayment = Payment;
            tOrder order = new tOrder()
            {
                fId = 1,
                fOrderDate = DateTime.Now,
                fConsigneeName = jConsigneeName,
                fConsigneeCellPhone = jConsigneeCellPhone,
                fConsigneeAddress = jConsigneeAddress,
                fOrderCompanyTitle = jOrderCompanyTitle,
                fOrderTaxIdDNumber = jOrderTaxIdDNumber,
                fOrderPostScript = jOrderPostScript,
                fPayment = jPayment
            };
            //tOrderDetail orderDetail = new tOrderDetail()
            //{
            //    fProductId = 2,
            //    fUnitPrice = 100,
            //    fOrderQuantity = 5
            //}
            db.tOrders.Add(order);
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }



















        public async Task<ActionResult> Index()
        {
            var tOrder = db.tOrders.Include(t => t.tUserProfile);
            return View(await tOrder.ToListAsync());
        }

        // GET: tOrders/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tOrder tOrder = await db.tOrders.FindAsync(id);
            if (tOrder == null)
            {
                return HttpNotFound();
            }
            return View(tOrder);
        }

        // GET: tOrders/Create
        public ActionResult Create()
        {
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId");
            return View();
        }

        // POST: tOrders/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "fOrderId,fId,fOrderDate,fShippedDate,fRequiredDate,fScore,fConsigneeName,fConsigneeTelephone,fConsigneeCellPhone,fConsigneeAddress,fOrderCompanyTitle,fOrderTaxIdDNumber,fOrderPostScript,fPayment")] tOrder tOrder)
        {
            if (ModelState.IsValid)
            {
                db.tOrders.Add(tOrder);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tOrder.fId);
            return View(tOrder);
        }

        // GET: tOrders/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tOrder tOrder = await db.tOrders.FindAsync(id);
            if (tOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tOrder.fId);
            return View(tOrder);
        }

        // POST: tOrders/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "fOrderId,fId,fOrderDate,fShippedDate,fRequiredDate,fScore,fConsigneeName,fConsigneeTelephone,fConsigneeCellPhone,fConsigneeAddress,fOrderCompanyTitle,fOrderTaxIdDNumber,fOrderPostScript,fPayment")] tOrder tOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tOrder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.fId = new SelectList(db.tUserProfiles, "fId", "fUserId", tOrder.fId);
            return View(tOrder);
        }

        // GET: tOrders/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tOrder tOrder = await db.tOrders.FindAsync(id);
            if (tOrder == null)
            {
                return HttpNotFound();
            }
            return View(tOrder);
        }

        // POST: tOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            tOrder tOrder = await db.tOrders.FindAsync(id);
            db.tOrders.Remove(tOrder);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
