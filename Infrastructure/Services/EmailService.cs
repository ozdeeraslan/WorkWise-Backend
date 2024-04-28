using ApplicationCore.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using ApplicationCore.DTOs.UserDto;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly UserManager<AppUser> _userManager;

        public EmailService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public void SendResetPasswordMail(string userId, string mail)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("WorkWise", "workwiseinfomail@gmail.com"));
                email.To.Add(new MailboxAddress("User", mail));

                email.Subject = "Reset Password Link";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"<!DOCTYPE html>
                           <html lang='tr'>
                           <body>
                               <div class='container'>
                                    <div>
                                        <h2>{email.Subject}</h2>           
                                        <p>We have received a password reset order from you. Please click <a href='https://workwisereact.azurewebsites.net/reset-password/{userId}'>here</a> to reset your password.</p>
                                    </div>
                               </div>
                           </body>
                           </html>"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    smtp.Authenticate("workwiseinfomail@gmail.com", "jwvemmunubvifajv");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SendPassword(string mail, string password)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("WorkWise", "workwiseinfomail@gmail.com"));
                email.To.Add(new MailboxAddress("User", mail));

                email.Subject = "Mail and Password";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $@"<!DOCTYPE html>
                           <html lang='tr'>
                           <body>
                               <div class='container'>
                                    <div>
                                        <h2>{email.Subject}</h2>           
                                        <p>Your password and e-mail address are as follows. For your security, we recommend that you change your password.</p>
                                        <p>Email: <u>{mail}</u></p>                                        
                                        <p>Password: <u>{password}</u></p>
                                    </div>
                               </div>
                           </body>
                           </html>"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    smtp.Authenticate("workwiseinfomail@gmail.com", "jwvemmunubvifajv");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
