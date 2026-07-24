var activityMonitor = {
    saveMonitoringData: function () {
        var self = this;
        var status = true;
        var formData = new FormData();

        // 🌟 हर बार सबमिट करने पर पुराने एरर हाइलाइट्स को साफ़ करें
        $(".doc-item, .support-item, input, select").removeClass('errr-highlight');

        // ==========================================
        // 🔹 1. Partnership Validation
        // ==========================================
        var isPartnership = $("input[name='partnership']:checked").val();
        if (!isPartnership) {
            toast.showToast('Validation Error', 'Please select if Partnership is applicable (Yes/No).', 'error');
            return false;
        }

        formData.append("IsPartnership", isPartnership);

        // ==========================================
        // 🔹 2. Government Validation
        // ==========================================
        var isGovernment = $("input[name='isGovernment']:checked").val();

        if (!isGovernment) {
            toast.showToast('Validation Error', 'Please select if Government contribution is applicable (Yes/No).', 'error');
            return false;
        }
        formData.append("IsGovernment", isGovernment);

        // ==========================================
        // 🔹 Government Details (Mandatory if Yes)
        // ==========================================
        if (isGovernment === "Yes") {
            var governmentDetails = $("#IsGovernmentDetails").val().trim();

            if (governmentDetails === "") {
                toast.showToast('Validation Error', 'Please enter Government contribution details.', 'error');
                $("#IsGovernmentDetails").focus();
                return false;
            }
            formData.append("IsGovernmentDetails", governmentDetails);
        }
        else {
            formData.append("IsGovernmentDetails", "");
        }

        formData.append("ActivityGuid", $("#hiddenActivityGuid").val());

        // ==========================================
        // 🔹 2. Documents (UPDATE SAFE + VALIDATION)
        // ==========================================
        var docIndex = 0;

        $(".options-container .doc-item").each(function () {
            var checkbox = $(this).find(".doc-check");
            var isChecked = checkbox.is(":checked");

            var docName = checkbox.parent().text().trim();
            var fileInput = $(this).find("input[type='file']")[0];
            var existingFile = $(this).find(".existing-file").val();
            var otherDocName = $(this).find(".other-doc-name").val() || "";

            if (isChecked) {

                if (docName === "Other" && otherDocName.trim() === "") {
                    status = false;
                    $(this).find(".other-doc-name").addClass("errr-highlight");
                }

                // 🔥 CASE 1: New File Upload
                if (fileInput && fileInput.files.length > 0) {
                    // फाइल साइज वैलिडेशन (मैनेज्ड सेफ्टी - Max 5MB)
                    if (fileInput.files[0].size > (5 * 1024 * 1024)) {
                        status = false;
                        $(this).addClass('errr-highlight');
                        toast.showToast('Size Error', `${docName} file size cannot exceed 5MB.`, 'error');
                    }
                    formData.append(`Documents[${docIndex}].DocumentType`, docName);
                    formData.append(`Documents[${docIndex}].OtherDocumentName`, otherDocName);
                    formData.append(`Documents[${docIndex}].File`, fileInput.files[0]);
                }
                // 🔥 CASE 2: Keep Old File
                else if (existingFile && existingFile !== "") {
                    formData.append(`Documents[${docIndex}].DocumentType`, docName);
                    formData.append(`Documents[${docIndex}].OtherDocumentName`, otherDocName);
                    formData.append(`Documents[${docIndex}].ExistingFileName`, existingFile);
                }
                // ❌ ERROR: डॉक्यूमेंट टिक किया है पर फाइल नहीं चुनी
                else {
                    status = false;
                    if (isPartnership === "Yes") {
                        $(this).addClass('errr-highlight');
                    }
                }
                docIndex++;
            }
        });

        // ❗ Partnership Document Mandatory Validation
        if (isPartnership === "Yes" && docIndex === 0) {
            toast.showToast('Validation Error', 'Partnership is marked "Yes". Please select and upload at least one mandatory document.', 'error');
            return false;
        }

        if (!status && isPartnership === "Yes") {
            toast.showToast('Validation Error', 'Please upload or retain the files for checked document options.', 'error');
            return false;
        }

        // ==========================================
        // 🔹 3. Financial Cross Validation (INR, USD, Source Matrix)
        // ==========================================
        var directInr = $("#directINR").val() ? $("#directINR").val().trim() : "";
        var directUsd = $("#directUSD").val() ? $("#directUSD").val().trim() : "";
        var directSrc = $("#directSource").val() ? $("#directSource").val().trim() : "";

        var indirectInr = $("#indirectINR").val() ? $("#indirectINR").val().trim() : "";
        var indirectUsd = $("#indirectUSD").val() ? $("#indirectUSD").val().trim() : "";
        var indirectSrc = $("#indirectSource").val() ? $("#indirectSource").val().trim() : "";

        // 🛑 Direct Fund Validation Group
        if (directInr !== "" || directUsd !== "" || directSrc !== "") {
            if (directInr === "" || parseFloat(directInr) < 0) { $("#directINR").addClass('errr-highlight'); status = false; }
            if (directUsd === "" || parseFloat(directUsd) < 0) { $("#directUSD").addClass('errr-highlight'); status = false; }
            if (directSrc === "") { $("#directSource").addClass('errr-highlight'); status = false; }

            if (!status) {
                toast.showToast('Financial Validation', 'For Direct Funding, please fill correct INR, USD values and select a valid Source.', 'error');
                return false;
            }
        }

        // 🛑 Indirect Fund Validation Group
        if (indirectInr !== "" || indirectUsd !== "" || indirectSrc !== "") {
            if (indirectInr === "" || parseFloat(indirectInr) < 0) { $("#indirectINR").addClass('errr-highlight'); status = false; }
            if (indirectUsd === "" || parseFloat(indirectUsd) < 0) { $("#indirectUSD").addClass('errr-highlight'); status = false; }
            if (indirectSrc === "") { $("#indirectSource").addClass('errr-highlight'); status = false; }

            if (!status) {
                toast.showToast('Financial Validation', 'For Indirect Funding, please fill correct INR, USD values and select a valid Source.', 'error');
                return false;
            }
        }

        // डेटा पेंड करें
        formData.append("DirectINR", directInr || "0");
        formData.append("DirectUSD", directUsd || "0");
        formData.append("DirectSource", directSrc);
        formData.append("IndirectINR", indirectInr || "0");
        formData.append("IndirectUSD", indirectUsd || "0");
        formData.append("IndirectSource", indirectSrc);

        // ==========================================
        // 🔹 4. Supports Details Validation
        // ==========================================
        var supIndex = 0;

        $(".support-item").each(function () {
            var checkbox = $(this).find(".support-check");

            if (checkbox.is(":checked")) {
                var label = checkbox.parent().text().trim();
                var targetId = checkbox.attr("data-target");
                var detailsInput = $("#" + targetId).find("input");
                var details = detailsInput.val() ? detailsInput.val().trim() : "";

                // 🛑 अगर चेक किया है तो अंदर का इनपुट खाली नहीं होना चाहिए
                if (details === "") {
                    detailsInput.addClass('errr-highlight');
                    $(this).addClass('errr-highlight');
                    status = false;
                }

                formData.append(`Supports[${supIndex}].SupportType`, label);
                formData.append(`Supports[${supIndex}].Details`, details);

                supIndex++;
            }
        });

        formData.append("ActivityMonitoringId", $("#hiddenActivityMonitoringId").val());

        // अंतिम स्टेटस चेक ब्रेक पॉइंट
        if (!status) {
            toast.showToast('Form Incomplete', 'Please fill all highlighted fields with proper contextual details.', 'error');
            return false;
        }

        // HTML में रेंडर हुए __RequestVerificationToken इनपुट बॉक्स से वैल्यू निकालें
        var token = $('input[name="__RequestVerificationToken"]').val();

        // ==========================================
        // 🔹 CONFIRM & SUBMIT VIA AJAX
        // ==========================================
        Swal.fire({
            title: 'Confirmation?',
            text: "Are you sure you want to save data?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#0d6efd',
            cancelButtonColor: '#6c757d',
            confirmButtonText: '<i class="bx bx-check-circle me-1"></i> Yes, Save Data'
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();

                $.ajax({
                    url: '/Management/SaveMonitoring',
                    type: 'POST',
                    data: formData,
                    contentType: false,
                    processData: false,
                    // 🌟 TOKEN ADDED HERE IN HEADERS BLOCK
                    headers: {
                        "RequestVerificationToken": token
                    },
                    success: function (r) {
                        common.HideLoader();

                        if (r.status || r.success) {
                            toast.showToast('Success', r.message || 'Monitoring transaction metrics logged.', 'success');
                            setTimeout(function () {
                                window.location.href = '/Management/ActivityManagement';
                            }, 1000);
                        } else {
                            toast.showToast('Execution Failure', r.message || 'Server rejected framework properties.', 'error');
                        }
                    },
                    error: function () {
                        common.HideLoader();
                        toast.showToast('Network Error', 'A critical frame lifecycle failure blocked background dataset syncing.', 'error');
                    }
                });
            }
        });
    },
    getMonitoringData: function (guid) {

        ajax.doPostAjax('/Management/GetMonitoring', { ActivityGuid: guid }, function (res) {

            if (res && res.data) {
                activityMonitor.bindMonitoringData(res.data);
            }
        });
    },
    bindMonitoringData: function (data) {

        // =========================
        // 🔹 1. Partnership (Radio)
        // =========================
        $("input[name='partnership'][value='" + data.isPartnership + "']")
            .prop("checked", true);

        // trigger show/hide section
        if (data.isPartnership === "Yes") {
            $("#specifySection").removeClass("hidden");
        }

        // =========================
        // 🔹 2. Documents
        // =========================
        if (data.documents && data.documents.length > 0) {

            data.documents.forEach(function (doc) {

                $(".options-container .doc-item").each(function () {

                    let label = $(this).find("label").text().trim();

                    if (label.includes(doc.documentType)) {

                        let checkbox = $(this).find("input[type='checkbox']");
                        checkbox.prop("checked", true);

                        let uploadId = checkbox.attr("data-toggle");

                        if (uploadId) {
                            $("#" + uploadId).removeClass("hidden");
                        }
                    }
                });

                // OTHER
                if (!["Letter Exchange", "Signed Work Plan", "Memorandum", "Letter of Understanding"]
                    .some(x => doc.documentType.includes(x))) {

                    $("#checkOther").prop("checked", true);
                    $("#uploadOtherContainer").removeClass("hidden");
                    $("#otherText").val(doc.documentType);
                }
            });
        }

        // =========================
        // 🔹 3. Financial
        // =========================
        if (data.financial) {

            $("#directINR").val(data.financial.directINR);
            $("#directUSD").val(data.financial.directUSD);

            $("#indirectINR").val(data.financial.indirectINR);
            $("#indirectUSD").val(data.financial.indirectUSD);

            // source
            $("#directINR").closest(".doc-item").find("input[type='text']")
                .val(data.financial.directSource);

            $("#indirectINR").closest(".doc-item").find("input[type='text']")
                .val(data.financial.indirectSource);
        }

        // =========================
        // 🔹 4. Support
        // =========================
        if (data.supports && data.supports.length > 0) {

            data.supports.forEach(function (sup) {

                $(".support-item").each(function () {

                    let label = $(this).find("label").text().trim();

                    if (label.includes(sup.supportType)) {

                        let checkbox = $(this).find("input[type='checkbox']");
                        checkbox.prop("checked", true);

                        let inputId = checkbox.attr("data-toggle");

                        if (inputId) {
                            $("#" + inputId).removeClass("hidden")
                                .find("input").val(sup.details);
                        }
                    }
                });
            });
        }
    },
    // =============================================
    // GET FULL DATA MAPPING (MINIMALIST EXECUTIVE VIEW)
    // =============================================
    getFullDataMap: function (guid) {
        var model = {
            guid: guid
        };

        ajax.doPostAjax('/Management/GetFullData', model, function (response) {

            // Safety verification check
            if (!response || !response.activity) {
                $('#activityDetailsContainer').html(`
                <div class="col-12 text-center text-danger py-4">
                    <i class="bx bx-error-circle font_size_18 mb-2"></i>
                    <h6 class="fw-bold mb-0">Configuration Unresolved</h6>
                    <small class="text-muted">Activity data mapping profile could not be compiled cleanly.</small>
                </div>
            `);
                return;
            }

            // 1. Process Core Activity Information
            var act = response.activity;
            var activityName = act.activityName || "N/A";

            // Activity Period Parsing Logic (Clean Badge style)
            var periodHtml = "";
            if (act.activityStartDate && act.activityEndDate) {
                periodHtml = `
                <span class="badge bg-light text-dark border fw-bold px-2.5 py-1.5">
                    <i class="bx bx-calendar text-primary me-1"></i> ${act.activityStartDate} 
                    <span class="text-muted fw-normal mx-1">to</span> 
                    <i class="bx bx-calendar-check text-success me-1"></i> ${act.activityEndDate}
                </span>`;
            } else {
                periodHtml = `<small class="text-muted italic">Period missing</small>`;
            }

            // 2. Separate Nodal Department from Supporting Departments
            var primaryNodalDept = "Not assigned";
            var associatedDeptsHtml = "";

            if (response.departments && response.departments.length > 0) {
                response.departments.forEach(function (dept) {
                    if (dept.isNodal === true || dept.isNodal === 1) {
                        primaryNodalDept = dept.departmentName;
                    } else {
                        associatedDeptsHtml += `<span class="badge bg-white text-secondary border px-2 py-1.5 me-2 mb-2 fw-normal"><i class="bx bx-building me-1 opacity-75"></i>${dept.departmentName}</span>`;
                    }
                });
            }
            if (!associatedDeptsHtml) {
                associatedDeptsHtml = `<small class="text-muted italic">No secondary departments linked</small>`;
            }

            // 3. Process Strategic Goals (SDG)
            var uniqueGoals = [];
            var goalsHtml = "";
            if (response.goals && response.goals.length > 0) {

                response.goals.forEach(function (g) {
                    if (g.goalName && $.inArray(g.goalName, uniqueGoals) === -1) {
                        uniqueGoals.push(g.goalName);
                        goalsHtml += `<span class="badge bg-white text-primary border border-primary-subtle px-2 py-1.5 me-2 mb-2 fw-semibold"><i class="bx bx-target-lock me-1"></i>${g.goalName}</span>`;
                    }
                });
            } else {
                goalsHtml = `<small class="text-muted italic">No strategic SDG goals mapped</small>`;
            }

            // 4. Process Strategic Pillars
            var pillarsHtml = "";
            if (response.pillars && response.pillars.length > 0) {
                // 🌟 1. pillarId के आधार पर DISTINCT लिस्ट निकालें
                var distinctPillars = response.pillars.filter((value, index, self) =>
                    index === self.findIndex((t) => t.pillarId === value.pillarId)
                );

                // 🌟 2. अब distinctPillars लिस्ट के ऊपर लूप चलाएं
                distinctPillars.forEach(function (p) {
                    if (p.pillarName) {
                        pillarsHtml += `<span class="badge bg-white text-success border border-success-subtle px-2 py-1.5 me-2 mb-2 fw-semibold"><i class="bx bx-shield me-1"></i>${p.pillarName}</span>`;
                    }
                });
            } else {
                pillarsHtml = `<small class="text-muted italic">No policy pillars mapped</small>`;
            }

            // 4. Process Nature Of Support
            var natureHtml = "";
            if (response.natureofsupport && response.natureofsupport.length > 0) {
                // 🌟 1. supportId के आधार पर DISTINCT लिस्ट निकालें
                var distinctNatureofsupport = response.natureofsupport.filter((value, index, self) =>
                    index === self.findIndex((t) => t.supportId === value.supportId)
                );

                // 🌟 2. अब distinctPillars लिस्ट के ऊपर लूप चलाएं
                distinctNatureofsupport.forEach(function (p) {
                    if (p.supportName) {
                        natureHtml += `<span class="badge bg-white  text-primary border border-success-subtle px-2 py-1.5 me-2 mb-2 fw-semibold"><i class="bx bx-shield me-1"></i>${p.supportName}</span>`;
                    }
                });
            } else {
                natureHtml = `<small class="text-muted italic">No mapped</small>`;
            }


            // 5. Minimalist UI Layout Markup (Clean List style)
            var html = `
            <div class="col-12 mb-3">
                <div class="p-3 bg-white border rounded">
                    <small class="text-uppercase text-muted fw-bold d-block mb-1 font-monospace" style="letter-spacing: 0.5px;">Activity</small>
                    <h5 class="fw-bold text-dark mb-2">${activityName}</h5>
                    <div class="d-flex align-items-center gap-2 mt-2">
                        <small class="text-secondary font-monospace text-uppercase">Timeline:</small>
                        ${periodHtml}
                    </div>
                </div>
            </div>
            
            <div class="col-12 mb-3">
                <div class="p-3 bg-white border rounded">
                    <!-- Row 1: Nodal Agency -->
                    <div class="row py-2.5 border-bottom align-items-center mx-0">
                        <div class="col-md-3 d-flex align-items-center mb-1 mb-md-0">
                            <i class="bx bx-git-repo-forked text-danger me-2 font_size_16"></i>
                            <span class="fw-bold text-secondary small text-uppercase font-monospace">Nodal Department</span>
                        </div>
                        <div class="col-md-9">
                            <span class="fw-bold text-dark">${primaryNodalDept}</span>
                        </div>
                    </div>

                    <!-- Row 2: Collaborating Ministries -->
                    <div class="row py-2.5 border-bottom align-items-start mx-0">
                        <div class="col-md-3 d-flex align-items-center mb-2 mb-md-0 pt-1">
                            <i class="bx bx-group text-info me-2 font_size_16"></i>
                            <span class="fw-bold text-secondary small text-uppercase font-monospace">Associated Department</span>
                        </div>
                        <div class="col-md-9">
                            <div class="d-flex flex-wrap">${associatedDeptsHtml}</div>
                        </div>
                    </div>

                    <!-- Row 3: SDG Mapping -->
                    <div class="row py-2.5 border-bottom align-items-start mx-0">
                        <div class="col-md-3 d-flex align-items-center mb-2 mb-md-0 pt-1">
                            <i class="bx bx-select-multiple text-warning me-2 font_size_16"></i>
                            <span class="fw-bold text-secondary small text-uppercase font-monospace">SDG Mapping</span>
                        </div>
                        <div class="col-md-9">
                            <div class="d-flex flex-wrap">${goalsHtml}</div>
                        </div>
                    </div>

                    <!-- Row 4: National Pillars -->
                    <div class="row py-2.5 border-bottom align-items-start mx-0 pt-2.5">
                        <div class="col-md-3 d-flex align-items-center mb-2 mb-md-0 pt-1">
                            <i class="bx bx-medal text-success me-2 font_size_16"></i>
                            <span class="fw-bold text-secondary small text-uppercase font-monospace">Viksit Rajasthan@2047</span>
                        </div>
                        <div class="col-md-9">
                            <div class="d-flex flex-wrap">${pillarsHtml}</div>
                        </div>
                    </div>
                    <!-- Row 4: Nature Of Support -->
                    <div class="row py-2.5 border-bottom align-items-start mx-0">
                        <div class="col-md-3 d-flex align-items-center mb-2 mb-md-0 pt-1">
                            <i class="bx bx-medal text-warning me-2 font_size_16"></i>
                            <span class="fw-bold text-secondary small text-uppercase font-monospace">Nature Of Support</span>
                        </div>
                        <div class="col-md-9">
                            <div class="d-flex flex-wrap">${natureHtml}</div>
                        </div>
                    </div>
                </div>
            </div>
        `;

            // 6. Update HTML Presentation Container
            $('#activityDetailsContainer').html(html);

            if (typeof syncMappingState === "function") {
                syncMappingState();
            }
        });
    },
}