using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tProductMetadata))]
    public partial class tProduct
    {
        public class tProductMetadata
        {
            [DisplayName("商品編號")]
            public int fProductID { get; set; }

            [DisplayName("品名")]
            [Required(ErrorMessage ="必填")]
            public string fProductChName { get; set; }

            [DisplayName("介紹")]
            [Required(ErrorMessage = "必填")]
            public string fProductDesc { get; set; }

            [DisplayName("單價")]
            [Required(ErrorMessage = "必填")]
            public Nullable<int> fUnitPrice { get; set; }

            [DisplayName("容量(ml)")]
            [Required(ErrorMessage = "必填")]
            public string fQuantityPerUnit { get; set; }

            [DisplayName("庫存")]
            public Nullable<int> fUnitsInStock { get; set; }

            [DisplayName("下架中")]
            public Nullable<bool> fDiscontinued { get; set; }

            [DisplayName("商品分類")]
            [Required(ErrorMessage = "必填")]
            public int fCategoryID { get; set; }

        }

        public string TempEfficacyListString { get; set; }
    }
}