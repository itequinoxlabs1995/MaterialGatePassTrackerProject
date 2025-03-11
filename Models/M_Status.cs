using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("M_Status")]

    public class M_Status
    {
        [Key]
        public int SID { get; set; }
        public string Status_Name { get; set; }
        public string Order_No { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
