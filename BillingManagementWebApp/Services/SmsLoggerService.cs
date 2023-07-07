namespace BillingManagementWebApp.Services
{
    public class SmsLoggerService : ILoggerService
    {
        public void Write(string message)
        {
            Console.WriteLine("[SmsLogger] - " + message);
        }
    }
}
