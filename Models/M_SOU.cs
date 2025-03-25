using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialGatePassTacker.Models
{
    [Table("M_SOU")]

    public class M_SOU
    {
        [Key]
        public int SOUID { get; set; }
        public int GID { get; set; }
        public int PID { get; set; }
        public string? Sou_code { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public string? Modified_By { get; set; }
        public string? Created_By { get; set; }
    }

}
