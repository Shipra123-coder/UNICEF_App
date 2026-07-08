using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.GauravProfile
{
    public class GauravProfileDashboardMO
    {
        public int DepartmentId { get; set; }
        public int GauravId { get; set; }
        public string GauravGuid { get; set; }
        public string GauravName { get; set; }
        public int? StatusId { get; set; }
    }

    #region Step1
    public class Step1MO
    {
        public int CurrentStep { get; set; }
        public string GauravGuid { get; set; }
        public string statusClass { get; set; }
  
        public List<MainQuestionMO> Questions { get; set; }
    }
    public class MainQuestionMO
    {
        public int QuestionMasterId { get; set; }
        public int SubQuestionMasterId { get; set; }
        public string DisplayNumber { get; set; }
        public string QuestionText { get; set; }
        public string AnswerValue { get; set; }
     
    }
    #endregion

    #region Step2
    public class Step2MO
    {
        public int CurrentStep { get; set; }
        public string GauravGuid { get; set; }
        public string statusClass { get; set; }
        public List<QuestionModel_step2> mainquestion { get; set; }
    }
    public class QuestionModel_step2
    {
        public int CurrentStep { get; set; }
        public long QuestionMasterId { get; set; }
        public string Guid { get; set; }
        public string DisplayNumber { get; set; }
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
       

        public List<SubQuestionModel_Step2> SubQuestions { get; set; } = new();
    }

    public class SubQuestionModel_Step2
    {
        public long SubQuestionMasterId { get; set; }
        public string Guid { get; set; }
        public string DisplayNumber { get; set; }
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public string Fieldtype { get; set; }
        public int QuestionType { get; set; }
    }
    public class Step2AnswerMO
    {
        public int QuestionMasterId { get; set; }
        public int SubQuestionMasterId { get; set; }
        public string AnswerValue { get; set; }
        public long GauravId { get; set; }
        public long CreatedBy { get; set; }
        public long rowId { get; set; }
    }
    #endregion

    #region Step3

    public class Step3MO
    {
        public int CurrentStep { get; set; }
        public string GauravGuid { get; set; }
        public string statusClass { get; set; }
        public List<MainQuestionStep3MO> mainQuestionStep3MOs { get; set; }
    }

    public class MainQuestionStep3MO
    {        
        public int QuestionMasterId { get; set; }
        public string Guid { get; set; }
        public int DistrictId { get; set; }
        public int GauravId { get; set; }
        public string QuestionType { get; set; }
        public string DisplayNumber { get; set; }
        public int OrderNumber { get; set; }
        public string QuestionText { get; set; }
        public bool IsSubQuestion { get; set; }
        public int MainQuestionId { get; set; }
        public bool IsActive { get; set; }

        // Answer fetched from Step1
        public string AnswerValue { get; set; }
   

        // List of sub questions
        public List<MainQuestionStep3MO> SubQuestions { get; set; } = new();
    }
    #endregion

    #region main profile
    public class mainProfileMO
    {
      public int? GauravMainProfileAnswerId { get; set; }
        public string Introduction { get; set; }
        public string DistrictProfile_one { get; set; }
        public string DistrictProfile_two { get; set; }
        public int? DistrictId { get; set; }
        public int? FinancialYear_Id { get; set; }
        public int? FinalSubmit { get; set; }
        public string? CreatedBy { get; set; }
        public List<mainProfile_VivranMO>? list { get; set; }
    }

    public class mainProfile_VivranMO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string GauravItem { get; set; }
        public string GauravVivran { get; set; }
    }
    #endregion

    #region print profile
    public class PrintProfile
    {
        public string GauravGuid { get; set; }
        public string District { get; set; }
        public int DistrictId { get; set; }
        public string Gaurav { get; set; }
        public string GauravItem { get; set; }
        public string FinancialYear { get; set; }
        public List<MainQuestionMO> step1 { get; set; }
        public List<QuestionModel_step2> step2 { get; set; }
        public List<MainQuestionStep3MO> step3 { get; set; }
        public string LastRemark { get; set; }
    }
    #endregion

    #region Pending Verification List
    public class ProfileVerificationPendingVM
    {
        public string DistrictName { get; set; }
        public string GauravName { get; set; }
        public string GauravGuid { get; set; }
        public long DistrictId { get; set; }
        public int FinancialYearId { get; set; }
    }
    public class ProfileVerifyActionVM
    {
        public string FinancialYearId { get; set; }
        public string DistrictId { get; set; }
        public string GauravGuid { get; set; }
        public string Remark { get; set; }
    }

    public class ProfileBasicDetail
    {
        public string GauravEng { get; set; }
        public string GauravMangal { get; set; }        
        public string GauravItemEng { get; set; }
        public string GauravItemMangal { get; set; }
        public string FinancialYear { get; set; }
        public string District { get; set; }
        public int DistrictId { get; set; }
    }
    #endregion

    #region District work Plan Trail

    public class ApplicationTrails
    {
        public string GauravGuid { get; set; }
        public List<ProfileBasicTrails> Trails { get; set; }       
    }

    public class ProfileBasicTrails
    {
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion    
}
