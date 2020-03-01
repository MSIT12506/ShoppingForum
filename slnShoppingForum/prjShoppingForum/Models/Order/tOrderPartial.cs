using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjShoppingForum.Models.Entity
{
    [MetadataType(typeof(tOrderMetaData))]
    public partial class tOrder
    {
        public class tOrderMetaData
        {
            [DisplayName("�q��s��")]
            public long fOrderId { get; set; }
            [DisplayName("�Ȥ�s��")]
            public int fId { get; set; }
            [Display(Name = "�q��إߤ��")]
            //[Range(DateTime.Now.Day, 999.99)]
            [DataType(DataType.Date, ErrorMessage = "�п�J���T������榡")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            [Required(ErrorMessage = "�п�J���T������榡")]
            public System.DateTime fOrderDate { get; set; }
            [DisplayName("�q��X�f���")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> fShippedDate { get; set; }
            [DisplayName("�Ʊ�e�F��")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> fRequiredDate { get; set; }
            [DisplayName("�ϥοn��")]
            public Nullable<int> fScore { get; set; }
            [DisplayName("�ϥ��u�f��")]
            public string fDiscountCode { get; set; }
            [DisplayName("���f�̩m�W")]
            public string fConsigneeName { get; set; }
            [DisplayName("���f�̥���")]
            public string fConsigneeTelephone { get; set; }
            [DisplayName("���f�̤�����X")]
            public string fConsigneeCellPhone { get; set; }
            [DisplayName("���f�̦a�}")]
            public string fConsigneeAddress { get; set; }
            [DisplayName("�o�����q���Y")]
            public string fOrderCompanyTitle { get; set; }
            [DisplayName("�Τ@�s��")]
            public Nullable<int> fOrderTaxIdDNumber { get; set; }
            [DisplayName("�q��Ƶ�")]
            public string fOrderPostScript { get; set; }
            [DisplayName("��I�覡")]
            public string fPayment { get; set; }
        }
    }
}
