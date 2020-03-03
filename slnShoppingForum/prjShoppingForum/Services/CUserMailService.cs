using prjShoppingForum.Models.Entity;
using prjShoppingForum.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace tw.com.essentialoil.Services
{
    public class CUserMailService
    {        
        CUserDataMethod membersService = new CUserDataMethod();
        Random r = new Random();

        private string gmail_account = ""; 
        private string gmail_password = ""; 
        private string gmail_mail = ""; 
  
        public string GetSalt()
        {
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var newrdn = new StringBuilder();
            for (var i = 0; i < 16; i++)
            {
                newrdn.Append(str[r.Next(0, str.Length)]);
            }
            return newrdn.ToString();
        }

        public string GetNewPassword()
        {
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var newpwd = new StringBuilder();
            for (var i = 0; i < 8; i++)
            {
                newpwd.Append(str[r.Next(0, str.Length)]);
            }    
            return newpwd.ToString();
        }
        
        public string GenerateNewMailContent(string TempString, string UserName, string ValidateUrl)
        {         
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);           
            return TempString;
        }

        //寄驗證信的方法(待測試修正)
        public void SendSignUpMail(string UserName, string newrdn)
        {
            string fName = membersService.GetUserProfile(UserName).fName.ToString();
            var senderEmail = new MailAddress("isgoldAoil@gmail.com", "ESSENCE SHOP");//管理員寄email所用的信箱，若要測試請填自己可用的email
            var receiverEmail = new MailAddress(UserName, fName);
            var password = "Cai3M!Ef6Z";//管理員寄email所用的信箱密碼，測試時請自行填入
            var sub = "會員驗證碼";
            var body = "恭喜您註冊為本店會員，請用下面的驗證碼進行登入:" + newrdn + " " + "!";
            //mail.IsBodyHtml = true;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                mess.IsBodyHtml = true;
                smtp.Send(mess);
            }
        }

        //寄忘記密碼的方法: OK
        public void SendNewMail(string UserName, string newrdn)
        {
            var senderEmail = new MailAddress("isgoldAoil@gmail.com", "ESSENCE SHOP");//管理員寄email所用的信箱，若要測試請填自己可用的email
            var receiverEmail = new MailAddress(UserName.ToString(), "Receiver");
            var password = "Cai3M!Ef6Z";//管理員寄email所用的信箱密碼，測試時請自行填入
            var sub = "更換密碼通知";
            var body = "您好，已收到您的忘記密碼請求，請用下面的新密碼重新登入:" + newrdn + " " + "!";    

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                mess.IsBodyHtml = true;
                smtp.Send(mess);
            }
        }
    }
}