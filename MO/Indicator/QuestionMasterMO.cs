using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Indicator
{
    public class QuestionMasterMO
    {
        public int QuestionMasterId { get; set; }
        public Guid Guid { get; set; }
        public int? DistrictId { get; set; }
        public int? GauravId { get; set; }
        public int? QuestionType { get; set; }       
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public string DisplayNumber { get; set; }        
        public bool IsSubQuestion { get; set; }
        public int? MainQuestionId { get; set; }
        public List<SubQuestionMasterMO> SubQuestions { get; set; } = new List<SubQuestionMasterMO>();
        public List<subQuestiontbl> SubColumn { get; set; } = new List<subQuestiontbl>();

        // For Table-Type Questions (Row-wise Answer Mapping)
        public List<List<GauravAnswerMO>> SubColumnRows { get; set; } = new List<List<GauravAnswerMO>>();

        public string? AnswerValue { get; set; }
        public int? AnswerSubId { get; set; }
        public int? rowId { get;set; }
    }

    public class GauravAnswerMO
    {
        public int GauravProfileAnswerId { get; set; }
        public int QuestionMasterId { get; set; }
        public int? SubQuestionId { get; set; }
        public string AnswerValue { get; set; }
        public int rowId { get; set; }
    }
    public class SubQuestionMasterMO
    {
        public int QuestionMasterId { get; set; }
        public Guid Guid { get; set; }
        public int? DistrictId { get; set; }
        public int? GauravId { get; set; }
        public int? QuestionType { get; set; }
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public string DisplayNumber { get; set; }
        public bool IsSubQuestion { get; set; }
        public int? MainQuestionId { get; set; }
        public List<subQuestiontbl> SubColumn { get; set; } = new List<subQuestiontbl>();
        public string? AnswerValue { get; set; }
        public int? AnswerSubId { get; set; }
        public int? rowId { get; set; }


    }

    public class subQuestiontbl
    {
        public int SubQuestionMasterId { get; set; }
        public string? Guid { get; set; }
        public int? QuestionType { get; set; }
        public int? OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public string? AnswerValue { get; set; }
        public int? rowId { get; set; }
    }

    public class Indicatorreport
    {
        public int? Id { get; set; }
        public int? districtId { get; set; }
    }

    public class GauravAnswer
    {
        public string gauravId { get; set; }
        public string? districtId { get; set; }
        public string? gauravDistId { get; set; }
        public string? financialYearId { get; set; }
        public List<AnswerBlock> model { get; set; }
    }

    public class AnswerItem
    {
        public string? mainQuestionId { get; set; }
        public string? subQuestionId { get; set; }
        public string? answer { get; set; }
        public int? rowId { get; set; }        
    }

    public class AnswerBlock
    {
        public string? questionId { get; set; }  // nullable for sub-question blocks        
        public List<AnswerItem>? answers { get; set; }
    }

    public class GauravVivranModel
    {        
        public string? gauravName { get; set; }
        public string? gauravDistname { get; set; }
        public string? mangalName { get; set; }
        public string? vivran { get; set; }
    }
    public class GauravMainProfileModel
    {
        public string? Introduction { get; set; }
        public string? DistrictProfileOne { get; set; }
        public string? DistrictProfileTwo { get; set; }  
        public string? CreatedBy { get; set; }
    }

    public class Profile_step1
    {
        public List<GauravVivranModel> _vivranList { get; set; }
        public GauravMainProfileModel _data { get; set; }
    }

    public class AT_GauravProfileAnswer
    {
        public long GauravProfileAnswerId { get; set; }
        public Guid? Guid { get; set; }

        public int QuestionMasterId { get; set; }
        public int? SubQuestionId { get; set; }

        public string AnswerValue { get; set; }

        public int GauravId { get; set; }
        public int DistrictId { get; set; }

        public string? DisplayNumber { get; set; }
        public string? QuestionText { get; set; }
        public int? OrderNumber { get; set; }

        public int? GauravDistId { get; set; }
        public int FinancialYear_Id { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public int? rowId { get; set; }
    }
}
