using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tOrderMetaData))]
    public partial class tOrder
    {
        public class tOrderMetaData
        {
            public long fOrderId { get; set; }
            public int fId { get; set; }
            public System.DateTime fOrderDate { get; set; }
            public Nullable<System.DateTime> fShippedDate { get; set; }
            public Nullable<System.DateTime> fRequiredDate { get; set; }
            public Nullable<int> fScore { get; set; }
            public string fConsigneeName { get; set; }
            public string fConsigneeTelephone { get; set; }
            public string fConsigneeCellPhone { get; set; }
            public string fConsigneeAddress { get; set; }
            public string fOrderCompanyTitle { get; set; }
            public Nullable<int> fOrderTaxIdDNumber { get; set; }
            public string fOrderPostScript { get; set; }
            public string fPayment { get; set; }
    
            public virtual tUserProfile tUserProfile { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<tOrderDetail> tOrderDetail { get; set; }
        }
    }
}
