using MO.Entities;


namespace BL.Services.UNSector
{
    public interface IUNSectorBL
    {
        Task<IEnumerable<mst_UNSector>> GetAllUNSectorsAsync();
        Task<mst_UNSector?> GetUNSectorByIdAsync(long id);
        Task<(bool Success, string Message)> SaveUNSectorAsync(mst_UNSector model);
        Task<(bool Success, string Message)> DeleteUNSectorAsync(long id);
        Task<(bool Success, string Message)> ToggleUNSectorStatusAsync(long id);
    }
}
