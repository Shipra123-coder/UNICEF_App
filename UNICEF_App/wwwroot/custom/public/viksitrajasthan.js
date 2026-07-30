var viksit = {
    loadCoverPagesByPillers: function () {
        var ViksitId = $('#hiddenViksitId').val();

        $.ajax({
            url: `/ViksitRajasthan/GetCoverPagesByViksit?viksitId=${ViksitId}`,
            type: 'GET',
            success: function (response) {
                // Dynamic Count
                $('#practiceCount').text(`${response.length} Results Found`);
                let html = '';
                if (response && response.length > 0) {
                    $.each(response, function (i, item) {
                        html += `
                    <div class="col-lg-4 col-md-6">
                        <div class="practice-card">

                            <div class="practice-image-box">
                                <img src="${item.coverImageUrl || '/images/no-image.png'}"
                                     class="practice-image"
                                     alt="${item.headingTitle || ''}">
                            </div>
                            <div class="practice-body">
                                <span class="practice-sector">
                                    ${item.sectorName || ''}
                                </span>
                                <h3 class="practice-title">
                                    ${item.headingTitle || ''}
                                </h3>
                                <p class="practice-desc">
                                    ${item.descriptionNotes || ''}
                                </p>
                                <div class="practice-footer">
                                    <span>
                                        ${item.locationName || ''}
                                    </span>
                                    <a href="/Home/BestPrecticesDetails?id=${item.coverPageId}"
                                       class="read-more-btn">
                                        Read More
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>`;
                    });
                }
                else {
                    html = `
                <div class="col-12">
                    <div class="alert alert-info text-center">
                        No Best Practices Found.
                    </div>
                </div>`;
                }

                $('#coverPageContainer').html(html);
            },
            error: function () {

                $('#coverPageContainer').html(`
                <div class="col-12">
                    <div class="alert alert-danger text-center">
                        Failed to load data.
                    </div>
                </div>
            `);
            }
        });
    },
    //loadDepartmentActivityList: function (viksitId) {
    //    $.ajax({
    //        url: `/ViksitRajasthan/GetDepartmentActivityList?viksitId=${viksitId}`,
    //        type: 'GET',
    //        success: function (response) {
    //            $('#departmentAccordion').html();
    //            $('#title').html('Department Wise Activities');
    //            console.log(response);
    //            let groupedDepartments = {};
    //            // =========================================
    //            // GROUP DATA
    //            // =========================================
    //            $.each(response, function (i, item) {
    //                if (!groupedDepartments[item.departmentId]) {
    //                    groupedDepartments[item.departmentId] = {
    //                        departmentName: item.departmentName,
    //                        activities: []
    //                    };
    //                }
    //                groupedDepartments[item.departmentId]
    //                    .activities.push(item);
    //            });
    //            // =========================================
    //            // HTML START
    //            // =========================================

    //            let html = '';
    //            let deptIndex = 1;
    //            // =========================================
    //            // LOOP DEPARTMENT
    //            // =========================================

    //            $.each(groupedDepartments, function (deptId, deptData) {

    //                html += `
    //            <div class="accordion-item">
    //                <h2 class="accordion-header"
    //                    id="headingDept${deptIndex}">
    //                    <button class="accordion-button
    //                                   ${deptIndex != 1 ? 'collapsed' : ''}"
    //                            type="button"
    //                            data-bs-toggle="collapse"
    //                            data-bs-target="#collapseDept${deptIndex}">
    //                        ${deptData.departmentName}
    //                         <div class="d-flex
    //                                    align-items-right
    //                                    w-100">
    //                        <span class="dept-count-badge dept-count-badge ms-auto">
    //                            ${deptData.activities.length} Activities
    //                        </span>
    //                        </div>
    //                    </button>
    //                </h2>
    //                <div id="collapseDept${deptIndex}"
    //                     class="accordion-collapse collapse
    //                     ${deptIndex == 1 ? 'show' : ''}"
    //                     data-bs-parent="#departmentAccordion">
    //                    <div class="accordion-body">
    //                        <div class="table-responsive">
    //                            <table class="table activity-table">
    //                                <thead>
    //                                    <tr>
    //                                        <th width="5%">
    //                                            #
    //                                        </th>
    //                                        <th>
    //                                            Activity Name
    //                                        </th>
    //                                        <th>
    //                                            Sector
    //                                        </th>
    //                                        <th>
    //                                            Nodal Department
    //                                        </th>
    //                                         <th>
    //                                            Associated Department
    //                                        </th>
    //                                        <th>Agency</th>
    //                                        <th>
    //                                            Status
    //                                        </th>
    //                                        <th>
    //                                            Action
    //                                        </th>
    //                                    </tr>
    //                                </thead>
    //                                <tbody>
    //            `;



    //                // =========================================
    //                // LOOP ACTIVITIES
    //                // =========================================
    //                $.each(deptData.activities,
    //                    function (index, activity) {
    //                        html += `
    //                    <tr>
    //                        <td>
    //                            ${index + 1}
    //                        </td>
    //                        <td>
    //                            <strong>
    //                                ${activity.activityName}
    //                            </strong>
    //                        </td>
    //                        <td>
    //                            ${activity.unSectorName ?? ''}
    //                        </td>
    //                        <td>
    //                            ${activity.departmentName ?? ''}
    //                        </td>
    //                        <td>
    //                            ${activity.associatedDepartments ?? ''}
    //                        </td>
    //                        <td>
    //                            ${activity.agencyName ?? ''}
    //                        </td>
    //                        <td>
    //                            ${activity.activityStatus ?? ''}
    //                        </td>
    //                        <td>
    //                            <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
    //                               class="table-view-btn">
    //                                View
    //                            </a>
    //                        </td>
    //                    </tr>
    //                `;
    //                    });
    //                html += `
    //                                </tbody>
    //                            </table>
    //                        </div>
    //                    </div>
    //                </div>
    //            </div>
    //            `;

    //                deptIndex++;
    //            });
    //            // =========================================
    //            // BIND HTML
    //            // =========================================

    //            $('#departmentAccordion').html(html);

    //        },
    //        error: function () {
    //            alert('Error loading sector activity list');
    //        }
    //    });

    //},
    loadDepartmentActivityList: function (viksitId1) {

        $.ajax({
            url: `/ViksitRajasthan/GetDepartmentActivityList?viksitId=${viksitId}`,
            type: 'GET',

            success: function (response) {

                $("#title").html("Department Wise Activities");

                let groupedDepartments = {};

                //=========================================
                // Group By Department
                //=========================================

                $.each(response, function (i, item) {

                    if (!groupedDepartments[item.departmentId]) {

                        groupedDepartments[item.departmentId] = {
                            departmentName: item.departmentName,
                            activities: []
                        };
                    }

                    groupedDepartments[item.departmentId].activities.push(item);

                });

                let html = "";
                let deptIndex = 1;

                $.each(groupedDepartments, function (deptId, dept) {

                    html += `
                <div class="accordion-item">

                    <h2 class="accordion-header" id="heading${deptIndex}">
                        <button class="accordion-button ${deptIndex == 1 ? "" : "collapsed"}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapse${deptIndex}">

                            ${dept.departmentName}

                            <span class="badge bg-primary ms-auto">
                                ${dept.activities.length} Activities
                            </span>

                        </button>
                    </h2>

                    <div id="collapse${deptIndex}"
                         class="accordion-collapse collapse ${deptIndex == 1 ? "show" : ""}"
                         data-bs-parent="#departmentAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table table-bordered table-striped">

                                    <thead>

                                        <tr>

                                            <th>#</th>
                                            <th>Activity</th>
                                            <th>Sector</th>
                                            <th>Associated SubThemes</th>
                                            <th>Nodal Department</th>
                                            <th>Associated Department</th>
                                            <th>Agency</th>
                                            <th>Status</th>
                                            <th>Action</th>

                                        </tr>

                                    </thead>

                                    <tbody>
                `;

                    $.each(dept.activities, function (i, activity) {
                        html += `
                    <tr>
                        <td>${i + 1}</td>
                        <td>
                            <strong>${activity.activityName}</strong>
                        </td>
                        <td>${activity.unSectorName ?? ""}</td>
                        <td>${activity.associatedSubThemes ?? ""}</td>
                        <td>${activity.nodalDepartmentName ?? ""}</td>
                        <td>${activity.associatedDepartments ?? ""}</td>
                        <td>${activity.agencyName ?? ""}</td>
                        <td>${activity.activityStatus ?? ""}</td>
                        <td>
                            <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
                               class="btn btn-sm btn-primary">
                                View
                            </a>
                        </td>
                    </tr>
                    `;

                    });

                    html += `
                                    </tbody>

                                </table>

                            </div>

                        </div>

                    </div>

                </div>
                `;

                    deptIndex++;

                });

                $("#departmentAccordion").html(html);

            },

            error: function () {

                alert("Unable to load department activities.");

            }

        });

    },
    loadSectorActivityList: function (viksitId) {

        $.ajax({
            url: `/ViksitRajasthan/GetSectorActivityList?viksitId=${viksitId}`,
            type: 'GET',

            success: function (response) {

                $("#title").html("Sector Wise Activities");

                let groupedSectors = {};

                //=========================================
                // GROUP BY SECTOR
                //=========================================

                $.each(response, function (i, item) {

                    if (!groupedSectors[item.unSectorId]) {

                        groupedSectors[item.unSectorId] = {
                            sectorName: item.unSectorName ?? 'N/A',
                            activities: []
                        };
                    }

                    groupedSectors[item.unSectorId].activities.push(item);

                });

                let html = '';
                let sectorIndex = 1;

                //=========================================
                // LOOP SECTOR
                //=========================================

                $.each(groupedSectors, function (sectorId, sector) {

                    html += `
                <div class="accordion-item">

                    <h2 class="accordion-header" id="headingSector${sectorIndex}">

                        <button class="accordion-button ${sectorIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseSector${sectorIndex}">

                            ${sector.sectorName}

                            <span class="badge bg-primary ms-auto">
                                ${sector.activities.length} Activities
                            </span>

                        </button>

                    </h2>

                    <div id="collapseSector${sectorIndex}"
                         class="accordion-collapse collapse ${sectorIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#departmentAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table activity-table">

                                    <thead>

                                        <tr>

                                            <th>#</th>
                                            <th>Activity Name</th>
                                            <th>Nodal Department</th>
                                            <th>Associated Department</th>
                                            <th>Associated Sub Themes</th>
                                            <th>Agency</th>
                                            <th>Status</th>
                                            <th>Action</th>

                                        </tr>

                                    </thead>

                                    <tbody>
                `;

                    $.each(sector.activities, function (i, activity) {

                        html += `
                        <tr>

                            <td>${i + 1}</td>

                            <td>
                                <strong>${activity.activityName}</strong>
                            </td>

                            <td>${activity.nodalDepartment ?? ''}</td>

                            <td>${activity.associatedDepartments ?? ''}</td>

                            <td>${activity.associatedSubThemes ?? ''}</td>

                            <td>${activity.agencyName ?? ''}</td>

                            <td>${activity.activityStatus ?? ''}</td>

                            <td>
                                <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
                                   class="table-view-btn">
                                    View
                                </a>
                            </td>

                        </tr>
                    `;

                    });

                    html += `
                                    </tbody>

                                </table>

                            </div>

                        </div>

                    </div>

                </div>
                `;

                    sectorIndex++;

                });

                $("#departmentAccordion").html(html);

            },

            error: function () {

                alert("Error loading sector activity list.");

            }

        });

    },
    loadAgencyActivityList: function (viksitId) {
        $.ajax({
            url: `/ViksitRajasthan/GetAgencyActivityList?viksitId=${viksitId}`,
            type: 'GET',

            success: function (response) {

                $('#title').html('Agency Wise Activities');

                let groupedAgencies = {};

                //=========================================
                // GROUP BY AGENCY
                //=========================================

                $.each(response, function (i, item) {

                    if (!groupedAgencies[item.agencyId]) {

                        groupedAgencies[item.agencyId] = {
                            agencyName: item.agencyName,
                            agencyCode: item.agencyCode,
                            logoURL: item.logoURL,
                            activities: []
                        };
                    }

                    groupedAgencies[item.agencyId].activities.push(item);

                });

                let html = '';
                let agencyIndex = 1;

                //=========================================
                // LOOP AGENCIES
                //=========================================

                $.each(groupedAgencies, function (agencyId, agency) {

                    html += `
                <div class="accordion-item">

                    <h2 class="accordion-header" id="headingAgency${agencyIndex}">

                        <button class="accordion-button ${agencyIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseAgency${agencyIndex}">

                            <img src="${agency.logoURL ?? '/images/no-image.png'}"
                                 class="agencylogo">

                            <strong>${agency.agencyName}</strong>

                            <span class="badge bg-primary ms-auto">
                                ${agency.activities.length} Activities
                            </span>

                        </button>

                    </h2>

                    <div id="collapseAgency${agencyIndex}"
                         class="accordion-collapse collapse ${agencyIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#departmentAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table activity-table">

                                    <thead>

                                        <tr>

                                            <th>#</th>
                                            <th>Activity Name</th>
                                            <th>Sector</th>
                                            <th>Nodal Department</th>
                                            <th>Associated Department</th>
                                            <th>Associated Sub Themes</th>
                                            <th>Status</th>
                                            <th>Action</th>

                                        </tr>

                                    </thead>

                                    <tbody>
                `;

                    $.each(agency.activities, function (index, activity) {

                        html += `
                        <tr>

                            <td>${index + 1}</td>

                            <td>
                                <strong>${activity.activityName}</strong>
                            </td>

                            <td>${activity.unSectorName ?? ''}</td>

                            <td>${activity.nodalDepartment ?? ''}</td>

                            <td>${activity.associatedDepartments ?? ''}</td>

                            <td>${activity.associatedSubThemes ?? ''}</td>

                            <td>${activity.activityStatus ?? ''}</td>

                            <td>
                                <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
                                   class="table-view-btn">
                                    View
                                </a>
                            </td>

                        </tr>
                    `;

                    });

                    html += `
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                </div>
                `;

                    agencyIndex++;

                });

                $('#departmentAccordion').html(html);

            },

            error: function () {
                alert('Error loading agency activity list.');
            }

        });

    },
    loadActivityList: function (viksitId) {

        $.ajax({
            url: `/ViksitRajasthan/GetSectorActivityList?viksitId=${viksitId}`,
            type: 'GET',

            success: function (response) {

                $("#title").html("All Activities");

                let html = `
            <div class="table-responsive">
                <table class="table activity-table">

                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Activity Name</th>
                            <th>Sector</th>
                            <th>Nodal Department</th>
                            <th>Associated Department</th>
                            <th>Associated Sub Themes</th>
                            <th>Agency</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>

                    <tbody>
            `;

                $.each(response, function (i, activity) {

                    html += `
                    <tr>

                        <td>${i + 1}</td>

                        <td>
                            <strong>${activity.activityName ?? ''}</strong>
                        </td>

                        <td>${activity.unSectorName ?? ''}</td>

                        <td>${activity.nodalDepartment ?? ''}</td>

                        <td>${activity.associatedDepartments ?? ''}</td>

                        <td>${activity.associatedSubThemes ?? ''}</td>

                        <td>${activity.agencyName ?? ''}</td>

                        <td>${activity.activityStatus ?? ''}</td>

                        <td>
                            <a href="/Home/ActivityDet?guid=${activity.activityGuid}"
                               class="table-view-btn">
                                View
                            </a>
                        </td>

                    </tr>
                `;

                });

                html += `
                    </tbody>
                </table>
            </div>`;

                $("#departmentAccordion").html(html);

            },

            error: function () {

                alert("Error loading activity list.");

            }

        });

    },
}