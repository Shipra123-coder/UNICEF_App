using MO.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SDGGoalService
{
    public interface ISDGGoalServices
    {
        Task<List<SDGGoalVM>> GetAllGoalsWithActivityCountAsync();
        Task<SDGGoalDashboardModel> GetGoalWiseCounts(int goalId);
        Task<List<SDGGoalDepartmentActivityModel>> GetDepartmentActivityList(int? goalId);
        Task<List<SDGGoalAgencyActivityModel>> GetAgencyActivityList(int? goalId);
        Task<List<SDGGoalSectorActivityModel>> GetSectorActivityList(int? goalId);
    }
}
