using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tUserProfileMetaData))]

    public partial class tUserProfile
    {
        public class tUserProfileMetaData
        {
        public int fId { get; set; }
        public string fUserId { get; set; }
        public string fPassword { get; set; }
        public string fPasswordSalt { get; set; }
        [DisplayName("±b¸¹")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrder> tOrders { get; set; }
        public virtual tScore tScore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tShoppingCart> tShoppingCarts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserBrowseHistory> tUserBrowseHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserDiscountList> tUserDiscountLists { get; set; }
        public virtual tUserLog tUserLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserProductFavorite> tUserProductFavorites { get; set; }
        }

        
    }
}
