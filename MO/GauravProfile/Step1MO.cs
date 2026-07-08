using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.GauravProfile
{
    //public class Step1MO
    //{
    //    public int CurrentStep { get; set; }
    //    public List<MainQuestionMO> Questions { get; set; }
    //}

    //public class MainQuestionMO
    //{
    //    public int QuestionMasterId { get; set; }
    //    public string DisplayNumber { get; set; }
    //    public string QuestionText { get; set; }
    //    public string AnswerValue { get; set; }
    //}

    //public class Step2MO
    //{
    //    public int CurrentStep { get; set; }
    //    public long QuestionMasterId { get; set; }
    //    public string QuestionText { get; set; }
    //    public int GauravId { get; set; }
    //    public int DistrictId { get; set; }
    //    public string Gatividhi { get; set; }
    //    public string GatividhiName { get; set; }
    //    public string Vyay { get; set; }
    //    public string NodalVibhag { get; set; }
    //    public string MPLAD { get; set; }
    //    public string CSR { get; set; }
    //    public string OtherMad { get; set; }
    //    public string PanchBudget { get; set; }
    //    public string WorkPlan { get; set; }
    //    public string TimeLimit { get; set; }
    //}
    public class Step2RowMOList
    {
        public long RowId { get; set; }
        public string Gatividhi { get; set; }
        public string GatividhiName { get; set; }
        public string Vyay { get; set; }
        public string NodalVibhag { get; set; }
        public string MPLAD { get; set; }
        public string CSR { get; set; }
        public string OtherMad { get; set; }
        public string PanchBudget { get; set; }
        public string WorkPlan { get; set; }
        public string TimeLimit { get; set; }
    }

    //public class Step2MO
    //{
    //    public int CurrentStep { get; set; }
    //    public List<QuestionModel_step2> mainquestion { get; set; }
    //}

    //public class QuestionModel_step2
    //{
    //    public int CurrentStep { get; set; }
    //    public long QuestionMasterId { get; set; }
    //    public string Guid { get; set; }
    //    public string DisplayNumber { get; set; }
    //    public int OrderNumber { get; set; }
    //    public string QuestionText { get; set; }
    //    public int QuestionType { get; set; }

    //    public List<SubQuestionModel_Step2> SubQuestions { get; set; } = new();
    //}

    //public class SubQuestionModel_Step2
    //{
    //    public long SubQuestionMasterId { get; set; }
    //    public string Guid { get; set; }
    //    public string DisplayNumber { get; set; }
    //    public int OrderNumber { get; set; }
    //    public string QuestionText { get; set; }
    //    public string Fieldtype { get; set; }
    //    public int QuestionType { get; set; }
    //}

    //public class Step2AnswerMO
    //{
    //    public int QuestionMasterId { get; set; }
    //    public int SubQuestionMasterId { get; set; }
    //    public string AnswerValue { get; set; }
    //    public long GauravId { get; set; }
    //    public long CreatedBy { get; set; }
    //}
}
