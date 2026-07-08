using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.WorkMode
{
    public class WorkMode
    {
        public int DistrictId { get; set; }

        public int BlockId { get; set; }

        public int PanchayatId { get; set; }

        public string SchemeName { get; set; }

        public string WorkTitle { get; set; }

        public string WorkType { get; set; }

        public decimal TotalApprovedAmount { get; set; }

        public string AgencyName { get; set; }

        public string OfficerName { get; set; }
    }
    public class BasicModel
    {
        public int WorkId { get; set; }
        public int DistrictId { get; set; }
        public int BlockId { get; set; }
        public int PanchayatId { get; set; }
        public int AssemblyId { get; set; }
        public int ParliamentId { get; set; }
    }
    public class WorkModel
    {
        public int WorkId { get; set; }

        public string SchemeName { get; set; }

        public string WorkTitle { get; set; }

        public string WorkType { get; set; }

        public string WorkNature { get; set; }

        public string WorkPlace { get; set; }

        public string Description { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpectedCompletion { get; set; }

        public DateTime? ActualCompletion { get; set; }
    }
    public class WorkBudgetModel
    {
        public int WorkId { get; set; }
        [Range(0, 999999999)]
        public decimal ApprovedAmount { get; set; }
        [Range(0, 999999999)]
        public decimal ExpenditureAmount { get; set; }

        public string ExpenditurePercent { get; set; }
        public string DistrictRank { get; set; }
        public string FinancialProgress { get; set; }
        public string PaymentStatus { get; set; }
        public string BudgetHeadName{ get; set; }

        public string BudgetHeadCode { get; set; }

        public int FinancialYearId { get; set; }
        public string NodalDepartment { get; set; }
    }
    public class WorkAgencyModel
    {
        public int WorkId { get; set; }

        public string AgencyName { get; set; }

        public string AgencyType { get; set; }

        public string RegistrationNo { get; set; }

        public string AgencyContactPerson { get; set; }

        public string AgencyContactPersonDesignation { get; set; }

        public string AgencyContactPersonMobileNo { get; set; }

        public string MonitoringOfficer { get; set; }

        public string MonitoringOfficerDesignation { get; set; }

        public string MonitoringOfficerMobile { get; set; }
    }
    public class WorkNodalOfficerModel
    {
        public int WorkId { get; set; }

        public string NodalOfficerName { get; set; }

        public string Designation { get; set; }

        public string OfficeName { get; set; }

        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string Level { get; set; }
    }
    public class WorkGeoTag
    {
        public int Id { get; set; }

        public int WorkId { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string? GeoTaggedPhoto { get; set; }

        public DateTime CreatedDate { get; set; }

   
    }

}
