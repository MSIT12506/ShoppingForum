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
    
    public partial class tNew
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tNew()
        {
            this.tNewsMessages = new HashSet<tNewsMessage>();
        }
    
        public int fNewsId { get; set; }
        public System.DateTime fNewsStart { get; set; }
        public System.DateTime fNewsEnd { get; set; }
        public string fClass { get; set; }
        public string fNewsTitle { get; set; }
        public string fNewsDesc { get; set; }
        public string fNewsArticle { get; set; }
        public Nullable<int> fNewsTag { get; set; }
        public Nullable<int> fGet_No { get; set; }
        public string fAddUser { get; set; }
        public string fChangUser { get; set; }
        public string fDeleteUser { get; set; }
        public string fApproved { get; set; }
        public Nullable<bool> fNewsDiscontinue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tNewsMessage> tNewsMessages { get; set; }
    }
}
