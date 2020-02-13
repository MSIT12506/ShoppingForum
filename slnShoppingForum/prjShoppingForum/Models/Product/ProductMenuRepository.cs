using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tw.com.essentialoil.Product.ViewModels;

namespace tw.com.essentialoil.Product.Models
{
    public class ProductMenuRepository
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        public ProductMenu GetProductMenu()
        {
            var productMenu = new ProductMenu();
            productMenu.CategoryList = db.tCategories.ToList();
            productMenu.EfficacyList = db.tEfficacies.ToList();
            productMenu.PartList = db.tParts.ToList();
            productMenu.NoteList = db.tNotes.ToList();
            productMenu.FeatureList = db.tfeatures.ToList();
            return productMenu;
        }

    }
}