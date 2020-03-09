//由Entity Framework產生，不改namespace
using prjShoppingForum.Models.Entity;

//------------------------------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using tw.com.essentialoil.Product.Models;
using tw.com.essentialoil.Product.ViewModels;

namespace tw.com.essentialoil.Controllers
{
    public class ProductFrontController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        DropDownList DropDownList = new DropDownList();
        ProductMenuRepository productMenuRepository = new ProductMenuRepository();
        ProductRepository productRepository = new ProductRepository();
        int pagesize = 12;


        // 檢視全部商品&分類、查詢、進階查詢檢視
        public ActionResult ProductFrontPage(SearchModel searchModel, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            ViewBag.SearchModel = searchModel == null ? new SearchModel() : searchModel;

            IQueryable<tProduct> products
            = productRepository.SearchProducts(searchModel.searchprod, searchModel.categoryId
            ,searchModel.efficacyId, searchModel.noteId, searchModel.partId, searchModel.featureId
            ,searchModel.fDiscontinued);

            ViewBag.productMenu = productMenuRepository.GetProductMenu();

            var pageResult = products.ToList().ToPagedList(currentPage, pagesize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("ProductFrontPage", pageResult);
            }

            return View(pageResult);
        }

        //檢視商品個別頁面
        public ActionResult ProductSinglePage(int productId)
        {
            ViewBag.CategoryList = db.tCategories.ToList();
            ViewBag.productMenu = productMenuRepository.GetProductMenu();
            var ProductSingle = db.tProducts.FirstOrDefault(p => p.fProductID == productId);
            return View(ProductSingle);
        }
        //商品進階查詢
        public ActionResult AdvanceQueryPage()
        {
            SearchModel searchModel = new SearchModel();
            ViewBag.PartDropDownList = DropDownList.GetPartDropDownList();
            ViewBag.NoteDropList = DropDownList.GetNoteDropList();
            ViewBag.CategoryDropList = DropDownList.GetCategoryDropList();
            ViewBag.EfficacyDropLise = DropDownList.GetEfficacyDropLise();
            ViewBag.featureDropList = DropDownList.GetfeatureDropList();
            ViewBag.efficacyDropList = DropDownList.GetEfficacyDropList();

            return View(searchModel);
        }


    }
}