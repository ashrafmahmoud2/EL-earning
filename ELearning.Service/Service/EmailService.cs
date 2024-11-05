using ELearning.Data.Settings;
using ELearning.Service.IService;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ELearning.Service.Service;
public class EmailService : IEmailService
{
    // Using in email Api https://app.mailjet.com/account/apikeys

    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new MailjetClient(_emailSettings.ApiKey, _emailSettings.SecretKey);

        var request = new MailjetRequest
        {
            Resource = Send.Resource,
        }
        .Property(Send.FromEmail, _emailSettings.SenderEmail)
        .Property(Send.FromName, _emailSettings.SenderName)
        .Property(Send.Subject, subject)
        .Property(Send.HtmlPart, body)
        .Property(Send.Recipients, new JArray {
            new JObject { { "Email", toEmail } }
        });

        var response = await client.PostAsync(request);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            Console.WriteLine($"Error sending email: {response.StatusCode}\n{response.GetData()}");
        }
    }
}
