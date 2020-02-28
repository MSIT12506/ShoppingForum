using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.LineChatBot.ViewModels
{
    public class CLineAccountLink
    {
        public string lineNonce { get; set; }
        public string lineUserId { get; set; }
    }

    public class CLineUserId
    {
        public string lineUserId { get; set; }
    }
}