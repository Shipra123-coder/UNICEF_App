var printActivity = {
    loadFullData: function (guid) {
        $.ajax({
            url: '/Management/GetFullData',
            type: 'GET',
            data: { guid: guid },
            success: function (response) {
                printActivity.BindBasicInfo(response);
                printActivity.BindDepartments(response.departments);
                printActivity.BindGoals(response.goals);
                printActivity.BindPillars(response.pillars);
                printActivity.BindNatureOfSupport(response.natureofsupport);
                printActivity.BindGeoCoverage(response.activity);
                printActivity.BindTaskDetails(response.activity)
            }
        });
    },
    BindBasicInfo: function (response) {
        var activity = response.activity;
        //Basic Information
        $("#lblActivityName").text(activity.activityName || "-");
        $("#lblStatus").text(activity.activityStatus || "-");
        $("#lblDescription").text(activity.description || "-");
        $("#lblActivityDescription").text(activity.description || "-");
        $("#lblStartDate").text(activity.activityStartDate || "-");
        $("#lblEndDate").text(activity.activityEndDate || "-");

        //Sector
        if (response.pillars != null && response.pillars.length > 0)
            $("#lblSector").text(response.pillars[0].sectorName);
        else
            $("#lblSector").text("-");

        //Agency
        if (response.agencies != null && response.agencies.length > 0) {
            var agency = $.map(response.agencies, function (x) {
                return x.agencyName;
            }).join(", ");

            $("#lblAgency").text(agency);
        }
        else {
            $("#lblAgency").text("-");
        }
    },
    BindDepartments: function (departments) {
        var html = "";
        var nodal = departments.find(x => x.isNodal);
        var others = departments.filter(x => !x.isNodal);
        html += "<tr>";
        html += "<td>";
        html += "<div class='nodal-department'>";
        html += nodal ? nodal.departmentName : "-";
        html += "</div>";
        html += "</td>";
        html += "<td>";
        if (others.length > 0) {
            html += "<table class='table table-sm table-bordered inner-department-table'>";
            html += "<thead>";
            html += "<tr>";
            html += "<th width='10%'>Sr No</th>";
            html += "<th width='90%'>Department Name</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            $.each(others, function (i, item) {
                html += "<tr>";
                html += "<td>" + (i + 1) + "</td>";
                html += "<td>" + item.departmentName + "</td>";
                html += "</tr>";
            });
            html += "</tbody>";
            html += "</table>";
        }
        else {
            html += "No Associated Department";
        }

        html += "</td>";
        html += "</tr>";
        $("#tblDepartmentBody").html(html);
    },
    BindGoals: function (goals) {
        let html = "";
        let grouped = {};
        $.each(goals, function (i, x) {
            if (!grouped[x.goalId]) {
                grouped[x.goalId] = {
                    goalName: x.goalName,
                    targets: []
                };
            }
            grouped[x.goalId].targets.push(x);
        });
        $.each(grouped, function (key, value) {
            html += `<tr>
                <td>${value.goalName.split(':')[0]}</td>
                <td><strong>${value.goalName}</strong></td>
                <td>
                <ul class="sub-theme-list">`;
            $.each(value.targets, function (i, t) {
                html += `<li>${t.targetName}</li>`;
            });
            html += `</ul></td></tr>`;
        });
        $("#tblGoals tbody").html(html);

    },
    BindPillars: function (data) {
        let html = "";
        let grouped = {};
        $.each(data, function (i, x) {
            if (!grouped[x.pillarId]) {
                grouped[x.pillarId] = {
                    pillarName: x.pillarName,
                    sectors: []
                };
            }
            grouped[x.pillarId].sectors.push(x.sectorName);
        });

        $.each(grouped, function (k, v) {
            html += `<tr>
                    <td><strong>${v.pillarName}</strong></td>
                    <td>
                    <ul class="sub-theme-list">`;

            $.each(v.sectors, function (i, s) {
                html += `<li>${s}</li>`;
            });
            html += `</ul></td>
                </tr>`;
        });
        $("#tblPillars tbody").html(html);
    },
    BindNatureOfSupport: function (data) {
        let html = "";
        let grouped = {};
        $.each(data, function (i, x) {
            if (!grouped[x.supportId]) {
                grouped[x.supportId] = {
                    supportName: x.supportName,
                    details: []
                };
            }
            grouped[x.supportId].details.push(x.detailName);
        });
        $.each(grouped, function (k, v) {
            html += `<tr>
            <td><strong>${v.supportName}</strong></td>
            <td><ul class="sub-theme-list">`;
            $.each(v.details, function (i, d) {
                html += `<li>${d}</li>`;
            });
            html += `</ul></td>
        </tr>`;
        });
        $("#tblSupport tbody").html(html);
    },
    
    BindGeoCoverage: function (activity) {

    let html = "";

    if (activity.hasSubActivity) {

        $.each(activity.subActivities, function (i, sub) {

            html += `<h5 class="card-titlesubactivity">${sub.subActivityName}</h5>`;

            html += printActivity.BindGeoTable(sub.tasks);

        });

    }
    else {

        html += printActivity.BindGeoTable(activity.directTasks);

    }

    $("#GeoCoverage").html(html);
},
    BindGeoTable: function (tasks) {

    let html = "";

    $.each(tasks, function (j, task) {

        html += `<table class="table table-bordered">
                    <tr>
                        <th colspan="2">
                            Task : ${task.taskName}
                        </th>
                    </tr>
                    <tr>
                        <th width="20%">Geo Level</th>
                        <th>Details</th>
                    </tr>`;

        if (task.geoLevelList != null && task.geoLevelList.length > 0) {

            let states = [];
            let districts = [];
            let blocks = {};
            let cities = {};

            $.each(task.geoLevelList, function (k, g) {

                if (g.geoLevel == "State") {
                    states.push("Rajasthan");
                }

                if (g.geoLevel == "District") {
                    districts.push(g.districtName);
                }

                if (g.geoLevel == "Block") {

                    if (!blocks[g.districtName]) {
                        blocks[g.districtName] = [];
                    }

                    blocks[g.districtName].push(g.blockName);
                }

                if (g.geoLevel == "City") {

                    if (!cities[g.districtName]) {
                        cities[g.districtName] = [];
                    }

                    cities[g.districtName].push(g.cityName);
                }

            });

            if (states.length > 0) {
                html += `<tr>
                            <td><strong>State</strong></td>
                            <td>${states.join(", ")}</td>
                         </tr>`;
            }

            if (districts.length > 0) {
                html += `<tr>
                            <td><strong>District</strong></td>
                            <td>${districts.join(", ")}</td>
                         </tr>`;
            }

            $.each(blocks, function (district, blockList) {

                html += `<tr>
                            <td><strong>Block</strong></td>
                            <td><strong>${district} → </strong>${blockList.join(", ")}</td>
                         </tr>`;
            });

            $.each(cities, function (district, cityList) {

                html += `<tr>
                            <td><strong>City</strong></td>
                            <td><strong>${district} → </strong>${cityList.join(", ")}</td>
                         </tr>`;
            });
        }

        html += `</table>`;

    });

    return html;
},
    BindTaskDetails: function (activity) {

        let html = "";
        let srNo = 1;

        //==========================
        // With Sub Activity
        //==========================
        if (activity.hasSubActivity == true &&
            activity.subActivities != null &&
            activity.subActivities.length > 0) {

            $.each(activity.subActivities, function (i, subActivity) {

                if (subActivity.tasks != null && subActivity.tasks.length > 0) {

                    $.each(subActivity.tasks, function (j, task) {

                        html += `<tr>
                                <td>${srNo++}</td>

                                <td>
                                    <strong>${task.taskName}</strong>
                                    <br/>
                                    <small class="text-primary">
                                        Sub Activity : ${subActivity.subActivityName}
                                    </small>
                                </td>

                                <td>
                                    ${task.taskStartDate || "-"}
                                    <strong> To </strong>
                                    ${task.taskEndDate || "-"}
                                </td>

                                <td>
                                    ${task.taskDetailDescription || "-"}
                                </td>

                                <td>
                                    ${task.tracking != null ? task.tracking.Status : "-"}
                                </td>

                            </tr>`;

                    });

                }

            });

        }

        //==========================
        // Direct Task
        //==========================
        else if (activity.directTasks != null &&
            activity.directTasks.length > 0) {

            $.each(activity.directTasks, function (i, task) {

                html += `<tr>

                        <td>${srNo++}</td>

                        <td>${task.taskName}</td>

                        <td>
                            ${task.taskStartDate || "-"}
                            <strong> To </strong>
                            ${task.taskEndDate || "-"}
                        </td>

                        <td>
                            ${task.taskDetailDescription || "-"}
                        </td>

                        <td>
                            ${task.tracking != null ? task.tracking.status : "-"}
                        </td>

                    </tr>`;

            });

        }

        if (html == "") {

            html = `<tr>
                    <td colspan="5" class="text-center">
                        No Task Available
                    </td>
                </tr>`;
        }

        $("#tblTaskBody").html(html);

    },
    printDivnew :function () {

    var divContents = document.getElementById("printDiv").innerHTML;

    // Page ki saari CSS copy karega
    var styles = "";
    document.querySelectorAll("style, link[rel='stylesheet']").forEach(function (node) {
        styles += node.outerHTML;
    });

    var printWindow = window.open("", "", "width=1200,height=800");

    printWindow.document.write(`
        <html>
        <head>
            <title>Print</title>
            ${styles}
        </head>
        <body>
            ${divContents}
        </body>
        </html>
    `);

    printWindow.document.close();

    printWindow.onload = function () {
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    };
},
    printDiv :function (nonce) {

    var divContents = document.getElementById("printDiv").innerHTML;

    var printWindow = window.open('', '', 'width=1000,height=700');

    printWindow.document.write(`
        <html>
        <head>
            <title>Print</title>

            <link rel="stylesheet" href="/css/bootstrap.min.css">

            <style nonce="${nonce}">
               body{
    font-family: Arial, sans-serif;
    font-size:14px;
    margin:0;
    padding:10px;
}

                img{
                    max-width:100%;
                    height:auto;
                }

                table{
                    width:100%;
                    border-collapse:collapse;
                    margin-bottom:0px !important;
                }
                table,th{
                    background:#d2d2d2 !important;
                    border:1px solid #ddd;
                }
                table,td{
                    border:1px solid #ddd;
                }
                *{
                     -webkit-print-color-adjust: exact !important;
                     print-color-adjust: exact !important;
                 }
                 
                 .card-title{
                     background:#d2d2d2 !important;
                     color:#000 !important;
                     padding: 10px !important;
                     font-weight: 700 !important;
                 } 
                 .description-text {
        border: 1px solid #dbeafe !important;
        padding: 10px !important;
    }
    .card-titlesubactivity {
        padding: 10px !important;
        font-weight: 700 !important;
        border: 1px solid #dbeafe !important;
        margin-bottom: 0px !important;
    }
    /* Header */

.header-top{
    background:#1f3b73 !important;
    color:#fff !important;
    text-align:center;
    padding:12px 15px !important;
}

.header-top h1{
    margin:0;
    font-size:16px !important;
    font-weight:700;
    line-height:22px;
    text-transform:uppercase;
}

.header-top p{
    margin:6px 0;
    font-size:12px !important;
    line-height:18px;
}

.report-title{
    margin-top:10px;
    font-size:15px !important;
    font-weight:700;
    line-height:20px;
    text-transform:uppercase;
}

.header-bottom{
    background:#3c78b5 !important;
    color:#fff !important;
    text-align:center;
    padding:10px 15px !important;
}

.header-bottom h2{
    margin:0;
    font-size:15px !important;
    font-weight:700;
}

.header-bottom p{
    margin:5px 0 0;
    font-size:12px !important;
    line-height:18px;
}

/* Ye bahut important hai */

h1,h2,h3,h4,h5,h6{
    margin:0;
    font-weight:700;
    color:inherit !important;
}

*{
    -webkit-print-color-adjust:exact !important;
    print-color-adjust:exact !important;
}
            </style>
        </head>

        <body>

            ${divContents}

        </body>
        </html>
    `);

    printWindow.document.close();

    printWindow.focus();

    setTimeout(function () {
        printWindow.print();
        printWindow.close();
    }, 500);
},
}