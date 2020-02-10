using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tEfficacyMetadata))]
    public partial class tEfficacy
    {
        public class tEfficacyMetadata
        {
            [DisplayName("功效編號")]
            public int fEfficacyID { get; set; }

            [DisplayName("功效")]
            public string fEfficacyName { get; set; }

        }
    }
}