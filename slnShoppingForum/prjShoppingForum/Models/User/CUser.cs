using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace tw.com.essentialoil.User.Models
{
    public class CUser
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();

        public tUserProfile checkLineLogin(string account, string password)
        {
            tUserProfile cust = (from u in db.tUserProfiles
                                 where u.fUserId == account
                                 select u).FirstOrDefault();

            if (cust != null)
            {
                //進行SHA256加密
                string backpwd = password + cust.fPasswordSalt;
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] source = Encoding.Default.GetBytes(backpwd);
                byte[] crypto = sha256.ComputeHash(source);
                string result = Convert.ToBase64String(crypto);

                if (result==cust.fPassword)
                {
                    return cust;
                }

            }

            return null;
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
                newUser.fAccountLinkDatetime = DateTime.Now;
                nonce = resultEncode;

                db.tLineBotAccountLinks.Add(newUser);
                db.SaveChanges();

                result = true;   //第一次綁定
            }
            else
            {
                result = false;  //已經綁定過
            }

            return result;
        }
    }
}