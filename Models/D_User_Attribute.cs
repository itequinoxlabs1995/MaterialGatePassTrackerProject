using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("D_User_Attribute")]

    public class D_User_Attribute
    {
        [Key]
        public int UAID { get; set; }
        public int UID { get; set; }
        public int RID { get; set; }
        public int PID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
