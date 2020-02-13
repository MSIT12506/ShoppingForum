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

namespace tw.com.essentialoil.Controllers
{
    public class ProductFrontController : Controller
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        DropDownList DropDownList = new DropDownList();
        ProductMenuRepository productMenuRepository = new ProductMenuRepository();
        ProductRepository productRepository = new ProductRepository();

        int pagesize = 10;

        // 檢視全部商品&商品分類檢視
        public ActionResult ProductFrontPage(string searchprod,int page = 1, int? categoryId = null, int? efficacyId = null
                                           , int? noteId = null, int? partId = null, int? featureId = null)
        {
            int currentPage = page < 1 ? 1 : page;

            IQueryable<tProduct> products
            = productRepository.SearchProducts(searchprod, categoryId, efficacyId, noteId, partId, featureId);

            ViewBag.productMenu = productMenuRepository.GetProductMenu();

            var pageResult = products.ToList().ToPagedList(currentPage, pagesize);

            if(Request.IsAjaxRequest())
            {
                return PartialView("_List_Product", pageResult);
            }

            return View(pageResult);
        }
       
        //檢視商品個別頁面
        public ActionResult ProductSinglePage(int productId)
        {
            ViewBag.CategoryList = db.tCategories.ToList();
            var ProductSingle = db.tProducts.FirstOrDefault(p => p.fProductID == productId);
            return View(ProductSingle);
        }

      


    }
}