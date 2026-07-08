document.addEventListener("DOMContentLoaded", function () {

    var gauravId = document.getElementById("GarauvId")?.value;
    var districtId = document.getElementById("DistrictId")?.value;

        if (gauravId) {

            $("#GarauvId").val(gauravId).trigger("change");
            $("#GarauvId").prop("disabled", true);
            onDistrictChange(gauravId, districtId);
            GetSummeryReport();
        }

        if (districtId) {

            $("#DistrictId").val(districtId).trigger("change");
            $("#DistrictId").prop("disabled", true);
            onDistrictChange(gauravId, districtId);
            GetSummeryReport();
        }
   
    
});

document.addEventListener("click", function (e) {

    const btn = e.target.closest(".btn-gaurav");
    if (!btn) return;

    const gauravid = btn.getAttribute("data-guid");
    const districtId = btn.getAttribute("data-district");
    const gauravname = btn.getAttribute("data-gauravname");
    const districtname = btn.getAttribute("data-districtname");

    if (!gauravid || !districtId) return;

    document.getElementById("formGauravId").value = gauravid;
    document.getElementById("formDistrictId").value = districtId;
    document.getElementById("formGauravName").value = gauravname;
    document.getElementById("formDistrictName").value = districtname;

    document.getElementById("vettingForm").submit();
});
function onDistrictChange(gauravid, districtid) {

    //var garauvId = document.getElementById("GarauvId").value;
    //var districtId = document.getElementById("DistrictId").value || document.getElementById("PendingDistrictId").value;
    var garauvId = gauravid;
    var districtId = districtid;
    $("#GauravIdVetted").val(garauvId);
    $("#DistrictIdVetted").val(districtId);
    if (!garauvId) return;
    if (!districtId) return;

    ajax.doPostAjaxHtml(
        "/Budget/GetPendingList",
        { garauvId: garauvId, districtId: districtId },
        function (response) {
            document.getElementById("vettingContainer").innerHTML = response;
            toggleVerificationButton();
        }
    );
    loadPendingList(0, garauvId, districtId, 0, 0)
}
function GetSummeryReport() {

    var garauvId = document.getElementById("GarauvId").value;
    var districtId = document.getElementById("DistrictId").value || document.getElementById("PendingDistrictId").value;
    if (!garauvId) return;
    if (!districtId) return;

    ajax.doPostAjaxHtml(
        "/Budget/GetSummeryReport",
        { garauvId: garauvId, districtId: districtId },
        function (response) {
            document.getElementById("SummaryReportBudget").innerHTML = response;
            toggleVerificationButton();
        }
    );
    loadPendingList(0, garauvId, districtId, 0, 0)
}
//pending list based on gauravid and districtId
function onsavegetpendinglist(gauravid, districtId) {

    //var garauvId = document.getElementById("GarauvId").value;
    //var districtId = document.getElementById("DistrictId").value;
    if (!gauravid) return;
    if (!districtId) return;

    ajax.doPostAjaxHtml(
        "/Budget/GetPendingList",
        { garauvId: gauravid, districtId: districtId },
        function (response) {
            document.getElementById("vettingContainer").innerHTML = response;
            // recompute summary in case vetted list changed in parallel
            //setTimeout(computePendingTotals, 200);
           toggleVerificationButton();
        }
    );
}
function bindGarauv() {
    common.BindDropdown("/Budget/BindGauravDropDown", "GarauvId", "Garauv", 0);
    setTimeout(function () {
        fixGarauvDropdown();
    }, 500);
}
function fixGarauvDropdown() {
    var ddl = document.getElementById("GarauvId");

    if (!ddl) return;

    // agar sirf 1 actual entry hai
    if (ddl.options.length === 2) {
        ddl.remove(0); // "Select Garauv" remove
        ddl.selectedIndex = 0;
       
        GetPendingDistrict();
    }
}
function bindDistrict() {
    common.BindDropdown("/Budget/BindDistrictDropDownAll", "DistrictId", "District", 0);
}
function GetPendingDistrict(gauravid) {
    //common.BindDropdown("/Budget/PendingDistrict", "PendingDistrictId", "PendingDistrict", 0);
    var gauravId = gauravid || $("#GarauvId").val();

    common.GetPendingDistrict(
        "/Budget/PendingDistrict",
        { gauravId: gauravId },
        "PendingDistrictId",
        "Pending District",
        0
    );
}

// ---- Bootstrap modal permanent cleanup ----
document.addEventListener('hidden.bs.modal', function () {
    document.querySelectorAll('.modal-backdrop')
        .forEach(e => e.remove());

    document.body.classList.remove('modal-open');
});


document.addEventListener("click", function (e) {

    const btn = e.target.closest(".verify-btn-vetting");
    if (!btn) return;

    const rawid = btn.getAttribute("data-rowid");
    const gauravid = btn.getAttribute("data-gauravid");
    const DistrictId = btn.getAttribute("data-districtid");
    const SubQuestionMasterId = btn.getAttribute("data-subquestionmasterid");
    const QuestionMasterId = btn.getAttribute("data-questionmasterid");
    //value set for popup for save value for same parameters
    document.getElementById("mRawId").value = rawid;
    document.getElementById("mGauravId").value = gauravid;
    document.getElementById("mDistrictId").value = DistrictId;
    document.getElementById("mSubQuestionMasterId").value = SubQuestionMasterId;
    document.getElementById("mQuestionMasterId").value = QuestionMasterId;

    ajax.doPostAjaxHtml(
        "/Budget/GetPendingVettingList",
        {
            RawId: rawid,
            garauvId: gauravid,
            DistrictId: DistrictId,
            SubQuestionMasterId: SubQuestionMasterId,
            QuestionMasterId: QuestionMasterId
        },
        function (response) {

            const tempDiv = document.createElement("div");
            tempDiv.innerHTML = response;

            const row = tempDiv.querySelector("tbody tr");

            const activity = row.children[1].innerText;
            const activityName = row.children[2].innerText;
            const total = row.children[3].innerText;
            const nodal = row.children[4].innerText;
            const MPLAD = row.children[5].innerText;
            const CSR = row.children[6].innerText;
            const other = row.children[7].innerText;
            const panchgaurav = row.children[8].innerText;
            const workplan = row.children[9].innerText;
            const date = row.children[10].innerText;


            document.getElementById("readonlyData").innerHTML = `
<div class="row mb-2">
    <div class="col-md-4 fw-bold">गतिविधि :</div>
    <div class="col-md-8">${activity}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">गतिविधि नाम :</div>
    <div class="col-md-8">${activityName}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">कूल प्रस्तावित व्यय :</div>
    <div class="col-md-8">${total}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">नोडल विभाग का व्यय :</div>
    <div class="col-md-8">${nodal}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">MPLAD, MLALAD से व्यय :</div>
    <div class="col-md-8">${MPLAD}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">CSR मद से व्यय :</div>
    <div class="col-md-8">${CSR}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">अन्य मद द्वारा व्यय :</div>
    <div class="col-md-8">${other}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">पंच-गौरव से बजट आवश्यकता :</div>
    <div class="col-md-8">${panchgaurav}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">कार्य योजना :</div>
    <div class="col-md-8">${workplan}</div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">समय सीमा :</div>
    <div class="col-md-8">${date}</div>
</div>
`;


            document.getElementById("editableData").innerHTML = `
<div class="row mb-2">
    <div class="col-md-4 fw-bold">गतिविधि :</div>
    <div class="col-md-8">
        ${activity}
        <input type="hidden" id="editActivity" value="${activity}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">गतिविधि नाम :</div>
    <div class="col-md-8">
        ${activityName}
        <input type="hidden" id="editActivityName" value="${activityName}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">कूल प्रस्तावित व्यय :</div>
    <div class="col-md-8">
        <input class="form-control" id="totalproposed" value="${total}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">नोडल विभाग का व्यय :</div>
    <div class="col-md-8">
        <input class="form-control" id="nodal" value="${nodal}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">MPLAD, MLALAD से व्यय :</div>
    <div class="col-md-8">
        <input class="form-control" id="MPLAD" value="${MPLAD}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">CSR मद से व्यय :</div>
    <div class="col-md-8">
        <input class="form-control" id="CSR" value="${CSR}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">अन्य मद द्वारा व्यय :</div>
    <div class="col-md-8">
        <input class="form-control" id="other" value="${other}">
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">पंच-गौरव से बजट आवश्यकता :</div>
    <div class="col-md-8">
        <input class="form-control" id="panchgaurav" value="${panchgaurav}" readonly>
    </div>
</div>

<div class="row mb-2">
    <div class="col-md-4 fw-bold">कार्य योजना :</div>
    <div class="col-md-8">
        <input class="form-control" id="workplan" value="${workplan}">
    </div>
</div>


`;

            const modal = new bootstrap.Modal(
                document.getElementById("verifyModalvetting")
            );
            modal.show();
        }
    );
});

const saveBtn = document.getElementById("saveVerify");

if (saveBtn) {
    saveBtn.addEventListener("click", function () {

        const rawid = document.getElementById("mRawId")?.value;
        const gauravid = document.getElementById("mGauravId")?.value;
        const districtId = document.getElementById("mDistrictId")?.value;

        const activity = document.getElementById("editActivity")?.value;
        const activityName = document.getElementById("editActivityName")?.value;
        const totalproposed = document.getElementById("totalproposed")?.value;
        const nodal = document.getElementById("nodal")?.value;
        const MPLAD = document.getElementById("MPLAD")?.value;
        const CSR = document.getElementById("CSR")?.value;
        const other = document.getElementById("other")?.value;
        const panchgaurav = document.getElementById("panchgaurav")?.value;
        const workplan = document.getElementById("workplan")?.value;

        ajax.doPostAjax(
            "/Budget/SaveVettingData",
            {
                RowId: rawid,
                GauravId: gauravid,
                DistrictId: districtId,
                Activity: activity,
                ActivityName: activityName,
                Budget: panchgaurav,
                Nodal: nodal,
                MPLAD: MPLAD,
                CSR: CSR,
                other: other,
                panchgaurav: panchgaurav,
                workplan: workplan,
                TotalProposed: totalproposed
            },
            function (res) {

                if (res.status) {

                    toggleVerificationButton();

                    bootstrap.Modal.getInstance(
                        document.getElementById("verifyModalvetting")
                    )?.hide();

                    onsavegetpendinglist(gauravid, districtId);

                    loadPendingList(rawid, gauravid, districtId);

                    GetSummeryReport(gauravid, districtId);

                } else {
                    toast.showToast('error', res.message, 'error');
                }

            }
        );
    });
}
function bindReadonly(data) {
    document.getElementById("readonlyData").innerHTML = `
        <div><strong>गतिविधि :</strong> ${data.activity}</div>
        <div><strong>गतिविधि नाम :</strong> ${data.activityName}</div>
        <div><strong>बजट :</strong> ${data.budget}</div>
    `;
}
function bindEditable(data) {
    document.getElementById("editableData").innerHTML = `
        <div><strong>गतिविधि :</strong> ${data.activity}</div>
        <div><strong>गतिविधि नाम :</strong>
            <input class="form-control" value="${data.activityName}">
        </div>
        <div><strong>बजट :</strong>
            <input class="form-control" value="${data.budget}">
        </div>
    `;
}
// vetted list
function loadPendingList(rawid, gauravid, districtId) {
    //$("#GauravIdHidden").val(gauravid);
    ajax.doGetAjaxVetting(
        "/Budget/GetPendingVettingList",
        {
            RowId: rawid,
            GauravId: gauravid,
            DistrictId: districtId
        },
        function (response) {

            document.getElementById("pendingListContainer")
                .innerHTML = response;
            toggleVerificationButton();
            // recompute summary when vetted list is loaded
            //setTimeout(computePendingTotals, 200);

            //new bootstrap.Modal(
            //    document.getElementById("verifyModalvetting")
            //).show();
        }
    );
}
document.addEventListener("click", function (e) {

    const btn = e.target.closest(".edit-btn");
    if (!btn) return;

    const tr = btn.closest("tr");

    //const activityName = tr.children[2].innerText.trim();
    const totalproposed = tr.children[3].innerText.trim();
    const nodal = tr.children[4].innerText.trim();
    const mplad = tr.children[5].innerText.trim();
    const csr = tr.children[6].innerText.trim();
    const other = tr.children[7].innerText.trim();
    const panchgaurav = tr.children[8].innerText.trim();
    const workplan = tr.children[9].innerText.trim();

    //tr.children[2].innerHTML =
    //`<input class="act-name table-input" value="${activityName}">`;

    tr.children[3].innerHTML =
        `<input class="totalproposed table-input" value="${totalproposed}">`;
    tr.children[4].innerHTML =
        `<input class="nodal table-input" value="${nodal}">`;
    tr.children[5].innerHTML =
        `<input class="mplad table-input" value="${mplad}">`;
    tr.children[6].innerHTML =
        `<input class="csr table-input" value="${csr}">`;
    tr.children[7].innerHTML =
        `<input class="other table-input" value="${other}">`;
    tr.children[8].innerHTML =
        `<input class="panchgaurav table-input" value="${panchgaurav}">`;
    tr.children[9].innerHTML =
        `<input class="workplan table-input" value="${workplan}">`;
    // Edit → Update
    // buttons toggle
    //btn.classList.add("d-none");
    //tr.querySelector(".update-btn")
    //    .classList.remove("d-none");
});
document.addEventListener("click", function (e) {

    const btn = e.target.closest(".delete-btn");
    if (!btn) return;

    const rawid = btn.getAttribute("data-rowid");
    const gauravid = btn.getAttribute("data-GauravId");
    const districtId = btn.getAttribute("data-DistrictId");
    //const subQuestionId = btn.getAttribute("data-SubQuestionMasterId");
    //const questionId = btn.getAttribute("data-QuestionMasterId");


    ajax.doPostAjax(
        "/Budget/DeleteVettedList",
        {
            RawId: rawid,
            garauvId: gauravid,
            DistrictId: districtId,
            //SubQuestionMasterId: subQuestionId,
            //QuestionMasterId: questionId
        },
        function (res) {
            if (res.status) {
                onsavegetpendinglist(gauravid, districtId);

                // loadPendingList(rawid, gauravid, districtId, subQuestionId, questionId);
                loadPendingList(0, gauravid, districtId, 0, 0)
                GetSummeryReport(gauravid, districtId);
                toast.showToast('success', res.message, 'success');
            } else {
                toast.showToast('error', res.message, 'error');
            }

        }
    );
});
document.addEventListener("click", function (e) {

    const btn = e.target.closest("#openVettedPopup");
    if (!btn) return;
    const selG = document.getElementById("GarauvId")?.value;
    const selD = document.getElementById("DistrictId")?.value;

    const invalidG = !selG || selG === "" || selG === "-1";
    const invalidD = !selD || selD === "" || selD === "-1";

    if (invalidG || invalidD) {
        // User must select both — prevent existing handler from opening modal
        if (invalidG) {
            toast.showToast('error', 'Please select Gaurav before adding a vetted entry.', 'error');
        } else {
            toast.showToast('error', 'Please select District before adding a vetted entry.', 'error');
        }

        // Stop other click handlers (including the existing #openVettedPopup handler)
        e.stopImmediatePropagation();
        e.preventDefault();
        return;
    }

    // Both selected — bind values to the button so existing logic can use data-gauravid / data-districtid
    btn.setAttribute('data-gauravid', selG);
    btn.setAttribute('data-districtid', selD);

    const gauravId = btn.getAttribute('data-gauravid')

    if (!gauravId) return;

    ajax.doPostAjaxHtml(
        "/Budget/VettedQuestions",
        { GauravId: gauravId },
        function (response) {

            $("#vettedModalContainer").html(response);

            let modal = new bootstrap.Modal(
                document.getElementById("vettedModal")
            );
            modal.show();

            // if a separate manageMaster script exists use it, otherwise call local loader
            if (typeof buildVettedForm === 'function') buildVettedForm();
            if (typeof loadDynamicForm === 'function') loadDynamicForm();
        }
    );

});
function buildVettedForm() {

    let container = $("#dynamicFormContainervetted");
    container.empty();

    questions.forEach(q => {
        container.append(
            `<div class="mb-2">
                <label>${q.questionName}</label>
                <input class="form-control" value="${q.answer ?? ''}">
            </div>`
        );
    });
}
function loadDynamicForm() {
    let container = document.getElementById("dynamicFormContainervetted");
    container.innerHTML = "";
    //alert(container);
    questions.forEach(q => {
        let card = `
            <div class="p-3 mb-3 shadow-sm">
                <h5 class="fw-bold">${q.questionText}</h5>      
                ${buildQuestionHtml(q)}
            </div>
        `;

        container.innerHTML += card;
    });
}
function buildQuestionHtml(q) {
    let html = `<div class="row">`;
    q.subQuestions.forEach(sub => {

        let name = `Q_${q.questionMasterId}_${sub.subQuestionMasterId}`;

        // dropdown
        if (sub.fieldtype === "DropDown") {
            let opts = "";
            opts = `<option value="">-- Select --</option>`;
            activityList.forEach(o => {
                opts += `<option value="${o.value}">${o.text}</option>`;
            });

            html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <select name="${name}" id="ActivityId" class="form-select">${opts}</select>
                </div>
            `;
        }
        // textarea
        else if (sub.fieldtype === "TextArea") {
            html += `
                <div class="col-md-12 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <textarea class="form-control alphaspace" id="WorkPlan" name="${name}" rows="2" placeholder="अपना उत्तर यहाँ लिखें..."></textarea>
                </div>
            `;
        }
        else if (sub.fieldtype === "DateTime") {

            let id = `DT_${q.questionMasterId}_${sub.subQuestionMasterId}`;

            html += `
        <div class="col-md-4 p-2">
            <label class="fw-bold">${sub.questionText}</label>
            <input
                type="text"
                class="form-control datepicker"

                 id="CompletionDate"
                name="${name}"
                placeholder="dd/mm/yyyy"
                autocomplete="off"
            />
        </div>
    `;
        }

        else {
            if (sub.questionText == "नोडल विभाग का व्यय") {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control numberonly" id="NodalAmount" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
            else if (sub.questionText == "MPLAD, MLALAD से व्यय") {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control numberonly" id="MPLADAmount" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
            else if (sub.questionText == "CSR मद से व्यय") {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control numberonly" id="CSRAmount" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
            else if (sub.questionText == "अन्य मद द्वारा  व्यय") {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control numberonly" id="OtherAmount" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
            else if (sub.questionText == "पंच-गौरव से बजट आवश्यकता") {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control numberonly" id="PanchGauravAmount" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." readonly />
                </div>
            `;
            }
            else if (sub.questionText == "कूल प्रस्तावित व्यय") {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control numberonly" id="TotalProposed" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
            else {
                html += `
                <div class="col-md-4 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <input class="form-control alphaspace" id="ActivityName" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
        }
    });
    html += `</div> <button type="button" id="savevettedquestions" class="btn btn-success mt-3">
        Add
    </button>`;
    return html;
}
function savevettedquestions() {

    var garauvId = $('#GarauvId').val();
    var districtId = $('#DistrictId').val();


    var model = {
        ActivityId: $('#ActivityId').val(),
        activityText: $('#ActivityId option:selected').text(),
        ActivityName: $('#ActivityName').val(),
        TotalProposed: Number($("#TotalProposed").val()) || null,
        NodalAmount: Number($("#NodalAmount").val()) || null,
        MPLADAmount: Number($("#MPLADAmount").val()) || null,
        CSRAmount: Number($("#CSRAmount").val()) || null,
        OtherAmount: Number($("#OtherAmount").val()) || null,
        PanchGauravAmount: Number($("#PanchGauravAmount").val()) || null,
        WorkPlan: $('#WorkPlan').val(),
        CompletionDate: $('#CompletionDate').val()
    };



    $.ajax({
        url: `/Budget/savevettedquestions?GauravId=${garauvId}&DistrictId=${districtId}`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(model),
        success: function (data) {

            if (data.status) {
                var modalEL = document.getElementById('vettedModal');
                var modal = bootstrap.Modal.getInstance(modalEL);
                if (modal) {
                    modal.hide();
                }

                loadPendingList(0, garauvId, districtId, 0, 0)
                GetSummeryReport(garauvId, districtId)
                // compute after the list refresh
                //setTimeout(computePendingTotals, 300);
                toast.showToast('success', data.message, 'success');
            }
        },
        error: function (xhr) {

            if (xhr.responseJSON && xhr.responseJSON.errors) {

                xhr.responseJSON.errors.forEach(function (msg) {
                    toast.showToast('error', msg, 'error');
                });

            } else {
                toast.showToast('error', "Something went wrong", 'error');
            }
        }
    });

}

// NEW: Open modal edit handler for separate button (.edit-btn-vetted)
// Keeps all existing logic unchanged.
document.addEventListener("click", function (e) {
    const btn = e.target.closest(".edit-btn-vetted");
    if (!btn) return;

    const rawid = btn.getAttribute("data-rowid");
    const gauravid = btn.getAttribute("data-GauravId");
    const DistrictId = btn.getAttribute("data-DistrictId");
    const VettedByDepartment = btn.getAttribute("data-VettedByDepartment");

    // set modal hidden inputs if present
    const setIfExists = (id, value) => {
        const el = document.getElementById(id);
        if (el) el.value = value ?? "";
    };
    setIfExists("mRawId", rawid);
    setIfExists("mGauravId", gauravid);
    setIfExists("mDistrictId", DistrictId);

    // read row values (for editable panel)
    const tr = btn.closest("tr");
    if (!tr) return;

    const activityRow = tr.children[1]?.innerText.trim() ?? "";
    const activityNameRow = tr.children[2]?.innerText.trim() ?? "";
    const totalRow = tr.children[3]?.innerText.trim() ?? "";
    const nodalRow = tr.children[4]?.innerText.trim() ?? "";
    const MPLADRow = tr.children[5]?.innerText.trim() ?? "";
    const CSRRow = tr.children[6]?.innerText.trim() ?? "";
    const otherRow = tr.children[7]?.innerText.trim() ?? "";
    const panchgauravRow = tr.children[8]?.innerText.trim() ?? "";
    const workplanRow = tr.children[9]?.innerText.trim() ?? "";
    const dateRow = (tr.children.length > 10) ? tr.children[10]?.innerText.trim() ?? "" : "";

    // fetch server row for readonly panel (keeps server authoritative)
    ajax.doPostAjaxHtml(
        "/Budget/GetPendingVettingList",
        {
            RawId: rawid,
            garauvId: gauravid,
            DistrictId: DistrictId
        },
        function (response) {
            const tmp = document.createElement("div");
            tmp.innerHTML = response;
            const serverRow = tmp.querySelector("tbody tr");

            const activityServer = serverRow ? (serverRow.children[1]?.innerText ?? activityRow) : activityRow;
            const activityNameServer = serverRow ? (serverRow.children[2]?.innerText ?? activityNameRow) : activityNameRow;
            const totalServer = serverRow ? (serverRow.children[3]?.innerText ?? totalRow) : totalRow;
            const nodalServer = serverRow ? (serverRow.children[4]?.innerText ?? nodalRow) : nodalRow;
            const MPLADServer = serverRow ? (serverRow.children[5]?.innerText ?? MPLADRow) : MPLADRow;
            const CSRServer = serverRow ? (serverRow.children[6]?.innerText ?? CSRRow) : CSRRow;
            const otherServer = serverRow ? (serverRow.children[7]?.innerText ?? otherRow) : otherRow;
            const panchgauravServer = serverRow ? (serverRow.children[8]?.innerText ?? panchgauravRow) : panchgauravRow;
            const workplanServer = serverRow ? (serverRow.children[9]?.innerText ?? workplanRow) : workplanRow;
            const dateServer = serverRow && serverRow.children.length > 10 ? (serverRow.children[10]?.innerText ?? dateRow) : dateRow;

            // If VettedByDepartment == "1" show NA in readonly panel, otherwise show server values.
            const showNA = String(VettedByDepartment) === "1";
            const rActivity = showNA ? "NA" : activityServer;
            const rActivityName = showNA ? "NA" : activityNameServer;
            const rTotal = showNA ? "NA" : totalServer;
            const rNodal = showNA ? "NA" : nodalServer;
            const rMPLAD = showNA ? "NA" : MPLADServer;
            const rCSR = showNA ? "NA" : CSRServer;
            const rOther = showNA ? "NA" : otherServer;
            const rPanch = showNA ? "NA" : panchgauravServer;
            const rWorkplan = showNA ? "NA" : workplanServer;
            const rDate = showNA ? "NA" : dateServer;

            // fill readonly panel with either NA or server values
            const readonlyEl = document.getElementById("readonlyData");
            if (readonlyEl) {
                readonlyEl.innerHTML = `
<div class="row mb-2"><div class="col-md-4 fw-bold">गतिविधि :</div><div class="col-md-8">${rActivity}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">गतिविधि नाम :</div><div class="col-md-8">${rActivityName}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">कूल प्रस्तावित व्यय :</div><div class="col-md-8">${rTotal}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">नोडल विभाग का व्यय :</div><div class="col-md-8">${rNodal}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">MPLAD, MLALAD से व्यय :</div><div class="col-md-8">${rMPLAD}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">CSR मद से व्यय :</div><div class="col-md-8">${rCSR}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">अन्य मद द्वारा व्यय :</div><div class="col-md-8">${rOther}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">पंच-गौरव से बजट आवश्यकता :</div><div class="col-md-8">${rPanch}</div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">कार्य योजना :</div><div class="col-md-8">${rWorkplan}</div></div>
${rDate ? `<div class="row mb-2"><div class="col-md-4 fw-bold">समय सीमा :</div><div class="col-md-8">${rDate}</div></div>` : ''}
                `;
            }

            // fill editable panel from table row values (not server) - unchanged
            const editableEl = document.getElementById("editableData");
            if (editableEl) {
                const esc = s => (s === null || s === undefined) ? '' : String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
                editableEl.innerHTML = `
<div class="row mb-2"><div class="col-md-4 fw-bold">गतिविधि :</div><div class="col-md-8">${activityRow}<input type="hidden" id="editActivity" value="${esc(activityRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">गतिविधि नाम :</div><div class="col-md-8">${activityNameRow}<input type="hidden" id="editActivityName" value="${esc(activityNameRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">कूल प्रस्तावित व्यय :</div><div class="col-md-8"><input class="form-control" id="totalproposed" value="${esc(totalRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">नोडल विभाग का व्यय :</div><div class="col-md-8"><input class="form-control" id="nodal" value="${esc(nodalRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">MPLAD, MLALAD से व्यय :</div><div class="col-md-8"><input class="form-control" id="MPLAD" value="${esc(MPLADRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">CSR मद से व्यय :</div><div class="col-md-8"><input class="form-control" id="CSR" value="${esc(CSRRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">अन्य मद द्वारा व्यय :</div><div class="col-md-8"><input class="form-control" id="other" value="${esc(otherRow)}"></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">पन्च-गौरव से बजट आवश्यकता :</div><div class="col-md-8"><input class="form-control" id="panchgaurav" value="${esc(panchgauravRow)}" readonly></div></div>
<div class="row mb-2"><div class="col-md-4 fw-bold">कार्य योजना :</div><div class="col-md-8"><input class="form-control" id="workplan" value="${esc(workplanRow)}"></div></div>
                `;
            }

            // open the modal used by verify flow
            const modalEl = document.getElementById("verifyModalvetting");
            if (modalEl) {
                const modal = new bootstrap.Modal(modalEl);
                modal.show();
            }
        }
    );
});


function parseAmount(text) {
    if (!text) return 0;
    // remove commas, currency symbols, whitespace, non-number chars (keep dot and minus)
    text = String(text).replace(/[,₹\s]/g, '').replace(/[^\d.-]/g, '');
    var v = parseFloat(text);
    return isNaN(v) ? 0 : v;
}

function collectVettedRows() {
    const table = document.querySelector('#pendingListContainer table');
    if (!table) return [];

    const rows = Array.from(table.querySelectorAll('tbody tr'));
    const items = [];

    rows.forEach(tr => {
      
        const editBtn = tr.querySelector('.edit-btn-vetted, .edit-btn');
        const deleteBtn = tr.querySelector('.delete-btn');
        const anyBtn = editBtn || deleteBtn;

        const rowIdAttr = anyBtn ? (anyBtn.getAttribute('data-rowid') || anyBtn.getAttribute('data-rowid')) : null;
        const rowId = rowIdAttr ? parseInt(rowIdAttr, 10) : null;

   
        const gauravIdHidden = document.getElementById('GauravIdVetted') ? document.getElementById('GauravIdVetted').value : null;
        const districtIdHidden = document.getElementById('DistrictIdVetted') ? document.getElementById('DistrictIdVetted').value : null;

        const gauravId = anyBtn && anyBtn.getAttribute('data-GauravId') ? parseInt(anyBtn.getAttribute('data-GauravId'), 10) : (gauravIdHidden ? parseInt(gauravIdHidden, 10) : null);
        const districtId = anyBtn && anyBtn.getAttribute('data-DistrictId') ? parseInt(anyBtn.getAttribute('data-DistrictId'), 10) : (districtIdHidden ? parseInt(districtIdHidden, 10) : null);

   
        const cells = tr.children;
        const activity = (cells[1] ? cells[1].innerText.trim() : '');
        const activityName = (cells[2] ? cells[2].innerText.trim() : '');
        const totalProposedByDist =
            parseAmount(document.getElementById("TotalProposedByDist")?.innerText);

        const totalPanchgauravByDist =
            parseAmount(document.getElementById("TotalPanchgauravByDist")?.innerText);

        const totalProposedByDept =
            parseAmount(document.getElementById("TotalProposedByDept")?.innerText);

        const totalPanchgauravByDept =
            parseAmount(document.getElementById("TotalPanchgauravByDept")?.innerText);

        const totalProposed = parseAmount(cells[3] ? cells[3].innerText : '');
        const nodal = parseAmount(cells[4] ? cells[4].innerText : '');
        const mplad = parseAmount(cells[5] ? cells[5].innerText : '');
        const csr = parseAmount(cells[6] ? cells[6].innerText : '');
        const other = parseAmount(cells[7] ? cells[7].innerText : '');
        const panchgaurav = parseAmount(cells[8] ? cells[8].innerText : '');
        const workplan = (cells[9] ? cells[9].innerText.trim() : '');

        const item = {
            RowId: rowId,
            Activity: activity,
            ActivityName: activityName,
            TotalProposed: totalProposed,
            Nodal: nodal,
            MPLAD: mplad,
            CSR: csr,
            other: other,
            panchgaurav: panchgaurav,
            workplan: workplan,
            GauravId: gauravId,
            DistrictId: districtId,
            TotalProposedByDist: totalProposedByDist,
            TotalPanchgauravByDist: totalPanchgauravByDist,
            TotalProposedByDept: totalProposedByDept,
            TotalPanchgauravByDept: totalPanchgauravByDept,

        };

        items.push(item);
    });

    return items;
}
function sendVettedForVerification() {
    const items = collectVettedRows();
    if (!items || items.length === 0) {
        toast.showToast('error', 'No vetted rows to send.', 'error');
        return;
    }

    Swal.fire({
        title: "Are you sure?",
        text: "Send all vetted entries for verification?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, proceed"
    }).then(result => {
        if (!result.isConfirmed) return;
        const token = $('meta[name="csrf-token"]').attr('content') || '';

        $.ajax({
            type: 'POST',
            url: '/Budget/SendVettedForVerification',
            data: JSON.stringify(items),
            contentType: 'application/json; charset=utf-8',
            headers: {
                'RequestVerificationToken': token
            },
            success: function (res) {
                if (res && res.status) {
                    toast.showToast('success', res.message || 'Sent for verification', 'success');
                    toggleVerificationButton();
                    GetSummeryReport();
                    
                    // Immediately reflect sent state in UI (remove verify buttons and replace edit/delete in vetted list)
                    //removeVerificationButtons();

                    // refresh lists (existing flow)
                    const gauravId = document.getElementById('GarauvId') ? document.getElementById('GarauvId').value : '';
                    const districtId = document.getElementById('DistrictId') ? document.getElementById('DistrictId').value : '';
                    onsavegetpendinglist(gauravId, districtId);
                    loadPendingList(0, gauravId, districtId);
                    GetPendingDistrict(gauravId);
                    // recompute after refresh
                   // setTimeout(computePendingTotals, 400);

                    // In case server-rendered HTML reintroduces buttons, remove/replace them again after reload
                    //setTimeout(removeVerificationButtons, 800);
                } else {
                    toast.showToast('error', (res && res.message) ? res.message : 'Failed to send', 'error');
                }
            },
            error: function (xhr) {
                toast.showToast('error', 'Error sending for verification', 'error');
            }
        });
    });
}

function enforceSentState() {
    try {
        const sentBtns = document.querySelectorAll('#pendingListContainer .send-btn');
        if (!sentBtns || sentBtns.length === 0) return;

        // collect the row ids marked as sent
        const sentIds = Array.from(sentBtns)
            .map(b => b.getAttribute('data-rowid'))
            .filter(Boolean);

        // remove/hide the summary send button if present
        const summaryBtn = document.getElementById('sendForVerificationBtn') || document.getElementById('btnSendVerification');
        if (summaryBtn) summaryBtn.remove();

        // For each sent row id, remove any verify/edit/delete controls that may exist
        sentIds.forEach(id => {
            // pending (verify) list -- remove verify button for this row
            document.querySelectorAll(`#vettingContainer .verify-btn-vetting[data-rowid="${id}"]`)
                .forEach(el => el.remove());

            // vetted list -- remove edit / delete buttons for this row
            document.querySelectorAll(`#pendingListContainer .edit-btn-vetted[data-rowid="${id}"]`)
                .forEach(el => el.remove());
            document.querySelectorAll(`#pendingListContainer .delete-btn[data-rowid="${id}"]`)
                .forEach(el => el.remove());
        });
    } catch (ex) {
        // silent fail to avoid breaking other logic
        console.error('enforceSentState error', ex);
    }
}


function toggleVerificationButton() {

    const pendingTable = document.querySelector('#vettingContainer table');
    const vettedTable = document.querySelector('#pendingListContainer table');

    // ---- Check Pending ----
    let pendingHasData = false;
    if (pendingTable) {
        const rows = Array.from(pendingTable.querySelectorAll('tbody tr'));
        pendingHasData = rows.some(row =>
            row.querySelectorAll('td').length > 1
        );
    }

    // ---- Check Vetted ----
    let vettedHasData = false;
    if (vettedTable) {
        const rows = Array.from(vettedTable.querySelectorAll('tbody tr'));
        vettedHasData = rows.some(row =>
            row.querySelectorAll('td').length > 1
        );
    }
    const sentExists = !!document.querySelector('#pendingListContainer .send-btn');
    if (sentExists) {
        $("#openVettedPopup").addClass('d-none');
    }
    else {
        $("#openVettedPopup").removeClass('d-none');
    }
    // ✅ Show only when Pending = 0 AND Vetted > 0
    if (!pendingHasData && vettedHasData && !sentExists) {
        $("#VerificationBtnDiv").removeClass('d-none');
       
    } else {
        $("#VerificationBtnDiv").addClass('d-none');
      
    }
}