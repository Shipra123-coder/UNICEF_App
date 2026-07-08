using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Login
{
    public class loginModels
    {
        public class loginCredentials
        {

            [Required(ErrorMessage = "Username is required.")]
            [StringLength(100, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 100 characters.")]
            [RegularExpression(@"^[a-zA-Z0-9_@.-]+$", ErrorMessage = "Username contains invalid characters.")]
            public string Username { get; set; }

            [BindNever] // Do not bind Password from POST
            [Required(ErrorMessage = "Password is required.")]
            [StringLength(1000, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
             ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
            public string? Password { get; set; } // used only in View            

            [Required(ErrorMessage = "Password is required.")]
            [StringLength(2000, MinimumLength = 64, ErrorMessage = "Hashed password must be 64 characters.")]
            public string HashedPassword { get; set; }

            ////public string username { get; set; }
            ////public string password { get; set; }
            ////public string? ssoid { get; set; }

            //[Required(ErrorMessage = "Username is required.")]
            //[StringLength(100, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 100 characters.")]
            //[RegularExpression(@"^[a-zA-Z0-9_@.-]+$", ErrorMessage = "Username contains invalid characters.")]
            //public string Username { get; set; }

            ////[Required(ErrorMessage = "Password is required.")]
            ////[StringLength(100, MinimumLength = 5, ErrorMessage = "Password hash must be 5 characters.")]
            ////[DataType(DataType.Password)]
            ////public string Password { get; set; }

            ////[Required(ErrorMessage = "Password is required.")]
            ////[StringLength(1000, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            ////[DataType(DataType.Password)]
            ////[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
            //// ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
            //public string? Password { get; set; }

            public string nonce { get; set; }

            //[Required(ErrorMessage = "Password is required.")]
            //[StringLength(1000, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            //[DataType(DataType.Password)]
            //public string HashedPassword { get; set; }

        }
        public class loginResult
        {
            public bool isAuthenticate { get; set; }
            public bool status { get; set; } // 👈 Add this if missing
            public bool isRedirect { get; set; }
            public string? message { get; set; }
            public string? redirectUrl { get; set; } // 👈 Add this if missing
            public userData? user { get; set; }
        }

        public class login_result
        {
            public bool isAuthenticate { get; set; }
            public bool status { get; set; } // 👈 Add this if missing
            public bool isRedirect { get; set; }
            public string? message { get; set; }
            public string? redirectUrl { get; set; } // 👈 Add this if missing
            public UserDetailsData? user { get; set; }
             
        }

        public class UserDetailsData
        {
            public int? userId { get; set; } 
            public string? name { get; set; }
            public string? password { get; set; }
            public string? ssoid { get; set; }
            public string? displayname { get; set; }
            public int? districtId { get; set; }            
            public string? districtName { get; set; }
            public int? agencyId { get; set; }
            public string? agencyName { get; set; }
            public string? agencyCode { get; set; }
            public string? userType { get; set; }
            public string? userLevel { get; set; }
            public int? departmentId { get; set; }
            public string? department { get; set; }
            public string? roleId { get; set; }
            public string? role { get; set; }
            public int? groupId { get; set; }
            public int? FinancialYear { get; set; }
            public string? FinancialYeartext { get; set; }
        }

        public class userData
        {
            public int userId { get; set; }
            public int loginId { get; set; }
            public int roleId { get; set; }
            public string? roleName { get; set; }
            public string? password { get; set; }
            public string? deptDistId { get; set; }
            public string? mobile { get; set; }
            public string? email { get; set; }
            public string? name { get; set; }
            public string pan { get; set; }
            public string gst { get; set; }
            public string? address { get; set; }
            public string pincode { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string isActive { get; set; }
            public string isDelete { get; set; }
            public string tPin { get; set; }
            public string? companyName { get; set; }
            public string? welcomeName { get; set; }
            public string? ssoid { get; set; }
            public string? ssoToken { get; set; }
        }
        public class registration
        {
            public int id { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public string companyName { get; set; }
            public string identityProof1 { get; set; }
            public string identityProof2 { get; set; }
            public string country { get; set; }
            public string address { get; set; }
            public string Services { get; set; }

        }
    }
}
