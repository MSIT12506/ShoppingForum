//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace prjShoppingForum.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tDiscount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tDiscount()
        {
            this.tUserDiscountLists = new HashSet<tUserDiscountList>();
        }
    
        public string fDiscountCode { get; set; }
        public string fDiscountName { get; set; }
        public string fDiscountCategory { get; set; }
        public bool fDiscountMoneyRule { get; set; }
        public int fMoneyLimit { get; set; }
        public decimal fDiscountContent { get; set; }
        public System.DateTime fStartdate { get; set; }
        public System.DateTime fEndDate { get; set; }
        public bool fEnable { get; set; }
        public int fCount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserDiscountList> tUserDiscountLists { get; set; }
    }
}
