using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaterialGatePassTacker.Models
{
    [Table("T_Gate_Pass")]

    public class T_Gate_Pass
    {
        [Key]
        public int GPID { get; set; }

        public int PID { get; set; }
		[ValidateNever]
		[NotMapped]

		public int GID { get; set; }
		[ValidateNever]

        public int Gate_ID { get; set; }

		
		[ValidateNever]
        public int SID { get; set; }
        [ValidateNever]

        public string? Vendor_Name { get; set; }
        [ValidateNever]

        public string? DO_Number { get; set; }
        [ValidateNever]

        public string? PO_Number { get; set; }
        [ValidateNever]

        public string? Driver_Name { get; set; }
        [ValidateNever]

        public string? Driver_ID { get; set; }
        [ValidateNever]

        public string? Driver_MobileNo { get; set; }
        [ValidateNever]

        public string? Vehicle_No { get; set; }
        [ValidateNever]

        public string? Clasification { get; set; }
        [ValidateNever]

        public string? CreatedBy { get; set; }

        [ValidateNever]
        public string? ModifiedBy { get; set; }
       // public bool IsActive { get; set; }
        [ValidateNever]
        public DateTime CreatedOn { get; set; }

        [ValidateNever]
        public string? FilePaths { get; set; }

        [NotMapped]
        [ValidateNever]
         public DateTime? ModifiedOn { get; set; }

		[NotMapped]
		[ValidateNever]
		public string? StartDate { get; set; }
		[ValidateNever]
		[NotMapped]

		public string? EndDate { get; set; }
		[ValidateNever]
		[NotMapped]

		public string? filters { get; set; }

		/*public T_Gate_Pass()
		{
			this.Soues = new List<SelectListItem>();
			this.Projects = new List<SelectListItem>();
			this.Gatess = new List<SelectListItem>();

		}
		public List<SelectListItem> Soues { get; set; }

		public List<SelectListItem> Projects { get; set; }

		public List<SelectListItem> Gatess { get; set; }

        */

		[NotMapped]
		[ValidateNever]
		public int SOUID { get; set; }

		[ValidateNever]
		[NotMapped]
		public IEnumerable<SelectListItem>? Soulist { get; set; }


		
		[ValidateNever]
		[NotMapped]
		public IEnumerable<SelectListItem>? Projectlist { get; set; }

		[ValidateNever]
		[NotMapped]
		public IEnumerable<SelectListItem>? Gatelist { get; set; }


	}
}
