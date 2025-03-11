using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("M_Gate")]

    public class M_Gate
    {
        [Key]
        public int GID { get; set; }
        public int? Gate_No { get; set; }
        public string? Gate_Location { get; set; }
        public int? PID { get; set; }
        public bool IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss tt}")]
        [DataType(DataType.Date)]
        [ValidateNever]
        public DateTime CreatedOn { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss tt}")]
        [DataType(DataType.Date)]
        [ValidateNever]
        public DateTime? ModifiedOn { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Gatelist { get; set; }

    }
}
