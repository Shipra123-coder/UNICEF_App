using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.BudgetMaster
{
    public class VettingList
    {
        public int Id { get; set; }
        public string Gatividhi { get; set; }
        public string GatividhiName { get; set; }
        public decimal? Budget { get; set; }
        public int QuestionMasterId { get; set; }
        public int SubQuestionMasterId { get; set; }
        public string AnswerValue { get; set; }
        public long GauravId { get; set; }
        public long CreatedBy { get; set; }
        public long rowId { get; set; }
        public int DistrictId { get; set; }
        public string Fieldtype { get; set; }
        public int? SendForVerification { get; set; }
    }
    public class VettingRowVM
    {
        public int RowId { get; set; }
        public int GauravId { get; set; }
        public int DistrictId { get; set; }
        public int QuestionMasterId { get; set; }
        public int SubQuestionMasterId { get; set; }
        public string AnswerValue { get; set; }
        public string Fieldtype { get; set; }
        public int? SendForVerification { get; set; }
        public Dictionary<int, string> Columns { get; set; } = new();
        public List<VettingDetailVM> Details { get; set; } = new();
    }
    public class VettingDetailVM
    {
        public int QuestionMasterId { get; set; }
        public int SubQuestionMasterId { get; set; }
        public string AnswerValue { get; set; }
        public string Fieldtype { get; set; }
    }
    public class VettingSaveVM
    {
        public int? UserId { get; set; }
        public int? RowId { get; set; }
        public string Activity { get; set; }
        public string ActivityName { get; set; }
        public decimal? Budget { get; set; }
        public int? GauravId { get; set; }
        public int? DistrictId { get; set; }
        public int? FinancialYear_Id { get; set; }
        public decimal? Nodal { get; set; }
        public decimal? TotalProposed { get; set; }
        public decimal? MPLAD { get; set; }
        public decimal? CSR { get; set; }
        public decimal? other { get; set; }
        public decimal? panchgaurav { get; set; }
        public string? workplan { get; set; }
        public int? VettedByDepartment { get; set; }
        public int? SendForVerification { get; set; }
        public decimal? TotalProposedByDist { get; set; }
        public decimal? TotalPanchgauravByDist { get; set; }
        public decimal? TotalProposedByDept { get; set; }
        public decimal? TotalPanchgauravByDept { get; set; }
    } 
    public class VettedQuestions
    {
        public int CurrentStep { get; set; }
        public string GauravGuid { get; set; }
        public string statusClass { get; set; }
        public List<QuestionModel_VettedQuestions> mainquestion { get; set; }
    }
    public class QuestionModel_VettedQuestions
    {
        public int CurrentStep { get; set; }
        public long QuestionMasterId { get; set; }
        public string Guid { get; set; }
        public string DisplayNumber { get; set; }
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }


        public List<SubQuestionModel_VettedQuestions> SubQuestions { get; set; } = new();
    }
    public class SubQuestionModel_VettedQuestions
    {
        public long SubQuestionMasterId { get; set; }
        public string Guid { get; set; }
        public string DisplayNumber { get; set; }
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public string Fieldtype { get; set; }
        public int QuestionType { get; set; }
    }
    public class VettedQuestionsSaveModel
    {
        [Required(ErrorMessage = "Activity is required")]
        public string ActivityId { get; set; }

        [Required(ErrorMessage = "Activity Name is required")]
        public string ActivityName { get; set; }
        public string activityText { get; set; }

        [Required(ErrorMessage = "Total Proposed is required")]
        public decimal? TotalProposed { get; set; }

        [Required(ErrorMessage = "Nodal Amount is required")]
        public decimal? NodalAmount { get; set; }

        [Required(ErrorMessage = "MPLAD Amount is required")]
        public decimal? MPLADAmount { get; set; }

        [Required(ErrorMessage = "CSR Amount is required")]
        public decimal? CSRAmount { get; set; }

        [Required(ErrorMessage = "Other Amount is required")]
        public decimal? OtherAmount { get; set; }

        [Required(ErrorMessage = "Panch Gaurav Amount is required")]
        public decimal? PanchGauravAmount { get; set; }

        [Required(ErrorMessage = "Work Plan is required")]
        public string ?WorkPlan { get; set; }

        [Required(ErrorMessage = "Completion Date is required")]
        public string ?CompletionDate { get; set; }
    }
    public class SaveVettedRequest
    {
        public List<VettedQuestionsSaveModel> Answers { get; set; }
        public VettedAmountModel ModelData { get; set; }
    }
    public class VettedAmountModel
    {
        public decimal? NodalAmount { get; set; }
        public decimal? MPLADAmount { get; set; }
        public decimal? CSRAmount { get; set; }
        public decimal? OtherAmount { get; set; }
        public decimal? PanchGauravAmount { get; set; }
        public decimal? TotalProposed { get; set; }
    }
    public class  SummeryReportBudget
    {
        public decimal? TotalProposedByDist { get; set; }
        public decimal? TotalPanchgauravByDist { get; set; }
        public decimal? TotalProposedByDept { get; set; }
        public decimal? TotalPanchgauravByDept { get; set; }
        public int? SendForVerification { get; set; }
    }
    public class PendingVerficationBudget
    {
        public int? SrNo { get; set; }
        public string? DistrictName { get; set; }
        public string? GauravName { get; set; }
        public int? GauravGuid { get; set; }
        public long? DistrictId { get; set; }
        public int? FinancialYearId { get; set; }
        public int? ByDistrictCount { get; set; }
        public int? ByVettedCount { get; set; }
        public int? Pending { get; set; }
    }
}
