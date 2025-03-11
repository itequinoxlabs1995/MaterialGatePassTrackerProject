using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialGatePassTracker.Models
{
    public class GatePassRequest
    {
        public int Store_ID { get; set; }
        public string Action { get; set; } // "Assign" or "Reject"
    }
}
