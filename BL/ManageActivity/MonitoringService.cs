using DL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MO.Common;
using MO.Management;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BL.ManageActivity.MonitoringService;

namespace BL.ManageActivity
{
    public class MonitoringService : IMonitoringService
    {
        #region Properties
        private readonly ISQLHelper _iSql;
        #endregion
        #region Constructor
        public MonitoringService(ISQLHelper iSql)
        {
            _iSql = iSql;

        }
        #endregion

        #region step1
        public async Task<result> SaveMonitoringAsync(MonitoringModel model, string user,string path)
        {
            result _result = new result();

            try
            {
                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@Action", "Save"),

            // 🔹 Main
            new SqlParameter("@ActivityGuid", model.ActivityGuid),
            new SqlParameter("@IsPartnership", model.IsPartnership == "Yes" ? 1 : 0 ),
            new SqlParameter("@IsGovernment", model.IsGovernment == "Yes" ? 1 : 0 ),
            new SqlParameter("@IsGovernmentDetails",model.IsGovernmentDetails ?? (object)DBNull.Value),

            // 🔹 Financial - Direct
            new SqlParameter("@DirectINR", model.DirectINR ?? (object)DBNull.Value),
            new SqlParameter("@DirectUSD", model.DirectUSD ?? (object)DBNull.Value),
            new SqlParameter("@DirectSource", model.DirectSource ?? (object)DBNull.Value),

            // 🔹 Financial - Indirect
            new SqlParameter("@IndirectINR", model.IndirectINR ?? (object)DBNull.Value),
            new SqlParameter("@IndirectUSD", model.IndirectUSD ?? (object)DBNull.Value),
            new SqlParameter("@IndirectSource", model.IndirectSource ?? (object)DBNull.Value),

            //// 🔹 JSON Data
            //new SqlParameter("@DocumentsJson",
            //    JsonConvert.SerializeObject(model.Documents ?? new List<DocumentModel>())),

            new SqlParameter("@SupportsJson",
                JsonConvert.SerializeObject(model.Supports ?? new List<SupportModel>()))
        };

                var ds = await _iSql.ExecuteProcedure("SP_ManageMonitoring", parameters.ToArray());

                var ActivityMonitoringId = ds.Tables[0].Rows[0]["ActivityMonitoringId"].ToString();

                // 🔥 Documents alag save karenge
                foreach (var doc in model.Documents)
                {
                    // 🔹 CASE 1: New File Upload
                    if (doc.File != null && doc.File.Length > 0)
                    {
                        //using (var ms = new MemoryStream())
                        //{
                        //    await doc.File.CopyToAsync(ms);
                        //    byte[] fileBytes = ms.ToArray();

                        string folderPath = Path.Combine(path,
                            "uploads",
                            "MonitoringDocuments");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Unique File Name
                        string fileName = Guid.NewGuid().ToString() +
                                          Path.GetExtension(doc.File.FileName);

                        string filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doc.File.CopyToAsync(stream);
                        }

                        var docParams = new List<SqlParameter>
                            {
                                new SqlParameter("@ActivityMonitoringId", ActivityMonitoringId),
                                new SqlParameter("@DocumentType", doc.DocumentType),
                                new SqlParameter("@OtherDocumentName",(object?)doc.OtherDocumentName ?? DBNull.Value),
                                new SqlParameter("@FileName", doc.File.FileName),
                                new SqlParameter("@FilePath","/uploads/MonitoringDocuments/" + fileName),
                                //new SqlParameter("@FileData", fileBytes),
                                new SqlParameter("@ContentType", doc.File.ContentType),
                                new SqlParameter("@IsUpdate", 1), // 🔥 flag
                                
                            };

                            await _iSql.ExecuteProcedure("SP_SaveMonitoringDocument", docParams.ToArray());
                        //}
                    }

                    // 🔹 CASE 2: NO NEW FILE → KEEP OLD
                    else if (!string.IsNullOrEmpty(doc.ExistingFileName))
                    {
                        var docParams = new List<SqlParameter>
                        {
                            new SqlParameter("@ActivityMonitoringId", ActivityMonitoringId),
                            new SqlParameter("@DocumentType", doc.DocumentType),
                            new SqlParameter("@OtherDocumentName", (object?)doc.OtherDocumentName ?? DBNull.Value),
                            //new SqlParameter("@FileName", doc.ExistingFileName),
                            //new SqlParameter("@FilePath", doc.ExistingFilePath), // Hidden field se bhejna hoga
                            //new SqlParameter("@ContentType", DBNull.Value),
                            new SqlParameter("@IsUpdate", 0)
                        };

                        //await _iSql.ExecuteProcedure("SP_SaveMonitoringDocument", docParams.ToArray());
                    }
                }

                // ✅ Read response from SP
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _result.status = Convert.ToBoolean(ds.Tables[0].Rows[0]["Status"]);
                    _result.message = ds.Tables[0].Rows[0]["Message"].ToString();
                }
                else
                {
                    _result.status = false;
                    _result.message = "No response from database.";
                }
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error: " + ex.Message;
            }

            return _result;
        }
        public async Task<MonitoringModel> GetMonitoringAsync(string activityGuid)
        {
            MonitoringModel monitoringModel = new MonitoringModel();
            try
            {
                var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetMonitoring"),
                new SqlParameter("@ActivityGuid", activityGuid)
            };

                var ds = await _iSql.ExecuteProcedure("SP_ManageMonitoring", parameters.ToArray());

                if (ds.Tables.Count < 3 || ds.Tables[0].Rows.Count == 0)
                    return null;

                // 🔹 MAIN
                var mainRow = ds.Tables[0].Rows[0];
                monitoringModel = new MonitoringModel
                {
                    ActivityMonitoringId = Convert.ToInt32(ds.Tables[0].Rows[0]["ActivityMonitoringId"]),
                    ActivityGuid = ds.Tables[0].Rows[0]["ActivityGuid"].ToString(),
                    IsPartnership = ds.Tables[0].Rows[0]["IsPartnership"].ToString() == "1" ? "Yes" : "No",
                    IsGovernment = ds.Tables[0].Rows[0]["IsGovernment"] != DBNull.Value && ds.Tables[0].Rows[0]["IsGovernment"].ToString() == "1" ? "Yes" : "No",
                    IsGovernmentDetails = ds.Tables[0].Rows[0]["IsGovernmentDetails"] != DBNull.Value ? ds.Tables[0].Rows[0]["IsGovernmentDetails"].ToString() : "", 
                    DirectINR = ds.Tables[0].Rows[0]["DirectINR"] as decimal?,
                    DirectUSD = ds.Tables[0].Rows[0]["DirectUSD"] as decimal?,
                    DirectSource = ds.Tables[0].Rows[0]["DirectSource"].ToString(),
                    IndirectINR = ds.Tables[0].Rows[0]["IndirectINR"] as decimal?,
                    IndirectUSD = ds.Tables[0].Rows[0]["IndirectUSD"] as decimal?,
                    IndirectSource = ds.Tables[0].Rows[0]["IndirectSource"].ToString(),
                    Documents = new List<DocumentModel>(),
                    Supports = new List<SupportModel>()
                };

                // 🔹 DOCUMENTS
                foreach (DataRow docRow in ds.Tables[1].Rows)
                {
                    monitoringModel.Documents.Add(new DocumentModel
                    {
                        DocId = Convert.ToInt32(docRow["DocumentId"]),
                        DocumentType = docRow["DocumentType"].ToString(),
                        FileName = docRow["FileName"].ToString(),
                        OtherDocumentName = docRow["OtherDocumentName"].ToString(),
                        //fi = docRow["FileName"].ToString(),
                        //// FileData is not returned for performance reasons
                    });
                }

                // 🔹 SUPPORTS
                foreach (DataRow supportRow in ds.Tables[2].Rows)
                {
                    monitoringModel.Supports.Add(new SupportModel
                    {
                        SupportType = supportRow["SupportType"].ToString(),
                        Details = supportRow["Details"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return monitoringModel;
        }
        #endregion

        #region Step2  
        public async Task<result> SaveReportingStepAsync(ReportingModel_step2 model, string user)
        {
            result _result = new result();

            // ======================================
            // DATE PARSING ENGINE
            // ======================================
            DateTime? activityStartDate = null;
            DateTime? activityEndDate = null;

            if (!string.IsNullOrWhiteSpace(model.ReportFromDate))
            {
                // Parsed using dd-MM-yyyy format
                activityStartDate = DateTime.ParseExact(model.ReportFromDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrWhiteSpace(model.ReportToDate))
            {
                // Parsed using dd-MM-yyyy format
                activityEndDate = DateTime.ParseExact(model.ReportToDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            try
            {
                string action =
                    model.ReportingId == null || model.ReportingId == 0
                    ? "SaveReporting"
                    : "UpdateReporting";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", action),
                    new SqlParameter("@ReportingId",model.ReportingId ?? (object)DBNull.Value),
                    new SqlParameter("@ActivityGuid",string.IsNullOrWhiteSpace(model.ActivityGuid)
                        ? (object)DBNull.Value
                        : model.ActivityGuid),
                    new SqlParameter("@ReportFromDate",activityStartDate ?? (object)DBNull.Value),
                    new SqlParameter("@ReportToDate",activityEndDate ?? (object)DBNull.Value),
                    new SqlParameter("@IsAligned",model.IsAligned),
                    new SqlParameter("@AlignmentDetails",
                        model.IsAligned == 0
                        ? (object)DBNull.Value
                        : string.IsNullOrWhiteSpace(model.AlignmentDetails)
                            ? (object)DBNull.Value
                            : model.AlignmentDetails),

                    new SqlParameter("@FundUtilization",
                        string.IsNullOrWhiteSpace(model.FundUtilization)
                        ? (object)DBNull.Value
                        : model.FundUtilization),

                    new SqlParameter("@HasChallenges",model.HasChallenges),

                    new SqlParameter("@ChallengeDetails",
                        model.HasChallenges == 0
                        ? (object)DBNull.Value
                        : string.IsNullOrWhiteSpace(model.ChallengeDetails)
                            ? (object)DBNull.Value
                            : model.ChallengeDetails),

                    new SqlParameter("@Suggestions",
                        string.IsNullOrWhiteSpace(model.Suggestions)
                        ? (object)DBNull.Value
                        : model.Suggestions),

                    new SqlParameter("@CreatedBy", user),
                    new SqlParameter("@UpdatedBy", user)
                };

                // =============================================
                // EXECUTE PROCEDURE
                // =============================================

                var ds = await _iSql.ExecuteProcedure(
                    "SP_ManageMonitoring_Step2",
                    parameters.ToArray());

                // =============================================
                // RESPONSE
                // =============================================

                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    _result.status = Convert.ToBoolean(
                        ds.Tables[0].Rows[0]["Status"]);

                    _result.message =
                        ds.Tables[0].Rows[0]["Message"].ToString();

                    if (ds.Tables[0].Columns.Contains("ReportingId"))
                    {
                        _result.id =
                            ds.Tables[0].Rows[0]["ReportingId"].ToString();
                    }
                }
                else
                {
                    _result.status = false;
                    _result.message = "No response from database.";
                }
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error: " + ex.Message;
            }

            return _result;
        }
        public async Task<ReportingModel_step2> GetReportingByActivityGuidAsync(string activityGuid)
        {
            ReportingModel_step2 model = new ReportingModel_step2();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "GetReporting"),
                    new SqlParameter("@ActivityGuid", activityGuid)
                };

                var ds = await _iSql.ExecuteProcedure("SP_ManageMonitoring_Step2", parameters.ToArray());
                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    model.ReportingId =Convert.ToInt32(dr["ReportingId"]);
                    model.ActivityGuid =dr["ActivityGuid"].ToString();
                    model.ReportFromDate = dr["ReportFromDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReportFromDate"]).ToString("dd-MM-yyyy") : null;
                    model.ReportToDate = dr["ReportToDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReportToDate"]).ToString("dd-MM-yyyy") : null;


                    model.IsAligned =Convert.ToInt32(dr["IsAligned"]);
                    model.AlignmentDetails =dr["AlignmentDetails"]?.ToString();
                    model.FundUtilization =dr["FundUtilization"]?.ToString();
                    model.HasChallenges =Convert.ToInt32(dr["HasChallenges"]);
                    model.ChallengeDetails =dr["ChallengeDetails"]?.ToString();
                    model.Suggestions =dr["Suggestions"]?.ToString();

                    model.DepartmentNodal = dr["DepartmentNodal"]?.ToString();
                    model.DepartmentDesignation = dr["DepartmentDesignation"]?.ToString();
                    model.DepartmentEmail = dr["DepartmentEmail"]?.ToString();
                    model.DepartmentContact = dr["DepartmentContact"]?.ToString();
                    model.DepartmentPlace = dr["DepartmentPlace"]?.ToString();
                    model.AgencyNodal = dr["AgencyNodal"]?.ToString();
                    model.AgencyDesignation = dr["AgencyDesignation"]?.ToString();
                    model.AgencyEmail = dr["AgencyEmail"]?.ToString();
                    model.AgencyContact = dr["AgencyContact"]?.ToString();
                    model.AgencyPlace = dr["AgencyPlace"]?.ToString();
                }
            }
            catch (Exception ex)
            {

            }

            return model;
        }
        #endregion

        #region BestPrectis  
        public async Task<bestPrectis_result> SaveBestPracticeAsync(BestPracticeModel model, string user)
        {
            bestPrectis_result _result = new bestPrectis_result();

            try
            {
                // =====================================
                // IMAGE TO BYTE ARRAY
                // =====================================

                byte[] imageBytes = null;

                if (model.MainImage != null &&
                    model.MainImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.MainImage.CopyToAsync(ms);

                        imageBytes = ms.ToArray();
                    }
                }

                // =====================================
                // PDF TO BYTE ARRAY
                // =====================================

                byte[] pdfBytes = null;

                if (model.PdfFile != null &&
                    model.PdfFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.PdfFile.CopyToAsync(ms);

                        pdfBytes = ms.ToArray();
                    }
                }

                // =====================================
                // PARAMETERS
                // =====================================

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@ActivityGuid",
                model.ActivityGuid),

            new SqlParameter("@SubActivityId",
                (object?)model.SubActivityId
                ?? DBNull.Value),

            new SqlParameter("@TaskId",
                model.TaskId),

            new SqlParameter("@GeoId",
                model.GeoId),

            new SqlParameter("@Heading",
                model.Heading ?? ""),

            new SqlParameter("@Description",
                model.Description ?? ""),

            // =================================
            // MAIN IMAGE
            // =================================

            new SqlParameter("@MainImage",
                (object?)imageBytes
                ?? DBNull.Value),

            new SqlParameter("@MainImageName",
                model.MainImage?.FileName
                ?? ""),

            new SqlParameter("@MainImageContentType",
                model.MainImage?.ContentType
                ?? ""),

            // =================================
            // PDF
            // =================================

            new SqlParameter("@PdfFile",
                (object?)pdfBytes
                ?? DBNull.Value),

            new SqlParameter("@PdfFileName",
                model.PdfFile?.FileName
                ?? ""),

            new SqlParameter("@PdfContentType",
                model.PdfFile?.ContentType
                ?? ""),

            // =================================
            // CREATED BY
            // =================================

            new SqlParameter("@CreatedBy",
                user)
        };

                // =====================================
                // EXECUTE PROCEDURE
                // =====================================

                var ds = await _iSql.ExecuteProcedure(
                    "SP_SaveBestPractices",
                    parameters.ToArray()
                );

                // =====================================
                // RESPONSE
                // =====================================

                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    _result.status =
                        Convert.ToBoolean(
                            ds.Tables[0].Rows[0]["Success"]
                        );

                    _result.message =
                        ds.Tables[0].Rows[0]["Message"]
                        .ToString();

                    _result.id =
                        Convert.ToInt64(
                            ds.Tables[0].Rows[0]["BestPracticeId"]
                        );
                }
                else
                {
                    _result.status = false;

                    _result.message =
                        "No response from database.";
                }

                // =====================================
                // SAVE GALLERY IMAGES
                // =====================================

                if (model.GalleryImages != null &&
                    model.GalleryImages.Count > 0)
                {
                    foreach (var file in model.GalleryImages)
                    {
                        if (file != null &&
                            file.Length > 0)
                        {
                            byte[] galleryBytes = null;

                            using (var ms = new MemoryStream())
                            {
                                await file.CopyToAsync(ms);

                                galleryBytes = ms.ToArray();
                            }

                            var galleryParams =
                                new List<SqlParameter>
                                {
                            new SqlParameter(
                                "@BestPracticeId",
                                _result.id
                            ),

                            new SqlParameter(
                                "@ImageData",
                                galleryBytes
                            ),

                            new SqlParameter(
                                "@FileName",
                                file.FileName
                            ),

                            new SqlParameter(
                                "@ContentType",
                                file.ContentType
                            )
                                };

                            await _iSql.ExecuteProcedure(
                                "SP_SaveBestPracticeGallery",
                                galleryParams.ToArray()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _result.status = false;

                _result.message =
                    "Error : " + ex.Message;
            }

            return _result;
        }
        // =============================================
        // REPOSITORY METHOD
        // =============================================
        public async Task<result> UploadGalleryImages(long bestPracticeId, List<IFormFile> files, string user)
        {
            result _result = new result();
            try
            {
                if (files == null || files.Count == 0)
                {
                    _result.status = false;
                    _result.message = "Please select images.";
                    return _result;
                }

                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        byte[] imageBytes;

                        using (MemoryStream ms =
                            new MemoryStream())
                        {
                            await file.CopyToAsync(ms);

                            imageBytes = ms.ToArray();
                        }

                        // =============================
                        // PARAMETERS
                        // =============================

                        var parameters =
                            new List<SqlParameter>
                            {
                        new SqlParameter(
                            "@BestPracticeId",
                            bestPracticeId
                        ),

                        new SqlParameter(
                            "@ImageName",
                            file.FileName
                        ),

                        new SqlParameter(
                            "@ImageData",
                            imageBytes
                        ),

                        new SqlParameter(
                            "@ContentType",
                            file.ContentType
                        )
                            };

                        // =============================
                        // EXECUTE SP
                        // =============================

                        await _iSql.ExecuteProcedure(
                            "SP_SaveBestPracticeGallery",
                            parameters.ToArray()
                        );
                    }
                }

                _result.status = true;
                _result.message =
                    "Gallery images uploaded successfully.";
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message =
                    "Error : " + ex.Message;
            }

            return _result;
        }
        #endregion

        #region Task Tracking
        public async Task<result> SaveTaskTrackingAsync(TaskTrackingModel model, int isFinalSubmit, string user)
        {
            result _result = new result();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action",model.TrackingId > 0? "UPDATE": "INSERT"),
                    new SqlParameter("@TrackingId",model.TrackingId),
                    new SqlParameter("@ActivityGuid",model.ActivityGuid),
                    new SqlParameter("@TaskId",model.TaskId),
                    new SqlParameter("@Status",model.Status ?? (object)DBNull.Value),
                    new SqlParameter("@Achievement",model.Achievement ?? (object)DBNull.Value),
                    new SqlParameter("@Remarks",model.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("@isFinalSubmit", isFinalSubmit),
                    new SqlParameter("@ReportingId", model.reportingId)
                    //new SqlParameter("@CreatedBy",model.CreatedBy),                
                    //new SqlParameter("@ModifiedBy",model.ModifiedBy)
                };

                var ds = await _iSql.ExecuteProcedure("SP_ManageTaskTracking", parameters.ToArray());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _result.status =
                        Convert.ToBoolean(ds.Tables[0].Rows[0]["Status"]);
                    _result.message = ds.Tables[0].Rows[0]["Message"].ToString();
                }
                else
                {
                    _result.status = false;
                    _result.message = "No response from database.";
                }
            }
            catch (Exception ex)
            {
                _result.status = false;
                _result.message = "Error : " + ex.Message;
            }
            return _result;
        }
        public async Task<List<TaskTrackingModel>> GetTaskTrackingAsync(long activityId)
        {
            List<TaskTrackingModel> list =
                new List<TaskTrackingModel>();

            try
            {
                // =========================================
                // PARAMETERS
                // =========================================

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@Action", "GETBYACTIVITY"),

            new SqlParameter("@ActivityId", activityId)
        };

                // =========================================
                // EXECUTE PROCEDURE
                // =========================================

                var ds = await _iSql.ExecuteProcedure
                (
                    "SP_ManageTaskTracking",
                    parameters.ToArray()
                );

                // =========================================
                // BIND DATA
                // =========================================

                if (ds != null
                    && ds.Tables.Count > 0
                    && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(new TaskTrackingModel
                        {
                            TrackingId = Convert.ToInt64(dr["TrackingId"]),
                            ActivityGuid = dr["ActivityGuid"].ToString(),
                            TaskId = Convert.ToInt64(dr["TaskId"]),
                            Status = dr["Status"]?.ToString(),
                            Remarks = dr["Remarks"]?.ToString(),
                            Achievement = dr["Achievement"]?.ToString(),
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return list;
        }
        public async Task<TaskTrackingModel> GetTaskTrackingByIdAsync(long taskId)
        {
            TaskTrackingModel model =
                new TaskTrackingModel();

            try
            {
                // =========================================
                // PARAMETERS
                // =========================================

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@Action", "GETBYTASK"),

            new SqlParameter("@TaskId", taskId)
        };

                // =========================================
                // EXECUTE PROCEDURE
                // =========================================

                var ds = await _iSql.ExecuteProcedure
                (
                    "SP_ManageTaskTracking",
                    parameters.ToArray()
                );

                // =========================================
                // BIND DATA
                // =========================================

                if (ds != null
                    && ds.Tables.Count > 0
                    && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    model.TrackingId = Convert.ToInt64(dr["TrackingId"]);
                    model.ActivityGuid = dr["ActivityGuid"].ToString();
                    model.TaskId = Convert.ToInt64(dr["TaskId"]);
                    model.Status = dr["Status"]?.ToString();
                    model.Remarks = dr["Remarks"]?.ToString();
                    model.Achievement = dr["Achievement"]?.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public async Task UpdateActivityStatusAsync(string activityGuid,string reportingId, string user)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "UpdateActivityStatus"),
                new SqlParameter("@ActivityGuid", activityGuid),
                new SqlParameter("@ReportingId", reportingId),
                new SqlParameter("@CreatedBy", user)
            };

            await _iSql.ExecuteProcedure(
                "SP_ManageActivityTracking",
                parameters.ToArray());
        }

        public async Task SaveContactDetailsAsync(string activityGuid,string reportingId,ContactDetailsModel model,string createdBy)
        {
            var parameters = new List<SqlParameter>
            {
                new("@Action", "SaveContactDetails"),
                new("@ActivityGuid", activityGuid),
                new("@ReportingId", reportingId),
            
                new("@DepartmentNodal", model.DepartmentOfficerName ?? ""),
                new("@DepartmentDesignation", model.DepartmentDesignation ?? ""),
                new("@DepartmentEmail", model.DepartmentEmail ?? ""),
                new("@DepartmentContact", model.DepartmentContactNumber ?? ""),
                new("@DepartmentPlace", model.DepartmentPlace ?? ""),
            
                new("@AgencyNodal", model.AgencyOfficerName ?? ""),
                new("@AgencyDesignation", model.AgencyDesignation ?? ""),
                new("@AgencyEmail", model.AgencyEmail ?? ""),
                new("@AgencyContact", model.AgencyContactNumber ?? ""),
                new("@AgencyPlace", model.AgencyPlace ?? ""),
            
                new("@CreatedBy", createdBy)
            };

            await _iSql.ExecuteProcedure(
                "SP_ManageActivityTracking",
                parameters.ToArray());
        }
        #endregion
    }
}
