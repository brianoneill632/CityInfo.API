namespace CityInfo.API.Services
{
    public class CloudMailService(IConfiguration configuration) : IMailService
    {
        private readonly string? _mailTo = configuration["mailSettings:MailTo"];
        private readonly string? _mailFrom = configuration["mailSettings:MailFrom"];

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
