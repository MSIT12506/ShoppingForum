using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tUserProfileMetaData))]

    public partial class tUserProfile
    {
        public class tUserProfileMetaData
        {
            [DisplayName("會員編號")]
            public int fId { get; set; }

            [DisplayName("帳號")]
            [Required(ErrorMessage = "請輸入Email作為後續您登入的帳號")]
            [StringLength(200, ErrorMessage = "Email長度最多200字元")]
            [EmailAddress(ErrorMessage = "這不是Email格式")]
            [Remote("NewUserAjax", "FrontUserProfile", ErrorMessage = "此帳號已被註冊過")]
            public string fUserId { get; set; }

            [DisplayName("密碼")]
            [Required(ErrorMessage = "必填")]
            [StringLength(50, MinimumLength = 6, ErrorMessage = "密碼至少需6字")]
            public string fPassword { get; set; }

            [DisplayName("密碼加鹽")]
            public string fPasswordSalt { get; set; }

            [DisplayName("姓名")]
            [Required(ErrorMessage = "必填")]
            public string fName { get; set; }

            [DisplayName("性別")]
            [Required(ErrorMessage = "必填")]
            public string fGender { get; set; }

            [DisplayName("生日")]
            //[Required(ErrorMessage = "必填")]
            public Nullable<System.DateTime> fBirthday { get; set; }

            [DisplayName("電話")]
            public string fTel { get; set; }

            [DisplayName("手機")]
            [Required(ErrorMessage = "必填")]
            public string fPhone { get; set; }

            [DisplayName("城市")]
            //[Required(ErrorMessage = "必填")]
            public string fCity { get; set; }

            [DisplayName("地址")]
            //[Required(ErrorMessage = "必填")]
            public string fAddress { get; set; }
            [DisplayName("相片")]
            public string fPhoto { get; set; }
            [DisplayName("註冊時間")]
            public System.DateTime fCreateDate { get; set; }
            [DisplayName("積分")]
            public Nullable<int> fScore { get; set; }
            [DisplayName("會員權限")]
            public string fAuth { get; set; }
            [DisplayName("發文權限")]
            public bool fAuthPost { get; set; }
            [DisplayName("留言權限")]
            public bool fAuthReply { get; set; }
            [DisplayName("信箱驗證碼")]
            public string fAuthCode { get; set; }
            [DisplayName("重設密碼驗證碼")]
            public string fpw_reset_AuthCode { get; set; }
        }
    }
}
