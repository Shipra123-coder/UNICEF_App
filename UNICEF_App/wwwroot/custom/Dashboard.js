var dashboard = {
    loadCoverPages: function () {
        $.ajax({
            url: '/Home/GetCoverPages',
            type: 'GET',
            success: function (response) {
                let html = '';
                $.each(response, function (i, item) {
                    html += `
                    <div class="swiper-slide">
                        <div class="practice-card">
                            <div class="practice-image-box">
                                <img src="${item.coverImageUrl}"
                                     class="practice-image"
                                     alt="${item.headingTitle}">
                            </div>
                            <div class="practice-body">
                                <span class="practice-sector">
                                    ${item.sectorName || ''}
                                </span>
                                <h3 class="practice-title">
                                    ${item.headingTitle || ''}
                                </h3>
                                <div class="practice-desc">${item.descriptionNotes || ''}</div>
                                <div class="practice-footer">
                                    <span>${item.locationName || ''}</span>
                                    <a href="/Home/BestPrecticesDetails/${item.coverPageId}"
                                       class="read-more-btn">
                                        Read More
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>`;
                });

                $("#coverPageContainer").html(html);

                dashboard.initializeSwiper();
            }
        });
    },
    initializeSwiper: function () {
        if (window.bestPracSwiper) {
            window.bestPracSwiper.destroy(true, true);
        }

        window.bestPracSwiper = new Swiper('.bestPracNewSlide', {
            slidesPerView: 1,
            spaceBetween: 24,
            loop: false,

            autoplay: {
                delay: 4000,
                disableOnInteraction: false
            },

            navigation: {
                nextEl: '.best-prac-next',
                prevEl: '.best-prac-prev'
            },

            // 🌟 यहाँ बदलाव किए गए हैं
            breakpoints: {
                // मोबाइल (640px तक 1 कार्ड)
                640: {
                    slidesPerView: 1,
                    spaceBetween: 15
                },
                // टैबलेट / छोटी स्क्रीन (2 कार्ड)
                768: {
                    slidesPerView: 2,
                    spaceBetween: 20
                },
                // सामान्य लैपटॉप स्क्रीन (3 या 4 कार्ड जो आपको सही लगे - यहाँ 4 रखा है)
                1024: {
                    slidesPerView: 4,
                    spaceBetween: 24
                },
                // 🆕 बड़ी डेस्कटॉप स्क्रीन (यहाँ 5 कार्ड्स हो जाएंगे)
                1400: {
                    slidesPerView: 5,
                    spaceBetween: 24
                }
            }
        });
    },

    loadCoverPagesBySector: function () {

        var SectorId = $('#hiddenSectorId').val();

        $.ajax({
            url: `/Home/GetCoverPagesBySector?sectorId=${SectorId}`,
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

                                    <a href="/Home/CoverPageDetails?coverPageId=${item.coverPageId}"
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
    loadDashboardCounts: function () {
        debugger;
        var sectorId = $("#UNSector").val();
        var agencyId = $("#Agencys").val();
        var departmentId = $("#Department").val();
        if (agencyId == "")
            agencyId = null
        if (sectorId == "")
            sectorId = null
        if (departmentId == "")
            departmentId = null
        $.ajax({
            url: '/Home/GetCountByAgencySectorDept',
            type: 'GET',
            data: {
                sectorId: sectorId || null,
                agencyId: agencyId || null,
                departmentId: departmentId || null
            },
            success: function (res) {

                $("#agencyCount").text(res.agencyCount);
                $("#departmentCount").text(res.departmentCount);
                $("#activityCount").text(res.activityCount);
                $("#taskCount").text(res.taskCount);
                $("#goalCount").text(res.goalCount);
                $("#targetCount").text(res.targetCount);
                $("#pillarCount").text(res.pillarCount);
                $("#subPillarCount").text(res.subPillarCount);

            },
            error: function () {
                console.log("Error loading dashboard counts");
            }
        });
    },
    loadDashboardCountsBySector: function (sectorId) {

        $.ajax({
            url: '/Home/GetCountByAgencySectorDept',
            type: 'GET',
            data: {
                sectorId: sectorId,
                agencyId: null,
                departmentId: null
            },
            success: function (res) {

                $("#agencyCount").text(res.agencyCount);
                $("#departmentCount").text(res.departmentCount);
                $("#activityCount").text(res.activityCount);
                $("#goalCount").text(res.goalCount);
                $("#targetCount").text(res.targetCount);
                $("#pillarCount").text(res.pillarCount);
                $("#subPillarCount").text(res.subPillarCount);
                $("#taskCount").text(res.taskCount);

            }
        });

    },

    loadAgencyList: function () {
        $.ajax({
            url: '/Home/GetAgencyList',
            type: 'GET',
            success: function (response) {
                $('#agencyCountAgency').text(response.length);
                let html = '';
                let colorClasses = [
                    'green-card',
                    'pink-card',
                    'blue-card',
                    'orange-card',
                    'purple-card',
                    'teal-card'
                ];
                $.each(response, function (index, item) {
                    let colorClass =
                        colorClasses[index % colorClasses.length];

                    html += `
                    <div class="agency-card ${colorClass}">
                        <div class="agency-top">
                            <div class="agency-logo">
                                <a href="${item.websitelink}" 
                               target="_blank">

                                <img src="${item.logoURL}" 
                                     alt="${item.agencyCode}" />

                            </a>
                            </div>
                            <div class="agency-status">
                                Active
                            </div>
                        </div>
                        <h3>
                            ${item.agencyCode}
                        </h3>
                        <p>
                            ${item.description}
                        </p>
                        <div class="agency-footer">
                            <span>
                                ${item.agencyName}
                            </span>
                            <a href="/Home/AgencyStatus?agencyId=${item.agencyId}"
   target="_blank"
   class="agency-view-btn">

    View →

</a>
                        </div>
                    </div>
                    `;
                });

                $('#agencyGrid').html(html);

            },

            error: function () {

                console.log('Error loading agency list');

            }
        });
    },
    loadAgencyStatus: function () {

        var AgencyId = $('#hiddenAgencyId').val();

        $.ajax({
            url: `/Home/GetCoverPagesByAgency?agencyId=${AgencyId}`,
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
                                    ${item.agencyName || ''}
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

                                    <a href="/Home/CoverPageDetails?coverPageId=${item.coverPageId}"
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
    loadDepartmentList: function () {

        $.ajax({
            url: '/Home/GetDepartmentList',
            type: 'GET',
            success: function (response) {

                $('#departmentCount').text(response.length);

                let html = '';
                $.each(response, function (index, item) {

                    // Department Name se image name banana
                    let imageName = item.departmentCode;
                        //.toLowerCase()
                        //.replace(/&/g, 'and')
                        //.replace(/\s+/g, '')
                        //.replace(/[^a-z0-9]/g, '');

                    html += `
                    <div class="col">
                        <div class="category-card h-100">
                            <img src="/public/img/DepartmentIcon/${imageName}.png"
                                 class="img-fluid mb-3"
                                 style="height:100px;"
                                 onerror="this.src='/public/img/DepartmentIcon/default.png';">

                            <h4>${item.departmentName}</h4>

                            <a href="/Home/DepartmentStatus?deptId=${item.departmentId}"
                               class="view-all-btn">
                                View More <i class="fa fa-arrow-right"></i>
                            </a>
                        </div>
                    </div>`;
                });

                $('#departmentGrid').html(html);

            },
            error: function () {
                console.log('Error loading departments');
            }
        });

    },  

    loadSectorDepartmentActivityList: function (sectorId) {
        $.ajax({
            url: `/Home/GetSectorDepartmentActivityList?sectorId=${sectorId}`,
            type: 'GET',
            success: function (response) {
                $('#departmentAccordion').html();
                $('#title').html('Department Wise Activities');
                console.log(response);
                let groupedDepartments = {};
                // =========================================
                // GROUP DATA
                // =========================================
                $.each(response, function (i, item) {
                    if (!groupedDepartments[item.departmentId]) {
                        groupedDepartments[item.departmentId] = {
                            departmentName: item.departmentName,
                            activities: []
                        };
                    }
                    groupedDepartments[item.departmentId]
                        .activities.push(item);
                });
                // =========================================
                // HTML START
                // =========================================

                let html = '';
                let deptIndex = 1;
                // =========================================
                // LOOP DEPARTMENT
                // =========================================

                $.each(groupedDepartments, function (deptId, deptData) {

                    html += `
                <div class="accordion-item">
                    <h2 class="accordion-header"
                        id="headingDept${deptIndex}">
                        <button class="accordion-button 
                                       ${deptIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseDept${deptIndex}">
                            ${deptData.departmentName}
                             <div class="d-flex
                                        align-items-right
                                        w-100">
                            <span class="dept-count-badge dept-count-badge ms-auto">
                                ${deptData.activities.length} Activities
                            </span>
                            </div>
                        </button>
                    </h2>
                    <div id="collapseDept${deptIndex}"
                         class="accordion-collapse collapse
                         ${deptIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#departmentAccordion">
                        <div class="accordion-body">
                            <div class="table-responsive">
                                <table class="table activity-table">
                                    <thead>
                                        <tr>
                                            <th width="5%">
                                                #
                                            </th>
                                            <th>
                                                Activity Name
                                            </th>
                                            <th>
                                                Sector
                                            </th>
                                            <th>
                                                Status
                                            </th>
                                            <th>
                                                Action
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                `;



                    // =========================================
                    // LOOP ACTIVITIES
                    // =========================================
                    $.each(deptData.activities,
                        function (index, activity) {
                            html += `
                        <tr>
                            <td>
                                ${index + 1}
                            </td>
                            <td>
                                <strong>
                                    ${activity.activityName}
                                </strong>
                            </td>
                            <td>
                                ${activity.unSectorName ?? ''}
                            </td>
                            <td>
                                ${activity.activityStatus ?? ''}
                            </td>
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

                    deptIndex++;
                });
                // =========================================
                // BIND HTML
                // =========================================

                $('#departmentAccordion').html(html);

            },
            error: function () {
                alert('Error loading department activity list');
            }
        });

    },
    loadSectorAgencyActivityList: function (sectorId) {
        $.ajax({
            url: `/Home/GetSectorAgencyActivityList?sectorId=${sectorId}`,
            type: 'GET',
            success: function (response) {
                $('#departmentAccordion').html();
                $('#title').html('Agency Wise Activities');
                console.log(response);

                let groupedAgencies = {};



                // =========================================
                // GROUP DATA
                // =========================================

                $.each(response, function (i, item) {

                    if (!groupedAgencies[item.agencyId]) {

                        groupedAgencies[item.agencyId] = {

                            agencyName: item.agencyName,

                            agencyCode: item.agencyCode,

                            logoURL: item.logoURL,

                            activities: []

                        };
                    }

                    groupedAgencies[item.agencyId]
                        .activities.push(item);

                });



                // =========================================
                // HTML START
                // =========================================

                let html = '';

                let agencyIndex = 1;



                // =========================================
                // LOOP AGENCY
                // =========================================

                $.each(groupedAgencies,
                    function (agencyId, agencyData) {

                        html += `

                <div class="accordion-item">

                    <h2 class="accordion-header"
                        id="headingAgency${agencyIndex}">

                        <button class="accordion-button
                                       ${agencyIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseAgency${agencyIndex}">

                            <div class="d-flex
                                        align-items-center
                                        w-100">

                                <img src="${agencyData.logoURL}"
                                     class="agency-small-logo me-3" width="100" />

                                <div>

                                    <strong>
                                        ${agencyData.agencyCode}
                                    </strong>

                                    <div class="small text-muted">

                                        ${agencyData.agencyName}

                                    </div>

                                </div>

                                <span class="dept-count-badge ms-auto">

                                    ${agencyData.activities.length}
                                    Activities

                                </span>

                            </div>

                        </button>

                    </h2>

                    <div id="collapseAgency${agencyIndex}"
                         class="accordion-collapse collapse
                         ${agencyIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#agencyAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table activity-table">

                                    <thead>

                                        <tr>

                                            <th width="5%">
                                                #
                                            </th>

                                            <th>
                                                Activity Name
                                            </th>

                                            <th>
                                                Sector
                                            </th>
                                            <th>
                                                Nodal Department
                                            </th>
                                            <th>
                                                Associated Department
                                            </th>
                                            <th>
                                                Status
                                            </th>

                                            <th>
                                                Action
                                            </th>

                                        </tr>

                                    </thead>

                                    <tbody>

                `;



                        // =========================================
                        // LOOP ACTIVITIES
                        // =========================================

                        $.each(agencyData.activities,
                            function (index, activity) {

                                html += `

                        <tr>

                            <td>
                                ${index + 1}
                            </td>

                            <td>

                                <strong>

                                    ${activity.activityName}

                                </strong>

                            </td>

                            <td>

                                ${activity.unSectorName ?? ''}

                            </td>
                            <td>

                                ${activity.nodalDepartment ?? ''}

                            </td>
                             <td>

                                ${activity.associatedDepartment ?? ''}

                            </td>

                            <td>

                                <span class="status-active">

                                    ${activity.activityStatus ?? ''}

                                </span>

                            </td>

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



                // =========================================
                // BIND HTML
                // =========================================

                $('#departmentAccordion').html(html);

            },
            error: function () {

                alert('Error loading agency activity list');

            }
        });
    },
    loadSectorActivityList: function (sectorId) {
        $.ajax({
            url: `/Home/GetSectorAgencyActivityList?sectorId=${sectorId}`,
            type: 'GET',
            success: function (response) {

                $('#title').html('Activity List');

                let html = `

            <div class="table-responsive">

                <table class="table activity-table">

                    <thead>
                        <tr>
                            <th width="5%">#</th>
                            <th>Activity Name</th>
                            <th>Agency</th>
                            <th>Sector</th>
                            <th>Nodal Department</th>
                            <th>Associated Department</th>
                            <th>Status</th>
                            <th width="100">Action</th>
                        </tr>
                    </thead>

                    <tbody>
            `;

                if (response.length > 0) {

                    $.each(response, function (index, activity) {

                        html += `
                        <tr>

                            <td>${index + 1}</td>

                            <td>
                                <strong>${activity.activityName}</strong>
                            </td>

                            <td>
                                <div class="d-flex align-items-center">
                                    <img src="${activity.logoURL}"
                                         class="agency-small-logo me-2"
                                         width="40"
                                         onerror="this.style.display='none';">

                                    <div>
                                        <strong>${activity.agencyCode ?? ''}</strong><br>
                                        <small>${activity.agencyName ?? ''}</small>
                                    </div>
                                </div>
                            </td>

                            <td>${activity.unSectorName ?? ''}</td>

                            <td>${activity.nodalDepartment ?? ''}</td>

                            <td>${activity.associatedDepartment ?? ''}</td>

                            <td>
                                <span class="status-active">
                                    ${activity.activityStatus ?? ''}
                                </span>
                            </td>

                            <td>
                                <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
                                   class="table-view-btn">
                                    View
                                </a>
                            </td>

                        </tr>
                    `;
                    });

                } else {

                    html += `
                    <tr>
                        <td colspan="8" class="text-center">
                            No activities found.
                        </td>
                    </tr>
                `;
                }

                html += `
                    </tbody>

                </table>

            </div>
            `;

                $('#departmentAccordion').html(html);

            },

            error: function () {
                alert('Error loading activity list');
            }
        });

    },

    loadAgencyDepartmentActivityList: function (agencyid) {

        $.ajax({

            url: `/Home/GetAgencyDepartmentActivityList?agencyId=${agencyid}`,

            type: 'GET',

            success: function (response) {
                $('#departmentAccordion').html();
                $('#title').html('Department Wise Activities');
                console.log(response);

                let groupedDepartments = {};



                // =========================================
                // GROUP DATA
                // =========================================

                $.each(response, function (i, item) {

                    if (!groupedDepartments[item.departmentId]) {

                        groupedDepartments[item.departmentId] = {

                            departmentName: item.nodalDepartment,

                            activities: []

                        };
                    }

                    groupedDepartments[item.departmentId]
                        .activities.push(item);

                });



                // =========================================
                // HTML START
                // =========================================

                let html = '';

                let deptIndex = 1;



                // =========================================
                // LOOP DEPARTMENT
                // =========================================

                $.each(groupedDepartments,
                    function (deptId, deptData) {

                        html += `

                <div class="accordion-item">

                    <h2 class="accordion-header"
                        id="headingDept${deptIndex}">

                        <button class="accordion-button
                                       ${deptIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseDept${deptIndex}">

                            ${deptData.departmentName}

                            <span class="dept-count-badge ms-auto">

                                ${deptData.activities.length}
                                Activities

                            </span>

                        </button>

                    </h2>

                    <div id="collapseDept${deptIndex}"
                         class="accordion-collapse collapse
                         ${deptIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#departmentAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table activity-table">

                                    <thead>

                                        <tr>

                                            <th width="5%">
                                                #
                                            </th>

                                            <th>
                                                Activity Name
                                            </th>

                                            <th>
                                                Sector
                                            </th>

                                            <th>
                                                Status
                                            </th>

                                            <th>
                                                Action
                                            </th>

                                        </tr>

                                    </thead>

                                    <tbody>

                `;



                        // =========================================
                        // LOOP ACTIVITIES
                        // =========================================

                        $.each(deptData.activities,
                            function (index, activity) {

                                html += `

                    <tr>

                        <td>
                            ${index + 1}
                        </td>

                        <td>

                            <strong>

                                ${activity.activityName}

                            </strong>

                        </td>

                        <td>

                            ${activity.unSectorName ?? ''}

                        </td>

                        <td>

                            ${activity.activityStatus ?? ''}

                        </td>

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

                        deptIndex++;

                    });



                // =========================================
                // BIND HTML
                // =========================================

                $('#departmentAccordion').html(html);

            },

            error: function () {

                alert('Error loading department activity list');

            }

        });

    },
    loadAgencySectorActivityList: function (agencyid) {
        $.ajax({
            url: `/Home/GetAgencySectorActivityList?agencyId=${agencyid}`,
            type: 'GET',
            success: function (response) {
                $('#departmentAccordion').html();
                $('#title').html('Sector Wise Activities');
                console.log(response);
                let groupedSectors = {};
                // =========================================
                // GROUP DATA
                // =========================================
                $.each(response, function (i, item) {

                    if (!groupedSectors[item.unSectorId]) {

                        groupedSectors[item.unSectorId] = {

                            unSectorName: item.unSectorName,

                            activities: []

                        };
                    }

                    groupedSectors[item.unSectorId]
                        .activities.push(item);

                });



                // =========================================
                // HTML START
                // =========================================

                let html = '';

                let sectorIndex = 1;



                // =========================================
                // LOOP SECTOR
                // =========================================

                $.each(groupedSectors,
                    function (sectorId, sectorData) {

                        html += `

                <div class="accordion-item">

                    <h2 class="accordion-header"
                        id="headingSector${sectorIndex}">

                        <button class="accordion-button
                                       ${sectorIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseSector${sectorIndex}">

                            ${sectorData.unSectorName}

                            <span class="dept-count-badge ms-auto">

                                ${sectorData.activities.length}
                                Activities

                            </span>

                        </button>

                    </h2>

                    <div id="collapseSector${sectorIndex}"
                         class="accordion-collapse collapse
                         ${sectorIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#sectorAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table activity-table">

                                    <thead>

                                        <tr>
                                            <th width="5%">
                                                #
                                            </th>

                                            <th> 
                                                Activity Name
                                            </th>
                                            <th>
                                                Nodal Department
                                            </th>
                                             <th>
                                                Associated Departments
                                            </th>
                                            <th>
                                                Status
                                            </th>

                                            <th>
                                                Action
                                            </th>

                                        </tr>

                                    </thead>

                                    <tbody>

                `;



                        // =========================================
                        // LOOP ACTIVITIES
                        // =========================================

                        $.each(sectorData.activities,
                            function (index, activity) {

                                html += `

                    <tr>

                        <td>
                            ${index + 1}
                        </td>

                        <td>

                            <strong>

                                ${activity.activityName}

                            </strong>

                        </td>
                        <td>

                            <strong>

                                ${activity.nodalDepartment}

                            </strong>

                        </td>
                        <td>${activity.associatedDepartments}</td>

                        <td>

                             ${activity.activityStatus}

                        </td>

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



                // =========================================
                // BIND HTML
                // =========================================

                $('#departmentAccordion').html(html);

            },

            error: function () {

                alert('Error loading sector activity list');

            }

        });

    },
    loadAgencyActivityList: function (agencyid) {

        $.ajax({
            url: `/Home/GetAgencySectorActivityList?agencyId=${agencyid}`,
            type: 'GET',
            success: function (response) {

                $('#title').html('Activity List');

                let html = `

                <div class="table-responsive">

                    <table class="table activity-table">

                        <thead>
                            <tr>
                                <th width="5%">#</th>
                                <th>Activity Name</th>
                                <th>Sector</th>
                                <th>Nodal Department</th>
                                <th>Associated Departments</th>
                                <th>Status</th>
                                <th width="100">Action</th>
                            </tr>
                        </thead>

                        <tbody>
            `;

                if (response.length > 0) {

                    $.each(response, function (index, activity) {

                        html += `
                        <tr>

                            <td>${index + 1}</td>

                            <td>
                                <strong>${activity.activityName ?? ''}</strong>
                            </td>

                            <td>
                                ${activity.unSectorName ?? ''}
                            </td>

                            <td>
                                ${activity.nodalDepartment ?? ''}
                            </td>

                            <td>
                                ${activity.associatedDepartments ?? ''}
                            </td>

                            <td>
                                <span class="status-active">
                                    ${activity.activityStatus ?? ''}
                                </span>
                            </td>

                            <td>
                                <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
                                   class="table-view-btn">
                                    View
                                </a>
                            </td>

                        </tr>
                    `;
                    });

                } else {

                    html += `
                    <tr>
                        <td colspan="7" class="text-center">
                            No activities found.
                        </td>
                    </tr>
                `;
                }

                html += `
                        </tbody>

                    </table>

                </div>
            `;

                $('#departmentAccordion').html(html);

            },

            error: function () {
                alert('Error loading sector activity list');
            }

        });

    },

    loadDepartmentAgencyActivityList: function (departmentId) {

        $.ajax({

            url: `/Home/GetDepartmentAgencyActivityList?departmentId=${departmentId}`,
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

                    groupedAgencies[item.agencyId]
                        .activities.push(item);

                });

                //=========================================
                // BUILD HTML
                //=========================================

                let html = "";
                let agencyIndex = 1;

                $.each(groupedAgencies, function (agencyId, agencyData) {

                html += `
                  <div class="accordion-item">
                      <h2 class="accordion-header" id="headingAgency${agencyIndex}">
                          <button class="accordion-button
                                       ${agencyIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseAgency${agencyIndex}">
                            <div class="d-flex
                                        align-items-center
                                        w-100">
                                <img src="${agencyData.logoURL}"
                                     class="agency-small-logo me-3" width="100" />
                                <div>
                                    <strong>
                                        ${agencyData.agencyCode}
                                    </strong>
                                    <div class="small text-muted">
                                        ${agencyData.agencyName}
                                    </div>
                                </div>
                                <span class="dept-count-badge ms-auto">
                                    ${agencyData.activities.length}
                                    Activities
                                </span>
                            </div>
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
                                              <th width="5%">#</th>
                                              <th>Activity Name</th>
                                              <th>
                                                Nodal Department
                                              </th>
                                              <th>
                                                  Associated Department
                                              </th>
                                              <th>Sector</th>
                                              <th>Status</th>
                                              <th>Action</th>
                                          </tr>
                                      </thead>
                                      <tbody>`;
                                      $.each(agencyData.activities, function (index, activity) {
                  
                                          html += `
                    <tr>
                    
                        <td>${index + 1}</td>
                    
                        <td>
                            <strong>${activity.activityName}</strong>
                        </td>
                    
                        <td>
                            ${activity.departmentName ?? ""}
                        </td>
                        <td>
                            ${activity.associatedDepartment ?? ""}
                        </td>
                        <td>
                            ${activity.unSectorName ?? ""}
                        </td>
                    
                        <td>
                            ${activity.activityStatus || "-"}
                        </td>
                    
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
                  </div>`;
                    agencyIndex++;
                });
                if (html === "") {
                    html = `
                <div class="alert alert-warning mb-0">
                    No activities found.
                </div>`;
                }
                $('#departmentAccordion').html(html);
            },
            error: function () {
                alert('Error loading agency activity list');
            }
        });
    },
    loadDepartmentSectorActivityList: function (departmentId) {
        $.ajax({
            url: `/Home/GetDepartmentSectorActivityList?departmentId=${departmentId}`,
            type: 'GET',
            success: function (response) {
                $('#departmentAccordion').html();
                $('#title').html('Sector Wise Activities');
                console.log(response);
                let groupedSectors = {};
                // =========================================
                // GROUP DATA
                // =========================================
                $.each(response, function (i, item) {

                    if (!groupedSectors[item.unSectorId]) {

                        groupedSectors[item.unSectorId] = {

                            unSectorName: item.unSectorName,

                            activities: []

                        };
                    }

                    groupedSectors[item.unSectorId]
                        .activities.push(item);

                });



                // =========================================
                // HTML START
                // =========================================

                let html = '';

                let sectorIndex = 1;



                // =========================================
                // LOOP SECTOR
                // =========================================

                $.each(groupedSectors,
                    function (sectorId, sectorData) {

                        html += `

                <div class="accordion-item">

                    <h2 class="accordion-header"
                        id="headingSector${sectorIndex}">

                        <button class="accordion-button
                                       ${sectorIndex != 1 ? 'collapsed' : ''}"
                                type="button"
                                data-bs-toggle="collapse"
                                data-bs-target="#collapseSector${sectorIndex}">

                            ${sectorData.unSectorName}

                            <span class="dept-count-badge ms-auto">

                                ${sectorData.activities.length}
                                Activities

                            </span>

                        </button>

                    </h2>

                    <div id="collapseSector${sectorIndex}"
                         class="accordion-collapse collapse
                         ${sectorIndex == 1 ? 'show' : ''}"
                         data-bs-parent="#sectorAccordion">

                        <div class="accordion-body">

                            <div class="table-responsive">

                                <table class="table activity-table">

                                    <thead>

                                        <tr>
                                            <th width="5%">
                                                #
                                            </th>

                                            <th> 
                                                Activity Name
                                            </th>
                                            <th>
                                                Nodal Department
                                            </th>
                                             <th>
                                                Associated Departments
                                            </th>
                                            <th>
                                                Status
                                            </th>

                                            <th>
                                                Action
                                            </th>

                                        </tr>

                                    </thead>

                                    <tbody>

                `;



                        // =========================================
                        // LOOP ACTIVITIES
                        // =========================================

                        $.each(sectorData.activities,
                            function (index, activity) {

                                html += `

                    <tr>

                        <td>
                            ${index + 1}
                        </td>

                        <td>

                            <strong>

                                ${activity.activityName}

                            </strong>

                        </td>
                        <td>

                            <strong>

                                ${activity.nodalDepartment}

                            </strong>

                        </td>
                        <td>${activity.associatedDepartments}</td>

                        <td>

                             ${activity.activityStatus}

                        </td>

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



                // =========================================
                // BIND HTML
                // =========================================

                $('#departmentAccordion').html(html);

            },

            error: function () {

                alert('Error loading sector activity list');

            }

        });

    },
    loadDepartmentActivityList: function (departmentId1) {

        $.ajax({
            url: `/Home/GetDepartmentSectorActivityList?departmentId=${departmentId}`,
            type: 'GET',
            success: function (response) {

                $('#title').html('Activity List');

                let html = `
                <div class="table-responsive">
                    <table class="table activity-table">

                        <thead>
                            <tr>
                                <th width="5%">#</th>
                                <th>Activity Name</th>
                                <th>Sector</th>
                                <th>Nodal Department</th>
                                <th>Associated Departments</th>
                                <th>Status</th>
                                <th width="100">Action</th>
                            </tr>
                        </thead>

                        <tbody>
            `;

                if (response.length > 0) {

                    $.each(response, function (index, activity) {

                        html += `
                        <tr>

                            <td>${index + 1}</td>

                            <td>
                                <strong>${activity.activityName ?? ''}</strong>
                            </td>

                            <td>
                                ${activity.unSectorName ?? ''}
                            </td>

                            <td>
                                ${activity.nodalDepartment ?? ''}
                            </td>

                            <td>
                                ${activity.associatedDepartments ?? ''}
                            </td>

                            <td>
                                <span class="status-active">
                                    ${activity.activityStatus ?? ''}
                                </span>
                            </td>

                            <td>
                                <a href="/Home/ActivityDetails?activityId=${activity.activityId}"
                                   class="table-view-btn">
                                    View
                                </a>
                            </td>

                        </tr>
                    `;
                    });

                } else {

                    html += `
                    <tr>
                        <td colspan="7" class="text-center">
                            No activities found.
                        </td>
                    </tr>
                `;
                }

                html += `
                        </tbody>

                    </table>

                </div>
            `;

                $('#departmentAccordion').html(html);

            },

            error: function () {

                alert('Error loading sector activity list');

            }

        });

    },

    loadChartAgency: function () {
        $.ajax({
            url: '/Home/GetAgencyChartData',
            type: 'GET',
            success: function (response) {
                const agencyCanvas =
                    document.getElementById("agencyWiseChart");

                if (!agencyCanvas)
                    return;

                const labels =
                    response.map(x => x.agencyName);

                const activities =
                    response.map(x => x.activityCount);

                const departments =
                    response.map(x => x.departmentCount);

                const sectores =
                    response.map(x => x.sectorCount);

                if (window.agencyChartInstance) {
                    window.agencyChartInstance.destroy();
                }

                window.agencyChartInstance =
                    new Chart(agencyCanvas, {

                        type: 'bar',

                        data: {

                            labels: labels,

                            datasets: [

                                {
                                    label: 'Activities',
                                    data: activities,
                                    backgroundColor: '#7c3aed',
                                    borderRadius: 8
                                },

                                {
                                    label: 'Departments',
                                    data: departments,
                                    backgroundColor: '#06b6d4',
                                    borderRadius: 8
                                },

                                {
                                    label: 'Sectors',
                                    data: sectores,
                                    backgroundColor: '#f43f5e',
                                    borderRadius: 8
                                }

                            ]
                        },

                        options: {

                            responsive: true,

                            maintainAspectRatio: false,

                            plugins: {

                                legend: {

                                    position: 'top',

                                    labels: {

                                        color: '#1e293b',

                                        font: {
                                            size: 13,
                                            weight: '600'
                                        }
                                    }
                                }
                            },

                            scales: {

                                x: {

                                    ticks: {

                                        color: '#334155',

                                        font: {
                                            weight: '600'
                                        }
                                    },

                                    grid: {
                                        display: false
                                    }
                                },

                                y: {

                                    beginAtZero: true,

                                    ticks: {
                                        color: '#64748b'
                                    },

                                    grid: {
                                        color: '#e2e8f0'
                                    }
                                }
                            }
                        }
                    });
            },

            error: function () {

                console.error(
                    "Error loading agency chart data"
                );
            }
        });
    },
    loadChartDepartment: function () {

        $.ajax({
            url: '/Home/GetDepartmentChartData',
            type: 'GET',

            success: function (response) {

                const departmentCanvas =
                    document.getElementById("departmentWiseChart");

                if (!departmentCanvas)
                    return;

                const labels =
                    response.map(x => x.departmentName);

                const activities =
                    response.map(x => x.activityCount);

                const agencies =
                    response.map(x => x.agencyCount);

                const sectors =
                    response.map(x => x.sectorCount);

                if (window.departmentChartInstance) {
                    window.departmentChartInstance.destroy();
                }

                window.departmentChartInstance =
                    new Chart(departmentCanvas, {

                        type: 'bar',

                        data: {

                            labels: labels,

                            datasets: [

                                {
                                    label: 'Activities',

                                    data: activities,

                                    backgroundColor: '#2563eb',

                                    borderRadius: 8
                                },

                                {
                                    label: 'Agencies',

                                    data: agencies,

                                    backgroundColor: '#10b981',

                                    borderRadius: 8
                                },

                                {
                                    label: 'Sectors',

                                    data: sectors,

                                    backgroundColor: '#f97316',

                                    borderRadius: 8
                                }

                            ]

                        },

                        options: {

                            responsive: true,

                            maintainAspectRatio: false,

                            plugins: {

                                legend: {

                                    position: 'top',

                                    labels: {

                                        color: '#1e293b',

                                        font: {
                                            size: 13,
                                            weight: '600'
                                        }

                                    }

                                }

                            },

                            scales: {

                                x: {

                                    ticks: {

                                        color: '#334155',

                                        font: {
                                            weight: '600'
                                        }

                                    },

                                    grid: {
                                        display: false
                                    }

                                },

                                y: {

                                    beginAtZero: true,

                                    ticks: {
                                        color: '#64748b'
                                    },

                                    grid: {
                                        color: '#e2e8f0'
                                    }

                                }

                            }

                        }

                    });

            },

            error: function () {

                console.error(
                    "Error loading department chart data"
                );

            }
        });
    },
    loadChartGoal: function () {

        $.ajax({
            url: '/Home/GetGoalChartData',
            type: 'GET',

            success: function (response) {

                const goalCanvas =
                    document.getElementById("goalWiseChart");

                if (!goalCanvas)
                    return;

                const labels =
                    response.goalchartmodel.map(x => x.goalName);

                const activities =
                    response.goalchartmodel.map(x => x.activityCount);

                const departments =
                    response.goalchartmodel.map(x => x.departmentCount);

                const agencies =
                    response.goalchartmodel.map(x => x.agencyCount);

                const sectors =
                    response.goalchartmodel.map(x => x.sectorCount);



                // Total Activities
                const totalActivities = response.activitycount;

                // Total Departments
                const totalDepartments = response.departmentcount;

                // Total Agencies
                const totalAgencies = response.agencycount;

                //// Total Sector
                //const totalsectors =
                //    response.reduce(
                //        (sum, item) => sum + (item.sectorCount || 0),
                //        0
                //    );

                /*$("#totalSectors").text(totalsectors);*/

                $("#totalActivities").text(totalActivities);

                $("#totalDepartments").text(totalDepartments);

                $("#totalAgencies").text(totalAgencies);

                // Load Goal Images
                const goalImages = [];

                response.goalchartmodel.forEach(item => {

                    const img = new Image();

                    //Option 1: Image Path from API
                    img.src = item.goalImage;

                    //// Option 2: GoalId Wise Images
                    //img.src = `/images/goals/goal${item.goalId}.png`;

                    goalImages.push(img);
                });

                // Custom Plugin for Image + Goal Name
                const imageLabelPlugin = {

                    id: 'imageLabelPlugin',

                    afterDraw(chart) {

                        const ctx = chart.ctx;
                        const xAxis = chart.scales.x;

                        goalImages.forEach((img, index) => {

                            const x =
                                xAxis.getPixelForTick(index);

                            const imageSize = 45;

                            const imageY =
                                chart.height - 60;

                            // Draw Goal Image
                            if (img.complete) {

                                ctx.drawImage(
                                    img,
                                    x - (imageSize / 2),
                                    imageY,
                                    imageSize,
                                    imageSize
                                );
                            }

                            // Draw Goal Name
                            ctx.save();

                            ctx.textAlign = 'center';
                            ctx.fillStyle = '#334155';
                            ctx.font = 'bold 11px Arial';

                            const words =
                                labels[index].split(' ');

                            if (words.length <= 2) {

                                ctx.fillText(
                                    labels[index],
                                    x,
                                    imageY + imageSize + 8
                                );
                            }
                            else {

                                const firstLine =
                                    words.slice(0, 2).join(' ');

                                const secondLine =
                                    words.slice(2).join(' ');

                                ctx.fillText(
                                    firstLine,
                                    x,
                                    imageY + imageSize + 8
                                );

                                ctx.fillText(
                                    secondLine,
                                    x,
                                    imageY + imageSize + 34
                                );
                            }

                            ctx.restore();
                        });
                    }
                };

                const valueLabelPlugin = {

                    id: 'valueLabelPlugin',

                    afterDatasetsDraw(chart) {

                        const { ctx } = chart;

                        chart.data.datasets.forEach((dataset, datasetIndex) => {

                            const meta = chart.getDatasetMeta(datasetIndex);

                            meta.data.forEach((bar, index) => {

                                const value = dataset.data[index];

                                // 0 ya null value hide
                                if (!value || value === 0)
                                    return;

                                ctx.save();

                                ctx.fillStyle = '#000';
                                ctx.font = 'bold 11px Arial';
                                ctx.textAlign = 'center';

                                ctx.fillText(
                                    value,
                                    bar.x,
                                    bar.y - 8
                                );

                                ctx.restore();

                            });

                        });

                    }

                };

                if (window.goalChartInstance) {
                    window.goalChartInstance.destroy();
                }

                window.goalChartInstance =
                    new Chart(goalCanvas, {

                        type: 'bar',

                        data: {

                            labels: labels,

                            datasets: [

                                {
                                    label: 'Activities',
                                    data: activities,
                                    backgroundColor: '#0F4C81',
                                    borderRadius: 0
                                },

                                {
                                    label: 'Departments',
                                    data: departments,
                                    backgroundColor: '#2E8B57',
                                    borderRadius: 0
                                },

                                {
                                    label: 'Agencies',
                                    data: agencies,
                                    backgroundColor: '#D4A017',
                                    borderRadius: 0
                                }

                                //,{
                                //    label: 'Sectors',
                                //    data: sectors,
                                //    backgroundColor: '#8B0000',
                                //    borderRadius: 0
                                //}

                            ]

                        },

                        plugins: [imageLabelPlugin, valueLabelPlugin],

                        options: {

                            responsive: true,

                            maintainAspectRatio: false,

                            layout: {

                                padding: {
                                    bottom: 60
                                }
                            },

                            plugins: {

                                legend: {

                                    position: 'top',

                                    labels: {

                                        color: '#1e293b',

                                        font: {
                                            size: 13,
                                            weight: '600'
                                        }

                                    }

                                }

                            },

                            scales: {

                                x: {

                                    ticks: {
                                        display: false
                                    },

                                    grid: {
                                        display: false
                                    }

                                },

                                y: {

                                    beginAtZero: true,

                                    ticks: {
                                        color: '#64748b'
                                    },

                                    grid: {
                                        display: false,
                                        color: '#e2e8f0'
                                    }

                                }

                            }

                        }

                    });

            },

            error: function () {

                console.error(
                    "Error loading goal chart data"
                );

            }
        });
    },
    loadActivityStatusChart: function () {

        $.ajax({
            url: '/Home/GetActivityStatusChart',
            type: 'GET',

            success: function (response) {

                const ctx =
                    document.getElementById("activityStatusChart");

                if (!ctx)
                    return;

                const labels =
                    response.map(x => x.activityStatus);

                const counts =
                    response.map(x => x.totalCount);

                // Destroy old chart
                if (window.activityStatusChartInstance) {
                    window.activityStatusChartInstance.destroy();
                }

                // =========================
                // CENTER TOTAL PLUGIN
                // =========================
                const centerTextPlugin = {

                    id: 'centerTextPlugin',

                    afterDraw(chart) {

                        const { ctx } = chart;

                        const total =
                            chart.data.datasets[0].data
                                .reduce((a, b) => a + b, 0);

                        const meta =
                            chart.getDatasetMeta(0);

                        if (!meta.data.length)
                            return;

                        const centerX = meta.data[0].x;
                        const centerY = meta.data[0].y;

                        ctx.save();

                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'middle';

                        // Total Count
                        ctx.fillStyle = '#0f172a';
                        ctx.font = 'bold 28px Arial';
                        ctx.fillText(
                            total,
                            centerX,
                            centerY - 10
                        );

                        // Label
                        ctx.fillStyle = '#64748b';
                        ctx.font = '13px Arial';
                        ctx.fillText(
                            'Total Activities',
                            centerX,
                            centerY + 15
                        );

                        ctx.restore();
                    }
                };

                // =========================
                // OUTSIDE VALUE LABELS
                // =========================
                const valueLabelPlugin = {

                    id: 'valueLabelPlugin',

                    afterDatasetsDraw(chart) {

                        const { ctx } = chart;

                        const meta =
                            chart.getDatasetMeta(0);

                        meta.data.forEach((arc, index) => {

                            const value =
                                chart.data.datasets[0].data[index];

                            if (value === 0)
                                return;

                            const angle =
                                (arc.startAngle + arc.endAngle) / 2;

                            const radius =
                                arc.innerRadius +
                                (arc.outerRadius - arc.innerRadius) / 2;

                            const x =
                                arc.x + Math.cos(angle) * radius;

                            const y =
                                arc.y + Math.sin(angle) * radius;

                            ctx.fillStyle = '#fff';
                            ctx.font = 'bold 14px Arial';

                            ctx.fillText(value, x, y);

                            //ctx.save();

                            //ctx.fillStyle = '#000';
                            //ctx.font = 'bold 13px Arial';
                            //ctx.textAlign = 'center';
                            //ctx.textBaseline = 'middle';

                            //ctx.fillText(
                            //    value,
                            //    x,
                            //    y
                            //);

                            ctx.restore();

                        });
                    }
                };

                // =========================
                // CREATE CHART
                // =========================

                window.activityStatusChartInstance =
                    new Chart(ctx, {

                        type: 'doughnut',

                        data: {

                            labels: labels,

                            datasets: [{
                                data: counts,

                                backgroundColor: [
                                    '#22c55e', // Completed
                                    '#f59e0b', // Ongoing
                                    '#64748b', // Not Started
                                    '#ef4444'  // Discontinued
                                ],

                                borderColor: '#ffffff',
                                borderWidth: 2,
                                hoverOffset: 8
                            }]
                        },

                        options: {

                            responsive: true,

                            maintainAspectRatio: false,

                            cutout: '65%',

                            plugins: {

                                legend: {

                                    position: 'bottom',

                                    labels: {

                                        usePointStyle: true,

                                        padding: 20,

                                        font: {
                                            size: 13,
                                            weight: '600'
                                        }
                                    }
                                },

                                tooltip: {

                                    callbacks: {

                                        label: function (context) {

                                            return context.label +
                                                ': ' +
                                                context.raw;
                                        }
                                    }
                                }
                            }
                        },

                        plugins: [
                            centerTextPlugin,
                            valueLabelPlugin
                        ]

                    });
            },

            error: function () {

                console.error(
                    "Error loading activity status chart"
                );

            }
        });
    },
    loadChartTask: function () {

        $.ajax({
            url: '/Home/GetTaskStatusChart',
            type: 'GET',

            success: function (response) {
                debugger;
                const ctx = document.getElementById("taskChart");

                if (!ctx)
                    return;

                const labels = response.map(x => x.taskStatus);
                const counts = response.map(x => x.totalCount);
                const totalCount = counts.reduce((sum, value) => sum + value, 0);
                $("#totalTaskCount").text(totalCount);

                if (window.taskChartInstance) {
                    window.taskChartInstance.destroy();
                }

                window.taskChartInstance = new Chart(ctx, {

                    type: 'bar',

                    data: {

                        labels: labels,

                        datasets: [{
                            data: counts,

                            // Aapke 8 statuses ke liye 8 premium colors
                            backgroundColor: [
                                '#4CAF50', // Completed (Green)
                                '#FF9800', // Partially On Track (Orange)
                                '#2196F3', // On Track (Blue)
                                '#FF5722', // Delayed/Constrained (Deep Orange)
                                '#9E9E9E', // Discontinued (Grey)
                                '#00BCD4', // In Progress (Cyan)
                                '#673AB7', // Not Started (Purple)
                                '#F44336'  // Delayed (Red)
                            ],

                            /*borderRadius: 8,*/
                            borderSkipped: false,
                            barThickness: 22
                        }]
                    },

                    options: {

                        indexAxis: 'y',

                        responsive: true,

                        maintainAspectRatio: false,

                        plugins: {

                            legend: {
                                display: false
                            },

                            tooltip: {

                                callbacks: {

                                    label: function (context) {

                                        return context.raw + " Tasks";

                                    }

                                }

                            }

                        },

                        scales: {

                            x: {

                                beginAtZero: true,

                                grid: {
                                    color: "#ECECEC"
                                },

                                ticks: {
                                    stepSize: 20
                                }

                            },

                            y: {

                                grid: {
                                    display: false
                                },

                                ticks: {

                                    color: "#374151",

                                    font: {
                                        size: 13,
                                        weight: "600"
                                    }

                                }

                            }

                        }

                    },

                    plugins: [{
                        id: 'totalCount',

                        beforeDraw(chart) {

                            const { ctx, chartArea } = chart;

                            ctx.save();

                            ctx.font = "bold 22px Arial";
                            ctx.fillStyle = "#0d6efd";
                            ctx.textAlign = "center";

                            ctx.fillText(
                                "Total : " + totalCount,
                                (chartArea.left + chartArea.right) / 2,
                                chartArea.top - 15
                            );

                            ctx.restore();
                        }
                    },
                    {
                        id: 'valueLabel',

                        afterDatasetsDraw(chart) {

                            const { ctx } = chart;

                            ctx.save();

                            ctx.font = "bold 12px Arial";
                            ctx.fillStyle = "#fff";
                            ctx.textAlign = "right";
                            ctx.textBaseline = "middle";

                            chart.getDatasetMeta(0).data.forEach(function (bar, index) {

                                const value = chart.data.datasets[0].data[index];

                                ctx.fillText(value, bar.x - 8, bar.y);

                            });

                            ctx.restore();
                        }
                    }]

                });

            },

            error: function () {

                console.log("Error loading task chart.");

            }

        });

    },
}