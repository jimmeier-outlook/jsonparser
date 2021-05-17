using System.Net.Mail;
using System.Reflection;

namespace JsonParser
{
    public static class EmailHelper
    {
        public static void SendMail(string body, string environment, string admin, string projName, string projectLoaded = null)
        {
            var message = new MailMessage();
            message.Headers.Add("X-QAI-Process", "JSONProcessor");
            message.Headers.Add("X-QAI-Environment", environment);
            message.Headers.Add("X-QAI-Administration", admin);
            message.Body = body;
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            message.Subject = $"Error in {assemblyName} for project {projName}  in {environment}";
            message.To.Add("jmeier@questarai.com");
            message.To.Add("tacheson@questarai.com");
            message.To.Add("mteff@questarai.com");
            message.To.Add("dvattikonda@questarai.com");
            message.From = new MailAddress("qaitask@questarai.com");
            var client = new SmtpClient("smtp.questarai.com")
            {
                Port = 25,
                EnableSsl = false
            };
            client.Send(message);
        }
    }
}