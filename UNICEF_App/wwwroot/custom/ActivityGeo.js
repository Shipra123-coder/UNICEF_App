// ==========================================
// GEO LEVEL OBJECT
// ==========================================

var geolevel = {    
    getdata: function () {
        var model = {
            ActivityGuid: $('#ActivityGuid').val(),
        }
        ajax.doPostAjax('/Management/GetActivityDetails', model, function (response) {
            console.log("Full Response:", response); // <-- Check here

            if (response != null) {
                // 1. Case Sensitivity handle karein (ActivityName vs activityName)
                var title = response.activityName || response.ActivityName || "No Title Found";
                //alert(title);
                // 2. Text update karein
                $('.activity-title').text(`🌍 ${title}`);               
                debugger;
                geolevel.renderActivity(response);
            } else {
                console.error("Response is null");
            }
        });
    },
    // ======================================
    // RENDER ACTIVITY
    // ======================================
    renderActivity: function (activityData) {

        $("#activityContainer").html('');

        // ====================================================
        // CASE 1 : HAS SUB ACTIVITY
        // ====================================================
        console.log(activityData);
        if (activityData.hasSubActivity &&
            activityData.subActivities &&
            activityData.subActivities.length > 0) {

            $.each(activityData.subActivities, function (i, sub) {

                let taskHtml = '';

                $.each(sub.tasks, function (j, task) {

                    taskHtml += geolevel.renderTaskCard(task);

                });

                $("#activityContainer").append(`

                <div class="mb-5">

                    <h4 class="mb-4 fw-bold text-primary">

                        ${sub.subActivityName}

                    </h4>

                    ${taskHtml}

                </div>

            `);

            });

        }

        // ====================================================
        // CASE 2 : DIRECT TASKS (WITHOUT SUB ACTIVITY)
        // ====================================================
        
        else if (!activityData.hasSubActivity &&
            activityData.directTasks &&
            activityData.directTasks.length > 0) {

            let taskHtml = '';

            $.each(activityData.directTasks, function (j, task) {

                taskHtml += geolevel.renderTaskCard(task);

            });

            $("#activityContainer").append(`

            <div class="mb-5">

                <h4 class="mb-4 fw-bold text-primary">

                    Direct Tasks

                </h4>

                ${taskHtml}

            </div>

        `);

        }

        // ====================================================
        // NO TASK FOUND
        // ====================================================

        else {

            $("#activityContainer").html(`

            <div class="alert alert-warning">

                No Tasks Available

            </div>

        `);

        }

        // ==================================
        // ADD GEO BUTTON
        // ==================================

        $(".addGeoBtn")
            .off("click")
            .on("click", function () {

                let taskId = $(this).data("taskid");

                geolevel.addGeoLevel(taskId);

            });

        // ==================================
        // DELETE GEO
        // ==================================

        $(document)
            .off("click", ".deleteGeoBtn")
            .on("click", ".deleteGeoBtn", function () {

                let geoId = $(this).data("id");
                let taskId = $(this).data("taskid");

                if (!confirm("Are you sure want to delete?")) {
                    return;
                }

                let btn = $(this);

                $.ajax({

                    url: '/Management/DeleteGeoLevel',
                    type: 'POST',
                    data: { geoId: geoId },

                    success: function (res) {

                        if (res.status) {

                            btn.closest("tr").remove();

                            // empty row
                            if ($("#geoTableBody_" + taskId + " tr").length == 0) {

                                $("#geoTableBody_" + taskId).html(`

                                <tr>

                                    <td colspan="4"
                                        class="text-center text-muted">

                                        No Geo Level Added

                                    </td>

                                </tr>

                            `);

                            }

                            alert("Deleted Successfully");

                        }
                        else {

                            alert(res.message);

                        }

                    },

                    error: function () {

                        alert("Error while deleting");

                    }

                });

            });

    },


    // ======================================
    // COMMON TASK CARD
    // ======================================

    renderTaskCard: function (task) {

        let savedGeoHtml = '';

        // ======================================
        // SAVED GEO LEVEL HTML
        // ======================================

        if (task.geoLevelList &&
            task.geoLevelList.length > 0) {

            $.each(task.geoLevelList, function (x, geo) {

                // ======================================
                // LOCATION NAME
                // ======================================

                let locationName = '-';

                if (geo.geoLevel == "State") {

                    locationName = 'All State';

                }
                else if (geo.geoLevel == "District") {

                    locationName = geo.districtName;

                }
                else if (geo.geoLevel == "Block") {

                    locationName = geo.blockName;

                }
                else if (geo.geoLevel == "City" || geo.geoLevel == "ULB") {

                    locationName = geo.cityName;

                }

                // ======================================
                // SECOND COLUMN
                // ======================================

                let secondColumn = '-';

                if (geo.geoLevel == "Block" ||
                    geo.geoLevel == "City" ||
                    geo.geoLevel == "ULB") {

                    secondColumn = geo.districtName || '-';

                }

                savedGeoHtml += `

                <tr>

                    <td>
                        ${geo.geoLevel}
                    </td>

                    <td>
                        ${locationName}
                    </td>

                    <td>
                        ${secondColumn}
                    </td>

                    <td>

                        <button type="button"
                                class="btn btn-sm btn-danger deleteGeoBtn"
                                data-id="${geo.geoId}"
                                data-taskid="${task.taskId}">

                            Delete

                        </button>

                    </td>

                </tr>

            `;

            });

        }

        // ======================================
        // RETURN TASK HTML
        // ======================================

        return `

        <div class="border rounded p-3 mb-4 shadow-sm">

            <div class="d-flex justify-content-between align-items-center">

                <div>

                    <div class="fw-semibold fs-5">

                        ${task.taskName}

                    </div>

                </div>

                <div>

                    <button type="button"
                            class="btn btn-primary addGeoBtn"
                            data-taskid="${task.taskId}">

                        <i class="bi bi-geo-alt"></i>
                        Add Geo Level

                    </button>

                </div>

            </div>

            <!-- GEO FORM -->

            <div class="geoLevelContainer mt-4"
                 id="geoContainer_${task.taskId}">

            </div>

            <!-- SAVED GEO LEVEL -->

            <div class="mt-4">

                <div class="fw-bold mb-2">

                    Saved Geo Levels

                </div>

                <div class="table-responsive">

                    <table class="table table-bordered align-middle">

                        <thead class="table-light">

                            <tr>

                                <th style="width:150px;">
                                    Geo Level
                                </th>

                                <th>
                                    Location
                                </th>

                                <th>
                                    District
                                </th>

                                <th style="width:90px;">
                                    Action
                                </th>

                            </tr>

                        </thead>

                        <tbody id="geoTableBody_${task.taskId}">

                            ${savedGeoHtml || `

                                <tr>

                                    <td colspan="4"
                                        class="text-center text-muted">

                                        No Geo Level Added

                                    </td>

                                </tr>

                            `}

                        </tbody>

                    </table>

                </div>

            </div>

        </div>

    `;

    },

    // ======================================
    // ADD GEO LEVEL
    // ======================================
    addGeoLevel: function (taskId) {
        $(".geoLevelContainer").html('');
        $("#geoContainer_" + taskId).html(`
            <div class="border rounded p-4 bg-light">
                <h5 class="mb-4">
                    Select Geo Level
                </h5>
                <!-- RADIO -->
                <div class="d-flex gap-4 mb-4 flex-wrap">
                    <div class="form-check">
                        <input class="form-check-input geo-radio"
                               type="radio"
                               name="geo_${taskId}"
                               value="State">
                        <label class="form-check-label">
                            State
                        </label>
                    </div>
                    <div class="form-check">
                        <input class="form-check-input geo-radio"
                               type="radio"
                               name="geo_${taskId}"
                               value="District">
                        <label class="form-check-label">
                            District
                        </label>
                    </div>
                    <div class="form-check">
                        <input class="form-check-input geo-radio"
                               type="radio"
                               name="geo_${taskId}"
                               value="Block">
                        <label class="form-check-label">
                            Block
                        </label>
                    </div>
                    <div class="form-check">
                        <input class="form-check-input geo-radio"
                               type="radio"
                               name="geo_${taskId}"
                               value="City">
                        <label class="form-check-label">
                            City
                        </label>
                    </div>
                </div>
                <!-- DYNAMIC AREA -->
                <div class="geoDynamicArea"></div>

            </div>
        `);
        // ==================================
        // RADIO CHANGE
        // ==================================
        $("#geoContainer_" + taskId + " .geo-radio")
            .off("change")
            .on("change", function () {
                let value = $(this).val();
                geolevel.renderGeoDropdowns(taskId, value);
            });
    },



    // ======================================
    // RENDER DROPDOWN
    // ======================================
    renderGeoDropdowns: function (taskId, selectedValue) {



        let html = '';



        // ==================================
        // STATE
        // ==================================

        if (selectedValue == "State") {

            html = `

        <div class="alert alert-success 
                    d-flex 
                    justify-content-between 
                    align-items-center">

            <span>
                State Level Selected
            </span>

            <button type="button"
                    class="btn btn-primary btn-sm addStateBtn"
                    data-taskid="${taskId}">

                + Add

            </button>

        </div>

    `;

        }



        // ==================================
        // DISTRICT
        // ==================================
        if (selectedValue == "District") {
            html = `
        <div class="row align-items-end">
            <div class="col-md-8">
                <label class="form-label fw-semibold">Select District</label>
                <select class="form-select districtDropdown" multiple></select>
            </div>
            <div class="col-md-2 mb-2">
                <div class="form-check">
                    <input type="checkbox" class="form-check-input selectAllDistricts" id="selectAllDist_${taskId}">
                    <label class="form-check-label" for="selectAllDist_${taskId}">Select All</label>
                </div>
            </div>
            <div class="col-md-2">
                <button type="button" class="btn btn-primary w-100 addDistrictBtn">Add</button>
            </div>
        </div>
        <div class="selectedGeoList mt-4"></div>`;
        }
        // ==================================
        // BLOCK
        // ==================================
        if (selectedValue == "Block") {
            html = `
        <div class="row align-items-end">
            <div class="col-md-4">
                <label class="form-label fw-semibold">Select District</label>
                <select class="form-select blockDistrictDropdown">
                    <option value="">Select District</option>
                </select>
            </div>
            <div class="col-md-4">
                <label class="form-label fw-semibold">Select Block</label>
                <select class="form-select blockDropdown" multiple></select>
            </div>
            <div class="col-md-2 mb-2">
                <div class="form-check">
                    <input type="checkbox" class="form-check-input selectAllBlocks" id="selectAllBlock_${taskId}">
                    <label class="form-check-label" for="selectAllBlock_${taskId}">Select All</label>
                </div>
            </div>
            <div class="col-md-2">
                <button type="button" class="btn btn-primary w-100 addBlockBtn">Add</button>
            </div>
        </div>
        <div class="selectedGeoList mt-4"></div>`;
        }

        // ==================================
        // CITY
        // ==================================
        if (selectedValue == "City") {
            html = `
        <div class="row align-items-end">
            <div class="col-md-4">
                <label class="form-label fw-semibold">Select District</label>
                <select class="form-select cityDistrictDropdown">
                    <option value="">Select District</option>
                </select>
            </div>
            <div class="col-md-4">
                <label class="form-label fw-semibold">Select City</label>
                <select class="form-select cityDropdown" multiple></select>
            </div>
            <div class="col-md-2 mb-2">
                <div class="form-check">
                    <input type="checkbox" class="form-check-input selectAllCities" id="selectAllCity_${taskId}">
                    <label class="form-check-label" for="selectAllCity_${taskId}">Select All</label>
                </div>
            </div>
            <div class="col-md-2">
                <button type="button" class="btn btn-primary w-100 addCityBtn">Add</button>
            </div>
        </div>
        <div class="selectedGeoList mt-4"></div>`;
        }





        // ==================================
        // BIND HTML
        // ==================================

        $("#geoContainer_" + taskId + " .geoDynamicArea").html(html);





        // ==================================
        // LOAD DISTRICT
        // ==================================

        geolevel.loadDistricts(taskId);





        // ==================================
        // INIT SELECT2
        // ==================================

        // ... baaki code same ...
        setTimeout(function () {
            var container = $("#geoContainer_" + taskId);

            // Sirf Select2 initialize karein
            container.find(".districtDropdown").select2({
                placeholder: "Select District",
                width: '100%',
                allowClear: true
            });

            container.find(".blockDropdown").select2({
                placeholder: "Select Block",
                width: '100%',
                allowClear: true
            });

            container.find(".cityDropdown").select2({
                placeholder: "Select City",
                width: '100%',
                allowClear: true
            });
        }, 100);
        // ... baaki code same ...





        // ==================================
        // BLOCK DISTRICT CHANGE
        // ==================================

        $("#geoContainer_" + taskId + " .blockDistrictDropdown")
            .off("change")
            .on("change", function () {

                let districtId = $(this).val();

                geolevel.loadBlocksByDistrict(taskId, districtId);

            });





        // ==================================
        // CITY DISTRICT CHANGE
        // ==================================

        $("#geoContainer_" + taskId + " .cityDistrictDropdown")
            .off("change")
            .on("change", function () {

                let districtId = $(this).val();

                geolevel.loadCitiesByDistrict(taskId, districtId);

            });

        // ==================================
        // ADD STATE + SAVE BACKEND
        // ==================================

        $("#geoContainer_" + taskId + " .addStateBtn")
            .off("click")
            .on("click", function () {

                // ==============================
                // REQUEST MODEL
                // ==============================

                let requestModel = {

                    taskId: taskId,
                    geoLevel: "State"
                };



                // ==============================
                // AJAX SAVE
                // ==============================

                $.ajax({

                    url: '/Management/SaveGeoLevel',

                    type: 'POST',

                    contentType: 'application/json',

                    data: JSON.stringify(requestModel),

                    success: function (response) {

                        if (response.status == true) {

                            geolevel.getdata();

                            // OPTIONAL HTML APPEND

                            // let html = `
                            //
                            // <tr>
                            //     <td>State</td>
                            //
                            //     <td>Rajasthan</td>
                            //
                            //     <td>
                            //         <span class="badge bg-success">
                            //             Saved
                            //         </span>
                            //     </td>
                            // </tr>
                            //
                            // `;
                            //
                            // $("#geoContainer_" + taskId + " .selectedGeoList")
                            //     .append(html);

                            alert(response.message);

                        }
                        else {

                            alert(response.message);

                        }

                    },

                    error: function () {

                        alert("Error while saving");

                    }

                });

            });
        // ==================================
        // ADD DISTRICT + SAVE BACKEND
        // ==================================

        $("#geoContainer_" + taskId + " .addDistrictBtn")
            .off("click")
            .on("click", function () {

                let districtIds =
                    $("#geoContainer_" + taskId + " .districtDropdown").val();

                let districtTexts =
                    $("#geoContainer_" + taskId + " .districtDropdown option:selected");



                // ==============================
                // VALIDATION
                // ==============================

                if (!districtIds || districtIds.length == 0) {

                    alert("Please Select District");
                    return;
                }



                // ==============================
                // REQUEST MODEL
                // ==============================

                let requestModel = {

                    taskId: taskId,
                    geoLevel: "District",
                    districtIds: districtIds
                };



                // ==============================
                // AJAX SAVE
                // ==============================

                $.ajax({

                    url: '/Management/SaveGeoLevel',

                    type: 'POST',

                    contentType: 'application/json',

                    data: JSON.stringify(requestModel),

                    success: function (response) {

                        if (response.status == true) {
                            geolevel.getdata();
                        //    let html = '';

                        //    districtTexts.each(function () {

                        //        html += `

                        //    <tr>
                        //        <td>District</td>

                        //        <td>${$(this).text()}</td>

                        //        <td>
                        //            <span class="badge bg-success">
                        //                Saved
                        //            </span>
                        //        </td>
                        //    </tr>

                        //`;
                        //    });



                        //    $("#geoContainer_" + taskId + " .selectedGeoList")
                        //        .append(html);



                            // RESET
                            $("#geoContainer_" + taskId + " .districtDropdown")
                                .val(null)
                                .trigger('change');



                            alert(response.message);
                        }
                        else {

                            alert(response.message);
                        }
                    },

                    error: function () {

                        alert("Error while saving");
                    }

                });

            });

        // ==================================
        // ADD BLOCK + SAVE BACKEND
        // ==================================

        $("#geoContainer_" + taskId + " .addBlockBtn")
            .off("click")
            .on("click", function () {

                let districtId =
                    $("#geoContainer_" + taskId + " .blockDistrictDropdown").val();

                let districtName =
                    $("#geoContainer_" + taskId + " .blockDistrictDropdown option:selected").text();

                let blockIds =
                    $("#geoContainer_" + taskId + " .blockDropdown").val();

                let blockTexts =
                    $("#geoContainer_" + taskId + " .blockDropdown option:selected");



                // ==============================
                // VALIDATION
                // ==============================

                if (!districtId) {

                    alert("Please Select District");
                    return;
                }

                if (!blockIds || blockIds.length == 0) {

                    alert("Please Select Block");
                    return;
                }



                // ==============================
                // BLOCK ARRAY
                // ==============================

                let blocks = [];

                $.each(blockIds, function (i, item) {

                    blocks.push({

                        districtId: districtId,
                        blockId: item
                    });

                });



                // ==============================
                // REQUEST MODEL
                // ==============================

                let requestModel = {

                    taskId: taskId,
                    geoLevel: "Block",
                    blocks: blocks
                };



                // ==============================
                // AJAX SAVE
                // ==============================

                $.ajax({

                    url: '/Management/SaveGeoLevel',

                    type: 'POST',

                    contentType: 'application/json',

                    data: JSON.stringify(requestModel),

                    success: function (response) {

                        if (response.status == true) {
                            geolevel.getdata();
                        //    let html = '';

                        //    blockTexts.each(function () {

                        //        html += `

                        //    <tr>

                        //        <td>Block</td>

                        //        <td>
                        //            ${districtName} - ${$(this).text()}
                        //        </td>

                        //        <td>
                        //            <span class="badge bg-success">
                        //                Saved
                        //            </span>
                        //        </td>

                        //    </tr>

                        //`;
                        //    });



                            //$("#geoContainer_" + taskId + " .selectedGeoList")
                            //    .append(html);



                            // RESET
                            $("#geoContainer_" + taskId + " .blockDropdown")
                                .val(null)
                                .trigger('change');



                            alert(response.message);
                        }
                        else {

                            alert(response.message);
                        }
                    },

                    error: function () {

                        alert("Error while saving");
                    }

                });

            });

        // ==================================
        // ADD CITY + SAVE BACKEND
        // ==================================

        $("#geoContainer_" + taskId + " .addCityBtn")
            .off("click")
            .on("click", function () {

                let districtId =
                    $("#geoContainer_" + taskId + " .cityDistrictDropdown").val();

                let districtName =
                    $("#geoContainer_" + taskId + " .cityDistrictDropdown option:selected").text();

                let cityIds =
                    $("#geoContainer_" + taskId + " .cityDropdown").val();

                let cityTexts =
                    $("#geoContainer_" + taskId + " .cityDropdown option:selected");



                // ==============================
                // VALIDATION
                // ==============================

                if (!districtId) {

                    alert("Please Select District");
                    return;
                }

                if (!cityIds || cityIds.length == 0) {

                    alert("Please Select City");
                    return;
                }



                // ==============================
                // CITY ARRAY
                // ==============================

                let cities = [];

                $.each(cityIds, function (i, item) {

                    cities.push({

                        districtId: districtId,
                        cityId: item
                    });

                });



                // ==============================
                // REQUEST MODEL
                // ==============================

                let requestModel = {

                    taskId: taskId,
                    geoLevel: "City",
                    cities: cities
                };



                // ==============================
                // AJAX SAVE
                // ==============================

                $.ajax({

                    url: '/Management/SaveGeoLevel',

                    type: 'POST',

                    contentType: 'application/json',

                    data: JSON.stringify(requestModel),

                    success: function (response) {


                        if (response.status == true) {

                            geolevel.getdata();
                        //    let html = '';

                        //    cityTexts.each(function () {

                        //        html += `

                        //    <tr>

                        //        <td>City</td>

                        //        <td>
                        //            ${districtName} - ${$(this).text()}
                        //        </td>

                        //        <td>
                        //            <span class="badge bg-success">
                        //                Saved
                        //            </span>
                        //        </td>

                        //    </tr>

                        //`;
                        //    });



                        //    $("#geoContainer_" + taskId + " .selectedGeoList")
                        //        .append(html);



                        //    // RESET
                        //    $("#geoContainer_" + taskId + " .cityDropdown")
                        //        .val(null)
                        //        .trigger('change');



                            alert(response.message);
                        }
                        else {

                            alert(response.message);
                        }
                    },

                    error: function () {

                        alert("Error while saving");
                    }

                });

            });
    },



    // ======================================
    // LOAD DISTRICTS
    // ======================================
    loadDistricts: function (taskId) {
        $.ajax({
            url: '/Master/DDL_District',
            type: 'GET',
            success: function (response) {
                let districtOptions = '';
                $.each(response.data, function (i, item) {
                    districtOptions += `
                        <option value="${item.id}">
                            ${item.name}
                        </option>
                    `;
                });
                $("#geoContainer_" + taskId + " .districtDropdown")
                    .html(districtOptions);

                $("#geoContainer_" + taskId + " .blockDistrictDropdown")
                    .append(districtOptions);

                $("#geoContainer_" + taskId + " .cityDistrictDropdown")
                    .append(districtOptions);

            }

        });
    },



    // ======================================
    // LOAD BLOCKS
    // ======================================
    loadBlocksByDistrict: function (taskId, districtId) {
        $.ajax({
            url: '/Master/DDL_Block',
            type: 'GET',
            data: {
                districtId: districtId
            },
            success: function (response) {
                let options = '';
                $.each(response.data, function (i, item) {
                    options += `
                        <option value="${item.id}">
                            ${item.name}
                        </option>
                    `;
                });
                $("#geoContainer_" + taskId + " .blockDropdown")
                    .html(options)
                    .trigger('change');

            }

        });

    },


    // ======================================
    // LOAD CITIES
    // ======================================
    loadCitiesByDistrict: function (taskId, districtId) {
        $.ajax({
            url: '/Master/DDL_City',
            type: 'GET',
            data: {
                districtId: districtId
            },
            success: function (response) {
                let options = '';
                $.each(response.data, function (i, item) {
                    options += `

                        <option value="${item.id}">
                            ${item.name}
                        </option>
                    `;
                });
                $("#geoContainer_" + taskId + " .cityDropdown")
                    .html(options)
                    .trigger('change');

            }

        });

    }

};

// ==========================================
// GLOBAL EVENT HANDLERS (Dynamic Elements ke liye)
// ==========================================
// 1. Handle Select All Checkbox Change
$(document).on('change', '.selectAllDistricts, .selectAllBlocks, .selectAllCities', function () {
    let container = $(this).closest('.row');
    let dropdown = container.find('select[multiple]');
    let isChecked = $(this).is(':checked');

    if (isChecked) {
        dropdown.find('option').prop('selected', true);
    } else {
        dropdown.find('option').prop('selected', false);
    }
    dropdown.trigger('change');
});

// 2. Sync Checkbox based on manual Dropdown selection
$(document).on('change', '.districtDropdown, .blockDropdown, .cityDropdown', function () {
    let container = $(this).closest('.row');
    let checkbox = container.find('.selectAllDistricts, .selectAllBlocks, .selectAllCities');
    let totalOptions = $(this).find('option').length;
    let selectedOptions = $(this).val() ? $(this).val().length : 0;

    if (totalOptions > 0 && totalOptions === selectedOptions) {
        checkbox.prop('checked', true);
    } else {
        checkbox.prop('checked', false);
    }
});
