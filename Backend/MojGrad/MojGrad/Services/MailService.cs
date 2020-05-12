using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MojGrad.Models;

namespace MojGrad.Services
{
    public class MailService
    {
        SmtpClient client;
        MimeMessage message;

        public MailService()
        {
            client = new SmtpClient();
            message = new MimeMessage();
            client.Connect("smtp.gmail.com", 587);
            client.Authenticate("moj.grad.royal@gmail.com", "mojgrad@royal");
            message.From.Add(new MailboxAddress("Moj Grad", "moj.grad.royal@gmail.com"));
        }

        public bool sendEmail(string imePrezime, string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Moj Grad", "moj.grad.royal@gmail.com"));
                message.To.Add(new MailboxAddress(imePrezime, email));

                message.Subject = subject;
                message.Body = new TextPart
                {
                    Text = body
                };

                client.Send(message);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void disconect()
        {
            client.Disconnect(true);
        }

        public bool sendRecoveryMail(Korisnik u)
        {
            var message = new MimeMessage();
            MojToken t = new MojToken();
            var token = t.dajRecToken(u.Id);
            message.From.Add(new MailboxAddress("Moj Grad", "moj.grad.royal@gmail.com"));
            message.To.Add(new MailboxAddress(u.Ime+" "+u.Prezime, u.Email));
           
            message.Subject = "Password recovery";
            
            BodyBuilder b = new BodyBuilder();
            b.HtmlBody = "<h1>Zaboravili ste lozinku</h1>";
            b.HtmlBody += "link za oporavak je <a href='"+URL.RecoveryUrl+"/t/"+token+"'>link</a>";
            
            message.Body = b.ToMessageBody();
            
            client.Send(message);

            return true;
        }
      
    }
}
