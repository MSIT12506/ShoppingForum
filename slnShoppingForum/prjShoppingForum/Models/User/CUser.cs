using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace tw.com.essentialoil.User.Models
{
    public class CUser
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        public tUserProfile checkLogin(string account, string password)
        {
            tUserProfile cust = (from u in db.tUserProfiles
                                 where u.fUserId == account && u.fPassword == password
                                 select u).FirstOrDefault();

            return cust;
        }

        public bool lineBotGetBase64String(tUserProfile cust, ref string nonce)
        {
            bool result;

            //產生GUID並且轉換BASE64作為綁定字串
            byte[] bytesEncode = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            string resultEncode = Convert.ToBase64String(bytesEncode);

            //把結果寫入資料庫
            tLineBotAccountLink user = (from u in db.tLineBotAccountLinks
                                        where cust.fId == u.fId
                                        select u).FirstOrDefault();

            if (user==null)
            {
                tLineBotAccountLink newUser = new tLineBotAccountLink();
                newUser.fId = cust.fId;
                newUser.fLineNonce = resultEncode;
                nonce = resultEncode;

                db.tLineBotAccountLinks.Add(newUser);
                db.SaveChanges();

                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}