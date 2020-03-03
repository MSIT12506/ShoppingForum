using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjShoppingForum.Models.Entity;
using System.Text;
using tw.com.essentialoil.ViewModels;

/*
 2020.02.28 vision 1.0 完成訂單後台初版
 2020.02.29 vision 1.1 存出檔案(未完成)
 2020.03.01 vision 1.2 UI初步優化、預計完成加入顯示訂單明細

*/

namespace tw.com.EssentialOil.Controllers.Order
{
    public class BacktOrdersController : Controller
    {
        private dbShoppingForumEntities db = new dbShoppingForumEntities();
        
        // GET: BacktOrders
        public async Task<ActionResult> Index()
        {
            var tOrders = db.tOrders.Include(t => t.tUserProfile);
            return View(await tOrders.ToListAsync());
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

        // GET: BacktOrders/Details/5
        public async Task<ActionResult> Details(int? id)
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

        //存出檔案(未完成)
        public ActionResult ExportFile(int? id)
        {
            var query = db.tOrders.Where(p => p.fOrderId == id);
            var items = db.tOrderDetails.Where(p => p.fOrderId == id);
            var customer = db.tUserProfiles.Where(p => p.fId == query.FirstOrDefault().fId);
            COrderViews views = new COrderViews() { Order = query, OrderDetail = items, UserProfile = customer  };
            string fileName = string.Format("Order-{0}.csv", DateTime.UtcNow.AddHours(8).ToString("yyyyMMdd"));
            StringBuilder sb = new StringBuilder();
            sb.Append(";fOrderId,fOrderDate,fShippedDate,fRequiredDate,fConsigneeName,fConsigneeCellPhone,fConsigneeAddress," +
                "fOrderCompanyTitle,fOrderTaxIdDNumber,fOrderPostScript");
            foreach (var row in views.Order.ToList())
            {
                sb.AppendFormat("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                    row.fOrderId, row.fOrderDate, row.fShippedDate, row.fRequiredDate, row.fConsigneeName, row.fConsigneeCellPhone, 
                    row.fConsigneeAddress, row.fOrderCompanyTitle, row.fOrderTaxIdDNumber, row.fOrderPostScript);
            }

            byte[] OutputContent = new UTF8Encoding().GetBytes(sb.ToString());

            return File(OutputContent, "text/csv", fileName);
        }

        //GET: BacktOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

        // POST: BacktOrders/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "fOrderId,fId,fOrderDate,fShippedDate,fRequiredDate,fScore,fConsigneeName,fConsigneeTelephone,fConsigneeCellPhone,fConsigneeAddress,fOrderCompanyTitle,fOrderTaxIdDNumber,fOrderPostScript,fDiscountCode,fPayment")] tOrder tOrder)
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
