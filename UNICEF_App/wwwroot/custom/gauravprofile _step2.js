

document.addEventListener("DOMContentLoaded", function () {
    loadDynamicForm();
    loadSavedTable();
});
function loadDynamicForm() {
    let container = document.getElementById("dynamicFormContainer");
    container.innerHTML = "";
    //alert(container);
    questions.forEach(q => {
        let card = `
            <div class="p-3 mb-3 shadow-sm">
                <h5 class="fw-bold">${q.questionText}</h5>      
                ${renderSubQuestions(q)}
            </div>
        `;
        
        container.innerHTML += card;
    });
}
function renderSubQuestions(q) {
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
                    <select name="${name}" class="form-select">${opts}</select>
                </div>
            `;
        }
        // textarea
        else if (sub.fieldtype === "TextArea") {
            html += `
                <div class="col-md-12 p-2">
                    <label class="fw-bold">${sub.questionText}</label>
                    <textarea class="form-control alphaspace" name="${name}" rows="2" placeholder="अपना उत्तर यहाँ लिखें..."></textarea>
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
                id="${id}"
                name="${name}"
                placeholder="dd/mm/yyyy"
                autocomplete="off"
            />
        </div>
    `;
        }

    //    else if (sub.fieldtype === "DateTime") {
    //        html += `
    //    <div class="col-md-4 p-2">
    //        <label class="fw-bold">${sub.questionText}</label>
    //        <input 
    //            type="date"
    //            class="form-control"
    //            name="${name}"
    //        />
    //    </div>
    //`;
    //    }
        // textbox
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
                    <input class="form-control alphaspace" name="${name}" type="text" placeholder="अपना उत्तर यहाँ लिखें..." />
                </div>
            `;
            }
        }
    });
    html += `</div> <button type="button" id="btnSaveStep2" class="btn btn-success mt-3">
        Add
    </button>`; 
    return html;
}
function loadSavedTable() {
    let tbody = document.querySelector("#savedTable tbody");
    tbody.innerHTML = "";

    if (!savedRows || savedRows.length === 0) return;

    // ⭐ Group by RowId
    let grouped = {};
    savedRows.forEach(item => {
        if (!grouped[item.rowId]) grouped[item.rowId] = [];
        grouped[item.rowId].push(item);
    });

    // ⭐ Loop each RowId
    Object.keys(grouped).forEach((rowId, index) => {

        let rowItems = grouped[rowId];

        // Convert SubQuestionMasterId → AnswerValue (column wise)
        let columns = {};
        rowItems.forEach(i => {
            columns[i.subQuestionMasterId] = i.answerValue;
        });

        // Dynamic table row
        let row = `
            <tr data-rowid="${rowId}">
                <td>${index+1}</td>
                <td>${columns[17] ?? ""}</td>
                <td>${columns[1] ?? ""}</td>  
                <td>${columns[2] ?? ""}</td>
                <td>${columns[3] ?? ""}</td>
                <td>${columns[4] ?? ""}</td>
                <td>${columns[5] ?? ""}</td>
                <td>${columns[6] ?? ""}</td>
                <td>${columns[7] ?? ""}</td>
                <td>${columns[15] ?? ""}</td>
                <td>${columns[16] ?? ""}</td>
                <td>
                    <button class="btn btn-primary btn-edit">Edit</button>
                    <button class="btn btn-danger btn-delete">Delete</button>
                </td>
            </tr>
        `;

        tbody.innerHTML += row;
    });
}

function deleteRow(index) {
    if (!confirm("Are you sure to delete?")) return;
    //alert(index);
    //alert(gauravGuid);
    //alert(savedRows[index].id);
    $.ajax({
        url: `/Profile/DeleteStep2`,
        type: 'POST',
        data: { rowId: index, guid: gauravGuid },
        success: function (r) {
            if (r.status) {
                alert("Deleted Successfully!!");
                location.reload();
            }
            else {
                alert("Error Saving!");
            }            
        }
    });
}
function editRow(index) {
    debugger;
    loadDynamicFormWithAnswers(index); 
    setTimeout(() => {
        calculatePanchGaurav();
    }, 200);
}

function loadDynamicFormWithAnswers(rowId) {

    $('#hiddenrowId').val(rowId);
    //loadDynamicForm(); // Load dynamic form fresh

    let items = savedRows.filter(x => x.rowId == rowId);
    if (items.length === 0) return;

    items.forEach(item => {

        // Build dynamic input name
        let inputName = `Q_${item.questionMasterId}_${item.subQuestionMasterId}`;

        let input = document.querySelector(`[name='${inputName}']`);

        if (!input) return;

        // ---- CASE 1: If element is a dropdown ----
        if (input.tagName === "SELECT") {

            // Set dropdown exact value
            input.value = item.answerValue;

            // If value not found, try match by text
            if (input.value !== item.answerValue) {

                let option = Array.from(input.options)
                    .find(opt => opt.text.trim() === item.answerValue.trim());

                if (option) input.value = option.value;
            }

            // Trigger change event
            input.dispatchEvent(new Event("change"));
        }

        // ---- CASE 2: Textbox or Textarea ----
        else {
            input.value = item.answerValue;
        }
    });

    $("#btnSaveStep2").text("Update");
    //alert("Dynamic form loaded with saved answers.");
}

function saveStep2() {
   
    var guid = $('#hiddenGauravGuid').val();
    var rowId = $('#hiddenrowId').val();
    let answers = [];

    // Read all dynamic fields
    $("[name^='Q_']").each(function () {

        let fullName = $(this).attr("name"); // Q_10_3
        let value = $(this).val();

        let parts = fullName.split("_");
        let questionId = parts[1];
        let subQuestionId = parts[2];

        answers.push({
            QuestionMasterId: questionId,
            SubQuestionMasterId: subQuestionId,
            AnswerValue: value
        });
    });

    console.log(answers); // Debug

    // POST to server
    $.ajax({
        url: `/Profile/Step2?guid=${guid}&rowId=${rowId}`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(answers),
        success: function (data) {
            alert("Saved Successfully!");
            location.reload(); // reload the page
        },
        error: function (xhr) {
            alert("Error Saving!");
        }
    });

}





