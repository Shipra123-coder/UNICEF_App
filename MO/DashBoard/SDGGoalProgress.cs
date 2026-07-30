using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.DashBoard
{
    #region ViksitRajasthan
    public class SDGGoalVM
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public string Description { get; set; }
        public int DisplayNumber { get; set; }
        public string ImageUrlFront { get; set; }
        public string ImageUrlBack { get; set; }
        public int TotalCount { get; set; }
    }
    public class SDGGoalDashboardModel
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public string GoalCode { get; set; }
        public string ImageUrl { get; set; }
        //public string ViksitCode { get; set; }
        public string Description { get; set; }

        public int BestPracticeCount { get; set; }
        public int DepartmentCount { get; set; }
        public int AgencyCount { get; set; }
        public int SectorCount { get; set; }
        public int ActivityCount { get; set; }
        public int TaskCount { get; set; }
        public int GoalCount { get; set; }
        public int TargetCount { get; set; }
        public int PillarCount { get; set; }
        public int SubPillarCount { get; set; }
    }
    public class SDGGoalDepartmentActivityModel
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }

        public string UNSectorName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int NodalDepartmentId { get; set; }

        public string NodalDepartmentName { get; set; }
        public string AssociatedDepartments { get; set; }

        public string AgencyName { get; set; }
        public string AssociatedAgencies { get; set; }

        public string AgencyCode { get; set; }

        public string LogoURL { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityStatus { get; set; }
        public string AssociatedSubThemes { get; set; }
    }
    public class SDGGoalAgencyActivityModel
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }

        public string NodalDepartment { get; set; }
        public string AssociatedDepartments { get; set; }
        public string AssociatedSubThemes { get; set; }

        public string UNSectorName { get; set; }

        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public string AgencyCode { get; set; }
        public string LogoURL { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string ActivityStatus { get; set; }
    }
    public class SDGGoalSectorActivityModel
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }

        public int UNSectorId { get; set; }

        public string UNSectorName { get; set; }

        public long ActivityId { get; set; }

        public string ActivityName { get; set; }
        public string NodalDepartment { get; set; }
        public string AssociatedDepartments { get; set; }
        public string AssociatedSubThemes { get; set; }
        public string AgencyName { get; set; }
        public string ActivityStatus { get; set; }
    }
    #endregion
}
