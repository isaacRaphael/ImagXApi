using ImagX_API.Services.HelperClasses;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Services
{
    public class EmailService
    {
        private readonly MailjetObj _options;

        public EmailService(IOptions<MailjetObj> options)
        {
            _options = options.Value;
        }

        public async Task SendMail(EmailModel model)
        {
            MailjetClient client = new MailjetClient(_options.Key, _options.Secret);
            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
                .Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "raphael.isaac@thebulbafrica.institute"},
                  {"Name", "IMagX"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", model.Receipient},
                   {"Name", "User"}
                   }
                  }},
                 {"Subject", model.Title},
                 {"TextPart", "Greetings from IMagX!"},
                 {"HTMLPart", $"{model.Body}"}
                 }
                });
            try
            {
                MailjetResponse response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                    Console.WriteLine(response.GetData());
                }
                else
                {
                    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                    Console.WriteLine(response.GetData());
                    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
