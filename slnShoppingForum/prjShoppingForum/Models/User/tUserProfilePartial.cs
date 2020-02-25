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
        [DisplayName("�|���s��")]
        public int fId { get; set; }

        [DisplayName("�H�c")]
        [Required(ErrorMessage ="����")]
        public string fUserId { get; set; }

        [DisplayName("�K�X")]
        [Required(ErrorMessage = "����")]
        public string fPassword { get; set; }

        [DisplayName("�K�X�[�Q")]
        public string fPasswordSalt { get; set; }

        [DisplayName("�m�W")]
        [Required(ErrorMessage = "����")]
        public string fName { get; set; }

        [DisplayName("�ʧO")]
        [Required(ErrorMessage = "����")]
        public string fGender { get; set; }

        [DisplayName("�ͤ�")]
        [Required(ErrorMessage = "����")]
        public System.DateTime fBirthday { get; set; }

        [DisplayName("�q��")]
        public string fTel { get; set; }

        [DisplayName("���")]
        [Required(ErrorMessage = "����")]
        public string fPhone { get; set; }
        [DisplayName("����")]
        [Required(ErrorMessage = "����")]
        public string fCity { get; set; }

        [DisplayName("�a�}")]
        [Required(ErrorMessage = "����")]
        public string fAddress { get; set; }
        [DisplayName("�ۤ�")]
        public string fPhoto { get; set; }
        [DisplayName("���U�ɶ�")]
        public System.DateTime fCreateDate { get; set; }
        [DisplayName("�n��")]
        public Nullable<int> fScore { get; set; }
        [DisplayName("�|���v��")]
        public string fAuth { get; set; }
        [DisplayName("�o���v��")]
        public bool fAuthPost { get; set; }
        [DisplayName("�d���v��")]
        public bool fAuthReply { get; set; }
        }
    }
}
