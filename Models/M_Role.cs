using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("M_Role")]

    public class M_Role
    {
        [Key]
        public int RID { get; set; }
        public string? Role_Name { get; set; }
        public string? Role_Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss tt}")]
        [DataType(DataType.Date)]
        [ValidateNever]
        public DateTime? CreatedOn { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DataType(DataType.Date)]
        [ValidateNever]
        public DateTime? ModifiedOn { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<SelectListItem> Rolelist { get; set; }
    }
    public class M_RoleViewModel
    {
        public M_Role? m_Role { get; set; }

       

    }
}
