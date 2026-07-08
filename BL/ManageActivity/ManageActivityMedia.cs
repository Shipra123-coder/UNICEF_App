using DL;
using MO.Common;
using MO.Management;
using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ManageActivity
{
    public class ManageActivityMediaService : IManageActivityMedia
    {
        private readonly ISQLHelper _iSql;


        public ManageActivityMediaService(ISQLHelper iSql)
        {
            _iSql = iSql;
        }

        #region Add Activity BestPrectices
        public async Task<long> SaveCoverPageAsync(string activityGuid, string heading, int orderNumber,
            string description, string imageUrl, string pdfUrl, string createdBy,
            long subActivityId,
            long taskId,
            long geoLocationId,string AgencyId)
        {
            try
            {
                // आपके SP पैटर्न के अनुसार पैरामीटर्स की लिस्ट तैयार करें
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "SaveCoverPage"),
                    new SqlParameter("@ActivityGuid", activityGuid),
                    new SqlParameter("@HeadingTitle", heading),
                    new SqlParameter("@DisplayOrder", orderNumber),
                    new SqlParameter("@CoverImageUrl", string.IsNullOrEmpty(imageUrl) ? (object)DBNull.Value : imageUrl),
                    new SqlParameter("@DescriptionPdfUrl", string.IsNullOrEmpty(pdfUrl) ? (object)DBNull.Value : pdfUrl),
                    new SqlParameter("@DescriptionNotes", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                    new SqlParameter("@CreatedBy", createdBy),
                    new SqlParameter("@SubActivityId", subActivityId),
                    new SqlParameter("@TaskId", taskId),
                    new SqlParameter("@GeoLocationId", geoLocationId),
                    new SqlParameter("@AgencyId", AgencyId)
                };

                // Stored Procedure एग्जीक्यूट करें और रिटर्न में न्यूली जनरेटेड ID प्राप्त करें
                // नोट: अगर आपका ExecuteProcedure डेटा टेबल या स्केलर रिटर्न करता है, तो उसे यहाँ टाइपकास्ट करें।
                // यदि आपका मेथड केवल डायरेक्ट एग्जीक्यूट करता है, तो आप रिटर्न टाइप बदल सकते हैं या SP से SCOPE_IDENTITY सेलेक्ट करवा सकते हैं।
                var ds = await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", parameters.ToArray());

                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    return Convert.ToInt64(ds.Tables[0].Rows[0][0]);
                }

                return 1; // Fallback success identifier
            }
            catch (Exception ex)
            {
                // एरर को डायग्नोस्टिक्स में थ्रो करें ताकि कंट्रोलर का कैच इसे हैंडल कर सके
                throw new Exception("Error in SaveCoverPageAsync: " + ex.Message, ex);
            }
        }


        public async Task<long> SaveInnerMediaAsync(long parentCoverId, string mediaType, string mediaAssetUrl, string description, string createdBy)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "SaveInnerMedia"),
                    new SqlParameter("@CoverPageId", parentCoverId),
                    new SqlParameter("@MediaType", mediaType),
                    new SqlParameter("@MediaAssetUrl", mediaAssetUrl),
                    new SqlParameter("@DescriptionNotes", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                    new SqlParameter("@CreatedBy", createdBy)
                };

                // उसी Stored Procedure के अंदर Action बदलकर कॉल करें
                var ds = await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", parameters.ToArray());

                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt64(ds.Tables[0].Rows[0][0]);
                }

                return 1; // Fallback success identifier
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SaveInnerMediaAsync: " + ex.Message, ex);
            }
        }

        public async Task<DataTable> GetCoverPagesListAsync(string activityGuid)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "GetCoversList"),
                    new SqlParameter("@ActivityGuid", activityGuid)
                };

                // SP को कॉल करके सीधे डाटा टेबल रिटर्न करें जिसे कंट्रोलर जेसन में बदलेगा
                DataSet ds = await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", parameters.ToArray());
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCoverPagesListAsync: " + ex.Message, ex);
            }
        }


        public async Task<DataTable> GetInnerMediaListAsync(long coverPageId)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Action", "GetInnerMediaList"),
                    new SqlParameter("@CoverPageId", coverPageId)
                };

                DataSet ds = await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", parameters.ToArray());
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetInnerMediaListAsync: " + ex.Message, ex);
            }
        }
        public async Task<result> GetActiveCoversByActivityAsync(string activityGuid)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "GetCoversDropdown"));
                param.Add(new("@ActivityGuid", activityGuid));
                DataSet ds = await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Value"]),
                            name = Convert.ToString(x["Text"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }
        public async Task<DataSet> GetAssetFilePathsAsync(string mode, long id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetAssetFilePath"),
                new SqlParameter("@Mode", mode),
                new SqlParameter("@CoverPageId", id) // Mapping dynamically in SP
            };
            return await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", parameters.ToArray());
        }
        public async Task<bool> DeleteMediaAssetAsync(string mode, long id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "DeleteAsset"),
                new SqlParameter("@Mode", mode),
                new SqlParameter("@CoverPageId", id)
            };
            var dt = await _iSql.ExecuteProcedure("SP_ManageActivityMediaShowcase", parameters.ToArray());
            return true; // Assuming execution block passes safely without syntax block breaks
        }
        #endregion
        #region Add Direct Best Prectices
        public async Task<long> SaveDirectCoverPageAsync(long sectorId, string heading, int orderNumber, string description, string imageUrl, string pdfUrl, string createdBy,string agencyId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "SaveCoverPage"),
                new SqlParameter("@SectorId", sectorId),
                new SqlParameter("@HeadingTitle", heading),
                new SqlParameter("@DisplayOrder", orderNumber),
                new SqlParameter("@CoverImageUrl", string.IsNullOrEmpty(imageUrl) ? (object)DBNull.Value : imageUrl),
                new SqlParameter("@DescriptionPdfUrl", string.IsNullOrEmpty(pdfUrl) ? (object)DBNull.Value : pdfUrl),
                new SqlParameter("@DescriptionNotes", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                new SqlParameter("@CreatedBy", createdBy),
                new SqlParameter("@AgencyId", agencyId)
            };

            DataSet ds = await _iSql.ExecuteProcedure("SP_ManageMediaShowcase", parameters.ToArray());
            return (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) ? Convert.ToInt64(ds.Tables[0].Rows[0][0]) : 1;
        }

        public async Task<long> SaveDirectInnerMediaAsync(long parentCoverId, string mediaType, string mediaAssetUrl, string description, string createdBy)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "SaveInnerMedia"),
                new SqlParameter("@CoverPageId", parentCoverId),
                new SqlParameter("@MediaType", mediaType),
                new SqlParameter("@MediaAssetUrl", mediaAssetUrl),
                new SqlParameter("@DescriptionNotes", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                new SqlParameter("@CreatedBy", createdBy)
            };

            DataSet ds = await _iSql.ExecuteProcedure("SP_ManageMediaShowcase", parameters.ToArray());
            return (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) ? Convert.ToInt64(ds.Tables[0].Rows[0][0]) : 1;
        }

        public async Task<DataSet> GetDirectCoverPagesListAsync(int sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetCoversList"),
                new SqlParameter("@SectorId", sectorId)
            };
            return await _iSql.ExecuteProcedure("SP_ManageMediaShowcase", parameters.ToArray());
        }

        public async Task<result> GetDirectCoversDropdownBySectorAsync(int sectorId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Action", "GetCoversDropdown"));
                param.Add(new("@SectorId", sectorId));
                DataSet ds = await _iSql.ExecuteProcedure("SP_ManageMediaShowcase", param.ToArray());
                if (ds.Tables.Count > 0)
                {
                    return new result
                    {
                        status = true,
                        data = ds.Tables[0].AsEnumerable().Select(x => new
                        {
                            id = Convert.ToInt32(x["Value"]),
                            name = Convert.ToString(x["Text"]),
                        }).ToList()
                    };
                }
                else
                {
                    return new result
                    {
                        status = false,
                        message = "Something went wrong",
                    };
                }
            }
            catch (Exception ex)
            {
                return new result
                {
                    status = false,
                    message = "Something went wrong",
                };
            }

        }       

        public async Task<DataSet> GetDirectInnerMediaListAsync(long coverPageId,int? sectorId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Action", "GetInnerMediaList"),
                new SqlParameter("@CoverPageId", coverPageId),
                new SqlParameter("@SectorId", sectorId)
            };
            return await _iSql.ExecuteProcedure("SP_ManageMediaShowcase", parameters.ToArray());
        }
        #endregion
    }
}