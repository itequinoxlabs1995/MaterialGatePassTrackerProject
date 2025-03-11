using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("M_Project")]

    public class M_Project
    {
        [Key]
        public int PID { get; set; }
        public int? SOUID { get; set; }
        public int? GID { get; set; }
        public string? Project_Name { get; set; }
        public string? Project_Description { get; set; }
        //public string? Unit { get; set; }
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
        [ValidateNever]
        public string? Action { get; set; }


    }
    public class M_ProjectViewModel
    {
        [ValidateNever]
        [NotMapped]
        public IEnumerable<SelectListItem>? Gatelist { get; set; }
        public M_Project? m_Project { get; set; }
        public M_Project? m_Project1 { get; set; }



    }
}
