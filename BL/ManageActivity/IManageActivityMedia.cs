using MO.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ManageActivity
{
    public interface IManageActivityMedia
    {
        #region Activity Media
        // 1. Master Cover Page को डेटाबेस में सेव करने के लिए मेथड
        Task<long> SaveCoverPageAsync(string activityGuid, string heading, int orderNumber, string description, string imageUrl, string pdfUrl, string createdBy, long subActivityId,
            long taskId,
            long geoLocationId,string AgencyId);

        // 2. Child Inner Media (Image/Video/Audio) को डेटाबेस में सेव करने के लिए मेथड
        Task<long> SaveInnerMediaAsync(long parentCoverId, string mediaType, string mediaAssetUrl, string description, string createdBy);
        Task<DataTable> GetCoverPagesListAsync(string activityGuid);
        Task<DataTable> GetInnerMediaListAsync(long coverPageId);
        Task<result> GetActiveCoversByActivityAsync(string activityGuid);
        Task<DataSet> GetAssetFilePathsAsync(string mode, long id);
        Task<bool> DeleteMediaAssetAsync(string mode, long id);
        #endregion
        #region Add Direct Media        
        Task<long> SaveDirectCoverPageAsync(long sectorId, string heading, int orderNumber, string description, string imageUrl, string pdfUrl, string createdBy,string AgencyId);
        Task<long> SaveDirectInnerMediaAsync(long parentCoverId, string mediaType, string mediaAssetUrl, string description, string createdBy);       
        Task<DataSet> GetDirectCoverPagesListAsync(int sectorId);
        Task<result> GetDirectCoversDropdownBySectorAsync(int sectorId);
        Task<DataSet> GetDirectInnerMediaListAsync(long coverPageId,int? sectorId);
        #endregion
    }
}
