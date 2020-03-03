using prjShoppingForum.Models.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace tw.com.essentialoil.Services
{
    public class CUsersDBService
    {
        dbShoppingForumEntities db = new dbShoppingForumEntities();
        Random r = new Random();

        public void New(tUserProfile c)
        {
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var salt = new StringBuilder();
            for (var i = 0; i < 16; i++)
            {
                salt.Append(str[r.Next(0, str.Length)]);
            }
            c.fPassword = HashPassword(c.fPassword, salt.ToString());
            c.fPasswordSalt = salt.ToString();
            c.fCreateDate = DateTime.UtcNow.AddHours(8);
            c.fScore = 0;
            c.fAuth = "1";
            c.fAuthPost = true;
            c.fAuthReply = true;
            db.tUserProfiles.Add(c);
            db.SaveChanges();
        }
      
        public string HashPassword(string Password, string salt)
        {
            var addsalt = Password + salt;
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(addsalt);
            byte[] crypto = sha256.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);
            return  result;
        }
      
        public tUserProfile GetByUserId(string Account)
        {
            tUserProfile Data = new tUserProfile();
            try
            {
                Data = db.tUserProfiles.FirstOrDefault(p => p.fUserId == Account);
            }
            catch (Exception e)
            {
                Data = null;
            }
            return Data;
        }     

    
        //信箱驗證
        public string EmailCheck(string Account, string AuthCode)
        {
            tUserProfile UserValidate = GetByUserId(Account);
            string ValidateString = "";
            if (UserValidate != null)
            {
                if (UserValidate.fAuthCode == AuthCode)
                {
                    try
                    {
                        UserValidate.fAuthCode = "0";
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message.ToString());
                    }
                    ValidateString = "帳號信箱驗證成功，您已經可以登入帳戶";
                }
                else
                {
                    ValidateString = "驗證碼錯誤，請重新輸入";
                }
            }
            else
            {
                ValidateString = "資料傳送錯誤，請重新確認或註冊";
            }
            return ValidateString;
        }      
       
        public bool PasswordCheck(tUserProfile CheckUser, string Password, string salt)
        {      
            bool result = CheckUser.fPassword.Equals(HashPassword(Password, salt));      
            return result;
        }      
      
        public string LoginCheck(string Account, string Password, string salt)
        {
 
            tUserProfile LoginedUser = GetByUserId(Account);  
            if (LoginedUser != null)
            {
      
                if (String.IsNullOrWhiteSpace(LoginedUser.fAuthCode))                
                {
       
                    if (PasswordCheck(LoginedUser, Password, salt))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                }
                else
                {
                    return "此帳號尚未經過Email驗證，請去收信";
                }
            }
            else
            {
                return "無此會員帳號，請去註冊";
            }
        }
       
            
        public string GetRole(string fUserId)
        {    
            string Role = "User";  
            tUserProfile cust = GetByUserId(fUserId);           
            return Role;
        }

    }
}