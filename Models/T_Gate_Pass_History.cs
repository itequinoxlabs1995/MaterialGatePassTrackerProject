using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("T_Gate_Pass_History")]

    public class T_Gate_Pass_History
    {
        [Key]
        public int GPHID { get; set; }
        public int GPID { get; set; }
        public string Process_Name { get; set; }
        public DateTime Gate_Entry_DT { get; set; }
        public int Entered_UID { get; set; }
        public int Assigned_UID { get; set; }
        public string Reason { get; set; }
        public int SID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
