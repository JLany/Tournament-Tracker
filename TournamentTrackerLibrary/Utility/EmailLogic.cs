using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TournamentTrackerLibrary.Utility;

public static class EmailLogic
{
    private static readonly string senderEmail = GlobalConfig.GetSenderEmailAddress();
    private static readonly string senderDisplayName = GlobalConfig.GetSenderDisplayName();
    private static readonly string senderEmailPassword = GlobalConfig.GetSenderEmailPassword();

    public static void SendEmail(string to, string subject, string body)
    {
        SendEmail(to, new List<string>(), subject, body);
    }

    public static void SendEmail(string to, List<string> bcc, string subject, string body)
    {
        var senderMailAddress = new MailAddress(senderEmail, senderDisplayName);

        var mail = new MailMessage();

        if (to.Length > 0)
        {
            mail.To.Add(to);
        }

        bcc.ForEach(x => mail.Bcc.Add(x));
        mail.From = senderMailAddress;
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential(senderEmail, senderEmailPassword),
        };

        client.SendAsync(mail, null);
    }
}
