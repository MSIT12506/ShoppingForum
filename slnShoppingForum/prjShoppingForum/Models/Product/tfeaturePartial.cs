using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tfeatureMetadata))]
    public partial class tfeature
    {
        public class tfeatureMetadata
        {
            [DisplayName("特性編號")]
            public int ffeatureID { get; set; }

            [DisplayName("特性")]
            public string ffeatureName { get; set; }
        }
    }
}