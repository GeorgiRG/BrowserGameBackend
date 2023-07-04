namespace BrowserGameBackend.Services.Utilities
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow();


    }
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        { 
            return DateTime.UtcNow;
        }
    }
}
