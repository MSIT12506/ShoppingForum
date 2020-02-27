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
    
    public partial class tUserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tUserProfile()
        {
            this.tForums = new HashSet<tForum>();
            this.tForums1 = new HashSet<tForum>();
            this.tForumAnalysis = new HashSet<tForumAnalysi>();
            this.tForumReplies = new HashSet<tForumReply>();
            this.tForumReplies1 = new HashSet<tForumReply>();
            this.tForumReplyAnalysis = new HashSet<tForumReplyAnalysi>();
            this.tOrders = new HashSet<tOrder>();
            this.tShoppingCarts = new HashSet<tShoppingCart>();
            this.tUserBrowseHistories = new HashSet<tUserBrowseHistory>();
            this.tUserDiscountLists = new HashSet<tUserDiscountList>();
            this.tUserProductFavorites = new HashSet<tUserProductFavorite>();
            this.tUserLogs = new HashSet<tUserLog>();
        }
    
        public int fId { get; set; }
        public string fUserId { get; set; }
        public string fPassword { get; set; }
        public string fPasswordSalt { get; set; }
        public string fName { get; set; }
        public string fGender { get; set; }
        public System.DateTime fBirthday { get; set; }
        public string fTel { get; set; }
        public string fPhone { get; set; }
        public string fCity { get; set; }
        public string fAddress { get; set; }
        public string fPhoto { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public Nullable<int> fScore { get; set; }
        public string fAuth { get; set; }
        public bool fAuthPost { get; set; }
        public bool fAuthReply { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tForum> tForums { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tForum> tForums1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tForumAnalysi> tForumAnalysis { get; set; }
        public virtual tForumAuth tForumAuth { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tForumReply> tForumReplies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tForumReply> tForumReplies1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tForumReplyAnalysi> tForumReplyAnalysis { get; set; }
        public virtual tLineBotAccountLink tLineBotAccountLink { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrder> tOrders { get; set; }
        public virtual tScore tScore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tShoppingCart> tShoppingCarts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserBrowseHistory> tUserBrowseHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserDiscountList> tUserDiscountLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserProductFavorite> tUserProductFavorites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserLog> tUserLogs { get; set; }
    }
}
