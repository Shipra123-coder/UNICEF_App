// Global Chart instances taaki memory leaks aur redraw collisions na ho
var activitiesDonutInstance = null;
var taskHorizontalBarInstance = null;

// Bar ke right-edge ke andar text likhne ke liye custom Chart.js plugin
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
                    ctx.fillStyle = '#000';
                    ctx.font = 'bold 11px sans-serif';
                    ctx.textAlign = 'right';
                    ctx.textBaseline = 'middle';
                    ctx.fillText(val, bar.x + 15, bar.y);
                    ctx.restore();
                }
            });
        });
    }
};
var printDepartmentReport = {
    // 1. Department Filter aur Data Load Function
    loadDepartmentWiseData: function () {
        //if (!deptId || deptId === "-1") {
        //    alert("Please select a department!");
        //    return;
        //}

        // Selected Department Name ko UI header me set karein
        var selectedDeptName = $("#Department option:selected").text();
        var DepartmentId = $("#Department").val();
        var AgencyId = $("#Agency").val();
        var SectorId = $("#Sector").val();
        //$("#lblDepartmentHeaderName").text(selectedDeptName);
        $("#lblDepartmentHeaderName_head").text(selectedDeptName);

        // Loader start karein
        $("#activityReportContainer").html('<div class="text-center my-4"><div class="spinner-border text-primary" role="status"></div><p class="mt-2">Loading Report Data...</p></div>');

        // AJAX Call - Backend Controller Endpoint
        $.ajax({
            url: '/AdminReport/ReportAnalize',
            type: 'GET',
            data: { departmentId: DepartmentId, agencyId: AgencyId, sectorId: SectorId },
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

                    if (data.filterDepartmentName) {
                        //$("#lblDepartmentHeaderName").text(data.filterDepartmentName);
                        $("#lblDepartmentHeaderName_head").text(data.filterDepartmentName);
                    }
                }

                // Grouped Report Render karein aur Summary pass karein
                printDepartmentReport.renderReportData(activities, summary);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching report data:", error);
                $("#activityReportContainer").html('<div class="alert alert-danger text-center">An error occurred while loading data. Please try again.</div>');
                printDepartmentReport.resetSummaryCards();
            }
        });
    },

    // 2. Main Render Function (Grouping + Data Table + Dual Charts)
    renderReportData: function (activities, summaryData) {
        var $container = $("#activityReportContainer");
        $container.empty();

        if (!activities || activities.length === 0) {
            $container.html('<div class="alert alert-warning text-center fw-bold">No activity records found for the selected department.</div>');
            printDepartmentReport.resetSummaryCards();
            return;
        }

        var totalActivities = activities.length;
        var totalTasksCount = 0;

        // Counters for Activities (Donut Chart)
        var activityStatusCounts = {
            completed: 0,
            discontinued: 0,
            notStarted: 0,
            ongoing: 0
        };

        // Counters for Tasks (Horizontal Bar Chart)
        var taskStatusCounts = {
            notStarted: 0,
            completed: 0,
            discontinued: 0,
            onTrack: 0,
            partiallyOnTrack: 0,
            inProgress: 0
        };

        var sectorsSet = new Set();
        var agenciesSet = new Set();
        var groupedData = {};

        // Loop over activities to group data and count activity statuses
        $.each(activities, function (index, act) {
            var sectorName = act.UNSectorName ? act.UNSectorName.trim() : 'Other Sector';
            var agencyName = (act.agency && act.agency.AgencyName) ? act.agency.AgencyName.trim() : 'N/A Agency';

            sectorsSet.add(sectorName);
            if (agencyName !== 'N/A Agency') agenciesSet.add(agencyName);

            if (!groupedData[sectorName]) groupedData[sectorName] = {};
            if (!groupedData[sectorName][agencyName]) groupedData[sectorName][agencyName] = [];

            groupedData[sectorName][agencyName].push(act);

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

        var activityCounter = 0;

        // Render Grouped Activities Table
        $.each(groupedData, function (sectorName, agenciesObj) {
            $container.append(`<div class="sector"><i class="bx bx-category"></i> Sector : ${sectorName}</div>`);

            $.each(agenciesObj, function (agencyName, agencyActivities) {
                $container.append(`<div class="agency"><i class="bx bx-building"></i> Agency : ${agencyName}</div>`);

                $.each(agencyActivities, function (aIdx, act) {
                    activityCounter++;

                    var nodalDeptName = (act.nodalDepartment && act.nodalDepartment.DepartmentName) ? act.nodalDepartment.DepartmentName.trim() : 'N/A';

                    var assocDeptsBadges = [];
                    if (act.associatedDepartments && act.associatedDepartments.length > 0) {
                        $.each(act.associatedDepartments, function (i, assoc) {
                            if (assoc.DepartmentName) {
                                assocDeptsBadges.push('<span class="badge bg-secondary me-1">' + assoc.DepartmentName.trim() + '</span>');
                            }
                        });
                    }
                    var assocDeptsText = assocDeptsBadges.length > 0 ? assocDeptsBadges.join(' ') : 'None';

                    // 🌟 1. SDG GOALS BADGES WITH DYNAMIC COLORS
                    var sdgGoalsBadges = [];
                    if (act.sdgGoals && act.sdgGoals.length > 0) {
                        $.each(act.sdgGoals, function (i, g) {
                            var goalNum = g.DisplayNumber ? parseInt(g.DisplayNumber, 10) : 0;
                            var goalClass = (goalNum >= 1 && goalNum <= 17) ? ('icon-g' + goalNum) : 'bg-success';
                            var textColor = (goalNum === 7) ? 'text-dark' : 'text-white'; // Goal 7 (Yellow) par readable text

                            var goalLabel = g.DisplayNumber ? 'Goal ' + g.DisplayNumber + ': ' + g.GoalName.trim() : g.GoalName.trim();
                            sdgGoalsBadges.push('<span class="badge ' + goalClass + ' ' + textColor + ' me-1 mb-1">' + goalLabel + '</span>');
                        });
                    }
                    var sdgGoalsText = sdgGoalsBadges.length > 0 ? sdgGoalsBadges.join(' ') : '-';


                    // 🌟 2. SDG TARGETS BADGES (Target 3.4 -> Goal 3 ka color)
                    var sdgTargetsBadges = [];
                    if (act.sdgTargets && act.sdgTargets.length > 0) {
                        $.each(act.sdgTargets, function (i, t) {
                            var targetLabel = t.DisplayNumber ? 'Target ' + t.DisplayNumber : t.TargetName.trim();

                            // Target Number (jaise "3.4", "10.b", "11.1") se Goal Number nikalna:
                            var targetGoalNum = 0;
                            if (t.DisplayNumber) {
                                var dotIndex = t.DisplayNumber.indexOf('.');
                                var rawNum = dotIndex !== -1 ? t.DisplayNumber.substring(0, dotIndex) : t.DisplayNumber;
                                targetGoalNum = parseInt(rawNum, 10);
                            } else if (t.GoalId || t.GoalNumber) {
                                // Agar backend se direct GoalId / GoalNumber aa raha ho
                                targetGoalNum = parseInt(t.GoalId || t.GoalNumber, 10);
                            }

                            // Color Class & Text Color Mapping
                            var targetClass = (targetGoalNum >= 1 && targetGoalNum <= 17) ? ('icon-g' + targetGoalNum) : 'bg-info';
                            var textColor = (targetGoalNum === 7) ? 'text-dark' : 'text-white';

                            sdgTargetsBadges.push('<span class="badge ' + targetClass + ' ' + textColor + ' me-1 mb-1" title="' + (t.TargetName || '') + '">' + targetLabel + '</span>');
                        });
                    }
                    var sdgTargetsText = sdgTargetsBadges.length > 0 ? sdgTargetsBadges.join(' ') : '-';

                    var pillarBadges = [];
                    if (act.pillars && act.pillars.length > 0) {
                        $.each(act.pillars, function (i, p) {
                            if (p.PillarName) {
                                pillarBadges.push('<span class="badge bg-primary me-1 mb-1">' + p.PillarName.trim() + '</span>');
                            }
                        });
                    }
                    var pillarText = pillarBadges.length > 0 ? pillarBadges.join(' ') : '-';

                    var subPillarBadges = [];
                    if (act.subPillars && act.subPillars.length > 0) {
                        $.each(act.subPillars, function (i, sp) {
                            if (sp.SubPillarName) {
                                subPillarBadges.push('<span class="badge bg-secondary me-1 mb-1">' + sp.SubPillarName.trim() + '</span>');
                            }
                        });
                    }
                    var subPillarText = subPillarBadges.length > 0 ? subPillarBadges.join(' ') : '-';

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

                    var activityHtml = `
                        <div class="activity-container">
                            <div class="activity-title-header d-flex justify-content-between align-items-center">
                                <span><strong>Activity ${activityCounter}:</strong> ${act.ActivityName || 'N/A'} (${act.ShortName || ''})</span>
                                <span class="badge bg-primary">${act.ActivityStatus || 'Ongoing'}</span>
                            </div>
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

                    if (allTasks.length > 0) {
                        activityHtml += `
                            <table class="table table-bordered table-striped task-table mb-0">
                                <thead>
                                    <tr>
                                        <th width="5%">S.No</th>
                                        <th width="20%">Details of Task</th>
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

                            // Categorize task status for Bar Chart
                            if (statusLower === 'completed') {
                                statusClass = 'completed';
                                taskStatusCounts.completed++;
                            } else if (statusLower === 'discontinued') {
                                statusClass = 'delayed';
                                taskStatusCounts.discontinued++;
                            } else if (statusLower === 'on track') {
                                statusClass = 'completed';
                                taskStatusCounts.onTrack++;
                            } else if (statusLower === 'partially on track') {
                                statusClass = 'progress';
                                taskStatusCounts.partiallyOnTrack++;
                            } else if (statusLower === 'in progress') {
                                statusClass = 'progress';
                                taskStatusCounts.inProgress++;
                            } else {
                                statusClass = 'not-started';
                                taskStatusCounts.notStarted++;
                            }

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

                        activityHtml += `</tbody></table>`;
                    } else {
                        activityHtml += `<div class="p-3 text-center text-muted border-top">No tasks are mapped for this activity.</div>`;
                    }

                    activityHtml += `</div>`;
                    $container.append(activityHtml);
                });
            });
        });

        // Top Summary Metric Cards
        if (summaryData) {
            $("#lblAgenciesCount").text(summaryData.agencyCount ?? 0);
            $("#lblSectorsCount").text(summaryData.sectorCount ?? 0);
            $("#lblActivitiesCount").text(summaryData.activityCount ?? 0);
            $("#lblTasksCount").text(summaryData.taskCount ?? 0);
            $("#lblBestPracticesCount").text(summaryData.bestPracticeCount ?? 0);
            $("#lblSDGGoalsCount").text(summaryData.goalCount ?? 0);
            $("#lblSDGTargetsCount").text(summaryData.targetCount ?? 0);
            $("#lblVRThemesCount").text(summaryData.pillarCount ?? 0);
            $("#lblVRSubThemesCount").text(summaryData.subPillarCount ?? 0);
        } else {
            $("#lblActivitiesCount").text(totalActivities);
            $("#lblTasksCount").text(totalTasksCount);
            $("#lblSectorsCount").text(sectorsSet.size);
            $("#lblAgenciesCount").text(agenciesSet.size);
            $("#lblBestPracticesCount").text(0);
            $("#lblSDGGoalsCount").text(sectorsSet.size);
            $("#lblSDGTargetsCount").text(totalActivities * 2);
            $("#lblVRThemesCount").text(sectorsSet.size > 0 ? 1 : 0);
            $("#lblVRSubThemesCount").text(sectorsSet.size > 0 ? 2 : 0);
        }

        // Render Donut & Bar Charts
        printDepartmentReport.renderActivitiesDonutChart(activityStatusCounts);
        printDepartmentReport.renderTaskHorizontalBarChart(taskStatusCounts);
    },

    // 3. Render Donut Chart (Activities Wise Status)
    // Function jo aapke object ko accept karega
    renderActivitiesDonutChart: function (counts) {
        if (typeof Chart === 'undefined') return;

        var ctx = document.getElementById('activitiesDonutChart');
        if (!ctx) return;

        // Purana instance destroy karein taaki chart overlap na ho
        if (activitiesDonutInstance) {
            activitiesDonutInstance.destroy();
        }

        // Safe integer values nikalna (NaN/undefined issue se bachne ke liye)
        var completed = Number(counts.completed) || 0;
        var discontinued = Number(counts.discontinued) || 0;
        var notStarted = Number(counts.notStarted) || 0;
        var ongoing = Number(counts.ongoing) || 0;

        var chartData = [completed, discontinued, notStarted, ongoing];
        var labels = ['Completed', 'Discontinued', 'Not Started', 'Ongoing'];
        var colors = [
            '#2563eb', // Completed (Blue)
            '#64748b', // Discontinued (Slate/Grey)
            '#ef4444', // Not Started (Red)
            '#22c55e'  // Ongoing (Green)
        ];

        // ==========================================
        // 1. PLUGIN: Center me Total Activities Text
        // ==========================================
        const centerTextPlugin = {
            id: 'centerTextPlugin',
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

                // Total Count Number
                ctx.fillStyle = '#0f172a';
                ctx.font = 'bold 30px Arial, sans-serif';
                ctx.fillText(total, centerX, centerY - 10);

                // Subtitle
                ctx.fillStyle = '#64748b';
                ctx.font = '500 13px Arial, sans-serif';
                ctx.fillText('Total Activities', centerX, centerY + 16);

                ctx.restore();
            }
        };

        // ==========================================
        // 2. PLUGIN: Donut Slices par Numbers Draw
        // ==========================================
        const sliceValuePlugin = {
            id: 'sliceValuePlugin',
            afterDatasetsDraw(chart) {
                const { ctx } = chart;
                const meta = chart.getDatasetMeta(0);

                meta.data.forEach((arc, index) => {
                    const value = chart.data.datasets[0].data[index];
                    if (!value || value === 0) return; // 0 hone par slice pe number hide rahega

                    // Slice ke center ka coordinate nikalna
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

        // ==========================================
        // 3. Create Donut Chart Instance
        // ==========================================
        activitiesDonutInstance = new Chart(ctx.getContext('2d'), {
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
                cutout: '70%', // Donut thickness
                plugins: {
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            pointStyle: 'circle',
                            padding: 16,
                            font: {
                                size: 13,
                                weight: '600'
                            },
                            // Legend ke text ke sath count show karne ke liye (e.g., "Completed (2)")
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

        if (taskHorizontalBarInstance) {
            taskHorizontalBarInstance.destroy();
        }

        var totalTasks = (taskCounts.notStarted || 0) +
            (taskCounts.completed || 0) +
            (taskCounts.discontinued || 0) +
            (taskCounts.onTrack || 0) +
            (taskCounts.partiallyOnTrack || 0) +
            (taskCounts.inProgress || 0);

        $("#lblBarTotalTasks").text(totalTasks);

        taskHorizontalBarInstance = new Chart(ctx.getContext('2d'), {
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
                        '#ef4444', // Red (Not Started)
                        '#2563eb', // Blue (Completed)
                        '#8d99ae', // Gray (Discontinued)
                        '#22c55e', // Green (On Track)
                        '#f59e0b', // Amber/Orange (Partially On Track)
                        '#06b6d4'  // Cyan (In Progress)
                    ],
                    borderRadius: 4,
                    barPercentage: 0.75,     // Bar ki motai card space ke anusaar
                    categoryPercentage: 0.85 // Bars ke beech ka spacing adjust karega
                }]
            },
            options: {
                indexAxis: 'y', // Horizontal
                responsive: true,
                maintainAspectRatio: false, // Parent wrapper ki full height lega
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
        $("#lblAgenciesCount, #lblSectorsCount, #lblActivitiesCount, #lblTasksCount, #lblBestPracticesCount").text(0);
        $("#lblSDGGoalsCount, #lblSDGTargetsCount, #lblVRThemesCount, #lblVRSubThemesCount").text(0);
        //$("#lblSummaryTotalTasks, #lblSummaryCompleted, #lblSummaryInProgress, #lblSummaryDelayed, #lblSummaryNotStarted").text(0);
    },

    // 5. Print Functionality (Nonce Supported)
    // 🌟 COMPACT PRINT FUNCTION (Extra Space & Blank Pages Fixed)
    printDiv: function (nonce) {
        var printContents = document.getElementById("printDiv");
        if (!printContents) {
            alert("No content found to print!");
            return;
        }

        var $clonedContent = $(printContents).clone();

        // 1. Convert Donut Canvas to Responsive Contained Image
        var donutCanvas = document.getElementById("activitiesDonutChart");
        if (donutCanvas && activitiesDonutInstance) {
            var donutImg = donutCanvas.toDataURL("image/png");
            var $clonedDonut = $clonedContent.find("#activitiesDonutChart");
            if ($clonedDonut.length > 0) {
                // Container ko flex center kiya aur img ko strictly max-width: 100% diya
                $clonedDonut.replaceWith(`
                <div class="divimg">
                    <img src="${donutImg}" class="imggraph" />
                </div>
            `);
            }
        }

        // 2. Convert Horizontal Bar Canvas to Responsive Contained Image
        var barCanvas = document.getElementById("taskHorizontalBarChart");
        if (barCanvas && taskHorizontalBarInstance) {
            var barImg = barCanvas.toDataURL("image/png");
            var $clonedBar = $clonedContent.find("#taskHorizontalBarChart");
            if ($clonedBar.length > 0) {
                // Bar chart ko exact bounding box mein constrain kiya
                $clonedBar.replaceWith(`
                <div class="divimg">
                    <img src="${barImg}" class="imggraph" />
                </div>
            `);
            }
        }

        var printWindow = window.open('', '', 'width=1100,height=800');

        printWindow.document.write(`
        <!DOCTYPE html>
        <html>
        <head>
            <title>Activities Progress Report</title>
            
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
                }

                .sector {
                    background: #dbeafe !important;
                    padding: 4px 8px !important;
                    font-size: 20px !important;
                    font-weight: bold !important;
                    border-left: 4px solid #0b5ed7 !important;                  
                    
                }

                .agency {
                    background: #e8f5e9 !important;
                    padding: 4px 8px !important;
                    font-size: 17px !important;
                    font-weight: 600 !important;
                    border-left: 4px solid #2e7d32 !important;                   
                }

                /* COMPACT ACTIVITY CONTAINERS */
                .activity-container {
                    
                    
                    border: 1px solid #cbd5e1 !important;
                    
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
                /* Card Container Styling */
.overview-card {
    border-radius: 18px !important;
    background: linear-gradient(180deg, #e5871f 0%, #d97706 100%) !important;
    box-shadow: 0 4px 15px rgba(217, 119, 6, 0.15) !important;
    overflow: hidden !important;
    border: none !important;
}

.overview-card-header {
    padding: 16px 20px 12px 20px !important;
    color: #ffffff !important;
}

.overview-card-header h5 {
    font-size: 18px !important;
    font-weight: 700 !important;
    margin-bottom: 2px !important;
    color: #ffffff !important;
}

.overview-card-header p {
    font-size: 12px !important;
    margin-bottom: 0 !important;
    opacity: 0.92 !important;
    color: #ffffff !important;
}

.overview-card-body {
    background: #f8fafc !important;
    padding: 14px !important;
}

.chart-inner-box {
    background: #ffffff !important;
    border: 1px solid #e2e8f0 !important;
    border-radius: 14px !important;
    padding: 16px !important;
    min-height: 295px !important;
}

/* Donut Center Count & Label */
.donut-chart-container {
    position: relative !important;
    height: 180px !important;
    width: 100% !important;
}

.donut-center-text {
    position: absolute !important;
    top: 50% !important;
    left: 50% !important;
    transform: translate(-50%, -50%) !important;
    text-align: center !important;
    pointer-events: none !important;
}

.donut-center-text .total-number {
    display: block !important;
    font-size: 24px !important;
    font-weight: 800 !important;
    color: #0f172a !important;
    line-height: 1 !important;
}

.donut-center-text .total-label {
    font-size: 10px !important;
    color: #64748b !important;
    font-weight: 600 !important;
}

/* Custom Dots Legend */
.custom-legend-grid {
    display: flex !important;
    flex-wrap: wrap !important;
    justify-content: center !important;
    gap: 10px 18px !important;
}

.legend-item {
    font-size: 12px !important;
    font-weight: 600 !important;
    color: #475569 !important;
    display: flex !important;
    align-items: center !important;
}

.dot {
    width: 12px !important;
    height: 12px !important;
    border-radius: 50% !important;
    display: inline-block !important;
    margin-right: 6px !important;
}

.dot-completed { background-color: #2563eb !important; }
.dot-discontinued { background-color: #5c6b73 !important; }
.dot-notstarted { background-color: #ef4444 !important; }
.dot-ongoing { background-color: #22c55e !important; }

.divimg{
    display: flex; justify-content: center; align-items: center; width: 100%; height: 160px; overflow: hidden;
}
.imggraph{
    max-width: 100% !important; max-height: 100% !important; height: auto !important; width: auto !important; object-fit: contain !important; display: block;
}
.icon-g1 {
        background-color: #e5233b
    }

    .icon-g2 {
        background-color: #dea73a
    }

    .icon-g3 {
        background-color: #4ca046
    }

    .icon-g4 {
        background-color: #c5202e
    }

    .icon-g5 {
        background-color: #ef402c
    }

    .icon-g6 {
        background-color: #29bee2
    }

    .icon-g7 {
        background-color: #fac315
    }

    .icon-g8 {
        background-color: #a21c44
    }

    .icon-g9 {
        background-color: #f26a2c
    }

    .icon-g10 {
        background-color: #dd1768
    }

    .icon-g11 {
        background-color: #f99d29
    }

    .icon-g12 {
        background-color: #bf8b2c
    }

    .icon-g13 {
        background-color: #407f44
    }

    .icon-g14 {
        background-color: #1c97d4
    }

    .icon-g15 {
        background-color: #5cba47
    }

    .icon-g16 {
        background-color: #01699e
    }

    .icon-g17 {
        background-color: #18496b
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
            ${$clonedContent.html()}
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