using MO.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.GauravProfile
{
    public class GauravQuestionVM
    {
        public string Guid { get; set; }
        public int DistrictId { get; set; }

        public List<QuestionMasterMO> Questions { get; set; }
    }

    public class QuestionItemVM
    {
        public int QuestionMasterId { get; set; }
        public string DisplayNumber { get; set; }
        public string QuestionText { get; set; }

        public List<SubQuestionVM> SubQuestions { get; set; }
        public List<SubColumnVM> SubColumn { get; set; }
    }

    public class SubQuestionVM
    {
        public int QuestionMasterId { get; set; }
        public string DisplayNumber { get; set; }
        public string QuestionText { get; set; }

        public List<SubColumnVM> SubColumn { get; set; }
    }

    public class SubColumnVM
    {
        public int SubQuestionMasterId { get; set; }
        public string QuestionText { get; set; }
    }

}
