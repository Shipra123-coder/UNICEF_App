using MO.Common;
using MO.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ManageActivity
{
    public interface IManageActivity 
    {
        #region Activity Master
        Task<result> SaveActivityWithTasks(ActivityMaster model, string createdBy, string agencyId);
        Task<List<ActivityMaster>> GetActivityList(string agencyId);
        Task<ActivityMaster> GetFullActivityDetails(string? guid);
        #endregion
        #region Department Mapping
        Task<result> SaveDepartmentMappingAsync(DeptMappingViewModel model, string userId);
        Task<List<DepartmentMapList>> GetDeptMap(string activityGuid);
        #endregion
        #region GoalTarget Mappng
        Task<result> SaveGoalMappingAsync(GoalMappingModel model, string userId);
        Task<List<GoalMapResponse>> GetGoalMappingAsync(string activityGuid);
        #endregion
        #region Pillar Mapping
        Task<result> SavePillarMappingAsync(PillarMappingModel model, string userId);
        Task<List<PillarMapResponse>> GetPillarMappingAsync(string activityGuid);
        #endregion
        #region Nature Of Support
        Task<result> SaveNatureOfSupportMappingAsync(NatureOfSupportMappingModel model, string userId);
        Task<List<NatureOfSupportMapResponse>> GetNatureOfSupportMappingAsync(string activityGuid);
        #endregion
        Task<result> SaveGeoLevelAsync(GeoLevelModel model, string userId);
        Task<result> DeleteGeoLevelAsync(long geoId);
        Task<string> GetActivityDataAsync(string activityGuid);
        Task<string> GetActivityDataForReportAsync(string activityGuid, string reportingId);
        //#region ActivityList
        //Task<List<ActivityMaster>> GetActivityList();
        //#endregion
    }
}
