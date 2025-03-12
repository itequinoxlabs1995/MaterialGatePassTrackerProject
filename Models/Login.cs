using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models

{
    [Table("D_User")]

    public class Login
    {


        [Key]

        public int UID { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        // [Remote("CheckPassIsAvailable", "User_Name")]

        public string? User_Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        // [Remote("CheckPassIsAvailable", "Enter Valid Password")]

        public string? Password { get; set; }

        // [Required(ErrorMessage = "Email ID is required")]
    }
}
