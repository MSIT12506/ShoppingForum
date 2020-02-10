using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tProductVegetableoilMetadata))]
    public partial class tProductVegetableoil
    {
        public class tProductVegetableoilMetadata
        {
            [DisplayName("商品編號")]
            public int fProductID { get; set; }

            [DisplayName("特性編號")]
            public Nullable<int> ffeatureID { get; set; }

        }

    }
}