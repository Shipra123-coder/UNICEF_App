using BL;
using BL.Account;
using BL.AI;
//using BL.BudgetMaster;
using BL.Common;
using BL.Dashboard;
using BL.ManageActivity;
//using BL.DashBoard;
//using BL.Department;
//using BL.FinancialYear;
//using BL.GauravMaster;
//using BL.GroupMaster;
//using BL.Log;
//using BL.ManageMaster;
using BL.PageAccessRequirement;
using BL.Report;
using BL.SDGGoalService;
using BL.Services.Agency;
using BL.Services.CMDetailsBL;
using BL.Services.ContactBL;
using BL.Services.Department;
using BL.Services.Group;
using BL.Services.MenuBL;
using BL.Services.MenuPermission;
using BL.Services.NatureOfSupportBL;
using BL.Services.NodalDetail;
using BL.Services.Permission;
using BL.Services.SubNatureOfSupportBL;
using BL.Services.SubThemes;
using BL.Services.Theme;
using BL.Services.UNSector;
using BL.Services.User;
using BL.Services.UserLevel;
using BL.ViksitService;

//using BL.ProfileUser;
//using BL.Progres;
//using BL.WebsiteMaster;
//using BL.WorkProgress;
using DL;
using DL.Repositories.Agency;
using DL.Repositories.CMDetailsRepository;
using DL.Repositories.ContactRepository;
using DL.Repositories.Department;
using DL.Repositories.Group;
using DL.Repositories.MenuPermission;
using DL.Repositories.MenuRepository;
using DL.Repositories.NodalDetail;
using DL.Repositories.Permission;
using DL.Repositories.SubNatureOfSupportRepository;
using DL.Repositories.SubThemes;
using DL.Repositories.ThemeRepository;
using DL.Repositories.UNSector;
using DL.Repositories.User;
using DL.Repositories.UserLevel;
using Microsoft.AspNetCore.Authorization;
using MO.Repositories;
using System.Data;
using UNICEF_App.Service;
namespace UNICEF_App
{
    public class ServiceInject
    {
        public ServiceInject(IServiceCollection services)
        {
            services.AddScoped(typeof(ISQLHelper), typeof(SQLHelper));
            services.AddScoped(typeof(ILogin), typeof(Login));
            services.AddScoped(typeof(ICommon), typeof(Common));
            services.AddScoped(typeof(IManageActivity), typeof(ManageActivity));
            services.AddScoped(typeof(IMonitoringService), typeof(MonitoringService));
            services.AddScoped(typeof(IManageActivityMedia), typeof(ManageActivityMediaService));
            services.AddScoped(typeof(IDashboard), typeof(Dashboard));
            //services.AddScoped(typeof(IProgressReport), typeof(ProgressReport));
            services.AddScoped(typeof(IInputValidator), typeof(InputValidator));
            //services.AddScoped(typeof(IGauravProfileAnswer), typeof(GauravProfileAnswer));
            //services.AddScoped(typeof(IProfileUser), typeof(ProfileUser));
            //services.AddScoped(typeof(IMenu), typeof(Menu));
            //services.AddScoped(typeof(IGroupMaster), typeof(GroupMaster));
            services.AddScoped(typeof(IPermission), typeof(Permission));
            services.AddScoped(typeof(IOpenAIService), typeof(OpenAIService));
            services.AddScoped(typeof(ISDGGoalServices),typeof(SDGGoalService));
            services.AddScoped(typeof(IViksitService),typeof(ViksitService));
            services.AddScoped(typeof(IDepartmentReportService), typeof(DepartmentReportService));
          

            // 2. Data Layer (DL) DI Registration
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLevelRepository, UserLevelRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<ISubThemesRepository, SubThemesRepository>();
            services.AddScoped<IUNSectorRepository, UNSectorRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<INatureOfSupportRepository, NatureOfSupportRepository>();
            services.AddScoped<ISubNatureOfSupportRepository, SubNatureOfSupportRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ICMDetailsRepository, CMDetailsRepository>();
            services.AddScoped<INodalDetailRepository, NodalDetailRepository>();
            services.AddScoped<IMenuPermissionRepository, MenuPermissionRepository>();

            // Repository & BL Bindings
            services.AddScoped<IPermissionRepository, PermissionRepository>();
           


            // 3. Business Logic Layer (BL) DI Registration
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IUserLevelBL, UserLevelBL>();
            services.AddScoped<IDepartmentBL, DepartmentBL>();            
            services.AddScoped<IAgencyBL, AgencyBL>();
            services.AddScoped<ISubThemesBL, SubThemesBL>();
            services.AddScoped<IThemeBL, ThemeBL>();
            services.AddScoped<IUNSectorBL, UNSectorBL>();
            services.AddScoped<IGroupBL, GroupBL>();
            services.AddScoped<INatureOfSupportBL, NatureOfSupportBL>();
            services.AddScoped<ISubNatureOfSupportBL, SubNatureOfSupportBL>();
            services.AddScoped<IMenuBL, MenuBL>();
            services.AddScoped<IContactBL, ContactBL>();            
            services.AddScoped<ICMDetailsBL, CMDetailsBL>();            
            services.AddScoped<INodalDetailBL, NodalDetailBL>();
            services.AddScoped<IMenuPermissionBL, MenuPermissionBL>();
            services.AddScoped<IPermissionBL, PermissionBL>();

            services.AddScoped<IAuthorizationHandler, PageAccessHandler>();            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
          

            services.AddHttpContextAccessor();          
        }
    }
}
