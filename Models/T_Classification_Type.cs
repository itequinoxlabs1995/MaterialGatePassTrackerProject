using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("T_Classification_Type")]
    public class T_Classification_Type
    {
        [Key]
        public int Classification_ID { get; set; }
        public string Classification_Name { get; set; }

    }
}
