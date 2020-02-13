using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace tw.com.essentialoil.Product.ViewModels
{
    public class ProductMenu
    {
        public List<tProduct> ProductsList { get; set; }
        public List<tCategory> CategoryList { get; set; }
        public List<tEfficacy> EfficacyList { get; set; }
        public List<tPart> PartList { get; set; }
        public List<tNote> NoteList { get; set; }
        public List<tfeature> FeatureList { get; set; }
    }
}