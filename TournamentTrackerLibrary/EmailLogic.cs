﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TournamentTrackerLibrary;

public static class EmailLogic
{
    private static readonly string senderEmail = GlobalConfig.GetSenderEmailAddress();
    private static readonly string senderDisplayName = GlobalConfig.GetSenderDisplayName();
    private static readonly string senderEmailPassword = GlobalConfig.GetSenderEmailPassword();

    public static void SendEmail(List<string> to, string subject, string body)
    {
        var senderMailAddress = new MailAddress(senderEmail, senderDisplayName);

        var mail = new MailMessage();
        to.ForEach(x => mail.To.Add(x));
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

        // TODO - Implement actual mail sending.
    }

    public static bool IsValidEmail(string email)
    {
        var validEmail = new Regex("^[\\w!#$%&'*+/=?^`{|}~-]+(\\.[\\w!#$%&'*+/=?^`{|}~-]+)*@(?:[\\w-]+\\.)+[a-zA-Z]{2,63}$");
        return validEmail.IsMatch(email);
    }
}
