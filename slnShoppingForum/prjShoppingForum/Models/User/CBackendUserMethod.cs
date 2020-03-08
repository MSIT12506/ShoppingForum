using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tw.com.essentialoil.User.Models
{
    public class CBackendUserMethod
    {   
        //後台使用:會員管理
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        public tUserProfile GetUserById(int fId)
        {
            tUserProfile buser = db.tUserProfiles.Where(p => p.fId == fId).FirstOrDefault();
            return buser;
        }

        public tUserProfile GetUserByUserId(string fUserId)
        {
            tUserProfile buser = db.tUserProfiles.Where(p => p.fUserId == fUserId).FirstOrDefault();
            return buser;
        }

        public tUserProfile GetUserByName(string fName)
        {
            tUserProfile buser = db.tUserProfiles.Where(p => p.fName == fName).FirstOrDefault();
            return buser;
        }

        public tUserProfile GetUserByCity(string fCity)
        {
            tUserProfile buser = db.tUserProfiles.Where(p => p.fCity == fCity).FirstOrDefault();
            return buser;
        }

        //會員停權
        //public void tUserProfile UserAuth(int fId)
        //{
        //    tUserProfile buserauth = (from i in db.tUserProfiles
        //                              where i.fId == fId && i.fAuth == "1"
        //                              select i).FirstOrDefault();

        //    if (buserauth != null)
        //    {
        //        buserauth.fAuth = "0";
        //        db.SaveChanges();
        //    }

        //}

    }
}