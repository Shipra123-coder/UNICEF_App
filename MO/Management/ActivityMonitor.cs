using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MO.Management
{
    public class MonitoringModel
    {
        public int? ActivityMonitoringId { get; set; }
        public string ActivityGuid { get; set; }
        public string IsPartnership { get; set; }

        public List<DocumentModel> Documents { get; set; }
        public decimal? DirectINR { get; set; }
        public decimal? DirectUSD { get; set; }
        public string DirectSource { get; set; }

        public decimal? IndirectINR { get; set; }
        public decimal? IndirectUSD { get; set; }
        public string IndirectSource { get; set; }
        public List<SupportModel> Supports { get; set; }
    }
    public class SupportModel
    {
        public string SupportType { get; set; }
        public string Details { get; set; }
    }

    public class DocumentModel
    {
        public int? DocId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }   // 🔥 IMPORTANT
        public byte[] FileBytes { get; set; } // 🔥 ADD THIS
        public string ExistingFileName { get; set; } // 🔥 update case
    }

    public class FinancialModel
    {
        public decimal? DirectINR { get; set; }
        public decimal? DirectUSD { get; set; }
        public string DirectSource { get; set; }

        public decimal? IndirectINR { get; set; }
        public decimal? IndirectUSD { get; set; }
        public string IndirectSource { get; set; }
    }

    

    #region Activity Geo Level Add
    public class GeoLevelModel
    {
        public int TaskId { get; set; }

        public string GeoLevel { get; set; }

        public List<int> DistrictIds { get; set; }

        public List<BlockModel> Blocks { get; set; }

        public List<CityModel> Cities { get; set; }
    }

    public class BlockModel
    {
        public int DistrictId { get; set; }

        public int BlockId { get; set; }
    }

    public class CityModel
    {
        public int DistrictId { get; set; }

        public int CityId { get; set; }
    }
    #endregion

    #region TASK TRACKING DTO
    public class TaskTrackingDTO
    {
        [JsonProperty("TrackingId")]
        public long TrackingId { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Remarks")]
        public string Remarks { get; set; }
    }

    #endregion
    #region GEOLEVEL VIEW
    public class ActivityDTO
    {
        [JsonProperty("ActivityGuid")]
        public string? ActivityGuid { get; set; }

        [JsonProperty("ActivityId")]
        public int ActivityId { get; set; }

        [JsonProperty("ActivityName")]
        public string ActivityName { get; set; }

        [JsonProperty("hasSubActivity")]
        public bool HasSubActivity { get; set; }

        [JsonProperty("ActivityStartDate")]
        public string ActivityStartDate { get; set; }
        [JsonProperty("ActivityEndDate")]
        public string ActivityEndDate { get; set; }

        [JsonProperty("subActivities")]
        public List<SubActivityDTO> SubActivities { get; set; }

        [JsonProperty("directTasks")]
        public List<TaskDTO> DirectTasks { get; set; }

    }
    public class SubActivityDTO
    {
        [JsonProperty("SubActivityId")]
        public int SubActivityId { get; set; }

        [JsonProperty("SubActivityName")]
        public string SubActivityName { get; set; }

        [JsonProperty("tasks")]
        public List<TaskDTO> Tasks { get; set; }
    }
    public class TaskDTO
    {
        [JsonProperty("TaskId")]
        public int TaskId { get; set; }

        [JsonProperty("taskName")]
        public string TaskName { get; set; }

        [JsonProperty("geography")]
        public string Geography { get; set; }

        /* =========================================
           TASK TRACKING
           ========================================= */

        [JsonProperty("tracking")]
        public TaskTrackingDTO Tracking { get; set; }

        /* =========================================
           GEO LEVEL LIST
           ========================================= */

        [JsonProperty("geoLevelList")]
        public List<GeoLevelDTO> GeoLevelList { get; set; }
    }
    public class GeoLevelDTO
    {
        [JsonProperty("GeoId")]
        public int GeoId { get; set; }

        [JsonProperty("GeoLevel")]
        public string GeoLevel { get; set; }

        [JsonProperty("DistrictId")]
        public int? DistrictId { get; set; }

        [JsonProperty("districtName")]
        public string DistrictName { get; set; }

        [JsonProperty("BlockId")]
        public int? BlockId { get; set; }

        [JsonProperty("blockName")]
        public string BlockName { get; set; }

        [JsonProperty("CityId")]
        public int? CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }
    #endregion

    public class trackingMainModel
    {
        public string ActivityGuid { get; set; }
       public ActivityDTO _activity { get; set; }
        public string reportingId { get; set; }
        

    }

    #region step2
    public class ReportingModel_step2
    {
        public int? ReportingId { get; set; }
        public string? ReportFromDate { get; set; }
        public string? ReportToDate { get; set; }
        public int IsAligned { get; set; }
        public string? AlignmentDetails { get; set; }
        public string? FundUtilization { get; set; }
        public int HasChallenges { get; set; }
        public string? ChallengeDetails { get; set; }
        public string? Suggestions { get; set; }
        public string? ActivityGuid { get; set; }
        public string? CreatedBy { get; set; }

        // Department Details
        public string DepartmentNodal { get; set; }
        public string DepartmentDesignation { get; set; }
        public string DepartmentEmail { get; set; }
        public string DepartmentContact { get; set; }
        public string DepartmentPlace { get; set; }
        // Agency Details
        public string AgencyNodal { get; set; }
        public string AgencyDesignation { get; set; }
        public string AgencyEmail { get; set; }
        public string AgencyContact { get; set; }
        public string AgencyPlace { get; set; }
    }
    #endregion

    #region BestPrectices
    
    public class BestPracticeModel
    {
        public long Id { get; set; }
        public Guid? ActivityGuid { get; set; }
        public long? SubActivityId { get; set; }
        public long TaskId { get; set; }
        public long GeoId { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public IFormFile MainImage { get; set; }       
        public IFormFile PdfFile { get; set; }
        public List<IFormFile> GalleryImages { get; set; } 
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // =============================================
    // RESULT MODEL
    // =============================================

    public class bestPrectis_result
    {
        public bool status { get; set; }

        public string message { get; set; }

        public long id { get; set; }
    }
    #endregion

    #region Task Tracking

    public class main_TaskTrackingModel
    {
        public ContactDetailsModel ContactDetails { get; set; }
        public List<TaskTrackingModel> TaskTrackingModel { get; set; }
    }

    public class ContactDetailsModel
    {
        // Department Details
        public string DepartmentOfficerName { get; set; }

        public string DepartmentDesignation { get; set; }

        public string DepartmentEmail { get; set; }

        public string DepartmentContactNumber { get; set; }

        public string DepartmentPlace { get; set; }

        // UN Agency Details
        public string AgencyOfficerName { get; set; }

        public string AgencyDesignation { get; set; }

        public string AgencyEmail { get; set; }

        public string AgencyContactNumber { get; set; }

        public string AgencyPlace { get; set; }
    }
    public class TaskTrackingModel
    {
        public long TrackingId { get; set; }
        public string ActivityGuid { get; set; }
        public long TaskId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }    
        
        public string reportingId { get; set; }
    }
    #endregion
}
