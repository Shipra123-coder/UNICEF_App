
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class FinancialYearListMO
    {
        public int? FinancialYear_Id { get; set; }
        public string Guid { get; set; }
        public string Code { get; set; }
        public string Year { get; set; }
        public string FinancialYear { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsCurrent { get; set; }
        public bool Status { get; set; }
    }

    public class FinancialYearCM
    {
        public int? FinancialYear_Id { get; set; }
        public string? Guid { get; set; }
        public string? Code { get; set; }

        // ✔ Year validation (Example: 2024)
        [Required(ErrorMessage = "Year is required")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Year must be 4 digits (e.g. 2024)")]
        public string? Year { get; set; }

        // ✔ Financial Year validation (Example: 2024-26)
        [Required(ErrorMessage = "Financial Year is required")]
        [RegularExpression(
            @"^(19|20)\d{2}\-\d{2}$",
            ErrorMessage = "Financial Year format must be like 2024-26"
        )]
        public string? FinancialYear { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }

        // UI helpers
        public string BtnSave { get; set; } = "Save";
        public List<FinancialYearCM> FinancialYear_List { get; set; } = new();
    }

}
