using BL.Common;
using BL.ManageActivity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
using MO.Common;
using MO.Management;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using UNICEF_App.Helpers;
using Ganss.Xss; // 🌟 सुनिश्चित करें कि NuGet से Ganss.Xss पैकेज इंस्टॉल है
using System.Reflection;
using System.Text.RegularExpressions;

namespace UNICEF_App.Controllers
{
    public class ManagementController : Controller
    {
        private readonly ICommon _iCommon;
        private readonly IManageActivity _iManageActivity;
        private readonly IMonitoringService _iMonitoringService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IManageActivityMedia _iActivityMediaService;
        public ManagementController(ICommon iCommon, IManageActivity iManageActivity, IMonitoringService iMonitoringService, IWebHostEnvironment hostingEnvironment, IManageActivityMedia iActivityMediaService)
        {
            _iCommon = iCommon;
            _iManageActivity = iManageActivity;
            _iMonitoringService = iMonitoringService;
            _hostingEnvironment = hostingEnvironment;
            _iActivityMediaService = iActivityMediaService;
        }

        public async Task<IActionResult> Main(string? guid)
        {
            // --- Claims Expired Check ---
            if (!CommonHelper.IsUserValid(User, out int groupId, out int userId))
            {
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Login", "Account");
            }

            //id =2;
            MainModels model = new MainModels();

            if (!string.IsNullOrEmpty(guid))
            {
                // EDIT MODE
                model.ActivityGuid = guid;
                model.Activity = await _iManageActivity.GetFullActivityDetails(guid);
                return View(model);
            }
            model.Activity = new ActivityMaster();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveActivity(ActivityMaster model)
        {
            try
            {
                // 🌟 1. HTML Sanitizer का इंस्टेंस बनाएं
                var sanitizer = new HtmlSanitizer();

                if (model != null)
                {
                    // -------------------------------------------------------------
                    // 🌟 स्तर ए: मुख्य ActivityMaster की सभी स्ट्रिंग प्रॉपर्टीज को क्लीन करें
                    // -------------------------------------------------------------
                    var mainStringProps = typeof(ActivityMaster).GetProperties()
                        .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

                    foreach (var prop in mainStringProps)
                    {
                        var val = prop.GetValue(model)?.ToString();

                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            val = sanitizer.Sanitize(val).Trim();

                            if (ContainsInvalidCharacters(val))
                            {
                                return Json(new
                                {
                                    status = false,
                                    message = $"{prop.Name} contains invalid special characters."
                                });
                            }

                            prop.SetValue(model, val);
                        }
                    }

                    // -------------------------------------------------------------
                    // 🌟 स्तर बी: इसके अंदर की SubActivities लिस्ट को डीप क्लीन करें
                    // -------------------------------------------------------------
                    if (model.SubActivities != null && model.SubActivities.Count > 0)
                    {
                        var subStringProps = typeof(SubActivityMaster).GetProperties()
                            .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

                        foreach (var subItem in model.SubActivities)
                        {
                            foreach (var prop in subStringProps)
                            {
                                var val = prop.GetValue(subItem)?.ToString();

                                if (!string.IsNullOrWhiteSpace(val))
                                {
                                    val = sanitizer.Sanitize(val).Trim();

                                    if (ContainsInvalidCharacters(val))
                                    {
                                        return Json(new
                                        {
                                            status = false,
                                            message = $"Sub Activity field {prop.Name} contains invalid characters."
                                        });
                                    }

                                    prop.SetValue(subItem, val);
                                }
                            }
                        }
                    }

                    // -------------------------------------------------------------
                    // 🌟 स्तर सी: इसके अंदर की Tasks लिस्ट को भी डीप क्लीन करें
                    // -------------------------------------------------------------
                    if (model.Tasks != null && model.Tasks.Count > 0)
                    {
                        var taskStringProps = typeof(TaskMaster).GetProperties()
                            .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

                        foreach (var taskItem in model.Tasks)
                        {
                            foreach (var prop in taskStringProps)
                            {
                                var val = prop.GetValue(taskItem)?.ToString();

                                if (!string.IsNullOrWhiteSpace(val))
                                {
                                    val = sanitizer.Sanitize(val).Trim();

                                    if (ContainsInvalidCharacters(val))
                                    {
                                        return Json(new
                                        {
                                            status = false,
                                            message = $"Task field {prop.Name} contains invalid characters."
                                        });
                                    }

                                    prop.SetValue(taskItem, val);
                                }
                            }
                        }
                    }
                }

                // 2. लॉगिन यूजर और एजेंसी कॉन्टेक्स्ट एक्स्ट्रैक्शन
                string userId = User.Identity?.Name ?? "SystemAdmin";
                string AgencyId = User.FindFirst("AgencyId")?.Value ?? "0";

                // 3. पूरी तरह से सुरक्षित डेटा पेलोड को आगे भेजें
                var response = await _iManageActivity.SaveActivityWithTasks(model, userId, AgencyId);

                if (response.status)
                {
                    return Json(new { status = true, message = response.message, activityGuid = response.id });
                }

                return Json(new { status = false, message = response.message });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveActivity Deep Security Error: {ex.Message}");
                return Json(new { status = false, message = "Input parameters failed structural security compliance checks." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDepartmentMapping(DeptMappingViewModel model)
        {
            try
            {
                // 1. प्राथमिक वैलिडेशन चेक
                if (model == null || string.IsNullOrEmpty(model.ActivityGuid))
                {
                    return Json(new { status = false, message = "Invalid Activity Reference." });
                }

                // 🌟 2. HTML Sanitizer का इंस्टेंस बनाएं
                var sanitizer = new HtmlSanitizer();

                // 🌟 स्तर ए: मॉडल की सभी डायरेक्ट स्ट्रिंग प्रॉपर्टीज को क्लीन करें (ActivityGuid, NodalDepartmentId)
                var stringProperties = typeof(DeptMappingViewModel).GetProperties()
                    .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

                foreach (var prop in stringProperties)
                {
                    var val = (string)prop.GetValue(model);
                    if (!string.IsNullOrEmpty(val))
                    {
                        prop.SetValue(model, sanitizer.Sanitize(val).Trim());
                    }
                }

                // 🌟 स्तर बी: लिस्ट के अंदर मौजूद आइटम्स (SupportingDepartmentIds) को क्लीन करें
                if (model.SupportingDepartmentIds != null && model.SupportingDepartmentIds.Count > 0)
                {
                    for (int i = 0; i < model.SupportingDepartmentIds.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(model.SupportingDepartmentIds[i]))
                        {
                            // लिस्ट की हर स्ट्रिंग वैल्यू से स्क्रिप्ट्स और टैग्स को साफ़ करें
                            
                            model.SupportingDepartmentIds[i] = SanitizeAndValidate(model.SupportingDepartmentIds[i]);
                        }
                    }
                }

                // 3. लॉगिन यूजर संदर्भ निकालें
                string userId = User.Identity?.Name ?? "SystemAdmin";

                // 4. सुरक्षित मॉडल को आगे रिपोजिटरी/सर्विस लेयर में पास करें
                var response = await _iManageActivity.SaveDepartmentMappingAsync(model, userId);

                if (response.status)
                {
                    return Json(new { status = true, message = response.message, activityGuid = model.ActivityGuid });
                }

                return Json(new { status = false, message = response.message });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveDepartmentMapping Security Error: {ex.Message}");
                return Json(new { status = false, message = "Input metrics failed security compliance validation." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> GetDeptMap(string activityGuid)
        {
            // --- Claims Expired Check ---
            if (!CommonHelper.IsUserValid(User, out int groupId, out int userId))
            {
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Login", "Account");
            }

            var data = await _iManageActivity.GetDeptMap(activityGuid);

            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGoalMapping(GoalMappingModel model)
        {
            try
            {
                // 1. प्राथमिक इनपुट वैलिडेशन चेक
                if (model == null || model.MappingData == null)
                {
                    return Json(new { status = false, message = "Invalid data submitted." });
                }

                // 🌟 2. HTML Sanitizer का इंस्टेंस बनाएं
                var sanitizer = new HtmlSanitizer();

                // ---------------------------------------------------------------------
                // 🌟 स्तर ए (Level 1): मुख्य मॉडल की स्ट्रिंग्स को साफ करें (ActivityGuid)
                // ---------------------------------------------------------------------
                if (!string.IsNullOrEmpty(model.ActivityGuid))
                {
                    model.ActivityGuid = sanitizer.Sanitize(model.ActivityGuid).Trim();
                }

                // ---------------------------------------------------------------------
                // 🌟 स्तर बी (Level 2): MappingData (Goals) लिस्ट को लूप करके साफ करें
                // ---------------------------------------------------------------------
                //foreach (var goal in model.MappingData)
                //{
                //    if (goal == null) continue;

                //    // Goal का नाम सैनिटाइज करें
                //    if (!string.IsNullOrEmpty(goal.GoalName))
                //    {
                //        goal.GoalName = SanitizeAndValidate(goal.GoalName);
                //    }

                //    // ---------------------------------------------------------------------
                //    // 🌟 स्तर सी (Level 3): प्रत्येक Goal के अंदर मौजूद Targets लिस्ट को साफ करें
                //    // ---------------------------------------------------------------------
                //    if (goal.Targets != null && goal.Targets.Count > 0)
                //    {
                //        foreach (var target in goal.Targets)
                //        {
                //            if (target == null) continue;

                //            // Target का नाम सैनिटाइज करें (सबसे गहरी सुरक्षा पॉइंट)
                //            if (!string.IsNullOrEmpty(target.TargetName))
                //            {
                //                target.TargetName = SanitizeAndValidate(target.TargetName);
                //            }
                //        }
                //    }
                //}

                // 3. लॉगिन यूजर संदर्भ निकालें
                string userId = User.Identity?.Name ?? "SystemAdmin";

                // 4. अब यह पेलोड 100% सिक्योर है, इसे सर्विस लेयर में पास करें
                var response = await _iManageActivity.SaveGoalMappingAsync(model, userId);

                if (response.status)
                {
                    return Json(new { status = true, message = response.message, activityGuid = model.ActivityGuid });
                }

                return Json(new { status = false, message = response.message });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveGoalMapping Nested Security Error: {ex.Message}");
                return Json(new { status = false, message = "Structural compliance security check failed on nested nodes." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetGoalMap(string activityGuid)
        {
            //int activityId = Convert.ToInt32(model.activityId);

            var result = await _iManageActivity.GetGoalMappingAsync(activityGuid);

            return Json(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePillarMapping(PillarMappingModel model)
        {
            try
            {
                // 🔹 1. Validation (प्राथमिक इनपुट वैलिडेशन चेक)
                if (model == null || model.MappingData == null || !model.MappingData.Any())
                {
                    return Json(new { status = false, message = "Invalid or empty data submitted." });
                }

                // 🌟 2. HTML Sanitizer का इंस्टेंस बनाएं
                var sanitizer = new HtmlSanitizer();

                // ---------------------------------------------------------------------
                // 🌟 स्तर 1 (Top Level): ActivityGuid को साफ करें
                // ---------------------------------------------------------------------
                if (!string.IsNullOrEmpty(model.ActivityGuid))
                {
                    model.ActivityGuid = sanitizer.Sanitize(model.ActivityGuid).Trim();
                }

                // ---------------------------------------------------------------------
                // 🌟 स्तर 2 (Level 2): MappingData (Pillars) लिस्ट को साफ करें
                // ---------------------------------------------------------------------
                //foreach (var pillar in model.MappingData)
                //{
                //    if (pillar == null) continue;

                //    if (!string.IsNullOrEmpty(pillar.PillarName))
                //    {
                //        pillar.PillarName =  SanitizeAndValidate(pillar.PillarName);
                //    }

                //    // ---------------------------------------------------------------------
                //    // 🌟 स्तर 3 (Level 3): प्रत्येक Pillar के अंदर मौजूद Sectors लिस्ट को साफ करें
                //    // ---------------------------------------------------------------------
                //    if (pillar.Sectors != null && pillar.Sectors.Count > 0)
                //    {
                //        foreach (var sector in pillar.Sectors)
                //        {
                //            if (sector == null) continue;

                //            if (!string.IsNullOrEmpty(sector.SectorName))
                //            {
                //                sector.SectorName = SanitizeAndValidate(sector.SectorName);
                //            }

                //            // ---------------------------------------------------------------------
                //            // 🌟 स्तर 4 (Level 4 - Deepest Point): प्रत्येक Sector के अंदर SubSectors को साफ करें
                //            // ---------------------------------------------------------------------
                //            if (sector.SubSectors != null && sector.SubSectors.Count > 0)
                //            {
                //                foreach (var subSector in sector.SubSectors)
                //                {
                //                    if (subSector == null) continue;

                //                    if (!string.IsNullOrEmpty(subSector.SubSectorName))
                //                    {
                //                        // सबसे गहरे नोड पर मौजूद स्ट्रिंग से भी XSS कचरा बाहर निकालें
                //                        subSector.SubSectorName = SanitizeAndValidate(subSector.SubSectorName);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                // 🔹 3. Logged-in User संदर्भ निकालें
                string userId = User.Identity?.Name ?? "SystemAdmin";

                // 🔹 4. Call BL / Interface (पूरी तरह सुरक्षित मॉडल पास करें)
                var response = await _iManageActivity.SavePillarMappingAsync(model, userId);

                // 🔹 5. Response
                if (response.status)
                {
                    return Json(new
                    {
                        status = true,
                        message = response.message,
                        activityGuid = model.ActivityGuid
                    });
                }

                return Json(new { status = false, message = response.message });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SavePillarMapping Deep Security Error: {ex.Message}");
                return Json(new { status = false, message = "Input elements failed structural anti-XSS compliance checks." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPillarMap(string activityGuid)
        {
            // 🔹 Call BL
            var result = await _iManageActivity.GetPillarMappingAsync(activityGuid);

            // 🔹 Return JSON
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNatureOfSupportMapping(NatureOfSupportMappingModel model)
        {
            try
            {
                // 🔹 1. Validation (प्राथमिक इनपुट वैलिडेशन चेक)
                if (model == null || model.SupportData == null)
                {
                    return Json(new { status = false, message = "Invalid dataset submitted." });
                }

                // 🌟 2. HTML Sanitizer का इंस्टेंस बनाएं
                var sanitizer = new HtmlSanitizer();

                // ---------------------------------------------------------------------
                // 🌟 स्तर 1 (Top Level): ActivityGuid को साफ करें
                // ---------------------------------------------------------------------
                if (!string.IsNullOrEmpty(model.ActivityGuid))
                {
                    model.ActivityGuid = sanitizer.Sanitize(model.ActivityGuid).Trim();
                }

                // ---------------------------------------------------------------------
                // 🌟 स्तर 2 (Level 2): SupportData (मुख्य सपोर्ट आइटम्स) लिस्ट को साफ करें
                // ---------------------------------------------------------------------
                //foreach (var support in model.SupportData)
                //{
                //    if (support == null) continue;

                //    // Support Name को सैनिटाइज करें
                //    if (!string.IsNullOrEmpty(support.SupportName))
                //    {
                //        support.SupportName = SanitizeAndValidate(support.SupportName);
                //    }

                //    // ---------------------------------------------------------------------
                //    // 🌟 स्तर 3 (Level 3 - Deepest Point): इसके अंदर की SupportDetails लिस्ट को साफ करें
                //    // ---------------------------------------------------------------------
                //    if (support.SupportDetails != null && support.SupportDetails.Count > 0)
                //    {
                //        foreach (var detail in support.SupportDetails)
                //        {
                //            if (detail == null) continue;

                //            // Detail Name को सैनिटाइज करें (जैसे सब-प्रकार का विवरण या रिमार्क्स)
                //            if (!string.IsNullOrEmpty(detail.DetailName))
                //            {
                //                detail.DetailName = SanitizeAndValidate(detail.DetailName); 
                //            }
                //        }
                //    }
                //}

                // 🔹 3. लॉगिन यूजर संदर्भ निकालें
                string userId = User.Identity?.Name ?? "SystemAdmin";

                // 🔹 4. सुरक्षित मॉडल को आगे रिपोजिटरी / बिजनेस लेयर में भेजें
                var response = await _iManageActivity.SaveNatureOfSupportMappingAsync(model, userId);

                // 🔹 5. Response हैंडलिंग
                if (response.status)
                {
                    return Json(new { status = true, message = response.message, activityGuid = model.ActivityGuid });
                }

                return Json(new { status = false, message = response.message });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveNatureOfSupportMapping Security Error: {ex.Message}");
                return Json(new { status = false, message = "Input parameters failed multi-layered structural security compliance checks." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetNatureOfSupportMap(string activityGuid)
        {
            //int activityId = Convert.ToInt32(model.activityId);

            var result = await _iManageActivity.GetNatureOfSupportMappingAsync(activityGuid);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetFullData(string guid)
        {
            var json_activity = await _iManageActivity.GetActivityDataAsync(guid);
            var data = new
            {
                activity = JsonConvert.DeserializeObject<ActivityDTO>(json_activity),
                departments = await _iManageActivity.GetDeptMap(guid),
                //sectors = await _iManageActivity.GetSectorMap(activityId),
                goals = await _iManageActivity.GetGoalMappingAsync(guid),
                pillars = await _iManageActivity.GetPillarMappingAsync(guid),
                natureofsupport = await _iManageActivity.GetNatureOfSupportMappingAsync(guid)
            };
            return Json(data);
        }

        public async Task<IActionResult> ActivityLand(string guid)
        {
            ViewBag.Guid = guid;
            return View(); // ye UI page open karega
        }
        public async Task<IActionResult> ActivityManagement()
        {
            string AgencyId = User.FindFirst("AgencyId")?.Value ?? "0";
            // --- Claims Expired Check ---
            if (!CommonHelper.IsUserValid(User, out int groupId, out int userId))
            {
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Login", "Account");
            }
            var data = await _iManageActivity.GetActivityList(AgencyId);
            return View(data);
        }
        public async Task<IActionResult> Activity()
        {
            return View();
        }

        public async Task<IActionResult> Index(string? guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                // Fetch existing monitoring data
                var monitoringData = await _iMonitoringService.GetMonitoringAsync(guid);

                if (monitoringData != null)
                {

                    // Pass the data to the view
                    ViewBag.SelectedSupports = monitoringData?.Supports ?? new List<SupportModel>();
                    ViewBag.MonitoringData = monitoringData;
                }
            }
            ViewBag.AllDocuments = new List<string>
            {
                "Letter Exchange",
                "Signed Work Plan",
                "Memorandum of Understanding",
                "Letter of Understanding",
                "Other"
            };
            ViewBag.AllSupports = new List<string>
            {
                "Technical Advice / Expertise",
                "Technical Assistance (NGO)",
                "Human Resources",
                "Financial Resources",
                "Project-based Support",
                "Other"
            };


            ViewBag.Guid = guid;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SaveMonitoring(MonitoringModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new
                    {
                        status = false,
                        message = "Invalid data"
                    });
                }

                // ==========================
                // Main Model
                // ==========================
                model.ActivityGuid = SanitizeAndValidate(model.ActivityGuid);
                model.IsPartnership = SanitizeAndValidate(model.IsPartnership);
                model.DirectSource = SanitizeAndValidate(model.DirectSource);
                model.IndirectSource = SanitizeAndValidate(model.IndirectSource);

                // ==========================
                // Supports
                // ==========================
                if (model.Supports != null)
                {
                    foreach (var item in model.Supports)
                    {
                        item.SupportType =
                            SanitizeAndValidate(item.SupportType);

                        item.Details =
                            SanitizeAndValidate(item.Details);
                    }
                }

                // ==========================
                // Documents
                // ==========================
                // ==========================
                // Documents Validation
                // ==========================
                if (model.Documents != null)
                {
                    foreach (var doc in model.Documents)
                    {
                        doc.DocumentType =
                            SanitizeAndValidate(doc.DocumentType);

                        doc.FileName =
                            SanitizeAndValidate(doc.FileName);

                        doc.ExistingFileName =
                            SanitizeAndValidate(doc.ExistingFileName);

                        if (doc.File != null)
                        {
                            string fileName =
                                Path.GetFileName(doc.File.FileName);

                            string extension =
                                Path.GetExtension(fileName);

                            // Only PDF allowed
                            if (!extension.Equals(".pdf",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                return Json(new
                                {
                                    status = false,
                                    message = "Only PDF files are allowed."
                                });
                            }

                            // Double extension check
                            string nameWithoutExt =
                                Path.GetFileNameWithoutExtension(fileName);

                            if (nameWithoutExt.Contains('.'))
                            {
                                return Json(new
                                {
                                    status = false,
                                    message = "Double extension files are not allowed."
                                });
                            }

                            // MIME Type Check
                            if (doc.File.ContentType != "application/pdf")
                            {
                                return Json(new
                                {
                                    status = false,
                                    message = "Invalid PDF file."
                                });
                            }

                            // Empty File Check
                            if (doc.File.Length <= 0)
                            {
                                return Json(new
                                {
                                    status = false,
                                    message = "Uploaded file is empty."
                                });
                            }

                            // Max Size 10 MB
                            if (doc.File.Length > 10 * 1024 * 1024)
                            {
                                return Json(new
                                {
                                    status = false,
                                    message = "PDF size cannot exceed 10 MB."
                                });
                            }
                        }
                    }
                }

                string userId = User.Identity?.Name ?? "SystemAdmin";

                var response =
                    await _iMonitoringService.SaveMonitoringAsync(model, userId);

                return Json(new
                {
                    status = response.status,
                    message = response.message,
                    activityGuid = model.ActivityGuid
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
        public async Task<IActionResult> MonitoringEntry(string? guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                // Fetch existing monitoring data
                var monitoringData = await _iMonitoringService.GetMonitoringAsync(guid);

                if (monitoringData != null)
                {

                    // Pass the data to the view
                    ViewBag.SelectedSupports = monitoringData?.Supports ?? new List<SupportModel>();
                    ViewBag.MonitoringData = monitoringData;
                }
            }
            ViewBag.AllDocuments = new List<string>
            {
                "Letter Exchange",
                "Signed Work Plan",
                "Memorandum of Understanding",
                "Letter of Understanding",
                "Other"
            };
            ViewBag.AllSupports = new List<string>
            {
                "Technical Advice / Expertise",
                "Technical Assistance (NGO)",
                "Human Resources",
                "Financial Resources",
                "Project-based Support",
                "Other"
            };


            ViewBag.Guid = guid;
            return View();
        }
        public async Task<IActionResult> MonitoringEntryTwo(string? guid, string? reportingId)
        {
            // --- Claims Expired Check ---
            if (!CommonHelper.IsUserValid(User, out int groupId, out int userId))
            {
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Login", "Account");
            }
            var json_activity = await _iManageActivity.GetActivityDataForReportAsync(guid, reportingId);

            //var json_activity = await _iManageActivity.GetActivityDataAsync(guid);

            ActivityDTO activity = JsonConvert.DeserializeObject<ActivityDTO>(json_activity);

            trackingMainModel _main = new trackingMainModel();
            _main.ActivityGuid = guid;
            _main._activity = new ActivityDTO();
            _main._activity = activity;

            return View(_main);
        }

        #region GeoLevel Add
        [HttpPost]
        public async Task<IActionResult> SaveGeoLevel([FromBody] GeoLevelModel model)
        {
            try
            {
                // SAVE IN DATABASE

                // model.taskId
                // model.geoLevel
                // model.district
                // model.cities
                // 🔹 2. Logged-in User
                string userId = User.Identity.Name;

                var result = await _iManageActivity.SaveGeoLevelAsync(model, userId);
                return Json(new
                {
                    status = result.status,
                    message = result.message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 0,
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGeoLevel(long geoId)
        {
            var result = await _iManageActivity.DeleteGeoLevelAsync(geoId);
            return Json(result);
        }

        //[HttpPost("GetActivityDetails")]
        [HttpPost]
        public async Task<IActionResult> GetActivityDetails(string activityGuid)
        {
            // --- Claims Expired Check ---
            if (!CommonHelper.IsUserValid(User, out int groupId, out int userId))
            {
                await HttpContext.SignOutAsync("Cookies");
                return RedirectToAction("Login", "Account");
            }

            var json = await _iManageActivity.GetActivityDataAsync(activityGuid);
            // 2. Bind it to the model
            ActivityDTO model = JsonConvert.DeserializeObject<ActivityDTO>(json);

            // 3. Return the model as a response
            return Ok(model);

            //if (result == null)
            //    return NotFound(new { message = "Data not found" });

            //return Ok(result);
        }

        #endregion

        #region Step2
        [HttpPost]
        public async Task<IActionResult> SaveReportingStep([FromBody] ReportingModel_step2 model)
        {
            try
            {
                string userId = User.Identity.Name;
                var result =
                    await _iMonitoringService.SaveReportingStepAsync(model, userId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new result
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetReportingByActivityGuid(string activityGuid)
        {
            var data = await _iMonitoringService
                .GetReportingByActivityGuidAsync(activityGuid);

            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTaskTracking([FromBody] main_TaskTrackingModel model, [FromQuery] int isFinalSubmit = 0)
        {
            result _result = new result();
            try
            {
                if (model == null || model.TaskTrackingModel == null || model.TaskTrackingModel.Count == 0)
                {
                    _result.status = false;
                    _result.message = "No tracking data found.";
                    return Json(_result);
                }
                string user =User.Identity?.Name ?? "Admin";
                string activityGuid =model.TaskTrackingModel.FirstOrDefault()?.ActivityGuid;
                string reportingId =model.TaskTrackingModel.FirstOrDefault()?.reportingId;

                // =====================================
                // SAVE CONTACT DETAILS
                // =====================================

                if (model.ContactDetails != null)
                {
                    await _iMonitoringService.SaveContactDetailsAsync(activityGuid,reportingId,model.ContactDetails,user);
                }

                foreach (var item in model.TaskTrackingModel)
                {
                    _result =
                        await _iMonitoringService
                        .SaveTaskTrackingAsync(item, isFinalSubmit, user);
                }

                string userId = User.Identity.Name;

                if(isFinalSubmit == 1)
                // Activity Status Update
                await _iMonitoringService
                    .UpdateActivityStatusAsync(model.TaskTrackingModel.FirstOrDefault()?.ActivityGuid,model.TaskTrackingModel.FirstOrDefault()?.reportingId, userId);

                return Json(new
                {
                    status = _result.status,
                    message = _result.message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
        #endregion

        //    [HttpGet("DownloadDocument")]
        //    public async Task<IActionResult> DownloadDocument(int id)
        //    {
        //        var parameters = new List<SqlParameter>
        //{
        //    new SqlParameter("@DocumentId", id)
        //};

        //        var ds = await _sql.ExecuteProcedure("SP_GetDocumentBinary", parameters.ToArray());

        //        if (ds.Tables[0].Rows.Count == 0)
        //            return NotFound();

        //        var row = ds.Tables[0].Rows[0];

        //        byte[] fileBytes = (byte[])row["FileData"];
        //        string contentType = row["ContentType"].ToString();
        //        string fileName = row["FileName"].ToString();

        //        return File(fileBytes, contentType, fileName);
        //    }
        // #endregion

        #region Best Prectices
        public async Task<IActionResult> BestPrectices(string guid)
        {
            var json_activity = await _iManageActivity.GetActivityDataAsync(guid);

            ActivityDTO activity = JsonConvert.DeserializeObject<ActivityDTO>(json_activity);

            trackingMainModel _main = new trackingMainModel();
            _main.ActivityGuid = guid;
            _main._activity = new ActivityDTO();
            _main._activity = activity;

            return View(_main);
        }

        // =============================================
        // SAVE BEST PRACTICE
        // =============================================

        [HttpPost]
        public async Task<IActionResult> SaveBestPrectices(BestPracticeModel model)
        {
            try
            {

                if (model == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid request."
                    });
                }
                if (model.TaskId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select task."
                    });
                }
                if (model.GeoId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select geo location."
                    });
                }
                if (string.IsNullOrWhiteSpace(model.Heading))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please enter heading."
                    });
                }
                // =====================================
                // USER
                // =====================================
                string user = User.Identity.Name ?? "Admin";
                // =====================================
                // SAVE
                // =====================================
                var response =
                    await _iMonitoringService.SaveBestPracticeAsync(model, user);
                // =====================================
                // RESPONSE
                // =====================================
                return Json(new
                {
                    success = response.status,
                    message = response.message,
                    bestPracticeId = response.id
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadGalleryImages(long bestPracticeId, List<IFormFile> GalleryImages)
        {
            result response = await _iMonitoringService
                .UploadGalleryImages(
                    bestPracticeId,
                    GalleryImages,
                    User.Identity.Name
                );

            return Json(new
            {
                success = response.status,
                message = response.message
            });
        }
        #endregion

        #region AddActivityBestPrectices
        public async Task<IActionResult> AddActivityBestPrectices(string guid)
        {
            var json_activity = await _iManageActivity.GetActivityDataAsync(guid);

            ActivityDTO activity = JsonConvert.DeserializeObject<ActivityDTO>(json_activity);

            trackingMainModel _main = new trackingMainModel();
            _main.ActivityGuid = guid;
            _main._activity = new ActivityDTO();
            _main._activity = activity;

            return View(_main);
        }

        [HttpPost]
        public async Task<IActionResult> SaveActivityMediaShowcase([FromForm] string activityGuid,
            [FromForm] string submissionMode,
            [FromForm] string description,
            [FromForm] string heading,
            [FromForm] int orderNumber,
            [FromForm] IFormFile coverImage,
            [FromForm] IFormFile descriptionPdf,
            [FromForm] long? parentCoverId,
            [FromForm] string mediaType,
            [FromForm] IFormFile mediaFile,
            [FromForm] long subActivityId,  // 🌟 NEW PARAMETER
            [FromForm] long taskId,         // 🌟 NEW PARAMETER
            [FromForm] long geoLocationId)   // 🌟 NEW PARAMETER
        {
            try
            {

                // 1. सेफ्टी और एथेंटिकेशन कॉन्टेक्स्ट (Fallback Username)
                string currentUser = User.Identity?.Name ?? "SystemAdmin";
                string AgencyId = User.FindFirst("AgencyId")?.Value ?? "0";

                activityGuid = SanitizeAndValidate(activityGuid);
                submissionMode = SanitizeAndValidate(submissionMode);
                description = SanitizeAndValidate(description);
                heading = SanitizeAndValidate(heading);
                mediaType = SanitizeAndValidate(mediaType);


                // 2. फिजिकल सर्वर पर फाइल अपलोड फोल्डर पाथ जनरेट करें
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "activity_media");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 🔷 पाथ ए: यदि यूजर 'COVER PAGE' कॉन्फ़िगरेशन सबमिट कर रहा है
                if (submissionMode == "CoverPage")
                {
                    string coverImageUrl = string.Empty;
                    string pdfUrl = null;

                    if (coverImage != null)
                    {
                      string[] allowedImages =
                      {
                         ".jpg",
                         ".jpeg",
                         ".png",
                         ".webp"
                     };

                        if (!IsAllowedFile(coverImage, allowedImages))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Only JPG, JPEG, PNG and WEBP images are allowed."
                            });
                        }
                    }
                    if (coverImage != null &&
                         !coverImage.ContentType.StartsWith("image/"))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Invalid image file."
                        });
                    }

                    // 1. बैनर इमेज अपलोड प्रोसेसिंग
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        string uniqueFileName = "COVER_" + Guid.NewGuid().ToString() + Path.GetExtension(coverImage.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverImage.CopyToAsync(fileStream);
                        }
                        coverImageUrl = "/uploads/activity_media/" + uniqueFileName;
                    }

                    if (descriptionPdf != null)
                    {
                        string[] allowedPdf = { ".pdf" };

                        if (!IsAllowedFile(descriptionPdf, allowedPdf))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Only PDF files are allowed."
                            });
                        }
                    }
                    if (descriptionPdf != null &&
                    descriptionPdf.ContentType != "application/pdf")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Invalid PDF content type."
                        });
                    }
                    // 2. डिस्क्रिप्शन पीडीएफ अपलोड प्रोसेसिंग (ऑप्शनल)
                    if (descriptionPdf != null && descriptionPdf.Length > 0)
                    {
                        string uniquePdfName = "DOC_" + Guid.NewGuid().ToString() + Path.GetExtension(descriptionPdf.FileName);
                        string pdfPath = Path.Combine(uploadsFolder, uniquePdfName);

                        using (var fileStream = new FileStream(pdfPath, FileMode.Create))
                        {
                            await descriptionPdf.CopyToAsync(fileStream);
                        }
                        pdfUrl = "/uploads/activity_media/" + uniquePdfName;
                    }

                    // 3. 🌟 इंटरफ़ेस सर्विस को नए पैरामीटर्स के साथ कॉल करें
                    long coverId = await _iActivityMediaService.SaveCoverPageAsync(
                        activityGuid, heading, orderNumber, description, coverImageUrl, pdfUrl, currentUser,
                        subActivityId, taskId, geoLocationId, AgencyId
                    );

                    if (coverId > 0)
                        return Json(new { success = true, message = "Master Cover Page configurations saved successfully." });
                }

                // 🔷 पाथ बी: यदि यूजर 'INNER IMAGE / MULTIMEDIA ASSET' सबमिट कर रहा है
                else if (submissionMode == "InnerImage")
                {
                    if (parentCoverId == null || parentCoverId <= 0)
                    {
                        return Json(new { success = false, message = "Invalid parent cover mapping context identifier." });
                    }

                    string mediaAssetUrl = string.Empty;

                    if (mediaFile != null)
                    {
                        string[] allowedMedia =
                        {
                               ".jpg",
                               ".jpeg",
                               ".png",
                               ".webp",
                               ".mp4",
                               ".mp3",
                               ".wav"
                           };

                        if (!IsAllowedFile(mediaFile, allowedMedia))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Invalid media file format."
                            });
                        }
                    }

                    // 1. डायनेमिक मीडिया फाइल (इमेज, वीडियो या ऑडियो) प्रोसेसिंग
                    if (mediaFile != null && mediaFile.Length > 0)
                    {
                        // फाइल के एक्सटेंशन को वैलिडेट करने और नाम को क्लीन रखने के लिए प्रिफिक्स लगाएं
                        string prefix = mediaType.ToUpper() + "_";
                        string uniqueMediaName = prefix + Guid.NewGuid().ToString() + Path.GetExtension(mediaFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueMediaName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await mediaFile.CopyToAsync(fileStream);
                        }
                        mediaAssetUrl = "/uploads/activity_media/" + uniqueMediaName;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Please browse and upload a valid media source file asset." });
                    }

                    // 2. इंटरफ़ेस सर्विस को डेटाबेस इंसर्ट के लिए कॉल करें
                    long innerMediaId = await _iActivityMediaService.SaveInnerMediaAsync(
                        parentCoverId.Value, mediaType, mediaAssetUrl, description, currentUser
                    );

                    if (innerMediaId > 0)
                        return Json(new { success = true, message = $"Inner {mediaType} asset mapped and recorded seamlessly." });
                }

                return Json(new { success = false, message = "Invalid submission layout action mode type encountered." });
            }
            catch (Exception ex)
            {
                // एरर होने पर लॉग जनरेट करें और क्लाइंट को सुरक्षित मैसेज भेजें
                System.Diagnostics.Debug.WriteLine($"Media Save Operation Stacktrace Error: {ex.Message}");
                return Json(new { success = false, message = "A system database operation layout execution error occurred." });
            }
        }


        [HttpPost]
        public async Task<JsonResult> GetCoverPagesList(string guid)
        {
            try
            {
                if (string.IsNullOrEmpty(guid))
                {
                    return Json(new List<object>());
                }

                // 1. सर्विस से DataTable प्राप्त करें
                var dt = await _iActivityMediaService.GetCoverPagesListAsync(guid);

                // 2. DataTable को List of Dictionaries में कन्वर्ट करें
                var rows = new List<Dictionary<string, object>>();
                foreach (DataRow row in dt.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        // DBNull को C# null में बदलें ताकि JSON में null जाए
                        dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                    }
                    rows.Add(dict);
                }

                // 3. अब क्लीन कनवर्टेड लिस्ट भेजें
                return Json(rows);
            }
            catch (Exception ex)
            {
                // एरर होने पर कंसोल में ट्रैक करने के लिए
                System.Diagnostics.Debug.WriteLine($"Error in GetCoverPagesList Controller: {ex.Message}");
                return Json(new { success = false, message = "Server serialization failure." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetInnerMediaList(long coverPageId)
        {
            try
            {

                // 1. सर्विस से DataTable प्राप्त करें
                var dt = await _iActivityMediaService.GetInnerMediaListAsync(coverPageId);

                // 2. DataTable को List of Dictionaries में कन्वर्ट करें
                var rows = new List<Dictionary<string, object>>();
                foreach (DataRow row in dt.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        // DBNull को C# null में बदलें ताकि JSON में null जाए
                        dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                    }
                    rows.Add(dict);
                }

                // 3. अब क्लीन कनवर्टेड लिस्ट भेजें
                return Json(rows);
            }
            catch (Exception ex)
            {
                // एरर होने पर कंसोल में ट्रैक करने के लिए
                System.Diagnostics.Debug.WriteLine($"Error in GetCoverPagesList Controller: {ex.Message}");
                return Json(new { success = false, message = "Server serialization failure." });
            }
        }

        [Route("Management/GetActiveCoversByActivity")]
        public async Task<IActionResult> GetActiveCoversByActivity(string guid)
        {
            return Json(await _iActivityMediaService.GetActiveCoversByActivityAsync(guid));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActivityMediaAsset(string mode, long id)
        {
            try
            {
                if (id <= 0 || string.IsNullOrEmpty(mode))
                {
                    return Json(new { success = false, message = "Invalid primary signature reference id." });
                }
                DataSet dtFile = await _iActivityMediaService.GetAssetFilePathsAsync(mode, id);

                bool isDeleted = await _iActivityMediaService.DeleteMediaAssetAsync(mode, id);

                if (isDeleted && dtFile != null && dtFile.Tables[0].Rows.Count > 0)
                {
                    // 3. फिजिकल फाइल को सर्वर स्टोरेज (wwwroot) से वाइप करें
                    foreach (DataRow row in dtFile.Tables[0].Rows)
                    {
                        string dbPath = row[0]?.ToString();
                        if (!string.IsNullOrEmpty(dbPath))
                        {
                            // URL पाथ को फिजिकल एब्सोल्यूट पाथ में बदलें
                            string physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, dbPath.TrimStart('/'));
                            if (System.IO.File.Exists(physicalPath))
                            {
                                System.IO.File.Delete(physicalPath);
                            }
                        }
                    }
                    return Json(new { success = true, message = "Asset and corresponding server files deleted successfully." });
                }

                return Json(new { success = false, message = "Failed to complete data truncation pipeline." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deletion Error: {ex.Message}");
                return Json(new { success = false, message = "System exception encountered during removal operation." });
            }
        }
        #endregion

        #region Add Direct Best Prectices
        public async Task<IActionResult> AddDirectBestPrectices()
        {
            //var json_activity = await _iManageActivity.GetActivityDataAsync(guid);

            //ActivityDTO activity = JsonConvert.DeserializeObject<ActivityDTO>(json_activity);

            trackingMainModel _main = new trackingMainModel();
            //_main.ActivityGuid = guid;
            //_main._activity = new ActivityDTO();
            //_main._activity = activity;

            return View(_main);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDirectMediaShowcase(
            [FromForm] string submissionMode,
            [FromForm] string description,
            [FromForm] string heading,
            [FromForm] int orderNumber,
            [FromForm] IFormFile coverImage,
            [FromForm] IFormFile descriptionPdf,
            [FromForm] long? parentCoverId,
            [FromForm] string mediaType,
            [FromForm] IFormFile mediaFile,
            [FromForm] long sectorId)
        {
            try
            {
                string currentUser = User.Identity?.Name ?? "SystemAdmin";
                string agencyId = User.FindFirst("AgencyId")?.Value ?? "0";
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "activity_media");


                
                submissionMode = SanitizeAndValidate(submissionMode);
                description = SanitizeAndValidate(description);
                heading = SanitizeAndValidate(heading);
                mediaType = SanitizeAndValidate(mediaType);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // 🔷 PATH A: COVER PAGE SUBMISSION
                if (submissionMode == "CoverPage")
                {
                    string coverImageUrl = string.Empty;
                    string pdfUrl = null;
                    if (coverImage != null)
                    {
                        string[] allowedImages =
                        {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

                        if (!IsAllowedFile(coverImage, allowedImages))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Only JPG, JPEG, PNG and WEBP images are allowed."
                            });
                        }
                    }
                    if (coverImage != null &&
    !coverImage.ContentType.StartsWith("image/"))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Invalid image file."
                        });
                    }
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        string uniqueFileName = "COVER_" + Guid.NewGuid().ToString() + Path.GetExtension(coverImage.FileName);
                        using (var stream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                        {
                            await coverImage.CopyToAsync(stream);
                        }
                        coverImageUrl = "/uploads/activity_media/" + uniqueFileName;
                    }
                    if (descriptionPdf != null)
                    {
                        string[] allowedPdf = { ".pdf" };

                        if (!IsAllowedFile(descriptionPdf, allowedPdf))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Only PDF files are allowed."
                            });
                        }
                    }
                    if (descriptionPdf != null &&
    descriptionPdf.ContentType != "application/pdf")
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Invalid PDF content type."
                        });
                    }
                    if (descriptionPdf != null && descriptionPdf.Length > 0)
                    {
                        string uniquePdfName = "DOC_" + Guid.NewGuid().ToString() + Path.GetExtension(descriptionPdf.FileName);
                        using (var stream = new FileStream(Path.Combine(uploadsFolder, uniquePdfName), FileMode.Create))
                        {
                            await descriptionPdf.CopyToAsync(stream);
                        }
                        pdfUrl = "/uploads/activity_media/" + uniquePdfName;
                    }

                    long coverId = await _iActivityMediaService.SaveDirectCoverPageAsync(
                        sectorId, heading, orderNumber, description, coverImageUrl, pdfUrl, currentUser, agencyId
                    );

                    if (coverId > 0)
                        return Json(new { success = true, message = "Master Cover Page configurations saved successfully." });
                }

                // 🔷 PATH B: INNER IMAGE / MULTIMEDIA ASSET SUBMISSION
                else if (submissionMode == "InnerImage")
                {
                    if (parentCoverId == null || parentCoverId <= 0)
                        return Json(new { success = false, message = "Invalid parent cover mapping context identifier." });

                    string mediaAssetUrl = string.Empty;
                    if (mediaFile != null)
                    {
                        string[] allowedMedia =
                        {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        ".mp4",
        ".mp3",
        ".wav"
    };

                        if (!IsAllowedFile(mediaFile, allowedMedia))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Invalid media file format."
                            });
                        }
                    }
                    if (mediaFile != null && mediaFile.Length > 0)
                    {
                        string prefix = mediaType.ToUpper() + "_";
                        string uniqueMediaName = prefix + Guid.NewGuid().ToString() + Path.GetExtension(mediaFile.FileName);
                        using (var stream = new FileStream(Path.Combine(uploadsFolder, uniqueMediaName), FileMode.Create))
                        {
                            await mediaFile.CopyToAsync(stream);
                        }
                        mediaAssetUrl = "/uploads/activity_media/" + uniqueMediaName;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Please browse and upload a valid media source file asset." });
                    }

                    long innerMediaId = await _iActivityMediaService.SaveDirectInnerMediaAsync(
                        parentCoverId.Value, mediaType, mediaAssetUrl, description, currentUser
                    );

                    if (innerMediaId > 0)
                        return Json(new { success = true, message = $"Inner {mediaType} asset mapped and recorded seamlessly." });
                }

                return Json(new { success = false, message = "Invalid action mode type." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save Error: {ex.Message}");
                return Json(new { success = false, message = "A system database operation layout execution error occurred." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDirectCoverPagesList(int sectorId)
        {
            DataSet ds = await _iActivityMediaService.GetDirectCoverPagesListAsync(sectorId);
            return Json(ConvertDataTableToDictionary(ds.Tables[0]));
        }
        [HttpPost]
        public async Task<IActionResult> GetDirectInnerMediaList(long coverPageId, int? sectorId)
        {
            if (coverPageId <= 0) return Json(new List<object>());
            DataSet ds = await _iActivityMediaService.GetDirectInnerMediaListAsync(coverPageId, sectorId);
            return Json(ConvertDataTableToDictionary(ds.Tables[0]));
        }
        [HttpGet]
        public async Task<JsonResult> GetActiveCoversBySector(int sectorId)
        {
            return Json(await _iActivityMediaService.GetDirectCoversDropdownBySectorAsync(sectorId));
            //DataSet ds = await _iActivityMediaService.GetDirectCoversDropdownBySectorAsync(sectorId);
            //return Json(ConvertDataTableToDictionary(ds.Tables[0]));
        }
        private List<Dictionary<string, object>> ConvertDataTableToDictionary(DataTable dt)
        {
            var rows = new List<Dictionary<string, object>>();
            if (dt == null) return rows;

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                rows.Add(dict);
            }
            return rows;
        }
        #endregion

        private bool ContainsInvalidCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Allow: A-Z, a-z, 0-9, space, comma, dot, dash, underscore, brackets
            return Regex.IsMatch(input, @"[^a-zA-Z0-9\s,\.\-_()/:&;]");
        }

        private string SanitizeAndValidate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var sanitizer = new HtmlSanitizer();

            value = sanitizer.Sanitize(value).Trim();

            if (ContainsInvalidCharacters(value))
                throw new Exception("Invalid special characters detected.");

            return value;
        }

        private bool HasDoubleExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string name = Path.GetFileName(fileName);

            return name.Count(c => c == '.') > 1;
        }

        private bool IsAllowedFile(IFormFile file, string[] allowedExtensions)
        {
            if (file == null || file.Length == 0)
                return false;

            string ext = Path.GetExtension(file.FileName)?.ToLower();

            if (string.IsNullOrWhiteSpace(ext))
                return false;

            if (HasDoubleExtension(file.FileName))
                return false;

            return allowedExtensions.Contains(ext);
        }

    }
}
