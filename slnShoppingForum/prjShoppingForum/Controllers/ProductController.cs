//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//安裝PagedList.Mvc 4.5.0 && PagedList 1.17.0
using PagedList;

namespace slnProducts.Controllers
{
    public class ProductController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        DropDownList DropDownList = new DropDownList();
        object lockObject = new object();
        int pagesize = 10;

        // 檢視全部商品
        public ActionResult ProductPage(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            var products = db.tProducts.ToList();
            var pageresult = products.ToPagedList(currentPage, pagesize);
            return View("ProductPage", pageresult);
        }

        //新增商品
        public ActionResult ProductCreate()
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            return View();
        }
        [HttpPost]
        public ActionResult ProductCreate(tProduct prod)
        {

            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();

            ViewBag.State = "success";

            //限定同時只有一位操作者能增加ProdcutID
            lock (lockObject)
            {
                int prodId = GetProductId(0);
                prod.fProductID = prodId;

                db.tProducts.Add(prod);
                db.SaveChanges();
            }
            return RedirectToAction("ProductPage");
        }
        //ProductID重複解法
        private int GetProductId(int prodId)
        {
            prodId = prodId == 0 ?
                db.tProducts.Max(p => p.fProductID) + 1 : prodId + 1;

            if (db.tProducts.Any(p => p.fProductID == prodId))
            {
                GetProductId(prodId);
            }

            return prodId;
        }

        //刪除商品
        public ActionResult ProductDelete(int prodID)
        {
            var prod = db.tProducts.Where(m => m.fProductID == prodID).FirstOrDefault();
            db.tProducts.Remove(prod);
            db.SaveChanges();
            return RedirectToAction("ProductPage");
        }

        //修改商品
        public ActionResult ProductEdit(int prodID)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();

            var prod = db.tProducts.Where(m => m.fProductID == prodID).FirstOrDefault();
            return View(prod);
        }
        [HttpPost]
        public ActionResult ProductEdit(tProduct prod)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();

            db.SaveChanges();
            return RedirectToAction("ProductPage");
        }
    }
}