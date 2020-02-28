using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjShoppingForum.Models.Entity
{
        [MetadataType(typeof(tNewsMessageMetaData))]
        public partial class tNewsMessage
        {
            public class tNewsMessageMetaData
            {

            [DisplayName("訊息編號")]
            public int fMessageId { get; set; }
            [DisplayName("消息編號")]
            public int fNewsId { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime fMessageTime { get; set; }
            [DisplayName("留言內容")]
            public string fMessageArticle { get; set; }
            [DisplayName("留言者")]
            public string fM_AddUser { get; set; }

            public virtual tNew tNew { get; set; }
            }
        }
    }
