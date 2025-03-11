namespace MaterialGatePassTracker.Models
{
    public class GatePassStatusRequest
    {
        public int GatePassId { get; set; }
        public string Action { get; set; }
        public string? Store { get; set; } // This holds the UserName for reassignment
    }
}
