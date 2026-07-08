using MO.Common;
using MO.Master;
using MO.ProfileUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public interface ICommon
    {
        Task<result> BindGauravDropDown(string userId);
        Task<result> BindDistrictDropDown(string userId);
        //Task<List<DistrictListDDL>> DDL_DistrictAsync();
        Task<List<RoleListDDL>> DDL_RoleAsync();        
        Task<List<UserLevelListDDL>> DDL_UserLevelAsync();        
        Task<List<GroupDDL>> DDL_GroupAsync();
        Task<List<DistrictListDDL>> DDL_DistrictByUserIdAsync(string userId);
        Task<List<FinancialYearDDL>> DDL_FinancialYearAsync();
        Task<List<FinancialYearListMO>> FinancialYearList(FinancialYearListMO modal);

        Task<result> DDL_AgencyAsync();
        Task<result> DDL_DepartmentAsync();
        Task<result> DDL_SupDepartmentAsync(int? Id);
        Task<result> DDL_PillerAsync();
        Task<result> DDL_SectorAsync(int? Id);
        Task<result> DDL_SubSectorAsync(int? Id);
        Task<result> DDL_GoalAsync();
        Task<result> DDL_TargetAsync(int? Id);

        Task<result> DDL_DistrictAsync();
        Task<result> DDL_BlockAsync(int? Id);
        Task<result> DDL_CityAsync(int? Id);

        Task<result> DDL_UNSectorAsync();
       Task<result> DDL_NatureOfSupportAsync();
        Task<result> DDL_SubNatureOfSupportAsync(int? Id);
    }
}
