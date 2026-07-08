document.addEventListener("DOMContentLoaded", function () {

    loadDistrict();
    bindEvents();
    $(document).on("input change", "input, select, textarea", function () {

        if ($(this).val().trim() !== "") {
            $(this).removeClass("is-invalid");
        }

    });
});
function bindEvents() {

    const saveBasic = document.getElementById("saveBasic");
    const district = document.getElementById("ddlDistrict");
    const block = document.getElementById("ddlBlock");
    const saveWork = document.getElementById("saveWork");
    const saveBudget = document.getElementById("saveBudget");
    const saveAgency = document.getElementById("saveAgency");
    const savenodal = document.getElementById("savenodal");
    const btnLocation = document.getElementById("btnLocation");
    const saveGeo = document.getElementById("saveGeo");
    const fianlsaveall = document.getElementById("FianlSaveAll");

    if (saveBasic) {
        saveBasic.addEventListener("click", SaveBasic);
    }
    if (district) {
        district.addEventListener("change", districtChange);
    }
    if (block) {
        block.addEventListener("change", blockChange);
    }
    if (saveWork) {
        saveWork.addEventListener("click", SaveWork);
    }
    if (saveBudget) {
        saveBudget.addEventListener("click", SaveBudget);
    }
    if (saveAgency) {
        saveAgency.addEventListener("click", SaveAgency);
    }
    if (savenodal) {
        savenodal.addEventListener("click", SavenNodal);
    }
    if (btnLocation) {
        btnLocation.addEventListener("click", getLocation);
    }
    if (saveGeo) {
        saveGeo.addEventListener("click", SaveGeo);
    }
    if (fianlsaveall) {
        fianlsaveall.addEventListener("click", FianlSaveAllTab);
    }
}
function loadDistrict() {

    common.BindDropdown(
        "/ProjectMonitoring/GetDistrict",
        "ddlDistrict",
        "District",
        ""
    );

}
function districtChange() {

    const districtId = document.getElementById("ddlDistrict")?.value;

    if (!districtId) return;

    common.BindDropdown(
        "/ProjectMonitoring/GetParliament?districtId=" + districtId,
        "ddlParliament",
        "Parliament",
        ""
    );
    common.BindDropdown(
        "/ProjectMonitoring/GetAssembly?districtId=" + districtId,
        "ddlAssembly",
        "Assembly",
        ""
    );
    common.BindDropdown(
        "/ProjectMonitoring/GetBlock?districtId=" + districtId,
        "ddlBlock",
        "Block",
        ""
    );

    clearDropdown("ddlAssembly");
    clearDropdown("ddlBlock");
    clearDropdown("ddlPanchayat");
    clearDropdown("ddlParliament");

}
function blockChange() {

    const blockId = document.getElementById("ddlBlock")?.value;

    if (!blockId) return;

    common.BindDropdown(
        "/ProjectMonitoring/GetPanchayat?blockId=" + blockId,
        "ddlPanchayat",
        "Panchayat",
        ""
    );

}
function clearDropdown(id) {

    const dropdown = document.getElementById(id);

    if (dropdown) {
        dropdown.innerHTML = '<option value="">--Select--</option>';
    }

}
function goToTab(tabName) {

    const tabLink = document.querySelector('[href="#' + tabName + '"]');

    if (tabLink) {

        const tab = new bootstrap.Tab(tabLink);
        tab.show();

    }

}
function SaveBasic() {

    const districtId = document.getElementById("ddlDistrict")?.value;
    const blockId = document.getElementById("ddlBlock")?.value;
    const panchayatId = document.getElementById("ddlPanchayat")?.value;
    const assemblyId = document.getElementById("ddlAssembly")?.value;
    const parliamentId = document.getElementById("ddlParliament")?.value;


    /* Validation */

    if (!districtId) {
        alert("Please select District");
        return;
    }

    if (!blockId) {
        alert("Please select Block");
        return;
    }

    if (!panchayatId) {
        alert("Please select Panchayat");
        return;
    }


    /* Data Object */

    const data = {
        DistrictId: districtId,
        BlockId: blockId,
        PanchayatId: panchayatId,
        AssemblyId: assemblyId,
        ParliamentId: parliamentId
    };


    /* Ajax Call using common.js */

    ajax.doPostAjax(
        "/ProjectMonitoring/SaveBasicDetails",
        data,
        function (result) {

            if (result.status) {

                toast.showToast('success', result.message, 'success');
                /* Next Tab */

                setTimeout(function () {
                    goToTab("work");
                }, 2000);

            }
            else {
                toast.showToast('error', result.message, 'error');
            }

        }
    );

}
function SaveWork() {

    const model = {

        //WorkId: document.getElementById("hdnWorkId").value,

        SchemeName: document.getElementById("txtSchemeName")?.value || "",
        WorkTitle: document.getElementById("txtWorkTitle")?.value || "",

        WorkType: document.getElementById("ddlWorkType")?.value || 0,
        WorkNature: document.getElementById("ddlWorkNature")?.value || 0,

        WorkPlace: document.getElementById("txtWorkPlace")?.value || "",
        Description: document.getElementById("txtWorkDescription")?.value || "",

        ApprovalDate: document.getElementById("txtApprovalDate")?.value || "",
        StartDate: document.getElementById("txtStartDate")?.value || "",
        ExpectedCompletion: document.getElementById("txtExpectedCompletion")?.value || "",
        ActualCompletion: document.getElementById("txtActualCompletion")?.value || ""

    };

    ajax.doPostAjax(
        "/ProjectMonitoring/SaveWorkDetails",
        model,
        function (result) {

            if (result.status) {

                toast.showToast('success', result.message, 'success');
                /* Next Tab */

                setTimeout(function () {
                    goToTab("budget");
                }, 2000);

            }
            else {

                toast.showToast('error', result.message, 'error');

            }

        });

}
function SaveBudget() {

    const model = {
      
            //WorkId: $("#WorkId").val(),
            ApprovedAmount: $("#ApprovedAmount").val(),
            ExpenditureAmount: $("#ExpenditureAmount").val(),
            ExpenditurePercent: $("#ExpenditurePercent").val(),
            DistrictRank: $("#DistrictRank").val(),
            FinancialProgress: $("#FinancialProgress").val(),
            PaymentStatus: $("#PaymentStatus").val(),
            BudgetHeadName: $("#BudgetHeadName").val(),
            BudgetHeadCode: $("#BudgetHeadCode").val(),
            FinancialYearId: $("#FinancialYearId").val(),
            NodalDepartment: $("#NodalDepartment").val()
      

    };

    ajax.doPostAjax(
        "/ProjectMonitoring/SaveWorkBudget",
        model,
        function (result) {

            if (result.status) {

                toast.showToast('success', result.message, 'success');
                /* Next Tab */

                setTimeout(function () {
                    goToTab("agency");
                }, 2000);

            }
            else {

                toast.showToast('error', result.message, 'error');

            }

        });

}
function SaveAgency() {

    const model = {
        //WorkId: $("#WorkId").val(),
        AgencyName: $("#AgencyName").val(),
        AgencyType: $("#AgencyType").val(),
        RegistrationNo: $("#RegistrationNo").val(),

        AgencyContactPerson: $("#AgencyContactPerson").val(),
        AgencyContactPersonDesignation: $("#AgencyContactPersonDesignation").val(),
        AgencyContactPersonMobileNo: $("#AgencyContactPersonMobileNo").val(),

        MonitoringOfficer: $("#MonitoringOfficer").val(),
        MonitoringOfficerDesignation: $("#MonitoringOfficerDesignation").val(),
        MonitoringOfficerMobile: $("#MonitoringOfficerMobile").val()


    };

    ajax.doPostAjax(
        "/ProjectMonitoring/SaveWorkAgency",
        model,
        function (result) {

            if (result.status) {

                toast.showToast('success', result.message, 'success');
                /* Next Tab */

                setTimeout(function () {
                    goToTab("nodal");
                }, 2000);

            }
            else {

                toast.showToast('error', result.message, 'error');

            }

        });

}
function SavenNodal() {

    const model = {
        //WorkId: $("#WorkId").val(),
        NodalOfficerName: $("#NodalOfficerName").val(),
        Designation: $("#Designation").val(),
        OfficeName: $("#OfficeName").val(),

        MobileNumber: $("#MobileNumber").val(),
        EmailId: $("#EmailId").val(),
        Level: $("#Level").val()


    };

    ajax.doPostAjax(
        "/ProjectMonitoring/SaveWorkNodalOfficer",
        model,
        function (result) {

            if (result.status) {

                toast.showToast('success', result.message, 'success');
                /* Next Tab */

                setTimeout(function () {
                    goToTab("geo");
                }, 2000);

            }
            else {

                toast.showToast('error', result.message, 'error');

            }

        });

}
function SaveGeo() {

    var fileInput = document.getElementById("geofile");
    var file = fileInput.files[0];

    if (!file) {
        alert("Please upload photo");
        return;
    }

    var reader = new FileReader();

    reader.onload = function (e) {

        const model = {
            Latitude: $("#Latitude").val(),
            Longitude: $("#Longitude").val(),
            GeoTaggedPhoto: e.target.result   // Base64 string
        };

        ajax.doPostAjax(
            "/ProjectMonitoring/SaveGeoTagging",
            model,
            function (result) {

                if (result.status) {

                    toast.showToast('success', result.message, 'success');

                    //setTimeout(function () {
                    //    goToTab("geo");
                    //}, 2000);

                } else {

                    toast.showToast('error', result.message, 'error');

                }
            });
    };

    reader.readAsDataURL(file); // convert file to Base64
}
function FianlSaveAllTab() {

    $(".form-control, .form-select").removeClass("is-invalid");

    let firstErrorTab = null;
    let firstErrorField = null;

    $(".tab-pane").each(function () {

        let tabId = $(this).attr("id");

        $(this).find("input, select, textarea").each(function () {

            if ($(this).prop("readonly")) return;

            if ($(this).val() == "" || $(this).val() == null) {

                $(this).addClass("is-invalid");

                if (!firstErrorTab) {
                    firstErrorTab = tabId;
                    firstErrorField = $(this);
                }

            }

        });

    });

    if (firstErrorTab) {

        // open tab
        $('.nav-link[href="#' + firstErrorTab + '"]').tab('show');

        // focus field
        setTimeout(function () {
            firstErrorField.focus();
        }, 300);

        toast.showToast('error', 'Please fill all required fields', 'error');

        return;
    }

    // if everything valid
    SaveAllData();
}
function getLocation() {

    if (navigator.geolocation) {

        navigator.geolocation.getCurrentPosition(function (position) {

            document.getElementById("Latitude").value = position.coords.latitude;
            document.getElementById("Longitude").value = position.coords.longitude;

        });

    } else {

        alert("Geolocation not supported");

    }

}