using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialGatePassTracker.Models
{
    [Table("Store")]
    public class Store
    {
        [Key]
        public int Store_ID { get; set; }
        public string? Store_Name_Desc { get; set; }
        public bool ISactive { get; set; }
        public string? Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public DateTime? Modified_On { get; set; }
        public string? Modified_By { get; set; }
        public int PID { get; set; } // Foreign key to T_Project
    }

}
