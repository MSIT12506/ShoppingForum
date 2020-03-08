using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.User.Models
{ 
    //前台使用
    public class CUserDataMethod
    {
       
        private dbShoppingForumEntities db = new dbShoppingForumEntities();

        public tUserProfile GetUserProfile(string fUserId)
        {
            tUserProfile user = db.tUserProfiles.Where(p => p.fUserId == fUserId).FirstOrDefault();
            return user;
        }

    }
}