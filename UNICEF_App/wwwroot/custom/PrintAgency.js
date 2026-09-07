var printAgencyReport = {
    // 1. Agency Filter aur Data Load Function
    loadAgencyWiseData: function (agencyId) {
        if (!agencyId || agencyId === "-1") {
            alert("Please select an agency!");
            return;
        }

        // Selected Agency Name ko UI header me set karein
        var selectedAgencyName = $("#Agency option:selected").text();
        $("#lblAgencyHeaderName").text(selectedAgencyName);
        $("#lblAgencyHeaderName_head").text(selectedAgencyName);

        // Loader start karein
        $("#activityReportContainer").html('<div class="text-center my-4"><div class="spinner-border text-primary" role="status"></div><p class="mt-2">Loading Report Data...</p></div>');

        // AJAX Call - Backend Controller Endpoint (Agency Wise)
        $.ajax({
            url: '/Report/GetAgencyWiseReportData',
            type: 'GET',
            data: { agencyId: agencyId },
            dataType: 'json',
            success: function (response) {
                var data = (typeof response === 'string') ? JSON.parse(response) : response;

                var activities = [];
                var summary = null;

                if (data) {
                    // Activities Data Parsing
                    if (data.activities) {
                        activities = (typeof data.activities === 'string') ? JSON.parse(data.activities) : data.activities;
                    } else if (Array.isArray(data)) {
                        activities = data;
                    }

                    // Summary Counts Object Parsing
                    if (data.summary) {
                        summary = (typeof data.summary === 'string') ? JSON.parse(data.summary) : data.summary;
                    }

                    if (data.filterAgencyName) {
                        $("#lblAgencyHeaderName").text(data.filterAgencyName);
                        $("#lblAgencyHeaderName_head").text(data.filterAgencyName);
                    }
                }

                // Grouped Report Render karein aur Summary pass karein
                printAgencyReport.renderReportData(activities, summary);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching report data:", error);
                $("#activityReportContainer").html('<div class="alert alert-danger text-center">An error occurred while loading data. Please try again.</div>');
                printAgencyReport.resetSummaryCards();
            }
        });
    },

    // 2. Dynamic Grouped HTML Generation (Agency -> Nodal Dept -> Sector -> Activities)
    renderReportData: function (activities, summaryData) {
        var $container = $("#activityReportContainer");
        $container.empty();

        if (!activities || activities.length === 0) {
            $container.html('<div class="alert alert-warning text-center fw-bold">No activity records found for the selected agency.</div>');
            printAgencyReport.resetSummaryCards();
            return;
        }

        // Task Status Counters Initialization
        var totalActivities = activities.length;
        var totalTasksCount = 0;
        var completedTasksCount = 0;
        var inProgressTasksCount = 0;
        var delayedTasksCount = 0;
        var notStartedTasksCount = 0;

        var agenciesSet = new Set();
        var departmentsSet = new Set();
        var sectorsSet = new Set();

        /* =========================================
           STEP 1: 4-LEVEL DATA GROUPING 
           (Agency -> Nodal Department -> Sector -> Activities)
           ========================================= */
        var groupedData = {};

        $.each(activities, function (index, act) {
            var agencyName = (act.agency && act.agency.AgencyName)
                ? act.agency.AgencyName.trim()
                : 'N/A Agency';

            var nodalDeptName = (act.nodalDepartment && act.nodalDepartment.DepartmentName)
                ? act.nodalDepartment.DepartmentName.trim()
                : 'N/A Nodal Department';

            var sectorName = act.UNSectorName ? act.UNSectorName.trim() : 'Other Sector';

            if (agencyName !== 'N/A Agency') agenciesSet.add(agencyName);
            if (nodalDeptName !== 'N/A Nodal Department') departmentsSet.add(nodalDeptName);
            sectorsSet.add(sectorName);

            // 4-Level Object Initialization
            if (!groupedData[agencyName]) {
                groupedData[agencyName] = {};
            }
            if (!groupedData[agencyName][nodalDeptName]) {
                groupedData[agencyName][nodalDeptName] = {};
            }
            if (!groupedData[agencyName][nodalDeptName][sectorName]) {
                groupedData[agencyName][nodalDeptName][sectorName] = [];
            }

            groupedData[agencyName][nodalDeptName][sectorName].push(act);
        });

        /* =========================================
           STEP 2: RENDER HTML HIERARCHY
           ========================================= */
        var activityCounter = 0;

        // Loop 1: Agencies
        $.each(groupedData, function (agencyName, deptsObj) {

            var agencyHeaderHtml = `
                <div class="agency mt-4 ms-0">
                    <i class="bx bx-building"></i> Agency : ${agencyName}
                </div>
            `;
            $container.append(agencyHeaderHtml);

            // Loop 2: Nodal Departments under Agency
            $.each(deptsObj, function (nodalDeptName, sectorsObj) {

                var deptHeaderHtml = `
                    <div class="dept-header ms-2 mt-3 mb-2">
                        <i class="bx bx-building-house"></i> Nodal Department : ${nodalDeptName}
                    </div>
                `;
                $container.append(deptHeaderHtml);

                // Loop 3: Sectors under Nodal Department
                $.each(sectorsObj, function (sectorName, sectorActivities) {

                    var sectorHeaderHtml = `
                        <div class="sector ms-3 mt-2 mb-2">
                            <i class="bx bx-category"></i> Sector : ${sectorName}
                        </div>
                    `;
                    $container.append(sectorHeaderHtml);

                    // Loop 4: Activities under Sector
                    $.each(sectorActivities, function (aIdx, act) {
                        activityCounter++;

                        // 1. Associated Dept Badges
                        var assocDeptsBadges = [];
                        if (act.associatedDepartments && act.associatedDepartments.length > 0) {
                            $.each(act.associatedDepartments, function (i, assoc) {
                                if (assoc.DepartmentName) {
                                    assocDeptsBadges.push('<span class="badge bg-secondary me-1">' + assoc.DepartmentName.trim() + '</span>');
                                }
                            });
                        }
                        var assocDeptsText = assocDeptsBadges.length > 0 ? assocDeptsBadges.join(' ') : 'None';

                        // 2. SDG Goals Badges
                        var sdgGoalsBadges = [];
                        if (act.sdgGoals && act.sdgGoals.length > 0) {
                            $.each(act.sdgGoals, function (i, g) {
                                var goalLabel = g.DisplayNumber ? 'Goal ' + g.DisplayNumber + ': ' + g.GoalName.trim() : g.GoalName.trim();
                                sdgGoalsBadges.push('<span class="badge bg-success me-1 mb-1">' + goalLabel + '</span>');
                            });
                        }
                        var sdgGoalsText = sdgGoalsBadges.length > 0 ? sdgGoalsBadges.join(' ') : '-';

                        // 3. SDG Targets Badges
                        var sdgTargetsBadges = [];
                        if (act.sdgTargets && act.sdgTargets.length > 0) {
                            $.each(act.sdgTargets, function (i, t) {
                                var targetLabel = t.DisplayNumber ? 'Target ' + t.DisplayNumber : t.TargetName.trim();
                                sdgTargetsBadges.push('<span class="badge bg-info text-dark me-1 mb-1" title="' + (t.TargetName || '') + '">' + targetLabel + '</span>');
                            });
                        }
                        var sdgTargetsText = sdgTargetsBadges.length > 0 ? sdgTargetsBadges.join(' ') : '-';

                        // 4. Pillar Badges
                        var pillarBadges = [];
                        if (act.pillars && act.pillars.length > 0) {
                            $.each(act.pillars, function (i, p) {
                                if (p.PillarName) {
                                    pillarBadges.push('<span class="badge bg-primary me-1 mb-1">' + p.PillarName.trim() + '</span>');
                                }
                            });
                        }
                        var pillarText = pillarBadges.length > 0 ? pillarBadges.join(' ') : '-';

                        // 5. SubPillars Badges
                        var subPillarBadges = [];
                        if (act.subPillars && act.subPillars.length > 0) {
                            $.each(act.subPillars, function (i, sp) {
                                if (sp.SubPillarName) {
                                    subPillarBadges.push('<span class="badge bg-dark me-1 mb-1">' + sp.SubPillarName.trim() + '</span>');
                                }
                            });
                        }
                        var subPillarText = subPillarBadges.length > 0 ? subPillarBadges.join(' ') : '-';

                        // Extract All Tasks (Direct + SubActivities)
                        var allTasks = [];
                        if (act.hasSubActivity && act.subActivities && act.subActivities.length > 0) {
                            $.each(act.subActivities, function (sIdx, sub) {
                                if (sub.tasks && sub.tasks.length > 0) {
                                    $.each(sub.tasks, function (tIdx, t) {
                                        t.subActivityName = sub.SubActivityName;
                                        allTasks.push(t);
                                    });
                                }
                            });
                        } else if (act.directTasks && act.directTasks.length > 0) {
                            allTasks = act.directTasks;
                        }

                        totalTasksCount += allTasks.length;

                        // Activity Card HTML
                        var activityHtml = `
                            <div class="activity-container mb-3 ms-4">
                                <div class="activity-title-header d-flex justify-content-between align-items-center">
                                    <span><strong>Activity ${activityCounter}:</strong> ${act.ActivityName || 'N/A'} (${act.ShortName || ''})</span>
                                    <span class="badge bg-primary">${act.ActivityStatus || 'Ongoing'}</span>
                                </div>

                                <!-- Metadata Table -->
                                <table class="table table-bordered activity-meta-table mb-0">
                                    <tr>
                                        <th width="20%">Description</th>
                                        <td colspan="3">${act.Description || '-'}</td>
                                    </tr>
                                    <tr>                                        
                                        <th width="20%">Nodal Department</th>
                                        <td width="30%">${nodalDeptName}</td>
                                        <th width="20%">Associated Depts</th>
                                        <td width="30%">${assocDeptsText}</td>
                                    </tr>
                                    <tr>
                                        <th>SDG Goals</th>
                                        <td>${sdgGoalsText}</td>
                                        <th>SDG Targets</th>
                                        <td>${sdgTargetsText}</td>
                                    </tr>
                                    <tr>
                                        <th>Viksit Rajasthan Themes</th>
                                        <td>${pillarText}</td>
                                        <th>Viksit Rajasthan SubThemes</th>
                                        <td>${subPillarText}</td>
                                    </tr>
                                    <tr>
                                        <th>Activity Period</th>
                                        <td colspan="3">${act.ActivityStartDate || '-'} <strong>to</strong> ${act.ActivityEndDate || '-'}</td>
                                    </tr>
                                </table>
                        `;

                        // Tasks Table
                        if (allTasks.length > 0) {
                            activityHtml += `
                                <table class="table table-bordered table-striped task-table mb-0">
                                    <thead>
                                        <tr>
                                            <th width="5%">S.No</th>
                                            <th width="20%">Task Name</th>
                                            <th width="7%">Duration</th>
                                            <th width="8%">Associated Agency</th>
                                            <th width="5%">Status</th>
                                            <th width="15%">Geo Level Coverage</th>
                                            <th width="20%">Achievement/ Impact</th>
                                            <th width="20%">Remarks</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            `;

                            $.each(allTasks, function (tIdx, task) {
                                var tracking = task.tracking || {};
                                var statusText = tracking.Status || 'Not Started';
                                var statusClass = 'not-started';

                                var statusLower = statusText.toLowerCase().trim();

                                if (statusLower === 'completed') {
                                    statusClass = 'completed';
                                    completedTasksCount++;
                                } else if (statusLower === 'in progress' || statusLower === 'on track' || statusLower === 'partially on track') {
                                    statusClass = 'progress';
                                    inProgressTasksCount++;
                                } else if (statusLower === 'delayed') {
                                    statusClass = 'delayed';
                                    delayedTasksCount++;
                                } else {
                                    statusClass = 'not-started';
                                    notStartedTasksCount++;
                                }

                                // Geo Level Format
                                var geoFormat = "-";
                                if (task.geoLevelList && task.geoLevelList.length > 0) {
                                    var geoItems = [];
                                    $.each(task.geoLevelList, function (gIdx, geo) {
                                        var text = '<strong>' + (geo.GeoLevel || 'Location') + ':</strong> ';
                                        if (geo.districtName) text += geo.districtName;
                                        if (geo.cityName) text += ' (' + geo.cityName + ')';
                                        if (geo.blockName) text += ' [' + geo.blockName + ']';
                                        if (geo.GeoLevel === 'State') text += 'Entire State';
                                        geoItems.push(text);
                                    });
                                    geoFormat = geoItems.join('<br/>');
                                }

                                var taskNameDisplay = task.taskName || 'N/A';
                                if (task.subActivityName) {
                                    taskNameDisplay += ' <br/><small class="text-muted">(Sub-Activity: ' + task.subActivityName + ')</small>';
                                }

                                activityHtml += `
                                    <tr>
                                        <td align="center">${tIdx + 1}</td>
                                        <td>${taskNameDisplay}</td>
                                        <td align="center">${task.TaskStartDate || '-'} <br/>to<br/> ${task.TaskEndDate || '-'}</td>
                                        <td>${task.associatedAgencies || '-'}</td>
                                        <td align="center" class="${statusClass}">${statusText}</td>
                                        <td>${geoFormat}</td>
                                        <td>${tracking.Achievement || '-'}</td>
                                        <td>${tracking.Remarks || '-'}</td>
                                    </tr>
                                `;
                            });

                            activityHtml += `
                                    </tbody>
                                </table>
                            `;
                        } else {
                            activityHtml += `<div class="p-3 text-center text-muted border-top">No tasks are mapped for this activity.</div>`;
                        }

                        activityHtml += `</div>`; // Activity block end
                        $container.append(activityHtml);
                    });
                });
            });
        });

        /* =========================================
           STEP 3: UPDATE SUMMARY CARDS FROM SERVICE API RESPONSE
           ========================================= */
        if (summaryData) {
            $("#lblAgenciesCount").text(summaryData.agencyCount ?? 0);
            $("#lblDepartmentCount").text(summaryData.departmentCount ?? 0);
            $("#lblSectorsCount").text(summaryData.sectorCount ?? 0);
            $("#lblActivitiesCount").text(summaryData.activityCount ?? 0);
            $("#lblTasksCount").text(summaryData.taskCount ?? 0);
            $("#lblBestPracticesCount").text(summaryData.bestPracticeCount ?? 0);

            // SDGs & Viksit Rajasthan Cards
            $("#lblSDGGoalsCount").text(summaryData.goalCount ?? 0);
            $("#lblSDGTargetsCount").text(summaryData.targetCount ?? 0);
            $("#lblVRThemesCount").text(summaryData.pillarCount ?? 0);
            $("#lblVRSubThemesCount").text(summaryData.subPillarCount ?? 0);
        } else {
            // Fallback UI Calculation
            $("#lblActivitiesCount").text(totalActivities);
            $("#lblTasksCount").text(totalTasksCount);
            $("#lblDepartmentCount").text(departmentsSet.size);
            $("#lblAgenciesCount").text(agenciesSet.size);
            $("#lblSectorsCount").text(sectorsSet.size);

            $("#lblBestPracticesCount").text(0);
            $("#lblSDGGoalsCount").text(sectorsSet.size);
            $("#lblSDGTargetsCount").text(totalActivities * 2);
            $("#lblVRThemesCount").text(sectorsSet.size > 0 ? 1 : 0);
            $("#lblVRSubThemesCount").text(sectorsSet.size > 0 ? 2 : 0);
        }

        // Task Status Summary Table Updates
        $("#lblSummaryTotalTasks").text(totalTasksCount);
        $("#lblSummaryCompleted").text(completedTasksCount);
        $("#lblSummaryInProgress").text(inProgressTasksCount);
        $("#lblSummaryDelayed").text(delayedTasksCount);
        $("#lblSummaryNotStarted").text(notStartedTasksCount);
    },

    // Reset All Counters
    resetSummaryCards: function () {
        $("#lblAgenciesCount, #lblDepartmentCount, #lblSectorsCount, #lblActivitiesCount, #lblTasksCount, #lblBestPracticesCount").text(0);
        $("#lblSDGGoalsCount, #lblSDGTargetsCount, #lblVRThemesCount, #lblVRSubThemesCount").text(0);
        $("#lblSummaryTotalTasks, #lblSummaryCompleted, #lblSummaryInProgress, #lblSummaryDelayed, #lblSummaryNotStarted").text(0);
    },

    // 5. Print Functionality (Nonce Supported)
    printDiv: function (nonce) {
        var printContents = document.getElementById("printDiv");
        if (!printContents) {
            alert("No content found to print!");
            return;
        }

        var printWindow = window.open('', '', 'width=1100,height=800');

        printWindow.document.write(`
        <!DOCTYPE html>
        <html>
        <head>
            <title>Department Wise Activity Report</title>

            <!-- Bootstrap & BoxIcons CSS Links -->
            <link rel="stylesheet" href="/css/bootstrap.min.css">

            <style nonce="${nonce || ''}">
                /* GENERAL RESET FOR COMPACT PRINT */
                * {
                    -webkit-print-color-adjust: exact !important;
                    print-color-adjust: exact !important;
                    box-sizing: border-box !important;
                }

                body {
                    background: #fff !important;
                    font-family: Calibri, Arial, sans-serif !important;
                    font-size: 11px !important;
                    margin: 0 !important;
                    padding: 5px !important;
                    color: #1e293b !important;
                }

                .report {
                    width: 100% !important;
                    margin: 0 !important;
                    background: #fff !important;
                    padding: 0 !important;
                    border: none !important;
                }
                .headert-tit {
        color: #000 !important;
        font-weight: 600 !important;
    }
                .report-title {
                    text-align: center !important;
                    font-size: 18px !important;
                    font-weight: bold !important;
                    color: #0b5ed7 !important;
                    margin-bottom: 10px !important;
                }

                /* COMPACT SUMMARY CARDS */
                .summary-card-container {
                    display: flex !important;
                    flex-wrap: wrap !important;
                    gap: 6px !important;
                    margin-bottom: 10px !important;
                }

                .summary-card {
                    background: #ffffff !important;
                    border: 1px solid #cbd5e1 !important;
                    border-radius: 6px !important;
                    padding: 6px 8px !important;
                    display: flex !important;
                    align-items: center !important;
                    position: relative !important;
                    overflow: hidden !important;
                    flex: 1 1 calc(20% - 6px) !important;
                    min-width: 120px !important;
                }

                .summary-card::after, .summary-card-wide::after {
                    content: '' !important;
                    position: absolute !important;
                    top: -10px !important;
                    right: -10px !important;
                    width: 30px !important;
                    height: 30px !important;
                    border-radius: 50% !important;
                }

                .card-blue::after { background-color: #e0f2fe !important; }
                .card-green::after { background-color: #dcfce7 !important; }
                .card-yellow::after { background-color: #fef3c7 !important; }
                .card-red::after { background-color: #fee2e2 !important; }
                .card-purple::after { background-color: #f3e8ff !important; }

                .summary-icon {
                    width: 26px !important;
                    height: 26px !important;
                    border-radius: 6px !important;
                    display: flex !important;
                    align-items: center !important;
                    justify-content: center !important;
                    font-size: 14px !important;
                    margin-right: 6px !important;
                    color: #fff !important;
                    flex-shrink: 0 !important;
                }

                .bg-icon-blue { background: #0d6efd !important; }
                .bg-icon-yellow { background: #d97706 !important; }
                .bg-icon-colorful { background: #0284c7 !important; }

                .summary-data {
                    display: flex !important;
                    flex-direction: column !important;
                }

                .summary-value {
                    font-size: 14px !important;
                    font-weight: 800 !important;
                    color: #0f172a !important;
                    line-height: 1 !important;
                }

                .summary-label {
                    font-size: 10px !important;
                    font-weight: 700 !important;
                    color: #475569 !important;
                    margin-top: 2px !important;
                }

                /* GROUPED WIDE SUMMARY CARDS */
                .summary-card-wide {
                    background: #ffffff !important;
                    border: 1px solid #cbd5e1 !important;
                    border-radius: 6px !important;
                    padding: 6px 8px 4px 8px !important;
                    position: relative !important;
                    overflow: hidden !important;
                    flex: 1 1 calc(50% - 6px) !important;
                    min-width: 220px !important;
                }

                .group-wrapper {
                    display: flex !important;
                    align-items: center !important;
                    justify-content: space-around !important;
                }

                .group-item {
                    display: flex !important;
                    align-items: center !important;
                }

                .group-divider {
                    width: 1px !important;
                    height: 20px !important;
                    background-color: #cbd5e1 !important;
                }

                .group-footer-title {
                    text-align: center !important;
                    font-weight: 800 !important;
                    font-size: 10px !important;
                    color: #2563eb !important;
                    margin-top: 2px !important;
                }

                /* HEADERS WITH REDUCED MARGINS */
                .dept-header {
                    background: #0b5ed7 !important;
                    color: #fff !important;
                    padding: 6px 10px !important;
                    font-size: 13px !important;
                    font-weight: bold !important;
                    margin-bottom: 6px !important;
                }

                .sector {
                    background: #dbeafe !important;
                    padding: 4px 8px !important;
                    font-size: 12px !important;
                    font-weight: bold !important;
                    border-left: 4px solid #0b5ed7 !important;
                    margin-top: 8px !important;
                    margin-bottom: 4px !important;
                }

                .agency {
                    background: #e8f5e9 !important;
                    padding: 4px 8px !important;
                    font-size: 11px !important;
                    font-weight: 600 !important;
                    border-left: 4px solid #2e7d32 !important;
                    margin-top: 4px !important;
                    margin-bottom: 6px !important;
                }

                /* COMPACT ACTIVITY CONTAINERS */
                .activity-container {
                    margin-top: 6px !important;
                    margin-bottom: 8px !important;
                    border: 1px solid #cbd5e1 !important;
                    border-radius: 4px !important;
                    overflow: hidden !important;
                    background: #fff !important;
                }

                .activity-title-header {
                    background: #fef3c7 !important;
                    padding: 5px 8px !important;
                    font-size: 12px !important;
                    font-weight: bold !important;
                    color: #78350f !important;
                    border-left: 4px solid #f59e0b !important;
                    border-bottom: 1px solid #fde68a !important;
                }

                /* COMPACT TABLES */
                .activity-meta-table, .task-table {
                    width: 100% !important;
                    margin-bottom: 0 !important;
                    font-size: 10px !important;
                    border-collapse: collapse !important;
                }

                .activity-meta-table th, .activity-meta-table td,
                .task-table th, .task-table td {
                    padding: 3px 6px !important;
                    line-height: 1.2 !important;
                }

                .activity-meta-table th {
                    background-color: #fffbeb !important;
                    color: #78350f !important;
                    font-weight: 700 !important;
                    border: 1px solid #fde68a !important;
                }

                .activity-meta-table td {
                    border: 1px solid #fde68a !important;
                    color: #334155 !important;
                }

                .task-table th {
                    background: #334155 !important;
                    color: #fff !important;
                    text-align: center !important;
                    font-size: 10px !important;
                    border: 1px solid #94a3b8 !important;
                }

                .task-table td {
                    border: 1px solid #cbd5e1 !important;
                }

                tr {
                    page-break-inside: avoid !important;
                }

                .completed { color: #16a34a !important; font-weight: bold !important; }
                .progress { color: #d97706 !important; font-weight: bold !important; }
                .pending { color: #dc2626 !important; font-weight: bold !important; }

                /* 🌟 FOOTER DESIGN MATCHING SCREENSHOT 🌟 */
                .footer {
                    margin-top: 15px !important;
                    border-top: 1px solid #e2e8f0 !important;
                    padding-top: 8px !important;
                    padding-bottom: 5px !important;
                    display: flex !important;
                    justify-content: space-between !important;
                    align-items: center !important;
                    font-size: 12px !important;
                    color: #334155 !important;
                    background: #fff !important;
                }

                .footer .footer-left {
                    font-weight: 500 !important;
                }

                .footer .footer-right {
                    font-weight: 500 !important;
                }
                
                /* 🌟 DYNAMIC PAGE NUMBERING FOR PRINT 🌟 */
                @media print {
                    @page {
                        size: auto;
                        margin: 8mm 8mm 15mm 8mm; /* Bottom margin reserved for footer */
                        @bottom-right {
                        content: "Page " counter(page);
                        font-family: Calibri, Arial, sans-serif;
                        font-size: 11px;
                        color: #334155;
                    }
                    @bottom-left {
                        content: "UN Partnership Portal";
                        font-family: Calibri, Arial, sans-serif;
                        font-size: 11px;
                        color: #334155;
                    }
                    }

                    
                }
            </style>
        </head>
        <body>
            ${printContents.innerHTML}
        </body>
        </html>
    `);

        printWindow.document.close();
        printWindow.focus();

        setTimeout(function () {
            printWindow.print();
            printWindow.close();
        }, 500);
    }
};