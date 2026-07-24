var monitoringreport = {
    // =============================================
    // SAVE REPORTING STEP
    // =============================================
    saveReportingStep: function () {
        debugger;
        // Converts "dd-mm-yyyy" string to a native JS Date object for accurate comparison
        function parseDateString(dateStr) {
            if (!dateStr) return null;
            var parts = dateStr.split('-');
            if (parts.length !== 3) return null;
            return new Date(parts[2], parts[1] - 1, parts[0]); // yyyy, mm-1, dd
        }
        var reportFromDate = $("#reportFromDate").val();
        var reportToDate = $("#reportToDate").val();

        // Convert parent activity dates to Date objects
        var fromDate = parseDateString(reportFromDate);
        var toDate = parseDateString(reportToDate);

        // To Date Required Check
        if (!reportToDate || reportToDate.trim() === "") {
            toast.showToast('error', 'Please select Reporting As On Date', 'error');
            return false;
        }

        if (fromDate && toDate && toDate < fromDate) {
            toast.showToast('error', 'Please select reporting period must be greater than Start Date', 'error');
            return false;
        }
        //let fromDate = $("#reportFromDate").val();
        //let toDate = $("#reportToDate").val();
        //if (fromDate == "" || toDate == "") {
        //    alert("Please select reporting period");
        //    return;
        //}
        //let reportingModel = monitoringreport.getReportingData();
        let reportingModel = {
            ReportingId: $("#reportingId").val() || 0,
            ReportFromDate: reportFromDate,
            ReportToDate: reportToDate,
            IsAligned: $("input[name='IsAligned']:checked").val() || 0,
            AlignmentDetails: $("#AlignmentDetails").val(),
            FundUtilization: $("#FundUtilization").val(),
            HasChallenges: $("input[name='HasChallenges']:checked").val() || 0,
            ChallengeDetails: $("#ChallengeDetails").val(),
            Suggestions: $("#Suggestions").val(),
            ActivityGuid: $("#ActivityGuid").val()
        };
        //return reportingModel;

        console.log(reportingModel);
        // =========================================
        // CONFIRMATION
        // =========================================

        Swal.fire({
            title: 'Are you sure?',
            text: 'Do you want to save reporting data?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes, Save',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                debugger;
                $.ajax({
                    url: '/Management/SaveReportingStep',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(reportingModel),
                    beforeSend: function () {
                        Swal.fire({
                            title: 'Please Wait...',
                            text: 'Saving reporting data...',
                            allowOutsideClick: false,
                            didOpen: () => {
                                Swal.showLoading();
                            }
                        });
                    },
                    success: function (response) {
                        Swal.close();
                        if (response.status) {
                            $('#reportingId').val(response.id),
                            Swal.fire({
                                icon: 'success',
                                title: 'Success',
                                text: response.message
                            }).then(() => {
                                currentStep = 2;
                                updateStepUI();
                                window.scrollTo(0, 0);
                            });
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: response.message
                            });
                        }
                    },
                    error: function () {
                        Swal.close();
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Error while saving reporting data'
                        });
                    }
                });
            }
        });

    },
    // =============================================
    // GET REPORTING DATA
    // =============================================
    getReportingData: function () {
        let reportingModel = {
            ReportingId: $("#ReportingId").val() || 0,
            ReportFromDate: $("#reportFromDate").val(),
            ReportToDate: $("#reportToDate").val(),
            IsAligned: $("input[name='IsAligned']:checked").val() || 0,
            AlignmentDetails: $("#AlignmentDetails").val(),
            FundUtilization: $("#FundUtilization").val(),
            HasChallenges: $("input[name='HasChallenges']:checked").val() || 0,
            ChallengeDetails: $("#ChallengeDetails").val(),
            Suggestions: $("#Suggestions").val(),
            ActivityGuid: $("#ActivityGuid").val()
        };
        return reportingModel;
    },

    // =============================================
    // LOAD REPORTING DATA
    // =============================================

    loadReportingData: function () {

        let activityGuid = $("#ActivityGuid").val();
        if (activityGuid == "")
            return;
        $.ajax({
            url: '/Management/GetReportingByActivityGuid',
            type: 'GET',
            data: {
                activityGuid: activityGuid
            },
            success: function (response) {
                debugger;
                if (response == null)
                    return;
                $("#reportingId").val(
                    response.reportingId
                );
                //$("#reportFromDate").val(response.reportFromDate);
                //    monitoringreport.formatDate(response.reportFromDate)
                //);

                //$("#reportToDate").val(response.reportToDate);
                //    monitoringreport.formatDate(response.reportToDate)
                //);
                $("input[name='IsAligned'][value='" +
                    response.isAligned + "']")
                    .prop("checked", true);

                $("#AlignmentDetails")
                    .val(response.alignmentDetails);

                if (response.isAligned == 1) {

                    $("#alignmentDetailsBox")
                        .removeClass("d-none")
                        .show();
                }
                $("#FundUtilization")
                    .val(response.fundUtilization);

                $("input[name='HasChallenges'][value='" +
                    response.hasChallenges + "']")
                    .prop("checked", true);

                $("#ChallengeDetails")
                    .val(response.challengeDetails);

                if (response.hasChallenges == 1) {

                    $("#challengeDetailsBox")
                        .removeClass("d-none")
                        .show();
                }
                $("#Suggestions")
                    .val(response.suggestions);

                flatpickr("#reportToDate", {
                    dateFormat: "d-m-Y",
                    defaultDate: `${response.reportToDate}`,
                    allowInput: true
                });

                $("#DepartmentOfficerName").val(response.departmentNodal);
                $("#DepartmentDesignation").val(response.departmentDesignation);
                $("#DepartmentEmail").val(response.departmentEmail);
                $("#DepartmentContactNumber").val(response.departmentContact);
                $("#DepartmentPlace").val(response.departmentPlace);
                // Agency
                $("#AgencyOfficerName").val(response.agencyNodal);
                $("#AgencyDesignation").val(response.agencyDesignation);
                $("#AgencyEmail").val(response.agencyEmail);
                $("#AgencyContactNumber").val(response.agencyContact);
                $("#AgencyPlace").val(response.agencyPlace);

            }

        });

    },
    loadConatctDetails: function () {

        let activityGuid = $("#ActivityGuid").val();
        //let reportingId = $("#reportingId").val();
        if (activityGuid == "")
            return;
        $.ajax({
            url: '/Management/GetReportingByActivityGuid',
            type: 'GET',
            data: {
                activityGuid: activityGuid
            },
            success: function (response) {
                debugger;
                if (response == null)
                    return;
                $("#reportingId").val(
                    response.reportingId
                );
                // Department

                $("#DepartmentOfficerName").val(response.departmentNodal);
                $("#DepartmentDesignation").val(response.departmentDesignation);
                $("#DepartmentEmail").val(response.departmentEmail);
                $("#DepartmentContactNumber").val(response.departmentContact);
                $("#DepartmentPlace").val(response.departmentPlace);
                // Agency
                $("#AgencyOfficerName").val(response.agencynNodal);
                $("#AgencyDesignation").val(response.agencyDesignation);
                $("#AgencyEmail").val(response.agencyEmail);
                $("#AgencyContactNumber").val(response.agencyContact);
                $("#AgencyPlace").val(response.agencyPlace);
            }

        });

    },
    //formatDate: function (dateString) {
    //    if (!dateString) return "";

    //    const date = new Date(dateString);

    //    return [
    //        String(date.getDate()).padStart(2, '0'),
    //        String(date.getMonth() + 1).padStart(2, '0'),
    //        date.getFullYear()
    //    ].join('-');
    //},

    SaveTracking: function (isFinalSubmit) {

        let trackingData = [];

        $("#taskTrackingContainer tbody tr").each(function () {

            let row = $(this);

            trackingData.push({
                TrackingId: parseInt(row.find(".tracking-id").val() || 0),
                ActivityGuid: $("#ActivityGuid").val(),
                TaskId: parseInt(row.find(".task-id").val()),
                Status: row.find(".tracking-status").val(),
                Achievement: row.find(".tracking-achievement").val() || "",
                Remarks: row.find(".tracking-remarks").val() || "",
                reportingId: $("#reportingId").val(),
            });

        });

        if (trackingData.length === 0) {
            Swal.fire('Error', 'No tracking data found.', 'error');
            return;
        }

        let model = {
            ContactDetails: {
                DepartmentOfficerName:$("#DepartmentOfficerName").val(),
                DepartmentDesignation:$("#DepartmentDesignation").val(),
                DepartmentEmail: $("#DepartmentEmail").val(),
                DepartmentContactNumber:$("#DepartmentContactNumber").val(),
                DepartmentPlace:$("#DepartmentPlace").val(),
                AgencyOfficerName:$("#AgencyOfficerName").val(),
                AgencyDesignation:$("#AgencyDesignation").val(),
                AgencyEmail:$("#AgencyEmail").val(),
                AgencyContactNumber:$("#AgencyContactNumber").val(),
                AgencyPlace:$("#AgencyPlace").val()
            },
            TaskTrackingModel: trackingData
        };

        debugger;
        $.ajax({
            url: `/Management/SaveTaskTracking?isFinalSubmit=${isFinalSubmit}`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),

            beforeSend: function () {

                let btn = isFinalSubmit
                    ? $("#btnFinalSubmit")
                    : $("#btnSaveTracking");

                btn.prop("disabled", true)
                    .html('<i class="bx bx-loader-alt bx-spin"></i> Processing...');
            },

            success: function (response) {

                if (response.status) {

                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: response.message
                    }).then(() => {

                       /* if (isFinalSubmit) {*/

                            window.location.href =
                                '/Management/ActivityManagement';
                      /*  }*/

                    });
                }
                else {

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                }
            },

            complete: function () {

                $("#btnSaveTracking")
                    .prop("disabled", false)
                    .html('<i class="bx bx-save"></i> Save');

                $("#btnFinalSubmit")
                    .prop("disabled", false)
                    .html('<i class="bx bx-check-circle"></i> Final Submit');
            }
        });
    },
}