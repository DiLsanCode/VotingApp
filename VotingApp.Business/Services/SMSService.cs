using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Interfaces;

namespace VotingApp.Business.Services
{
    public class SMSService : ISMSService
    {
        private readonly IConfiguration _config;

        public SMSService(IConfiguration config)
        {
            _config = config;
        }

        public void SendMessage(string password, string phone)
        {
            var message = new MailMessage();
            message.From = new MailAddress("voting.app.dv@gmail.com");

            message.To.Add(new MailAddress("359"+ phone + "@sms.yettel.bg"));
            //message.To.Add(new MailAddress("dilyan.vlahov@gmail.com"));
            message.Subject = "Kod za vlizane";
            message.Body = password;

            var smtpClient = new SmtpClient(_config.GetValue<string>("Smtp:Host"))
            {
                UseDefaultCredentials = false,
                Port = int.Parse(_config.GetValue<string>("Smtp:Port")),
                Credentials = new NetworkCredential(_config.GetValue<string>("Smtp:Username"), _config.GetValue<string>("Smtp:Password")),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
