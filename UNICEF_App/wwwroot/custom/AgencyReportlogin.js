// Global Chart instances taaki memory leaks aur redraw collisions na ho
var agencyActivitiesDonutInstance = null;
var agencyTaskHorizontalBarInstance = null;

// Bar ke right-edge ke andar/upar text likhne ke liye custom Chart.js plugin
var chartDataLabelsPlugin = {
    id: 'customDataLabels',
    afterDatasetsDraw: function (chart) {
        if (chart.config.type !== 'bar') return;
        var ctx = chart.ctx;
        chart.data.datasets.forEach(function (dataset, i) {
            var meta = chart.getDatasetMeta(i);
            meta.data.forEach(function (bar, index) {
                var val = dataset.data[index];
                if (val > 0) {
                    ctx.save();
                    ctx.fillStyle = '#1e293b';
                    ctx.font = 'bold 11px sans-serif';
                    ctx.textAlign = 'left';
                    ctx.textBaseline = 'middle';
                    ctx.fillText(val, bar.x + 6, bar.y);
                    ctx.restore();
                }
            });
        });
    }
};
var printAgencyReport = {
    // 1. Agency Filter aur Data Load Function (Updated for Dept & Sector)
    loadAgencyWiseData: function (agencyId, departmentId, sectorId) {
        if (!agencyId || agencyId === "-1") {
            alert("Agency details not found!");
            return;
        }

        // Loader start karein
        $("#activityReportContainer").html('<div class="text-center my-4"><div class="spinner-border text-primary" role="status"></div><p class="mt-2">Loading Report Data...</p></div>');

        // Optional: Convert "-1" to null if your backend expects null for no filter
        var deptParam = (departmentId && departmentId !== "-1") ? departmentId : null;
        var sectorParam = (sectorId && sectorId !== "-1") ? sectorId : null;

        // AJAX Call - Backend Controller Endpoint (Agency Wise with filters)
        $.ajax({
            url: '/AdminReport/GetAgencyLoginReport',
            type: 'GET',
            data: {
                agencyId: agencyId,
                deptId: deptParam,
                sectorId: sectorParam
            },
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

                    // Update UI Header Name
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
            $container.html('<div class="alert alert-warning text-center fw-bold">No activity records found for the selected filters.</div>');
            printAgencyReport.resetSummaryCards();
            return;
        }

        // Task Status Counters Initialization
        var totalActivities = activities.length;
        var totalTasksCount = 0;

        // Counters for Activities (Donut Chart)
        var activityStatusCounts = {
            completed: 0,
            discontinued: 0,
            notStarted: 0,
            ongoing: 0
        };

        // Counters for Tasks (Horizontal Bar Chart & Summary)
        var taskStatusCounts = {
            notStarted: 0,
            completed: 0,
            discontinued: 0,
            onTrack: 0,
            partiallyOnTrack: 0,
            inProgress: 0
        };

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

            // Activity status classification
            var actStatus = (act.ActivityStatus || '').toLowerCase().trim();
            if (actStatus === 'completed') {
                activityStatusCounts.completed++;
            } else if (actStatus === 'discontinued') {
                activityStatusCounts.discontinued++;
            } else if (actStatus === 'not started') {
                activityStatusCounts.notStarted++;
            } else {
                activityStatusCounts.ongoing++;
            }
        });

        /* =========================================
           STEP 2: RENDER HTML HIERARCHY
           ========================================= */
        var activityCounter = 0;

        // Loop 1: Agencies
        $.each(groupedData, function (agencyName, deptsObj) {

            //var agencyHeaderHtml = `
            //    <div class="agency mt-4 ms-0">
            //        <i class="bx bx-building"></i> Agency : ${agencyName}
            //    </div>
            //`;
            //$container.append(agencyHeaderHtml);
            $("#lblAgencyHeaderName_head").text(`${agencyName}`);
            // Loop 2: Nodal Departments under Agency
            $.each(deptsObj, function (nodalDeptName, sectorsObj) {

                var deptHeaderHtml = `
                    <div class="dept-header">
                        <i class="bx bx-building-house"></i> Department : ${nodalDeptName}
                    </div>
                `;
                $container.append(deptHeaderHtml);

                // Loop 3: Sectors under Nodal Department
                $.each(sectorsObj, function (sectorName, sectorActivities) {

                    var sectorHeaderHtml = `
                        <div class="sector">
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
                            <div class="activity-container">
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
                                var statusLower = statusText.toLowerCase().trim();
                                var statusClass = 'not-started';


                                if (statusLower === 'completed') {
                                    statusClass = 'completed';
                                    taskStatusCounts.completed++;
                                    completedTasksCount++;
                                } else if (statusLower === 'discontinued') {
                                    statusClass = 'delayed';
                                    taskStatusCounts.discontinued++;
                                    delayedTasksCount++;
                                } else if (statusLower === 'on track') {
                                    statusClass = 'completed';
                                    taskStatusCounts.onTrack++;
                                    completedTasksCount++;
                                } else if (statusLower === 'partially on track') {
                                    statusClass = 'progress';
                                    taskStatusCounts.partiallyOnTrack++;
                                    inProgressTasksCount++;
                                } else if (statusLower === 'in progress') {
                                    statusClass = 'progress';
                                    taskStatusCounts.inProgress++;
                                    inProgressTasksCount++;
                                } else {
                                    statusClass = 'not-started';
                                    taskStatusCounts.notStarted++;
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
            //$("#lblAgenciesCount").text(summaryData.agencyCount ?? 0);
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
            //$("#lblAgenciesCount").text(agenciesSet.size);
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

        // Render Donut & Horizontal Bar Charts
        printAgencyReport.renderActivitiesDonutChart(activityStatusCounts);
        printAgencyReport.renderTaskHorizontalBarChart(taskStatusCounts);
    },

    // 3. Render Donut Chart (Activities Status)
    renderActivitiesDonutChart: function (counts) {
        if (typeof Chart === 'undefined') return;

        var ctx = document.getElementById('activitiesDonutChart');
        if (!ctx) return;

        if (agencyActivitiesDonutInstance) {
            agencyActivitiesDonutInstance.destroy();
        }

        var completed = Number(counts.completed) || 0;
        var discontinued = Number(counts.discontinued) || 0;
        var notStarted = Number(counts.notStarted) || 0;
        var ongoing = Number(counts.ongoing) || 0;

        var chartData = [completed, discontinued, notStarted, ongoing];
        var labels = ['Completed', 'Discontinued', 'Not Started', 'Ongoing'];
        var colors = ['#2563eb', '#64748b', '#ef4444', '#22c55e'];

        const centerTextPlugin = {
            id: 'agencyCenterTextPlugin',
            afterDraw(chart) {
                const { ctx } = chart;
                const total = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);
                const meta = chart.getDatasetMeta(0);
                if (!meta.data.length) return;

                const centerX = meta.data[0].x;
                const centerY = meta.data[0].y;

                ctx.save();
                ctx.textAlign = 'center';
                ctx.textBaseline = 'middle';
                ctx.fillStyle = '#0f172a';
                ctx.font = 'bold 30px Arial, sans-serif';
                ctx.fillText(total, centerX, centerY - 10);

                ctx.fillStyle = '#64748b';
                ctx.font = '500 13px Arial, sans-serif';
                ctx.fillText('Total Activities', centerX, centerY + 16);
                ctx.restore();
            }
        };

        const sliceValuePlugin = {
            id: 'agencySliceValuePlugin',
            afterDatasetsDraw(chart) {
                const { ctx } = chart;
                const meta = chart.getDatasetMeta(0);

                meta.data.forEach((arc, index) => {
                    const value = chart.data.datasets[0].data[index];
                    if (!value || value === 0) return;

                    const angle = (arc.startAngle + arc.endAngle) / 2;
                    const radius = arc.innerRadius + (arc.outerRadius - arc.innerRadius) / 2;
                    const x = arc.x + Math.cos(angle) * radius;
                    const y = arc.y + Math.sin(angle) * radius;

                    ctx.save();
                    ctx.fillStyle = '#ffffff';
                    ctx.font = 'bold 13px Arial, sans-serif';
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';
                    ctx.fillText(value, x, y);
                    ctx.restore();
                });
            }
        };

        agencyActivitiesDonutInstance = new Chart(ctx.getContext('2d'), {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: chartData,
                    backgroundColor: colors,
                    borderWidth: 2,
                    borderColor: '#ffffff',
                    hoverOffset: 6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '70%',
                plugins: {
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            pointStyle: 'circle',
                            padding: 16,
                            font: { size: 13, weight: '600' },
                            generateLabels: function (chart) {
                                const data = chart.data;
                                return data.labels.map((label, i) => {
                                    const count = data.datasets[0].data[i];
                                    return {
                                        text: `${label} (${count})`,
                                        fillStyle: data.datasets[0].backgroundColor[i],
                                        strokeStyle: '#ffffff',
                                        lineWidth: 1,
                                        index: i
                                    };
                                });
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (ctx) {
                                return ' ' + ctx.label + ': ' + ctx.raw;
                            }
                        }
                    }
                }
            },
            plugins: [centerTextPlugin, sliceValuePlugin]
        });
    },

    // 4. Render Horizontal Bar Chart (Task Status Overview)
    renderTaskHorizontalBarChart: function (taskCounts) {
        if (typeof Chart === 'undefined') return;

        var ctx = document.getElementById('taskHorizontalBarChart');
        if (!ctx) return;

        if (agencyTaskHorizontalBarInstance) {
            agencyTaskHorizontalBarInstance.destroy();
        }

        var totalTasks = (taskCounts.notStarted || 0) +
            (taskCounts.completed || 0) +
            (taskCounts.discontinued || 0) +
            (taskCounts.onTrack || 0) +
            (taskCounts.partiallyOnTrack || 0) +
            (taskCounts.inProgress || 0);

        $("#lblBarTotalTasks").text(totalTasks);

        agencyTaskHorizontalBarInstance = new Chart(ctx.getContext('2d'), {
            type: 'bar',
            plugins: typeof chartDataLabelsPlugin !== 'undefined' ? [chartDataLabelsPlugin] : [],
            data: {
                labels: [
                    'Not Started',
                    'Completed',
                    'Discontinued',
                    'On Track',
                    'Partially On Track',
                    'In Progress'
                ],
                datasets: [{
                    data: [
                        taskCounts.notStarted || 0,
                        taskCounts.completed || 0,
                        taskCounts.discontinued || 0,
                        taskCounts.onTrack || 0,
                        taskCounts.partiallyOnTrack || 0,
                        taskCounts.inProgress || 0
                    ],
                    backgroundColor: [
                        '#ef4444', // Red
                        '#2563eb', // Blue
                        '#8d99ae', // Gray
                        '#22c55e', // Green
                        '#f59e0b', // Amber/Orange
                        '#06b6d4'  // Cyan
                    ],
                    borderRadius: 4,
                    barPercentage: 0.75,
                    categoryPercentage: 0.85
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        grid: { color: '#f1f5f9' },
                        ticks: {
                            precision: 0,
                            font: { size: 12 }
                        }
                    },
                    y: {
                        grid: { display: false },
                        ticks: {
                            font: { size: 12, weight: '600' },
                            color: '#1e293b'
                        }
                    }
                }
            }
        });
    },

    // Reset All Counters
    resetSummaryCards: function () {
        $("#lblDepartmentCount, #lblSectorsCount, #lblActivitiesCount, #lblTasksCount, #lblBestPracticesCount").text(0);
        $("#lblSDGGoalsCount, #lblSDGTargetsCount, #lblVRThemesCount, #lblVRSubThemesCount").text(0);
        $("#lblSummaryTotalTasks, #lblSummaryCompleted, #lblSummaryInProgress, #lblSummaryDelayed, #lblSummaryNotStarted").text(0);
    },

    // 5. Print Functionality (Nonce Supported)
    printDiv: function (nonce) {
        var originalDiv = document.getElementById("printDiv");
        if (!originalDiv) {
            alert("No content found to print!");
            return;
        }

        // 1. Original Div ka ek DOM Clone banayein (taaki UI pe effect na pade)
        var cloneDiv = originalDiv.cloneNode(true);

        // 2. Original aur Cloned dono mein se sabhi Canvas elements ko find karein
        var originalCanvases = originalDiv.querySelectorAll("canvas");
        var clonedCanvases = cloneDiv.querySelectorAll("canvas");

        // 3. Har Canvas ka image data (base64) nikaalein aur Clone wale mein <img> tag se replace kar dein
        for (var i = 0; i < originalCanvases.length; i++) {
            var img = document.createElement("img");
            img.src = originalCanvases[i].toDataURL("image/png"); // Canvas ko image mein convert kiya

            // Image styling adjust karein
            img.style.width = "100%";
            img.style.maxWidth = originalCanvases[i].style.width || "100%";
            img.style.height = "auto";
            img.className = clonedCanvases[i].className;

            // Clone Div ke andar canvas ko hata kar yeh image laga dein
            clonedCanvases[i].parentNode.replaceChild(img, clonedCanvases[i]);
        }

        var printWindow = window.open('', '', 'width=1100,height=800');

        // Yahan 'printContents.innerHTML' ki jagah 'cloneDiv.innerHTML' bhejenge
        printWindow.document.write(`
        <!DOCTYPE html>
        <html>
        <head>
            <title>Activites Progress Report</title>
            <!-- Bootstrap & BoxIcons CSS Links -->
            <link rel="stylesheet" href="/css/bootstrap.min.css">
            <link href="/lib/boxicons/css/boxicons.css" rel="stylesheet" />
            <link href="/lib/boxicons/css/boxicons.min.css" rel="stylesheet" />
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
                .summary-card-container { display: flex !important; flex-wrap: wrap !important; gap: 6px !important; margin-bottom: 10px !important; }
                .summary-card { background: #ffffff !important; border: 1px solid #cbd5e1 !important; border-radius: 6px !important; padding: 6px 8px !important; display: flex !important; align-items: center !important; position: relative !important; overflow: hidden !important; flex: 1 1 calc(20% - 6px) !important; min-width: 120px !important; }
                .summary-card::after, .summary-card-wide::after { content: '' !important; position: absolute !important; top: -10px !important; right: -10px !important; width: 30px !important; height: 30px !important; border-radius: 50% !important; }
                .card-blue::after { background-color: #e0f2fe !important; }
                .card-green::after { background-color: #dcfce7 !important; }
                .card-yellow::after { background-color: #fef3c7 !important; }
                .card-red::after { background-color: #fee2e2 !important; }
                .card-purple::after { background-color: #f3e8ff !important; }

                .summary-icon { width: 26px !important; height: 26px !important; border-radius: 6px !important; display: flex !important; align-items: center !important; justify-content: center !important; font-size: 14px !important; margin-right: 6px !important; color: #fff !important; flex-shrink: 0 !important; }
                .bg-icon-blue { background: #0d6efd !important; }
                .bg-icon-yellow { background: #d97706 !important; }
                .bg-icon-colorful { background: #0284c7 !important; }

                .summary-data { display: flex !important; flex-direction: column !important; }
                .summary-value { font-size: 14px !important; font-weight: 800 !important; color: #0f172a !important; line-height: 1 !important; }
                .summary-label { font-size: 10px !important; font-weight: 700 !important; color: #475569 !important; margin-top: 2px !important; }

                .summary-card-wide { background: #ffffff !important; border: 1px solid #cbd5e1 !important; border-radius: 6px !important; padding: 6px 8px 4px 8px !important; position: relative !important; overflow: hidden !important; flex: 1 1 calc(50% - 6px) !important; min-width: 220px !important; }
                .group-wrapper { display: flex !important; align-items: center !important; justify-content: space-around !important; }
                .group-item { display: flex !important; align-items: center !important; }
                .group-divider { width: 1px !important; height: 20px !important; background-color: #cbd5e1 !important; }
                .group-footer-title { text-align: center !important; font-weight: 800 !important; font-size: 10px !important; color: #2563eb !important; margin-top: 2px !important; }

                .dept-header { background: #0b5ed7 !important; color: #fff !important; padding: 6px 10px !important; font-size: 13px !important; font-weight: bold !important; }
                .sector { background: #dbeafe !important; padding: 4px 8px !important; font-size: 12px !important; font-weight: bold !important; border-left: 4px solid #0b5ed7 !important; }
                .agency { background: #e8f5e9 !important; padding: 4px 8px !important; font-size: 11px !important; font-weight: 600 !important; border-left: 4px solid #2e7d32 !important; }

                .activity-container { margin-bottom: 8px !important; border: 1px solid #cbd5e1 !important; border-radius: 4px !important; overflow: hidden !important; background: #fff !important; }
                .activity-title-header { background: #fef3c7 !important; padding: 5px 8px !important; font-size: 12px !important; font-weight: bold !important; color: #78350f !important; border-left: 4px solid #f59e0b !important; border-bottom: 1px solid #fde68a !important; }

                .activity-meta-table, .task-table { width: 100% !important; margin-bottom: 0 !important; font-size: 10px !important; border-collapse: collapse !important; }
                .activity-meta-table th, .activity-meta-table td, .task-table th, .task-table td { padding: 3px 6px !important; line-height: 1.2 !important; }
                .activity-meta-table th { background-color: #fffbeb !important; color: #78350f !important; font-weight: 700 !important; border: 1px solid #fde68a !important; }
                .activity-meta-table td { border: 1px solid #fde68a !important; color: #334155 !important; }
                .task-table th { background: #334155 !important; color: #fff !important; text-align: center !important; font-size: 10px !important; border: 1px solid #94a3b8 !important; }
                .task-table td { border: 1px solid #cbd5e1 !important; }
                
                tr { page-break-inside: avoid !important; }

                .completed { color: #16a34a !important; font-weight: bold !important; }
                .progress { color: #d97706 !important; font-weight: bold !important; }
                .pending { color: #dc2626 !important; font-weight: bold !important; }

                .footer { margin-top: 15px !important; border-top: 1px solid #e2e8f0 !important; padding-top: 8px !important; padding-bottom: 5px !important; display: flex !important; justify-content: space-between !important; align-items: center !important; font-size: 12px !important; color: #334155 !important; background: #fff !important; }
                .footer .footer-left, .footer .footer-right { font-weight: 500 !important; }

                .overview-card { border-radius: 18px !important; background: linear-gradient(180deg, #e5871f 0%, #d97706 100%) !important; box-shadow: 0 4px 15px rgba(217, 119, 6, 0.15) !important; overflow: hidden !important; border: none !important; }
                .overview-card-header { padding: 16px 20px 12px 20px !important; color: #ffffff !important; }
                .overview-card-header h5 { font-size: 18px !important; font-weight: 700 !important; margin-bottom: 2px !important; color: #ffffff !important; }
                .overview-card-header p { font-size: 12px !important; margin-bottom: 0 !important; opacity: 0.92 !important; color: #ffffff !important; }
                .overview-card-body { background: #f8fafc !important; padding: 14px !important; }
                .chart-inner-box { background: #ffffff !important; border: 1px solid #e2e8f0 !important; border-radius: 14px !important; padding: 16px !important; min-height: 295px !important; }

                .donut-center-text { position: absolute !important; top: 50% !important; left: 50% !important; transform: translate(-50%, -50%) !important; text-align: center !important; pointer-events: none !important; }
                .donut-center-text .total-number { display: block !important; font-size: 24px !important; font-weight: 800 !important; color: #0f172a !important; line-height: 1 !important; }
                .donut-center-text .total-label { font-size: 10px !important; color: #64748b !important; font-weight: 600 !important; }

                .custom-legend-grid { display: flex !important; flex-wrap: wrap !important; justify-content: center !important; gap: 10px 18px !important; }
                .legend-item { font-size: 12px !important; font-weight: 600 !important; color: #475569 !important; display: flex !important; align-items: center !important; }
                .dot { width: 12px !important; height: 12px !important; border-radius: 50% !important; display: inline-block !important; margin-right: 6px !important; }
                .dot-completed { background-color: #2563eb !important; }
                .dot-discontinued { background-color: #5c6b73 !important; }
                .dot-notstarted { background-color: #ef4444 !important; }
                .dot-ongoing { background-color: #22c55e !important; }

                /* SDG Colors Here... */
                .icon-g1 { background-color: #e5233b } .icon-g2 { background-color: #dea73a } .icon-g3 { background-color: #4ca046 }
                .icon-g4 { background-color: #c5202e } .icon-g5 { background-color: #ef402c } .icon-g6 { background-color: #29bee2 }
                .icon-g7 { background-color: #fac315 } .icon-g8 { background-color: #a21c44 } .icon-g9 { background-color: #f26a2c }
                .icon-g10 { background-color: #dd1768 } .icon-g11 { background-color: #f99d29 } .icon-g12 { background-color: #bf8b2c }
                .icon-g13 { background-color: #407f44 } .icon-g14 { background-color: #1c97d4 } .icon-g15 { background-color: #5cba47 }
                .icon-g16 { background-color: #01699e } .icon-g17 { background-color: #18496b }

                .horibarchart { position: relative; height: 260px; width: 100%; }

                @media print {
                    @page {
                        size: auto;
                        margin: 8mm 8mm 15mm 8mm; 
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
            <!-- Yahan cloneDiv ko print karwa rahe hain, jisme canvas ki jagah <img> set hai -->
            ${cloneDiv.innerHTML}
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