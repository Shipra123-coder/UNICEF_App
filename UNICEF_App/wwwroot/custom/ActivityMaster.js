let currentStep = 1;
const totalSteps = 6;
// GLOBAL STATE
let selectedDepts = [];
let selectedSectors = [];
let selectedGoals = [];
let selectedPillars = [];
let mappingData = {};
let pillarData = {}; // 🔥 Source of truth
let selectedNatureOfSupport = [];
let themeData = {};
let supportData = [];

const nextBtn = document.getElementById('nextBtn');
const prevBtn = document.getElementById('prevBtn');

let deletedSubIds = [];
let deletedTaskIds = [];

// =======================
// STEP UI CONTROL
// =======================

var activityMaster = {
    savedata: async function () {
        if (currentStep === 1) {
            await activityMaster.saveActivityData();
        }
        else if (currentStep === 2) {
            await activityMaster.saveGeoLevel();
        }
        else if (currentStep === 3) {
            await activityMaster.saveDept();
        }
        else if (currentStep === 4) {
            await activityMaster.saveGoal();
        }
        else if (currentStep === 5) {
            await activityMaster.savePillar();
        }
        else if (currentStep === 6) {
            await activityMaster.saveNatureOfSupport();
        }

        // 🔥 SAVE SUCCESS hone ke baad hi next step
        //activityMaster.nextStep();
    },
    saveActivityData: function () {

        var status = true;

        // ======================================
        // DATE PARSING HELPER
        // ======================================
        // Converts "dd-mm-yyyy" string to a native JS Date object for accurate comparison
        function parseDateString(dateStr) {
            if (!dateStr) return null;
            var parts = dateStr.split('-');
            if (parts.length !== 3) return null;
            return new Date(parts[2], parts[1] - 1, parts[0]); // yyyy, mm-1, dd
        }

        // ======================================
        // MAIN FIELDS VALIDATION
        // ======================================
        var ActivityName = $("input[name='activityName']").val();
        var ShortName = $("input[name='shortName']").val();
        var Description = $("textarea[name='Description']").val();
        var StartDateStr = $("#StartDate").val();
        var EndDateStr = $("#EndDate").val();
        var UNSector = $("#UNSector").val();

        if (!ActivityName || ActivityName.trim() === "") {
            $('#activityName').addClass('errr-highlight').removeClass('sucess-highlight');
            status = false;
        } else {
            $('#activityName').removeClass('errr-highlight').addClass('sucess-highlight');
        }

        if (!ShortName || ShortName.trim() === "") {
            $('#shortName').addClass('errr-highlight').removeClass('sucess-highlight');
            status = false;
        } else {
            $('#shortName').removeClass('errr-highlight').addClass('sucess-highlight');
        }

        if (!Description || Description.trim() === "") {
            $('#description').addClass('errr-highlight').removeClass('sucess-highlight');
            status = false;
        } else {
            $('#description').removeClass('errr-highlight').addClass('sucess-highlight');
        }

        if (!StartDateStr || !EndDateStr) {
            toast.showToast('error', 'Please select activity period', 'error');
            return false;
        }

        // Convert parent activity dates to Date objects
        var activityStartDate = parseDateString(StartDateStr);
        var activityEndDate = parseDateString(EndDateStr);

        if (activityStartDate && activityEndDate && activityEndDate < activityStartDate) {
            toast.showToast('error', 'Activity End Date must be greater than Start Date', 'error');
            return false;
        }

        if (!status) {
            toast.showToast('error', 'Please fill all required fields', 'error');
            return false;
        }

        // Check Yes/No for Sub-Activities
        var hasSubActivity = $("input[name='hasSubActivity']:checked").val();
        var SubActivities = [];

        // ======================================
        // SHARED TASK DATE VALIDATION FUNCTION
        // ======================================
        function validateTaskDates(taskStartDateStr, taskEndDateStr) {
            if (!taskStartDateStr || !taskEndDateStr) {
                toast.showToast('error', 'Please select task start and end date', 'error');
                return false;
            }

            var taskStartDate = parseDateString(taskStartDateStr);
            var taskEndDate = parseDateString(taskEndDateStr);

            if (taskStartDate && taskEndDate && taskEndDate < taskStartDate) {
                toast.showToast('error', 'Task end date must be greater than start date', 'error');
                return false;
            }

            if (taskStartDate < activityStartDate || taskEndDate > activityEndDate) {
                toast.showToast('error', 'Task dates must be inside Activity Period', 'error');
                return false;
            }

            return true;
        }
        debugger;
        // ======================================
        // CASE 1: WITH SUB-ACTIVITY
        // ======================================
        if (hasSubActivity == "1") {

            $(".sub-activity-block").each(function () {
                var $block = $(this);
                var subActivityId = parseInt($block.find(".sub-id").val()) || 0;
                var subName = $block.find("input[name='SubActivityName[]']").val();

                if (!subName || subName.trim() === "") {
                    status = false;
                    $block.find("input[name='SubActivityName[]']").addClass('errr-highlight');
                    return;
                }

                var subActivity = {
                    SubActivityId: subActivityId,
                    SubActivityName: subName,
                    guid: $('#guid').val(),
                    Tasks: []
                };

                var loopStatus = true;
                $block.find(".task-list .task-item").each(function () {
                    var $taskRow = $(this);
                    var taskDesc = $taskRow.find("input[name='Tasks[]']").val();
                    var taskStartDateStr = $taskRow.find("input[name='TaskStartDate[]']").val();
                    var taskEndDateStr = $taskRow.find("input[name='TaskEndDate[]']").val();

                    if (!taskDesc || taskDesc.trim() === "") {
                        return; // Skip empty/unfilled tasks
                    }

                    // Run date validation rules
                    if (!validateTaskDates(taskStartDateStr, taskEndDateStr)) {
                        status = false;
                        loopStatus = false;
                        return false; // Break loop
                    }

                    subActivity.Tasks.push({
                        TaskId: parseInt($taskRow.find(".task-id").val()) || 0,
                        TaskDescription: taskDesc,
                        StartDate: taskStartDateStr,
                        EndDate: taskEndDateStr,
                        SubActivityId: subActivityId
                    });
                });

                if (!loopStatus) return false; // Break out of block loop if date validation failed

                // At least 1 valid task required per Sub-Activity
                if (subActivity.Tasks.length === 0) {
                    status = false;
                    toast.showToast('error', 'Each sub-activity must have at least one task', 'error');
                    return false;
                }

                SubActivities.push(subActivity);
            });
        }

        // ======================================
        // CASE 2: ONLY TASK (NO SUB-ACTIVITY)
        // ======================================
        else {
            var mainTasks = [];
            var loopStatus = true;

            $("#mainTaskList .task-item").each(function () {
                var $taskRow = $(this);
                var taskDesc = $taskRow.find("input[name='Tasks[]']").val();
                var taskDescDetail = $taskRow.find("textarea[name='TaskDetailDescription[]']").val();
                //var taskDescDetail = $taskRow.find("input[name='TaskDetailDescription[]']").val();
                var taskStartDateStr = $taskRow.find("input[name='TaskStartDate[]']").val();
                var taskEndDateStr = $taskRow.find("input[name='TaskEndDate[]']").val();

                if (!taskDesc || taskDesc.trim() === "") {
                    return; // Skip empty tasks
                }
                if (!taskDescDetail || taskDescDetail.trim() === "") {
                    return; // Skip empty tasks
                }

                // Run date validation rules
                if (!validateTaskDates(taskStartDateStr, taskEndDateStr)) {
                    status = false;
                    loopStatus = false;
                    return false; // Break loop
                }

                mainTasks.push({
                    TaskId: parseInt($taskRow.find("input[name='TaskId[]']").val()) || 0,
                    TaskDescription: taskDesc,
                    TaskDetailDescription: taskDescDetail,
                    StartDate: taskStartDateStr,
                    EndDate: taskEndDateStr,
                    SubActivityId: 0
                });
            });

            if (!loopStatus) return false;

            if (mainTasks.length === 0) {
                toast.showToast('error', 'Please add at least one task', 'error');
                return false;
            }

            // Wrap into ONE dummy sub-activity structure for backend data consistency
            SubActivities.push({
                SubActivityId: 0,
                SubActivityName: null,
                guid: $('#guid').val(),
                Tasks: mainTasks
            });
        }

        if (!status) {
            return false;
        }

        // ======================================
        // FINAL MODEL OBJECT PREPARATION
        // ======================================
        var model = {
            ActivityGuid: $('#ActivityGuid').val(),
            ActivityName: ActivityName,
            ShortName: ShortName,
            Description: Description,
            UNSector: UNSector,
            StartDate: StartDateStr,
            EndDate: EndDateStr,
            HasSubActivity: hasSubActivity == "1" ? true : false,

            DeletedSubIds: $("#DeletedSubIds").val(),
            DeletedTaskIds: $("#DeletedTaskIds").val(),

            SubActivities: SubActivities
        };

        console.log(model);        

        // ======================================
        // CONFIRMATION AND SUBMISSION
        // ======================================
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save this activity!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {

            if (result.isConfirmed) {

                common.ShowLoader();
                return new Promise((resolve, reject) => {
                    ajax.doPostAjax(`/Management/SaveActivity`, model, function (r) {

                        common.HideLoader();

                        if (r.status) {

                            toast.showToast('success', r.message, 'success');

                            if ($("#ActivityGuid").length > 0) {
                                $("#ActivityGuid").val(r.activityGuid);
                            }

                            // Handle Form Multi-Step UI Navigation
                            $(".step-content").removeClass("active").hide();
                            $("#content2").addClass("active").show();

                            $("#s1").addClass("completed").removeClass("active");
                            $("#s2").addClass("active");

                            currentStep = 2;
                            $("#prevBtn").prop("disabled", false);

                            window.scrollTo(0, 0);
                            if (typeof activityMaster !== 'undefined' && activityMaster.updateStepUI) {
                                activityMaster.updateStepUI();
                            }
                            if (typeof geolevel !== 'undefined' && geolevel.getdata) {
                                geolevel.getdata();
                            }

                        } else {
                            toast.showToast('error', r.message, 'error');
                        }
                    });
                });
            }
        });
    },
    saveGeoLevel: function () {

        // HTML में रेंडर हुए __RequestVerificationToken इनपुट बॉक्स से वैल्यू निकालें
        var token = $('input[name="__RequestVerificationToken"]').val();
        var status = true;
        // 🔹 Confirmation
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save this Geo Level!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {

            if (result.isConfirmed) {

                common.ShowLoader();
                return new Promise((resolve, reject) => {
                    common.HideLoader();
                    toast.showToast('success', 'GeoLevel' || 'Saved successfully', 'success');


                    // Wizard Movement: Step 2 to Step 3
                    setTimeout(function () {
                        $("#content2").removeClass("active").hide();
                        $("#content3").addClass("active").show();

                        $("#s2").addClass("completed").removeClass("active");
                        $("#s3").addClass("active");
                        currentStep = 3;
                        activityMaster.updateStepUI();
                        window.scrollTo(0, 0);
                        //geolevel.getdata();
                        // Previous button enable karein
                        //$("#prevBtn").prop("disabled", false);
                    }, 1000);
                });
            }
        });
    },
    saveDept: function () {
        var status = true;

        // 1. Nodal Department Validation
        var nodalDeptId = $('#Department').val();
        if (!nodalDeptId || nodalDeptId === "-1" || nodalDeptId === "") {
            $('#Department').addClass('errr-highlight');
            status = false;
        } else {
            $('#Department').removeClass('errr-highlight');
        }

        // 2. Collect Supporting Departments from Chips
        // Hum "selectedDepts" array ka use karenge jo aapke script mein pehle se define hai
        // Agar selectedDepts available nahi hai toh hum chips se bhi utha sakte hain:
        var supportingDepts = [];
        $('#deptChipsContainer .btn-remove-dept').each(function () {
            supportingDepts.push($(this).attr('data-id'));
        });

        // 3. Activity ID Check (Jo pichle step se aayi hogi)
        var ActivityGuid = $('#ActivityGuid').val();
        if (!ActivityGuid || ActivityGuid === "0") {
            toast.showToast('error', 'Please save Activity details first', 'error');
            return false;
        }

        if (!status) {
            toast.showToast('error', 'Please select Nodal Department', 'error');
            return false;
        }

        // 4. Model Construction
        var model = {
            ActivityGuid: $('#ActivityGuid').val(),
            NodalDepartmentId: nodalDeptId,
            SupportingDepartmentIds: supportingDepts // List of string/int
        };

        console.log("Saving Dept Mapping:", model);

        

        // 5. Confirmation & AJAX Call
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save department mapping!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();

                // Aapka custom AJAX method
                ajax.doPostAjax(`/Management/SaveDepartmentMapping`, model, function (r) {
                    common.HideLoader();

                    if (r.status || r.success) {
                        toast.showToast('success', r.message || 'Saved successfully', 'success');


                        // Wizard Movement: Step 2 to Step 3
                        setTimeout(function () {
                            $("#content3").removeClass("active").hide();
                            $("#content4").addClass("active").show();

                            $("#s3").addClass("completed").removeClass("active");
                            $("#s4").addClass("active");
                            currentStep = 4;
                            activityMaster.updateStepUI();
                            window.scrollTo(0, 0);
                            // Previous button enable karein
                            //$("#prevBtn").prop("disabled", false);
                        }, 1000);
                    } else {
                        toast.showToast('error', r.message || 'Error occurred', 'error');
                    }
                });
            }
        });
    },
    getDeptMap: function () {
        var model = {
            ActivityGuid: $('#ActivityGuid').val(),
        }
        ajax.doPostAjax('/Management/GetDeptMap', model, function (response) {

            selectedDepts = [];
            $('#deptChipsContainer').empty();

            response.forEach(function (item) {

                if (item.isNodal == 0) {   // 👈 filter condition

                    selectedDepts.push(item.departmentId);

                    const chip = `
                     <div class="dept-chip">
                         <span>${item.departmentName}</span>
                         <button type="button" class="btn-remove-dept" data-id="${item.departmentId}">
                             <i class="bx bx-x"></i>
                         </button>
                     </div>
                    `;

                    $('#deptChipsContainer').append(chip);
                }
                else {
                    common.BindDropdown("/Master/DDL_Department", "Department", "Department", `${item.departmentId}`);
                    common.BindDropdown(`/Master/DDL_SupDepartment?Id=${item.departmentId}`, "SubDepartment", "SubDepartment", $('#hiddensupDeptId').val());
                }
            });

            updateHiddenInput();

            function updateHiddenInput() {
                $('#hiddenSupportingDepts').val(selectedDepts.join(','));
            }
        });
    },
    saveGoal: function () {

        var status = true;

        // 🔹 1. ActivityId Validation
        var ActivityGuid = $('#ActivityGuid').val();
        if (!ActivityGuid || ActivityGuid === "0") {
            toast.showToast('error', 'Please save Activity details first', 'error');
            return false;
        }

        let mappingJson = $('#mappingJsonData').val();

        if (!mappingJson || mappingJson === "{}") {
            toast.showToast('error', 'Please add at least one Goal-Target mapping', 'error');
            return false;
        }

        let rawData = JSON.parse(mappingJson);

        // 🔥 Convert into proper model format
        let mappingData = [];

        Object.keys(rawData).forEach(gId => {

            let goal = rawData[gId];

            mappingData.push({
                GoalId: parseInt(gId),   // ✅ Important
                //GoalName: goal.name,
                Targets: goal.targets.map(t => ({
                    TargetId: parseInt(t.id),
                    //TargetName: t.name
                }))
            });

        });

        console.log("Formatted Mapping:", mappingData);

        // 🔹 3. Model Construction (ActivityId add kiya)
        var model = {
            ActivityGuid: ActivityGuid,
            MappingData: mappingData
        };

        console.log("Saving Goal Mapping:", model);

        
        // 🔹 4. Confirmation + AJAX
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save goal mapping!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {

            if (result.isConfirmed) {

                common.ShowLoader();

                ajax.doPostAjax(`/Management/SaveGoalMapping`, model, function (r) {

                    common.HideLoader();

                    if (r.status || r.success) {

                        toast.showToast('success', r.message || 'Saved successfully', 'success');

                        // 🔹 Wizard Step Move (Step 3 → Step 4)
                        setTimeout(function () {

                            $("#content4").removeClass("active").hide();
                            $("#content5").addClass("active").show();

                            $("#s4").addClass("completed").removeClass("active");
                            $("#s5").addClass("active");

                            currentStep = 5;
                            activityMaster.updateStepUI();
                            window.scrollTo(0, 0);

                        }, 1000);

                    } else {
                        toast.showToast('error', r.message || 'Error occurred', 'error');
                    }
                });
            }
        });
    },
    getGoalMap: function () {

        var model = {
            ActivityGuid: $('#ActivityGuid').val()
        };

        ajax.doPostAjax('/Management/GetGoalMap', model, function (response) {

            // 🔹 Reset

            $('#mappingContainer').empty();

            if (!response || response.length === 0) {
                $('#mappingContainer').html('<div class="empty-state">No mapping found</div>');
                $('#mappingJsonData').val('');
                return;
            }

            // 🔹 Grouping (Goal → Targets)
            response.forEach(function (item) {

                let gId = item.goalId;

                if (!mappingData[gId]) {
                    mappingData[gId] = {
                        name: item.goalName,
                        targets: []
                    };
                }

                mappingData[gId].targets.push({
                    id: item.targetId,
                    name: item.targetName
                });
            });

            // 🔹 Render UI (reuse your existing render logic)
            renderGoalUI(mappingData);

            // 🔹 Save JSON in hidden field
            $('#mappingJsonData').val(JSON.stringify(mappingData));

        });


        // 🔥 UI Render Function (same as your add logic)
        function renderGoalUI(mappingData) {

            const goalIds = Object.keys(mappingData);
            let totalTargets = 0;

            goalIds.forEach(gId => {

                const goal = mappingData[gId];
                totalTargets += goal.targets.length;

                let targetHtml = '';

                goal.targets.forEach((t, idx) => {
                    targetHtml += `
                    <div class="target-entry">
                        <span class="target-name">${t.name}</span>
                        <button type="button" class="btn-remove-target"
                            data-goal="${gId}" data-target-idx="${idx}">
                            <i class="bx bx-x"></i>
                        </button>
                    </div>`;
                });

                const card = `
                <div class="goal-group-card">
                    <div class="goal-group-header">
                        <span><i class="bx bx-bullseye"></i> ${goal.name}</span>
                        <button class="btn btn-danger btn-sm btn-remove-goal" data-goal="${gId}">
                            Remove
                        </button>
                    </div>
                    <div class="target-list-container">
                        ${targetHtml}
                    </div>
                </div>
            `;

                $('#mappingContainer').append(card);
            });

            $('#totalCountBadge').text(`${totalTargets} Targets Total`);
        }
    },
    savePillar: function () {
        // =========================================
        // VALIDATION
        // =========================================
        var ActivityGuid =
            $('#ActivityGuid').val();

        if (!ActivityGuid || ActivityGuid === "0") {
            toast.showToast(
                'error',
                'Please save Activity details first',
                'error'
            );
            return false;
        }

        // =========================================
        // JSON DATA
        // =========================================

        let mappingJson =
            $('#pillarJsonData').val();
        if (!mappingJson || mappingJson === "{}") {
            toast.showToast(
                'error',
                'Please add at least one Theme mapping',
                'error'
            );
            return false;
        }

        let rawData =
            JSON.parse(mappingJson);
        // =========================================
        // FORMAT DATA
        // =========================================

        let mappingData = [];
        Object.keys(rawData).forEach(tId => {
            let theme = rawData[tId];
            mappingData.push({
                PillarId: parseInt(tId),
                //PillarName: theme.name,
                Sectors: (
                    Array.isArray(theme.subThemes)
                        ? theme.subThemes
                        : Object.values(theme.subThemes || {})

                ).map(st => ({
                    SectorId: parseInt(st.id),
                    //SectorName: st.name
                }))
            });
        });

        console.log(
            "Formatted Theme Mapping:",
            mappingData
        );

        // =========================================
        // MODEL
        // =========================================

        var model = {
            ActivityGuid: ActivityGuid,
            MappingData: mappingData
        };

        // =========================================
        // CONFIRMATION
        // =========================================


        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save Themes mapping!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                // =========================================
                // AJAX
                // =========================================

                ajax.doPostAjax(
                    `/Management/SavePillarMapping`,
                    model,
                    function (r) {
                        common.HideLoader();
                        //if (r.status || r.success) {
                        //    toast.showToast(
                        //        'success',
                        //        r.message || 'Saved successfully',
                        //        'success'
                        //    );

                        //    // Redirect
                        //    setTimeout(function () {
                        //        window.location.href =
                        //            '/Management/ActivityManagement';
                        //    }, 1000);
                        //    activityMaster.updateStepUI();
                        //}
                        if (r.status || r.success) {

                            toast.showToast('success', r.message || 'Saved successfully', 'success');

                            // 🔹 Wizard Step Move (Step 3 → Step 4)
                            setTimeout(function () {

                                $("#content5").removeClass("active").hide();
                                $("#content6").addClass("active").show();

                                $("#s5").addClass("completed").removeClass("active");
                                $("#s6").addClass("active");

                                currentStep = 6;
                                activityMaster.updateStepUI();
                                window.scrollTo(0, 0);

                            }, 1000);

                        }
                        else {
                            toast.showToast(
                                'error',
                                r.message || 'Error occurred',
                                'error'
                            );
                        }
                    }
                );
            }
        });
    },
    getPillarMap: function () {

        var model = {

            activityGuid:
                $('#ActivityGuid').val()
        };

        ajax.doPostAjax(

            '/Management/GetPillarMap',

            model,

            function (response) {

                // =========================================
                // RESET
                // =========================================

                themeData = {};

                $('#pillarMappingContainer').empty();

                // =========================================
                // NO DATA
                // =========================================

                if (!response || response.length === 0) {

                    $('#pillarMappingContainer').html(`
                    <div class="empty-mapping">
                        No mapping found
                    </div>
                `);

                    $('#pillarJsonData').val('');

                    return;
                }

                // =========================================
                // GROUPING
                // THEME -> SUBTHEME
                // =========================================

                response.forEach(function (item) {

                    // 🔥 OLD RESPONSE NAME SAME HI RAKHE
                    let tId = item.pillarId;

                    // Theme Create
                    if (!themeData[tId]) {

                        themeData[tId] = {

                            name: item.pillarName,

                            subThemes: []
                        };
                    }

                    // =====================================
                    // DUPLICATE CHECK
                    // =====================================

                    let exists =
                        themeData[tId]
                            .subThemes
                            .some(st =>
                                st.id == item.sectorId);

                    // =====================================
                    // ADD SUBTHEME
                    // =====================================

                    if (!exists) {

                        themeData[tId]
                            .subThemes
                            .push({

                                id: item.sectorId,

                                name: item.sectorName
                            });
                    }
                });

                // =========================================
                // RENDER
                // =========================================

                renderUI();

                // =========================================
                // SAVE JSON
                // =========================================

                $('#pillarJsonData').val(
                    JSON.stringify(themeData)
                );
            });

        // =========================================
        // RENDER UI
        // =========================================

        function renderUI() {

            const themeIds =
                Object.keys(themeData);

            // =========================================
            // EMPTY STATE
            // =========================================

            if (themeIds.length === 0) {

                $('#pillarMappingContainer').html(`
                <div class="empty-mapping">
                    No mapping found
                </div>
            `);

                $('#pillarCountBadge')
                    .text("0 SubThemes");

                return;
            }

            let total = 0;

            $('#pillarMappingContainer').empty();

            // =========================================
            // LOOP THEMES
            // =========================================

            themeIds.forEach(tId => {

                const theme =
                    themeData[tId];

                let subThemeHtml = '';

                // =====================================
                // LOOP SUBTHEMES
                // =====================================

                theme.subThemes.forEach((st, idx) => {

                    total++;

                    subThemeHtml += `

                    <div class="subsector-entry">

                        <span class="subsector-name">

                            ${st.name}

                        </span>

                        <button
                            class="btn-remove-sub"
                            data-t="${tId}"
                            data-idx="${idx}">

                            <i class="bx bx-trash"></i>

                        </button>

                    </div>
                `;
                });

                // =====================================
                // THEME CARD
                // =====================================

                const card = `

                <div class="pillar-card mb-3">

                    <div class="pillar-group-header">

                        <span>

                            ${theme.name}

                        </span>

                        <button
                            class="btn btn-danger btn-sm btn-remove-theme"
                            data-t="${tId}">

                            <i class="bx bx-trash"></i>

                        </button>

                    </div>

                    <div class="sector-block">

                        <div class="sector-title">

                            SubThemes

                        </div>

                        ${subThemeHtml}

                    </div>

                </div>
            `;

                $('#pillarMappingContainer')
                    .append(card);
            });

            // =========================================
            // BADGE COUNT
            // =========================================

            $('#pillarCountBadge')
                .text(`${total} SubThemes Mapped`);
        }
    },
    // =============================================
    // SAVE NATURE OF SUPPORT
    // =============================================

    saveNatureOfSupport: function () {
        var ActivityGuid = $('#ActivityGuid').val();

        // =========================================
        // VALIDATION
        // =========================================
        if (!ActivityGuid || ActivityGuid === "0") {
            toast.showToast(
                'Error',
                'Please save Activity details first',
                'error'
            );
            return false;
        }

        let supportJson = $('#supportJsonData').val();

        if (!supportJson || supportJson === "{}" || supportJson.trim() === "") {
            toast.showToast(
                'Error',
                'Please add at least one Nature of Support mapping',
                'error'
            );
            return false;
        }

        // =========================================
        // PARSE JSON & INITIALIZE MODEL ARRAY
        // =========================================
        let rawData = JSON.parse(supportJson);
        supportData = []; // FIXED: Restored variable initialization to prevent reference crash errors

        // =========================================
        // FORMAT MODEL
        // =========================================
        Object.keys(rawData).forEach(sId => {
            let support = rawData[sId];
            // 🌟 CRITICAL FIX: अगर डेटा null या undefined है, तो उसे यहीं स्किप करो
            if (support === null || typeof support !== 'object') {
                return; // Continue to next loop item
            }
            // सुरक्षा जांच: सुनिश्चित करें कि details एरे मौजूद है
            let detailsArray = support.details || [];

            supportData.push({
                SupportId: parseInt(sId),
                //SupportName: support.name || "N/A",
                SupportDetails: detailsArray.map(d => ({
                    DetailId: parseInt(d.id),
                    //DetailName: d.name
                }))
            });
        });
        // दोबारा जांचें कि क्लीन करने के बाद कुछ डेटा बचा है या नहीं
        if (supportData.length === 0) {
            toast.showToast('Error', 'Please add at least one valid Nature of Support mapping', 'error');
            return false;
        }

        console.log("Formatted Support Mapping:", supportData);

        // =========================================
        // FINAL DATA MODEL OBJECT
        // =========================================
        var model = {
            ActivityGuid: ActivityGuid,
            SupportData: supportData
        };

        console.log("Saving Nature Of Support:", model);

        

        // =========================================
        // CONFIRMATION POPUP (SweetAlert2)
        // =========================================
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save Nature of Support mapping!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();

                // =====================================
                // AJAX POST REQUEST SUBMISSION
                // =====================================
                ajax.doPostAjax(
                    `/Management/SaveNatureOfSupportMapping`,
                    model,
                    function (r) {
                        common.HideLoader();

                        if (r.status || r.success) {
                            toast.showToast(
                                'Success',
                                r.message || 'Saved successfully',
                                'success'
                            );

                            // Trigger step navigation adjustment framework rules
                            if (typeof activityMaster !== 'undefined' && activityMaster.updateStepUI) {
                                activityMaster.updateStepUI();
                            }

                            // Seamless view switch redirection with safety delay buffer
                            setTimeout(function () {
                                window.location.href = '/Management/ActivityManagement';
                            }, 1000);
                        }
                        else {
                            toast.showToast(
                                'Error',
                                r.message || 'Error occurred while saving.',
                                'error'
                            );
                        }
                    }
                );
            }
        });
    },
    // =============================================
    // GET NATURE OF SUPPORT MAP (EDIT POPULATION)
    // =============================================
    getNatureOfSupportMap: function () {
        var model = {
            ActivityGuid: $('#ActivityGuid').val()
        };

        ajax.doPostAjax('/Management/GetNatureOfSupportMap', model, function (response) {
            // Clear the layout presentation layer
            $('#supportContainer').empty();

            // Reset local scope helper object state
            var supportMappingData = {};

            // Handle empty database records payload conditions cleanly
            if (!response || response.length === 0) {
                $('#supportContainer').html(`
                <div class="empty-state">
                    <i class="fas fa-handshake fa-3x mb-3 opacity-25"></i>
                    <p>No Nature Of Support mapped yet.</p>
                </div>
            `);
                $('#supportJsonData').val('');
                $('#totalSupportBadge').text("0 Supports Total");
                return;
            }

            // Parse flat database join table items into nested group collections
            response.forEach(function (item) {
                // FIX: Convert numeric database IDs to Strings to match HTML attribute type conventions
                let sId = item.supportId ? item.supportId.toString() : "";
                let dId = item.detailId ? item.detailId.toString() : "";

                if (!sId || !dId) return; // Skip corrupted database pairs if any

                if (!supportMappingData[sId]) {
                    supportMappingData[sId] = {
                        name: item.supportName,
                        details: []
                    };
                }

                supportMappingData[sId].details.push({
                    id: dId, // Stored safely as a string
                    name: item.detailName
                });
            });
            debugger;
            // =========================================================
            // CRITICAL SYNC STEP: Save map data directly into your main 
            // global script scope state object variable.
            // =========================================================
            supportData = supportMappingData;

            // Execute unified structural UI template assembly routine
            activityMaster.renderSupportUI();
        });

        // =========================================
        // UNIFIED PRESENTATION ENGINE
        // =========================================

    },
    renderSupportUI: function () {
        // Ensure accurate tracking referencing the unified global data object map
        const ids = Object.keys(supportData);

        if (ids.length === 0) {
            $('#supportContainer').html(`
                <div class="empty-state">
                    <i class="fas fa-handshake fa-3x mb-3 opacity-25"></i>
                    <p>No Nature Of Support mapped yet.</p>
                </div>`);
            $('#totalSupportBadge').text("0 Supports Total");
            $('#supportJsonData').val("");
            return;
        }

        $('#supportContainer').empty();
        let totalCounter = 0;

        ids.forEach(sId => {
            const supportItem = supportData[sId];
            totalCounter += supportItem.details.length;

            let detailHtml = '';
            supportItem.details.forEach(d => {
                detailHtml += `
                <div class="support-entry">
                    <span class="support-name">${d.name}</span>
                    <button type="button" class="btn-remove-support" data-support="${sId}" data-detail-id="${d.id}">
                        <i class="fas fa-times me-1"></i> Remove
                    </button>
                </div>`;
            });

            const card = `
            <div class="support-group-card">
                <div class="support-group-header">
                    <span>
                        <i class="fas fa-layer-group me-2"></i> ${supportItem.name}
                    </span>
                    <button type="button" class="btn btn-outline-danger btn-sm btn-remove-main" data-support="${sId}">
                        <i class="fas fa-trash-alt me-1"></i> Remove Full
                    </button>
                </div>
                <div class="support-list-container">
                    ${detailHtml}
                </div>
            </div>`;

            $('#supportContainer').append(card);
        });

        // Sync visual counter badges and output hidden payload strings
        $('#totalSupportBadge').text(`${totalCounter} Supports Total`);
        $('#supportJsonData').val(JSON.stringify(supportData));
    },
    updateStepUI: function () {

        document.querySelectorAll('.step-content').forEach(content => {
            content.classList.remove('active');
            content.style.display = ''; // 🔥 remove inline display:none
        });

        document.getElementById('content' + currentStep).classList.add('active');

        document.querySelectorAll('.step-item').forEach((item, idx) => {
            const stepIdx = idx + 1;

            item.classList.remove('active', 'completed');

            if (stepIdx === currentStep) item.classList.add('active');
            if (stepIdx < currentStep) item.classList.add('completed');
        });

        prevBtn.disabled = (currentStep === 1);

        if (currentStep === totalSteps) {
            nextBtn.innerText = "Save";   // ✅ last step
            //nextBtn.classList.add('d-none');
            /*submitBtn.classList.remove('d-none');*/
        } else {
            nextBtn.innerText = "Save & Next →"; // ✅ other steps
            //nextBtn.classList.remove('d-none');
            /*submitBtn.classList.add('d-none');*/
        }
    },
}
document.addEventListener('DOMContentLoaded', function () {
    /*const submitBtn = document.getElementById('submitBtn');*/
    //// NEXT
    //nextBtn.addEventListener('click', function () {

    //    if (currentStep < totalSteps) {
    //        currentStep++;
    //        updateStepUI();
    //    }

    //});

    // PREVIOUS
    prevBtn.addEventListener('click', function () {
        if (currentStep > 1) {
            currentStep--;
            activityMaster.updateStepUI();
        }
    });

    // INIT
    //updateStepUI();

});

