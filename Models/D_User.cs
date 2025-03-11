using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MaterialGatePassTacker.Models

{
    [Table("D_User")]
    public class D_User
    {
        [Key]

        public int UID { get; set; }

        [Required(ErrorMessage = "User Name is required")]

        public string? User_Name { get; set; }

        [Required(ErrorMessage = "Role is required")]

        public int? RID { get; set; }

        [Required(ErrorMessage = "Password is required")]

        public string? Password { get; set; }

        [Required(ErrorMessage = "Email ID is required")]

        // [NotMapped]
        //  [ValidateNever]
        public string? Email_ID { get; set; }

        [Required(ErrorMessage = "Mobile No is required")]

        // [NotMapped]
        // [ValidateNever]

        public string? Mobile_No { get; set; }

        // [Required(ErrorMessage = "Active is required")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "The field Is Active must be checked.")]

        // [NotMapped]
        // [ValidateNever]

        public bool IsActive { get; set; }

        // [NotMapped]
        // [ValidateNever]
        public DateTime CreatedOn { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Action { get; set; }

        //[NotMapped]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DataType(DataType.Date)]
        [ValidateNever]
        public DateTime? ModifiedOn { get; set; }

    }


    

    public class D_UserViewModel
    {

        [ValidateNever]
        [NotMapped]
        public IEnumerable<SelectListItem>? Rolelist { get; set; }
        public D_User? d_user { get; set; }
        public D_User? d_user1 { get; set; }

        [NotMapped]

        public List<D_User>? d_userlist { get; set; }
   
    }
    
}
