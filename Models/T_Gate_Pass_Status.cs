using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialGatePassTracker.Models
{
    [Table("T_Gate_Pass_Status")]
    public class T_Gate_Pass_Status
    {
        [Key]
        public int GPSID { get; set; }
        public int GPID { get; set; }
        public string? AssignUser { get; set; }
        public string? AssignStore { get; set; }
        public string? ReassignUser { get; set; }
        public string? ReassignStore { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? Status { get; set; }
    }

}
