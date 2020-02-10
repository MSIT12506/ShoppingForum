using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tw.com.essentialoil.Discount.Models;

namespace prjShoppingForum.Controllers
{
    public class DiscountController : Controller
    {
        //TODO - 待完成其他優惠代碼的後台功能，目前先以新增完畢後可以訂單結果優先開發

        //All Discount Code List
        public ActionResult List()
        {
            CDiscount discount = new CDiscount();
            return View(discount.queryAllDiscount());
        }

        //Creare Discount Code - view
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tDiscount data)
        {
            CDiscount discount = new CDiscount();
            discount.craeteDiscount(data);
            return View();
        }

        //計算優惠折扣測試用
        public ActionResult Calcute()
        {
            CDiscount discount = new CDiscount();
            ViewBag.result = discount.calculatePriceByDiscountCode();
            return View();
        }
    }
}