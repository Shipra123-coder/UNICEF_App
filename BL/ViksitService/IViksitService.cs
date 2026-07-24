using MO.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ViksitService
{
    public interface IViksitService
    {
        #region Public Methods
        Task<List<ViksitVM>> GetAllPillarsWithActivityCountAsync();
        Task<ViksitDashboardModel> GetPillarWiseCounts(int viksitId);
        Task<List<ViksitDepartmentActivityModel>> GetDepartmentActivityList(int? viksitId);
        Task<List<ViksitAgencyActivityModel>> GetAgencyActivityList(int? viksitId);
        Task<List<ViksitSectorActivityModel>> GetSectorActivityList(int? viksitId);
        #endregion
    }
}
