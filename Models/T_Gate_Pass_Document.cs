using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("T_Gate_Pass_Document")]

    public class T_Gate_Pass_Document
    {
        [Key]
        public int GPDID { get; set; }
        public int GPID { get; set; }
        public string BlobURL { get; set; }
        public string DocumentType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
