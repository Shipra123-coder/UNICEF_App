var actdetail = {
    loadFullData: function (guid) {       
        $.ajax({
            url: '/Management/GetFullData',
            type: 'GET',
            data: { guid: guid },

            success: function (res) {
                actdetail.bindHeader(res.activity);
                actdetail.bindDepartments(res.departments);
                actdetail.bindGoals(res.goals);
                actdetail.bindPillars(res.pillars);
                actdetail.bindSubActivities(res.activity?.subActivities);
                actdetail.bindSummary(res);
            }
        });
    },
    bindHeader: function (activity) {

        $('.fw-bold.mb-1').text(`🌍 ${activity.activityName}`);
        $('.text-muted').text(`Short Name: ${activity.shortName} | Created: ${actdetail.formatDate(activity.createdDate)}`);
        $('.text-muted1').text(`Description : ${activity.description}`);

    },
    bindSummary: function (res) {

        $('.stat-card:eq(0) h3').text(res.departments.length);
        $('.stat-card:eq(1) h3').text(actdetail.getUniqueGoals(res.goals).length);
        $('.stat-card:eq(2) h3').text(actdetail.getUniqueSectors(res.pillars).length);
        $('.stat-card:eq(3) h3').text(actdetail.getUniquePillars(res.pillars).length);

    },

    getUniqueGoals: function (data) {
        return [...new Set(data.map(x => x.goalId))];
    },

    getUniquePillars: function (data) {
        return [...new Set(data.map(x => x.pillarId))];
    },

    getUniqueSectors: function (data) {
        return [...new Set(data.map(x => x.sectorId))];
    },
    bindDepartments: function (data) {

        let nodal = data.find(x => x.isNodal == 1);
        let supporting = data.filter(x => x.isNodal == 0);

        // Nodal
        $('.nodal-dept-box span').text(nodal?.departmentName || 'N/A');

        // Supporting
        let html = '';
        supporting.forEach(d => {
            html += `<span class="badge bg-light text-dark border">${d.departmentName}</span>`;
        });

        $('#supportDeptContainer').html(html);
    },
    bindGoals: function (data) {

        let grouped = {};

        data.forEach(item => {

            if (!grouped[item.goalId]) {
                grouped[item.goalId] = {
                    name: item.goalName,
                    targets: []
                };
            }

            grouped[item.goalId].targets.push(item.targetName);
        });

        let html = '';

        Object.keys(grouped).forEach((gId, index) => {

            let goal = grouped[gId];

            let targets = goal.targets.map(t =>
                `<li class="list-group-item">${t}</li>`
            ).join('');

            html += `
        <div class="accordion-item">
            <h2 class="accordion-header">
                <button class="accordion-button ${index !== 0 ? 'collapsed' : ''}"
                    data-bs-toggle="collapse"
                    data-bs-target="#g${gId}">
                    ${goal.name}
                </button>
            </h2>

            <div id="g${gId}" class="accordion-collapse collapse ${index === 0 ? 'show' : ''}">
                <div class="accordion-body">
                    <ul class="list-group">${targets}</ul>
                </div>
            </div>
        </div>`;
        });

        $('#goalAccordion').html(html);
    },
    bindPillars: function (data) {

        let grouped = {};

        data.forEach(item => {

            if (!grouped[item.pillarId]) {
                grouped[item.pillarId] = {
                    name: item.pillarName,
                    sectors: {}
                };
            }

            if (!grouped[item.pillarId].sectors[item.sectorId]) {
                grouped[item.pillarId].sectors[item.sectorId] = {
                    name: item.sectorName,
                    subsectors: []
                };
            }

            grouped[item.pillarId].sectors[item.sectorId]
                .subsectors.push(item.subSectorName);
        });

        let html = '';

        Object.keys(grouped).forEach((pId, index) => {

            let pillar = grouped[pId];

            let sectorHtml = '';

            Object.values(pillar.sectors).forEach(sec => {

                let subs = sec.subsectors.map(s =>
                    `<li class="list-group-item">${s}</li>`
                ).join('');

                sectorHtml += `
            <div class="mb-3">
                <div class="fw-bold text-primary">Sector: ${sec.name}</div>
                <ul class="list-group">${subs}</ul>
            </div>`;
            });

            html += `
        <div class="accordion-item">
            <h2 class="accordion-header">
                <button class="accordion-button ${index !== 0 ? 'collapsed' : ''}"
                    data-bs-toggle="collapse"
                    data-bs-target="#p${pId}">
                    ${pillar.name}
                </button>
            </h2>

            <div id="p${pId}" class="accordion-collapse collapse ${index === 0 ? 'show' : ''}">
                <div class="accordion-body">
                    ${sectorHtml}
                </div>
            </div>
        </div>`;
        });

        $('#pillarAccordion').html(html);
    },
    bindSubActivities: function (data) {

        if (!data) return;

        let html = '';

        data.forEach((sub, index) => {

            let tasks = sub.tasks.map(t => {

                let geoHtml = '';

                if (t.geoLevelList && t.geoLevelList.length > 0) {

                    geoHtml = t.geoLevelList.map(g => {

                        let detail = '';

                        // District
                        if (g.districtName) {
                            detail += `
                    <span class="badge bg-primary me-1">
                        District : ${g.districtName}
                    </span>
                `;
                        }

                        // Block
                        if (g.blockName) {
                            detail += `
                    <span class="badge bg-success me-1">
                        Block : ${g.blockName}
                    </span>
                `;
                        }

                        // City
                        if (g.cityName) {
                            detail += `
                    <span class="badge bg-warning text-dark me-1">
                        City : ${g.cityName}
                    </span>
                `;
                        }

                        return `
                <div class="geo-level-box mt-2">                  

                    <div>
                        ${detail}
                    </div>

                </div>
            `;

                    }).join('');
                }

                return `
        <div class="border rounded p-3 mb-3 bg-light">

            <div class="fw-bold mb-2">
                ✔ ${t.taskName}
            </div>

            ${geoHtml}

        </div>
    `;

            }).join('');

            html += `
        <div class="accordion-item">
            <h2 class="accordion-header">
                <button class="accordion-button ${index !== 0 ? 'collapsed' : ''}"
                    data-bs-toggle="collapse"
                    data-bs-target="#sub${index}">
                    📁 ${sub.subActivityName}
                    <span class="badge bg-primary ms-2">${sub.tasks.length} Tasks</span>
                </button>
            </h2>

            <div id="sub${index}" class="accordion-collapse collapse ${index === 0 ? 'show' : ''}">
                <div class="accordion-body">
                    <ul class="task-list">${tasks}</ul>
                </div>
            </div>
        </div>`;
        });

        $('#subActivityAccordion').html(html);
    },
    formatDate: function (dateStr) {
        let d = new Date(dateStr);
        return d.toLocaleDateString('en-GB', {
            day: '2-digit',
            month: 'short',
            year: 'numeric'
        });
    },
}