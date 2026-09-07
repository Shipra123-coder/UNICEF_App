using MO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.User
{
    public interface IUserBL
    {
        Task<UserCreateViewModel> GetUserFormDataAsync();
        Task<(bool Success, string Message)> SaveUserAsync(UserCreateViewModel model, string createdBy);

        Task<IEnumerable<UserListViewModel>> GetAllUsersAsync();
        ///Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<UserEditViewModel?> GetUserForEditAsync(long id);
        Task<(bool Success, string Message)> UpdateUserAsync(UserEditViewModel model,string updatedBy);
    }
}
