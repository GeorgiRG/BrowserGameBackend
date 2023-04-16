namespace BrowserGameBackend.Types.Options
{
    public class EmailOptions
    {
        public const string OptionName = "Gmail";

        public string Sender { get; set; } = "";
        public string SenderPass { get; set; } = "";

    }
}
