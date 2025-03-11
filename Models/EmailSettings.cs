using System.Collections.Generic;


namespace MaterialGatePassTracker.Models
{
    public class EmailSettings
    {
        public List<string> ToRecipients { get; set; } = new();
        public List<string> CCRecipients { get; set; } = new();
        public List<string> BCCRecipients { get; set; } = new();

        public required string ApiUrl { get; set; }
        public required string AuthKey { get; set; }
    }
}
