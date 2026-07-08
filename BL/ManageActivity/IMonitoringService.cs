using Microsoft.AspNetCore.Http;
using MO.Common;
using MO.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ManageActivity
{
    public interface IMonitoringService
    {
        #region monitoring step1
        Task<result> SaveMonitoringAsync(MonitoringModel model,string user);
        Task<MonitoringModel> GetMonitoringAsync(string activityGuid);
        #endregion

        #region monitoring step2
        Task<result> SaveReportingStepAsync(ReportingModel_step2 model, string user);
        Task<ReportingModel_step2> GetReportingByActivityGuidAsync(string activityGuid);
        #endregion

        #region BestPrectices
        Task<bestPrectis_result> SaveBestPracticeAsync(BestPracticeModel model, string user);
        Task<result> UploadGalleryImages(long bestPracticeId,List<IFormFile> files,string user);
        #endregion

        #region Task Tracking
        Task<result> SaveTaskTrackingAsync(TaskTrackingModel model,int isFinalSubmit, string user);
        Task<List<TaskTrackingModel>> GetTaskTrackingAsync(long activityId);
        Task<TaskTrackingModel> GetTaskTrackingByIdAsync(long trackingId);
        Task UpdateActivityStatusAsync(string activityGuid, string reportingId, string user);
        Task SaveContactDetailsAsync(string activityGuid,string reportingId,ContactDetailsModel model,string createdBy);

        #endregion
    }
}
