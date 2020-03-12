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
using System.IO;
using tw.com.essentialoil.Product.Models;


namespace tw.com.essentialoil.Controllers
{
    public class ProductController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        ProductRepository productRepository = new ProductRepository();
        DropDownList DropDownList = new DropDownList();

        // 檢視全部商品
        public ActionResult ProductPage(int page = 1)
        {
            IPagedList<tProduct> pageresult =productRepository.ProdPageList(page);
            return View(pageresult);
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
        public ActionResult ProductCreate(tProduct prod, HttpPostedFileBase prodImg, HttpPostedFileBase produraImg)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            productRepository.InsertProduct(prod, prodImg, produraImg, Server);

            return RedirectToAction("ProductPage");
        }

        //刪除商品
        public ActionResult ProductDelete(int prodId)
        {
            productRepository.deleteProd(prodId);
            return RedirectToAction("ProductPage");
        }

        //修改商品
        public ActionResult ProductEdit(int prodId)
        {
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();

            var prod = db.tProducts.Where(m => m.fProductID == prodId).FirstOrDefault();
            return View(prod);
        }
        [HttpPost]
        public ActionResult ProductEdit(tProduct prod, HttpPostedFileBase prodImg, HttpPostedFileBase produraImg)
        {
            productRepository.UpdateProduct(prod, prodImg, Server, produraImg);
            tProductImage productImage = new tProductImage();
            return RedirectToAction("ProductPage");
        }
    }
}