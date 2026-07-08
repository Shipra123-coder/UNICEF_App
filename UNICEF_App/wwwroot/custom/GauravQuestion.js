

var gauravQuestion = {
    getdata: function () {
      
        //alert("in");
        var status = true;
        var Gaurav = $('#Gaurav option:selected').val();
        if (Gaurav == "-1") {
            $('label[for="error"]').show();
            errorGaurav.textContent = "Please Select  Gaurav";
            $('#Gaurav').addClass('colordiv');
            $('#Gaurav').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Gaurav').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorGaurav').html('');
        }
        var District = $('#District option:selected').val();
        if (District == "-1") {
            $('label[for="error"]').show();
            errorDistrict.textContent = "Please Select  District";
            $('#District').addClass('colordiv');
            $('#District').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#District').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorDistrict').html('');
        }
        if (!status) {
            return false;
        }

        common.ShowLoader();
    

        ajax.doGetAjax(`${domain.getdomain()}/Indicator/GetGauravQuestion?gauravId=${Gaurav}&&districtId=${District}`, function (r) {
            console.log(r);
            common.HideLoader();
            if (r.status) {
                $('#myTable').html('');
                var htmlstring = "";
                if (r.data.length > 0) {
                    var k = 1;
                    if (r.status && r.data.length > 0) {
                        console.log(r);

                        // Example: Replace this with your actual `data` variable from AJAX
                        const response = r; // Replace with actual object from API

                        let questions = response.data;
                        let tbody = $('#myTable');
                        tbody.empty();

                        questions.forEach(q => {
                            // If this question has sub-questions, render nested table
                            if (q.subQuestions && q.subQuestions.length > 0 && q.questionType != 0) {
                                // Question header row
                        let questionRow = $('<tr>').append(
                            $('<td>').text(q.questionText)
                                .addClass('font_text')
                            //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                        );
                                tbody.append(questionRow);
                                let nestedTable = $('<table>').addClass('table table-bordered').css('margin-left', '20px');
                                let thead = $('<thead>').append(
                                    $('<tr>').append(
                                        ['क्र.', 'गतिविधि', 'प्रस्तावित व्यय', 'CSR', 'MLA-LAD', 'MPLAD', 'अन्य'].map(text =>
                                            $('<th>').text(text)
                                        )
                                    )
                                );
                                let nestedTbody = $('<tbody>');

                                q.subQuestions.forEach((subQ, index) => {
                                    let subRow = $('<tr>');
                                    subRow.append($('<td>').text(index + 1)); // क्र.
                                    subRow.append($('<td>').text(subQ.displayNumber + ". " + subQ.questionText)); // गतिविधि

                                    // Input fields for 5 funding columns
                                    for (let i = 0; i < 5; i++) {
                                        subRow.append(
                                            $('<td>').append(
                                                $('<input>').attr({
                                                    type: 'text',
                                                    placeholder: '',
                                                    name: `q_${subQ.questionMasterId}_col${i}`
                                                }).addClass('form-control form-control-sm')
                                            )
                                        );
                                    }

                                    nestedTbody.append(subRow);
                                });

                                nestedTable.append(thead).append(nestedTbody);
                                tbody.append(
                                    $('<tr>').append(
                                        $('<td>').append(nestedTable)
                                    )
                                );
                            }
                            else if (q.subQuestions && q.subQuestions.length > 0 && q.questionType == 0) {
                                // Add a heading row for the main question
                                let questionRow = $('<tr>').append(
                                    $('<td>').text(q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);

                                // Create the nested table for sub-questions
                                let nestedTable = $('<table>').addClass('table table-bordered').css('margin-left', '20px');
                                let nestedTbody = $('<tbody>');

                                q.subQuestions.forEach((subQ, index) => {
                                    let subRow = $('<tr>').append(
                                        $('<td>').append(
                                            $('<label>').text(subQ.displayNumber + ". " + subQ.questionText).css({ 'font-weight': 'bold' }),
                                            $('<textarea>')
                                                .attr({
                                                    rows: 5,
                                                    cols: 170,
                                                    placeholder: "अपना उत्तर यहाँ लिखें...",
                                                    name: `q_${subQ.questionMasterId}`
                                                })
                                                .addClass('form-control mt-2')
                                        )
                                    );
                                    nestedTbody.append(subRow);
                                });

                                nestedTable.append(nestedTbody);
                                tbody.append(
                                    $('<tr>').append(
                                        $('<td>').append(nestedTable)
                                    )
                                );
                            }
                            else {
                                // Question header row
                                let questionRow = $('<tr>').append(
                                    $('<td>').text(q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);
                                // Render a textarea row for normal question
                                let inputRow = $('<tr>').append(
                                    $('<td>').append(
                                        $('<textarea>')
                                            .attr({
                                                rows: 5,
                                                cols: 170,
                                                placeholder: "अपना उत्तर यहाँ लिखें...",
                                                name: `q_${q.questionMasterId}`
                                            })
                                            .addClass('form-control')
                                    )
                                );
                                tbody.append(inputRow);
                            }
                        });

                        //$('#myTable').html(htmlstring);
                        console.log(htmlstring);
                        if (r.source.toLowerCase() == 'nodal officer')
                            $("#btnSave").prop("disabled", false);
                        $('#divresultnew').find('.form-control.test').css('border', '1px solid #000');
                    } else {
                        tbody.append('<tr><td colspan="4">No data found</td></tr>');
                    }
                    //htmlstring += `<tr>
                    //                        <td colspan="9">Name Of Gaurav : ${r.data[0].gauravDistrictName}</td>
                    //                    </tr>`;
                    //$(r.data).each(function () {
                    //    if (this.indicatortype == 'main') {
                    //        if (r.source.toLowerCase() == 'nodal officer') {
                    //            htmlstring += `<tr><th hidden>${this.indicatorId}</th><th hidden>${this.districtId}</th><th hidden>${this.gauravId}</th><th hidden>${this.gauravIdistId}</th>
                    //                        <th>${i}</th>
                    //                        <th>${this.indicatorname}</th>
                    //                        <th>${this.unit}</th>'/'
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.basevalue}" placeholder="" style="border:1px solid #000" ></th>
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.targetvalue}" placeholder="" style="border:1px solid #000"></th>
                    //     </tr>`;
                    //        }
                    //        else {
                    //            htmlstring += `<tr><th hidden>${this.indicatorId}</th><th hidden>${this.districtId}</th><th hidden>${this.gauravId}</th><th hidden>${this.gauravIdistId}</th>
                    //                        <th>${i}</th>
                    //                        <th>${this.indicatorname}</th>
                    //                        <th>${this.unit}</th>'/'
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.basevalue}" placeholder="" style="border:1px solid #000" readonly></th>
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.targetvalue}" placeholder="" style="border:1px solid #000" readonly></th>
                    //     </tr>`;
                    //        }
                    //    }
                    //    else if (this.indicatortype == 'group_main') {
                    //        if (r.source.toLowerCase() == 'nodal officer') {
                    //            htmlstring += `<tr><th hidden>${this.indicatorId}</th><th hidden>${this.districtId}</th><th hidden>${this.gauravId}</th><th hidden>${this.gauravIdistId}</th>
                    //                        <th>${i}</th>
                    //                        <th>${this.indicatorname}</th>
                    //                        <th>${this.unit}</th>'/'
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.basevalue}" placeholder="" style="border:1px solid #000" ></th>
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.targetvalue}" placeholder="" style="border:1px solid #000"></th>
                    //     </tr>`;
                    //        }
                    //        else {
                    //            htmlstring += `<tr><th hidden>${this.indicatorId}</th><th hidden>${this.districtId}</th><th hidden>${this.gauravId}</th><th hidden>${this.gauravIdistId}</th>
                    //                        <th>${i}</th>
                    //                        <th>${this.indicatorname}</th>
                    //                        <th>${this.unit}</th>'/'
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.basevalue}" placeholder="" style="border:1px solid #000" readonly></th>
                    //                        <th><input type="text" class="form-control form-control-sm mb-3" value="${this.targetvalue}" placeholder="" style="border:1px solid #000" readonly></th>
                    //     </tr>`;
                    //        }

                    //    }
                    //    i++;
                    //    $('#myTable').html(htmlstring);
                    //    console.log(htmlstring);
                    //    if (r.source.toLowerCase() == 'nodal officer')
                    //    $("#btnSave").prop("disabled", false);
                    //    $('#divresultnew').find('.form-control.test').css('border', '1px solid #000');
                    //});
                }
                else {
                    toast.showToast('error', r.message, 'error');
                }
            }
        });
    },

    savedata: function () {
       
        var tableData = [];
        var gauravName = "";

        // Loop through each row in the table body
        $("#myTable tr").each(function (index) {
            var $row = $(this);

            // Row with Gaurav Name
            if ($row.find("td[colspan='9']").length > 0) {
                var text = $row.find("td").text().trim();
                gauravName = text.replace("Name Of Gaurav :", "").trim();
                return; // Skip this row
            }

            var ths = $row.find("th");

            // Ensure row has the correct number of cells
            if (ths.length >= 9) {
                var rowData = {
                    Indicator_Id: $(ths[0]).text().trim(),
                    District_Id: $(ths[1]).text().trim(),
                    Gaurav_Id: $(ths[2]).text().trim(),
                    GauravDist_Id: $(ths[3]).text().trim(),
                    Unit: $(ths[6]).text().trim(),
                    Basevalue: $(ths[7]).find("input").val().trim(),
                    TargetValue: $(ths[8]).find("input").val().trim(),
                    //Remark: $(ths[9]).find("input").val().trim(),
                };
                tableData.push(rowData);
            }
        });
        
        console.log(tableData);
        common.ShowLoader();
        ajax.AjaxPost(`${domain.getdomain()}/Indicator/SaveIndicatorTargetMaster`, JSON.stringify(tableData), function (r) {
            common.HideLoader();
            if (r.status) {
                toast.showToast('success', r.message, 'success');
                // Additional success logic...
            } else {
                toast.showToast('error', r.message, 'error');
            }
        });
    },

    getdatanw: function () {
       
        //alert("in");
        var status = true;
        var Gaurav = $('#Gaurav option:selected').val();
        if (Gaurav == "-1") {
            $('label[for="error"]').show();
            errorGaurav.textContent = "Please Select  Gaurav";
            $('#Gaurav').addClass('colordiv');
            $('#Gaurav').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Gaurav').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorGaurav').html('');
        }
        var District = $('#District option:selected').val();
        if (District == "-1") {
            $('label[for="error"]').show();
            errorDistrict.textContent = "Please Select  District";
            $('#District').addClass('colordiv');
            $('#District').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#District').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorDistrict').html('');
        }
        if (!status) {
            return false;
        }

        common.ShowLoader();
       
        ajax.doGetAjax(`${domain.getdomain()}/Indicator/GetGauravQuestion?gauravId=${Gaurav}&&districtId=${District}`, function (r) {
            console.log(r);
            common.HideLoader();
            if (r.status) {
                $('#myTable').html('');
                var htmlstring = "";
                if (r.data.length > 0) {
                    var k = 1;
                    if (r.status && r.data.length > 0) {
                        console.log(r);

                        // Example: Replace this with your actual `data` variable from AJAX
                        const response = r; // Replace with actual object from API

                        let questions = response.data;
                        let tbody = $('#myTable');
                        tbody.empty();
                        questions.forEach(q => {
                            // If this question has sub-questions, render nested table
                            if (q.subQuestions && q.subQuestions.length > 0) {
                                // Question header row
                                let questionRow = $('<tr>').append(
                                    $('<td>').text(q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);

                                // Create nested table for subQuestions
                                let nestedTable = $('<table>').addClass('table table-bordered').css({
                                    'border': '1px solid #000'
                                });

                                // Loop through subQuestions and add rows to the nested table
                                q.subQuestions.forEach(sub => {

                                    if (sub.subColumn && sub.subColumn.length > 0) {
                                        // Sub-question header row
                                        let subRow = $('<tr>').append(
                                            $('<td colspan="100%">').text(sub.displayNumber + ". " + sub.questionText)
                                                .css({ 'padding-left': '10px', 'font-weight': 'bold' })
                                        );
                                        nestedTable.append(subRow);

                                        let tableId = 'nestedTable_' + sub.guid;

                                        // Create nested table with empty thead and tbody
                                        let nestedTableHtml = $(`<table id="${tableId}"  data-qid="${sub.questionMasterId}" class="nested-table tbl_class">
                                <thead><tr></tr></thead>
                                <tbody></tbody>
                            </table>`);

                                        nestedTable.append($('<tr>').append($('<td colspan="100%">').append(nestedTableHtml)));

                                        const $headerRow = nestedTableHtml.find('thead tr');
                                        const $body = nestedTableHtml.find('tbody');

                                        // Populate column headers
                                        sub.subColumn.forEach(col => {
                                            $headerRow.append($('<th>').text(col.questionText).css({
                                                'border': '1px solid #ccc',
                                                'padding': '8px',
                                                'background': '#f9f9f9'
                                            }));
                                        });

                                        // Add Action column header
                                        $headerRow.append($('<th>').text('Action').css({
                                            'border': '1px solid #ccc',
                                            'padding': '8px',
                                            'background': '#f9f9f9',
                                            'width': '100px'
                                        }));

                                        // Function to create a data row
                                        function createDataRow() {
                                            let $dataRow = $('<tr>');

                                            sub.subColumn.forEach(col => {
                                                let safeId = `subq_${sub.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 10000)}`;
                                                $dataRow.append(
                                                    $('<td>').html(`<input type="text" style="width:100%;" class="form-control" id="${safeId}" placeholder="अपना उत्तर यहाँ लिखें..." />`).css({
                                                        'border': '1px solid #ccc',
                                                        'padding': '6px'
                                                    })
                                                );
                                            });

                                            // Add action cell with + and − buttons
                                            let $actionTd = $('<td>').css({
                                                'border': '1px solid #ccc',
                                                'text-align': 'center',
                                                'white-space': 'nowrap'
                                            });

                                            let $addBtn = $('<button type="button">')
                                                .addClass('btn btn-sm btn-success me-1')
                                                .text('+')
                                                .on('click', function () {
                                                    $body.append(createDataRow());
                                                });

                                            let $removeBtn = $('<button type="button">')
                                                .addClass('btn btn-sm btn-danger')
                                                .text('−')
                                                .on('click', function () {
                                                    $(this).closest('tr').remove();
                                                });

                                            $actionTd.append($addBtn).append($removeBtn);
                                            $dataRow.append($actionTd);

                                            return $dataRow;
                                        }

                                        // Add initial row
                                        $body.append(createDataRow());
                                    }

                                    else {
                                        // Sub-question header
                                        let subRow = $('<tr>').append(
                                            $('<td>').text(sub.displayNumber + ". " + sub.questionText)
                                                .css({ 'padding-left': '5px', 'font-weight': 'bold' })
                                        );
                                        nestedTable.append(subRow);

                                        // Sub-question input
                                        let subInputRow = $('<tr>').append(
                                            $('<td>').append(
                                                $('<textarea>')
                                                    .attr({
                                                        rows: 3,
                                                        cols: 150,
                                                        placeholder: "अपना उत्तर यहाँ लिखें...",
                                                        name: `q_${sub.questionMasterId}`,
                                                        id: `q_${sub.questionMasterId}`
                                                    })
                                                    .addClass('form-control')
                                            )
                                        );
                                        nestedTable.append(subInputRow);
                                    }

                                });

                                // Append the nested table row into the main table
                                let nestedTableRow = $('<tr>').append(
                                    $('<td>').append(nestedTable)
                                );
                                tbody.append(nestedTableRow);
                            }
                            else if (q.subQuestions && q.subQuestions.length == 0 && q.subColumn.length > 0) {
                                // Header row for question
                                let questionRow = $('<tr>').append(
                                    $('<td colspan="100%">')
                                        .text(q.displayNumber + ". " + q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);

                                // Nested table
                                let nestedTable = $('<table>')
                                    .addClass('table table-bordered nested-table tbl_class')
                                    .attr('data-qid', q.questionMasterId) // <-- Add this line
                                //.css({
                                //    'border': '1px solid #000',
                                //    'border-collapse': 'collapse',
                                //    'width': '100%',
                                //    'margin-left': '0px'
                                //});

                                // Table head
                                let thead = $('<thead>');
                                let headerRow = $('<tr>');

                                // SubColumn headers
                                q.subColumn.forEach(col => {
                                    let th = $('<th>').text(col.questionText).css({
                                        'border': '1px solid #000',
                                        'padding': '8px',
                                        'background-color': '#f1f1f1'
                                    });
                                    headerRow.append(th);
                                });

                                // Add "Action" column
                                headerRow.append($('<th>').text('Action').css({
                                    'border': '1px solid #000',
                                    'padding': '8px',
                                    'background-color': '#f1f1f1',
                                    'width': '100px'
                                }));

                                thead.append(headerRow);

                                // Table body
                                let subTbody = $('<tbody>');

                                // Row creation function with Add & Remove buttons
                                function createDataRow() {
                                    let dataRow = $('<tr>');

                                    // Input fields for each column
                                    q.subColumn.forEach(col => {
                                        let safeId = `subq_${q.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 10000)}`;
                                        let td = $('<td>').html(`<input type="text" style="width:100%;" class="form-control" id="${safeId}" placeholder="अपना उत्तर यहाँ लिखें..." />`).css({
                                            'border': '1px solid #ccc',
                                            'padding': '6px'
                                        });
                                        dataRow.append(td);
                                    });

                                    // Action buttons (Add & Remove)
                                    let actionTd = $('<td>').css({
                                        'border': '1px solid #ccc',
                                        'text-align': 'center',
                                        'white-space': 'nowrap'
                                    });

                                    let addButton = $('<button type="button">')
                                        .addClass('btn btn-sm btn-success me-1')
                                        .text('+')
                                        .on('click', function () {
                                            subTbody.append(createDataRow());
                                        });

                                    let removeButton = $('<button type="button">')
                                        .addClass('btn btn-sm btn-danger')
                                        .text('−')
                                        .on('click', function () {
                                            $(this).closest('tr').remove();
                                        });

                                    actionTd.append(addButton).append(removeButton);
                                    dataRow.append(actionTd);

                                    return dataRow;
                                }

                                // Initial row
                                subTbody.append(createDataRow());

                                // Append to table
                                nestedTable.append(thead).append(subTbody);
                                let nestedRow = $('<tr>').append($('<td colspan="100%">').append(nestedTable));
                                tbody.append(nestedRow);
                            }
                            else {
                                // Question header row
                                let questionRow = $('<tr>').append(
                                    $('<td>').text(q.displayNumber + ". " + q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);
                                // Render a textarea row for normal question
                                let inputRow = $('<tr>').append(
                                    $('<td>').append(
                                        $('<textarea>')
                                            .attr({
                                                rows: 2,
                                                cols: 170,
                                                placeholder: "अपना उत्तर यहाँ लिखें...",
                                                name: `q_${q.questionMasterId}`,
                                                id: `q_${q.questionMasterId}`
                                            })
                                            .addClass('form-control')
                                    )
                                );
                                tbody.append(inputRow);
                            }
                        });

                    } else {
                        tbody.append('<tr><td colspan="4">No data found</td></tr>');
                    }

                }
                else {
                    toast.showToast('error', r.message, 'error');
                }

                setTimeout(gauravQuestion.getEditAnswer(guid), 2000);
            }
            else {
                $('#myTable').html('');
            }
        });

        gauravQuestion.GetGauravVivran();
        gauravQuestion.getMainGauravProfile();
    },

    getEditAnswer: function (guid) {
        //alert("in");
        //var Gaurav = $('#Gaurav').val();
        var District = $('#District').val();
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetAnswersForEdit?gauravId=${guid}&districtId=${District}`, function (r) {
            console.log(r);
            //common.HideLoader();
            if (r.status) {
                const answers = r.data;
                answers.forEach(block => {
                    // ✅ Redirect if block.finalSubmit is true
                    //if (block.answers[0].finalSubmit === true) {
                    //    window.location.href = `${domain.getdomain()}/GauravProfile/PrintPreview`;
                    //    return; // Stop further processing
                    //}
                    // Main Question without sub-question
                    if (block.answers.length === 1 && block.answers[0].subQuestionId === "") {
                        const textareaId = `#q_${block.questionId}`;
                        $(textareaId).val(block.answers[0].answer);
                    }
                    else {
                        // find the table for this questionId
                        const $table = $(`.nested-table[data-qid="${block.questionId}"]`);
                        const $tbody = $table.find('tbody');
                        // 1) filter down to only question "13"
                        //
                        const qid = block.questionId;
                        const only13 = block.answers.filter(e => e.mainQuestionId === qid);

                        // 2) map to the rowId (default to 1 if somehow missing)
                        const rowIds = only13.map(e => e.rowId || 1);

                        // 3) take the max (or 1 if there are no entries)
                        const maxRowId = rowIds.length > 0
                            ? Math.max(...rowIds)
                            : 1;

                        console.log(maxRowId);  // → 2

                        const subQuestionMap = {};

                        // Group subQuestionIds by rowId
                        block.answers.forEach(item => {
                            const rowId = item.rowId;

                            if (!subQuestionMap[rowId]) {
                                subQuestionMap[rowId] = [];
                            }

                            if (!subQuestionMap[rowId].includes(item.subQuestionId)) {
                                subQuestionMap[rowId].push(item.subQuestionId);
                            }
                        });

                        // Output the result
                        console.log(subQuestionMap);

                        // assume maxRowId already computed, and createEmptyRow(qid) returns a new <tr> for that question
                        if (maxRowId > 1) {
                            for (let i = 1; i < maxRowId; i++) {
                                // append one empty row for this question
                                $tbody.append(createEmptyRow(qid));
                            }
                        }


                        block.answers.forEach(sub => {
                            // rowId is 1-based; convert to zero‐based index
                            const rowIndex = (sub.rowId || 1) - 1;
                            if (sub.rowId > 1) {
                                //
                            }

                            // pick that <tr>
                            const $row = $table.find('tbody tr').eq(rowIndex);

                            // now find the cell/input whose id starts with subq_{main}_{sub}_
                            const $input = $row.find(
                                `input[id^="subq_${sub.mainQuestionId}_${sub.subQuestionId}_"]`
                            );

                            $input.val(sub.answer);
                        });

                        // helper: create one new empty <tr> for a given question
                        function createEmptyRow(qid) {
                            const colIds = subQuestionMap[1];
                            //
                            const $tr = $('<tr>');
                            colIds.forEach(subQId => {
                                const rnd = Math.floor(Math.random() * 1e4);
                                const id = `subq_${qid}_${subQId}_${rnd}`;
                                $tr.append(
                                    $('<td>').html(
                                        `<input type="text" class="form-control" id="${id}" style="width:100%;" />`
                                    ).css({ border: '1px solid #ccc', padding: '6px' })
                                );
                            });

                            // append your action cell
                            const $actions = $('<td>').css({ border: '1px solid #ccc', textAlign: 'center' });
                            $actions.append(
                                $('<button class="btn btn-sm btn-success me-1">+</button>').on('click', () => {
                                    $tr.closest('tbody').append(createEmptyRow(qid));
                                }),
                                $('<button class="btn btn-sm btn-danger">−</button>').on('click', () => {
                                    $tr.remove();
                                })
                            );
                            $tr.append($actions);
                            return $tr;
                        }
                    }
                    //else {
                    //    block.answers.forEach(sub => {
                    //        // Match dynamic input by partial ID (format: subq_{Main}_{Sub}_{Random})
                    //        const inputSelector = `input[id^='subq_${sub.mainQuestionId}_${sub.subQuestionId}_']`;
                    //        $(inputSelector).val(sub.answer);
                    //    });
                    //}
                });
            }
        });
    },

    getClosestQuestionId: function ($table) {
        // Try to find a question display number row before this table
        let $prevRow = $table.closest('tr').prevAll('tr').first();
        let text = $prevRow.find('td').first().text();
        let match = text.match(/^(\d+)\.\s/); // Extract "1. Question Text"
        return match ? match[1] : null;
    },

    //saveGauravAnswers: function () {

    //    var resultData = [];
    //    // ✅ 1. Get answers from textarea-based questions
    //    $('textarea[id^="q_"]').each(function () {
    //        const $textarea = $(this);
    //        const questionId = $textarea.attr('id').replace('q_', '');
    //        const value = $textarea.val().trim();

    //        if (value) {
    //            resultData.push({
    //                questionId: questionId,
    //                answers: [value]
    //            });
    //        }
    //    });

    //    // ✅ 2. Get answers from simple subColumn-based tables (no subQuestions)
    //    $('.nested-table').each(function () {
    //        const $table = $(this);
    //        const $thead = $table.find('thead');
    //        const $tbody = $table.find('tbody');

    //        // Only proceed if this table has input rows
    //        if ($tbody.length > 0) {
    //            $tbody.find('tr').each(function (rowIndex) {
    //                const $row = $(this);
    //                const rowId = rowIndex + 1;   // 1-based row number; or just use rowIndex
    //                let rowAnswers = [];

    //                $row.find('input[type="text"]').each(function () {
    //                    const inputId = $(this).attr('id'); // e.g., "subq_8_1_2559"
    //                    const parts = inputId.split('_');   // ["subq", "8", "1", "2559"]
    //                    const mainQuestionId = parts[1];
    //                    const subQuestionId = parts[2];
    //                    const value = $(this).val().trim();

    //                    rowAnswers.push({
    //                        mainQuestionId: mainQuestionId,
    //                        subQuestionId: subQuestionId,
    //                        answer: value,
    //                        rowId           // <-- include the row’s index/ID here
    //                    });
    //                });

    //                if (rowAnswers.length > 0 && rowAnswers.some(ans => ans !== '')) {
    //                    resultData.push({
    //                        questionId: gauravQuestion.getClosestQuestionId($table),
    //                        answers: rowAnswers
    //                    });
    //                }
    //            });
    //        }
    //    });

    //    console.log("Final Saved Data:", resultData);
    //    //
    //    alert(resultData);
    //    resultData = resultData.map(entry => {
    //        if (entry.questionId) {
    //            entry.answers = entry.answers.map(ans => ({
    //                mainQuestionId: entry.questionId,
    //                subQuestionId: null,
    //                answer: ans
    //            }));
    //            entry.questionId = null;
    //        }
    //        return entry;
    //    });

    //    var model = {
    //        gauravId: $('#Gaurav').val(),
    //        model: resultData
    //    }
    //    console.log("Final Saved Data:", resultData);
    //    //common.ShowLoader();
    //    //gauravQuestion.saveMainGauravProfile();
    //    gauravQuestion.saveGauravprofile(model);
    //    //gauravQuestion.saveGauravVivran();

    //},
    //saveGauravprofile: function (model) {
    //    //common.ShowLoader();
    //    ajax.AjaxPost(`${domain.getdomain()}/GauravProfile/SaveGauravAnswers`, JSON.stringify(model), function (r) {
    //        common.HideLoader();
    //        if (r.status) {
    //            //toast.showToast('success', r.message, 'success');
    //            // Additional success logic...
    //            //alert('saved successfully!');
    //            toast.showToast('success', 'saved successfully!', 'success');
    //            window.location.href = `${domain.getdomain()}/GauravProfile/PrintPreview?guid=${model.gauravId}`;
    //        } else {
    //            toast.showToast('error', r.message, 'error');
    //        }
    //    });
    //},
    saveGauravprofile: function (model) {
        common.ShowLoader();
        return new Promise(function (resolve, reject) {
            ajax.AjaxPost(`${domain.getdomain()}/GauravProfile/SaveGauravAnswers`, JSON.stringify(model), function (r) {
                common.HideLoader();
                if (r.status) {
                    // Optionally show success message here, or let caller decide
                    toast.showToast('success', 'saved successfully!', 'success');
                    window.location.href = `${domain.getdomain()}/GauravProfile/PrintPreview?guid=${model.gauravId}`;
                    resolve(r); // Call success
                } else {
                    toast.showToast('error', r.message, 'error');
                    reject(r); // Let caller handle error
                }
            });
        });
    },

    saveGauravVivran: function () {
        var District = $('#District').val();
        const dataToSave = gauravQuestion.getGauravVivranSavePayload(District); // pass districtId

        ajax.AjaxPost(`${domain.getdomain()}/GauravProfile/SaveGauravVivran`, JSON.stringify(dataToSave), function (r) {
            if (r.status) {
                alert('Gaurav Vivran saved successfully!');
            } else {
                alert('Failed to save.');
            }
        });
    },
    GetGauravVivran: function () {
        var District = $('#District option:selected').val();
        // Get Gaurav Vivran
       
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetGauravVivran?districtId=${District}`, function (r) {
            console.log(r);
            
            common.HideLoader();
            if (r.status && r.data.data.length > 0) {
                r.data.data.forEach(item => {
                    
                    const name = item.gauravName.toLowerCase();
                    const gauravDistName = item.gauravDistname.toLowerCase();
                    const mangal = item.mangalName || "";
                    const vivran = item.vivran || "";

                    switch (name) {
                        case 'crop':
                            $('#crop_name').text(gauravDistName);
                            $('#crop_vivran').val(vivran);
                            break;
                        case 'botanical species':
                            $('#botanical_name').text(gauravDistName);
                            $('#botanical_vivran').val(vivran);
                            break;
                        case 'product':
                            $('#product_name').text(gauravDistName);
                            $('#product_vivran').val(vivran);
                            break;
                        case 'tourist place':
                            $('#tourist_name').text(gauravDistName);
                            $('#tourist_vivran').val(vivran);
                            break;
                        case 'sports':
                            $('#sports_name').text(gauravDistName);
                            $('#sports_vivran').val(vivran);
                            break;
                        default:
                            console.warn('Unrecognized Gaurav:', item.gauravName);
                    }
                    
                    if (item.finalSubmit) {
                        // Make all input and textarea fields in the Vivran table readonly
                        $('#tbl_GauravVivran').find('input, textarea')
                            .prop('readonly', true)
                            .addClass('bg-readonly'); // Apply class

                        // Also add class to specific elements
                        $('#prastwana, #distprofile1, #distprofile2').addClass('bg-readonly');

                        // Optionally remove save/final-save buttons
                        $('.row.justify-content-end').hide();
                    } else {
                        $('#tbl_GauravVivran').find('input, textarea')
                            .prop('readonly', false)
                            .removeClass('bg-readonly'); // Remove class when not readonly

                        $('#prastwana, #distprofile1, #distprofile2').removeClass('bg-readonly');


                        $('.row.justify-content-end').show();
                    }
                });
            } else {
                alert('No data found.');
            }
        });
    },
    getGauravVivranSavePayload: function (District) {
        
        return [
            {
                gauravName: 'Crop',
                gauravDistname: $('#crop_name').text().trim(),
                mangalName: '', // if needed
                vivran: $('#crop_vivran').val().trim()
            },
            {
                gauravName: 'Botanical Species',
                gauravDistname: $('#botanical_name').text().trim(),
                mangalName: '',
                vivran: $('#botanical_vivran').val().trim()
            },
            {
                gauravName: 'Product',
                gauravDistname: $('#product_name').text().trim(),
                mangalName: '',
                vivran: $('#product_vivran').val().trim()
            },
            {
                gauravName: 'Tourist Place',
                gauravDistname: $('#tourist_name').text().trim(),
                mangalName: '',
                vivran: $('#tourist_vivran').val().trim()
            },
            {
                gauravName: 'Sports',
                gauravDistname: $('#sports_name').text().trim(),
                mangalName: '',
                vivran: $('#sports_vivran').val().trim()
            }
        ];
    },
    saveMainGauravProfile: function () {
        var District = $('#District').val();
        const data = {
            Introduction: $('#prastwana').val().trim(),
            DistrictProfileOne: $('#distprofile1').val().trim(),
            DistrictProfileTwo: $('#distprofile2').val().trim(),
            //DistrictId: District, // Replace with actual districtId variable
        };
        ajax.AjaxPost(`${domain.getdomain()}/GauravProfile/SaveGauravMainProfileAnswer`, JSON.stringify(data), function (r) {
            if (r.status) {
                alert('Main Gaurav Profile saved successfully!');
            } else {
                alert('Failed to save.');
            }
        });
    },
    getMainGauravProfile: function () {
        // get Gauravmainprofile
        var District = $('#District option:selected').val();
        common.ShowLoader();
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetMainGauravProfile?districtId=${District}`, function (r) {
            console.log(r);
            
            common.HideLoader();

            if (r.status && r.data) {
                const data = r.data.data[0];  // ✅ Use r.data directly (not r.data.forEach)

                $('#prastwana').val(data.introduction || '');
                $('#distprofile1').val(data.districtProfile_One || '');
                $('#distprofile2').val(data.districtProfile_Two || '');
                if (data.finalSubmit === true) {
                    // Make textareas readonly
                    $('#prastwana').prop('readonly', true);
                    $('#distprofile1').prop('readonly', true);
                    $('#distprofile2').prop('readonly', true);

                    // Remove the Save/Final Save button section
                    $('.row.justify-content-end').hide();
                }
                else {
                    $('.row.justify-content-end').show();
                }
            } else {
                alert('No data found.');
            }
        });
    },

    //step 1
    saveMainProfile: function () {
        //gauravQuestion.saveMainGauravProfile();
        //gauravQuestion.saveGauravVivran();
        var District = $('#District').val();
        const data = {
            Introduction: $('#prastwana').val().trim(),
            DistrictProfileOne: $('#distprofile1').val().trim(),
            DistrictProfileTwo: $('#distprofile2').val().trim(),
            //DistrictId: District, // Replace with actual districtId variable
        };

        const dataToSave = gauravQuestion.getGauravVivranSavePayload(District); // pass districtId

        var model = {
            _vivranList: dataToSave,
            _data: data
        }
        common.ShowLoader();
        ajax.AjaxPost(`${domain.getdomain()}/GauravProfile/Profile_step1`, JSON.stringify(model), function (r) {
            if (r.status) {
                // Redirect on success
                alert('saved successfully!');
                //window.location.href = `${domain.getdomain()}/GauravProfile/Profiles`;
                gauravQuestion.getMainGauravProfile();

            } else {
                alert('Failed to save.');
            }
        });
    },

    saveMainProfile_Final: function () {
        var District = $('#District').val();
        const data = {
            Introduction: $('#prastwana').val().trim(),
            DistrictProfileOne: $('#distprofile1').val().trim(),
            DistrictProfileTwo: $('#distprofile2').val().trim(),
            //DistrictId: District, // Replace with actual districtId variable
        };

        const dataToSave = gauravQuestion.getGauravVivranSavePayload(District); // pass districtId

        var model = {
            _vivranList: dataToSave,
            _data: data
        }
        common.ShowLoader();
        //common.ShowLoader();
        Swal.fire({
            title: 'Are you sure?',
            text: "Do you really want to final submit?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // ✅ Perform your action here
                ajax.AjaxPost(`${domain.getdomain()}/GauravProfile/Profile_step1_Final`, JSON.stringify(model), function (r) {
                    common.HideLoader();
                    if (r.status) {
                        // Redirect on success
                        //alert('saved successfully!');
                        window.location.href = `${domain.getdomain()}/GauravProfile/Profile`;
                        //gauravQuestion.getMainGauravProfile();

                    } else {
                        alert('Failed to save.');
                    }
                });

            }
            else {
                common.HideLoader();
            }
        });

    },

    //step 2
    getGauravQuestions: function (guid) {
        
        var status = true;
        var District = $('#District option:selected').val();
        if (District == "-1") {
            $('label[for="error"]').show();
            errorDistrict.textContent = "Please Select  District";
            $('#District').addClass('colordiv');
            $('#District').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#District').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorDistrict').html('');
        }
        if (!status) {
            return false;
        }
        $('.row.justify-content-end').show();
       
        ajax.doGetAjax(`${domain.getdomain()}/Indicator/GetGauravQuestion?gauravId=${guid}&&districtId=${District}`, function (r) {
            console.log(r);
            common.HideLoader();
            //if (r.status) {
            //    gauravQuestion.renderQuestions(r.data);
            //}
            if (r.status) {
                         $('#myTable').html('');
                         var htmlstring = "";
                         if (r.data.length > 0) {
                             var k = 1;
                             if (r.status && r.data.length > 0) {
                                 console.log(r);

                                 // Example: Replace this with your actual `data` variable from AJAX
                                 const response = r; // Replace with actual object from API

                                 let questions = response.data;
                                 let tbody = $('#myTable');
                                 tbody.empty();
                                 questions.forEach(q => {
                                     // If this question has sub-questions, render nested table
                                     if (q.subQuestions && q.subQuestions.length > 0) {
                                         // Question header row
                                         console.log("in 1");
                                         let questionRow = $('<tr>').append(
                                             $('<td>').text(q.displayNumber + ". " + q.questionText)
                                                 .addClass('font_text')
                                                 //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                         );
                                         tbody.append(questionRow);

                                         // Create nested table for subQuestions
                                         let nestedTable = $('<table>').addClass('table table-bordered').css({
                                             'border': '1px solid #000'
                                         });

                                         // Loop through subQuestions and add rows to the nested table
                                         q.subQuestions.forEach(sub => {

                                             if (sub.subColumn && sub.subColumn.length > 0) {
                                                 // Sub-question header row
                                                 let subRow = $('<tr>').append(
                                                     $('<td colspan="100%">').text(sub.displayNumber + ". " + sub.questionText)
                                                         .css({ 'padding-left': '10px', 'font-weight': 'bold' })
                                                 );
                                                 nestedTable.append(subRow);

                                                 let tableId = 'nestedTable_' + sub.guid;

                                                 // Create nested table with empty thead and tbody
                                                 let nestedTableHtml = $(`<table id="${tableId}"  data-qid="${sub.questionMasterId}" class="nested-table tbl_class">
                                 <thead><tr></tr></thead>
                                 <tbody></tbody>
                             </table>`);

                                                 nestedTable.append($('<tr>').append($('<td colspan="100%">').append(nestedTableHtml)));

                                                 const $headerRow = nestedTableHtml.find('thead tr');
                                                 const $body = nestedTableHtml.find('tbody');

                                                 // Populate column headers
                                                 sub.subColumn.forEach(col => {
                                                     $headerRow.append($('<th>').text(col.questionText).css({
                                                         'border': '1px solid #ccc',
                                                         'padding': '8px',
                                                         'background': '#f9f9f9'
                                                     }));
                                                 });

                                                 // Add Action column header
                                                 $headerRow.append($('<th>').text('Action').css({
                                                     'border': '1px solid #ccc',
                                                     'padding': '8px',
                                                     'background': '#f9f9f9',
                                                     'width': '100px'
                                                 }));

                                                 // Function to create a data row
                                                 function createDataRow() {
                                                     let $dataRow = $('<tr>');

                                                     sub.subColumn.forEach(col => {
                                                         let safeId = `subq_${sub.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 10000)}`;
                                                         $dataRow.append(
                                                             $('<td>').html(`<input type="text" style="width:100%;" class="form-control" id="${safeId}" placeholder="अपना उत्तर यहाँ लिखें..." />`).css({
                                                                 'border': '1px solid #ccc',
                                                                 'padding': '6px'
                                                             })
                                                         );
                                                     });

                                                     // Add action cell with + and − buttons
                                                     let $actionTd = $('<td>').css({
                                                         'border': '1px solid #ccc',
                                                         'text-align': 'center',
                                                         'white-space': 'nowrap'
                                                     });

                                                     let $addBtn = $('<button type="button">')
                                                         .addClass('btn btn-sm btn-success me-1')
                                                         .text('+')
                                                         .on('click', function () {
                                                             $body.append(createDataRow());
                                                         });

                                                     let $removeBtn = $('<button type="button">')
                                                         .addClass('btn btn-sm btn-danger')
                                                         .text('−')
                                                         .on('click', function () {
                                                             $(this).closest('tr').remove();
                                                         });

                                                     $actionTd.append($addBtn).append($removeBtn);
                                                     $dataRow.append($actionTd);

                                                     return $dataRow;
                                                 }

                                                 // Add initial row
                                                 $body.append(createDataRow());
                                             }

                                             else {
                                                 // Sub-question header
                                                 let subRow = $('<tr>').append(
                                                     $('<td>').text(sub.displayNumber + ". " + sub.questionText)
                                                         .css({ 'padding-left': '5px', 'font-weight': 'bold' })
                                                 );
                                                 nestedTable.append(subRow);

                                                 // Sub-question input
                                                 let subInputRow = $('<tr>').append(
                                                     $('<td>').append(
                                                         $('<textarea>')
                                                             .attr({
                                                                 rows: 3,
                                                                 cols: 150,
                                                                 placeholder: "अपना उत्तर यहाँ लिखें...",
                                                                 name: `q_${sub.questionMasterId}`,
                                                                 id: `q_${sub.questionMasterId}`
                                                             })
                                                             .addClass('form-control')
                                                     )
                                                 );
                                                 nestedTable.append(subInputRow);
                                             }

                                         });

                                         // Append the nested table row into the main table
                                         let nestedTableRow = $('<tr>').append(
                                             $('<td>').append(nestedTable)
                                         );
                                         tbody.append(nestedTableRow);
                                     }
                                     else if (q.subQuestions && q.subQuestions.length == 0 && q.subColumn.length > 0) {
                                         console.log("in 2");

                                         // Question Title
                                         let questionRow = $('<tr>').append(
                                             $('<td colspan="100%">')
                                                 .text(q.displayNumber + ". " + q.questionText)
                                                 .addClass('font_text')
                                         );
                                         tbody.append(questionRow);

                                         // Wrapper row where DIV block will be placed
                                         let wrapperRow = $('<tr>');
                                         let wrapperCol = $('<td colspan="100%">');

                                         // Container for activity blocks
                                         let container = $('<div class="activity-container"></div>');

                                         // Function to create one activity div block
                                         function createActivityBlock() {
                                             let block = $('<div class="activity-block border rounded p-3 mb-3"></div>');
                                             let row = $('<div class="row g-2"></div>');

                                             q.subColumn.forEach(col => {
                                                 let colDiv = $(`
                <div class="col-md-4">
                    <label class="form-label">${col.questionText}</label>
                    <input type="text" class="form-control" placeholder="अपना उत्तर यहाँ लिखें...">
                </div>
            `);
                                                 row.append(colDiv);
                                             });

                                             // Add + / − buttons row
                                             let actionDiv = $(`
            <div class="col-md-6 d-flex align-items-end justify-content-end">
                <button class="btn btn-success btn-sm me-2 add-activity">+</button>
                <button class="btn btn-danger btn-sm remove-activity">−</button>
            </div>
        `);

                                             row.append(actionDiv);
                                             block.append(row);

                                             // ADD / REMOVE Events
                                             block.find('.add-activity').on('click', function () {
                                                 container.append(createActivityBlock());
                                             });

                                             block.find('.remove-activity').on('click', function () {
                                                 block.remove();
                                             });

                                             return block;
                                         }

                                         // Add first activity block
                                         container.append(createActivityBlock());

                                         wrapperCol.append(container);
                                         wrapperRow.append(wrapperCol);
                                         tbody.append(wrapperRow);
                                     }

                                     //else if (q.subQuestions && q.subQuestions.length == 0 && q.subColumn.length > 0) {
                                     //    console.log("in 2");
                                     //    // Header row for question
                                     //    let questionRow = $('<tr>').append(
                                     //        $('<td colspan="100%">')
                                     //            .text(q.displayNumber + ". " + q.questionText)
                                     //            .addClass('font_text')
                                     //            //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                     //    );
                                     //    tbody.append(questionRow);

                                     //    // Nested table
                                     //    let nestedTable = $('<table>')
                                     //        .addClass('table table-bordered nested-table tbl_class')
                                     //        .attr('data-qid', q.questionMasterId) // <-- Add this line
                                     //        //.css({
                                     //        //    'border': '1px solid #000',
                                     //        //    'border-collapse': 'collapse',
                                     //        //    'width': '100%',
                                     //        //    'margin-left': '0px'
                                     //        //});

                                     //    // Table head
                                     //    let thead = $('<thead>');
                                     //    let headerRow = $('<tr>');

                                     //    // SubColumn headers
                                     //    q.subColumn.forEach(col => {
                                     //        let th = $('<th>').text(col.questionText).css({
                                     //            'border': '1px solid #000',
                                     //            'padding': '8px',
                                     //            'background-color': '#f1f1f1'
                                     //        });
                                     //        headerRow.append(th);
                                     //    });

                                     //    // Add "Action" column
                                     //    headerRow.append($('<th>').text('Action').css({
                                     //        'border': '1px solid #000',
                                     //        'padding': '8px',
                                     //        'background-color': '#f1f1f1',
                                     //        'width': '100px'
                                     //    }));

                                     //    thead.append(headerRow);

                                     //    // Table body
                                     //    let subTbody = $('<tbody>');

                                     //    // Row creation function with Add & Remove buttons
                                     //    function createDataRow() {
                                     //        let dataRow = $('<tr>');

                                     //        // Input fields for each column
                                     //        q.subColumn.forEach(col => {
                                     //            let safeId = `subq_${q.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 10000)}`;
                                     //            let td = $('<td>').html(`<input type="text" style="width:100%;" class="form-control" id="${safeId}" placeholder="अपना उत्तर यहाँ लिखें..." />`).css({
                                     //                'border': '1px solid #ccc',
                                     //                'padding': '6px'
                                     //            });
                                     //            dataRow.append(td);
                                     //        });

                                     //        // Action buttons (Add & Remove)
                                     //        let actionTd = $('<td>').css({
                                     //            'border': '1px solid #ccc',
                                     //            'text-align': 'center',
                                     //            'white-space': 'nowrap'
                                     //        });

                                     //        let addButton = $('<button type="button">')
                                     //            .addClass('btn btn-sm btn-success me-1')
                                     //            .text('+')
                                     //            .on('click', function () {
                                     //                subTbody.append(createDataRow());
                                     //            });

                                     //        let removeButton = $('<button type="button">')
                                     //            .addClass('btn btn-sm btn-danger')
                                     //            .text('−')
                                     //            .on('click', function () {
                                     //                $(this).closest('tr').remove();
                                     //            });

                                     //        actionTd.append(addButton).append(removeButton);
                                     //        dataRow.append(actionTd);

                                     //        return dataRow;
                                     //    }

                                     //    // Initial row
                                     //    subTbody.append(createDataRow());

                                     //    // Append to table
                                     //    nestedTable.append(thead).append(subTbody);
                                     //    let nestedRow = $('<tr>').append($('<td colspan="100%">').append(nestedTable));
                                     //    tbody.append(nestedRow);
                                     //}
                                     else {
                                         console.log("in 3");
                                         // Question header row
                                         let questionRow = $('<tr>').append(
                                             $('<td>').text(q.displayNumber + ". " + q.questionText)
                                                 .addClass('font_text')
                                                 //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                         );
                                         tbody.append(questionRow);
                                         // Render a textarea row for normal question
                                         let inputRow = $('<tr>').append(
                                             $('<td>').append(
                                                 $('<textarea>')
                                                     .attr({
                                                         rows: 2,
                                                         cols: 170,
                                                         placeholder: "अपना उत्तर यहाँ लिखें...",
                                                         name: `q_${q.questionMasterId}`,
                                                         id: `q_${q.questionMasterId}`
                                                     })
                                                     .addClass('form-control')
                                             )
                                         );
                                         tbody.append(inputRow);
                                     }
                                 });

                             } else {
                                 tbody.append('<tr><td colspan="4">No data found</td></tr>');
                             }

                         }
                         else {
                             toast.showToast('error', r.message, 'error');
                         }

                         setTimeout(gauravQuestion.getEditAnswer(guid),2000);
                     }
            else {
                $('#myTable').html('');
            }
        });
    },
    renderQuestions: function (questions) {
        let html = "";

        questions.forEach(q => {

            // Normal Questions
            if (!q.isSubQuestion && q.subQuestions.length === 0) {

                html += `
                <div class="question-block">
                    <h4>${q.displayNumber}. ${q.questionText}</h4>
                </div>
                <hr/>
            `;
            }

            // Questions with subQuestions
            if (q.subQuestions.length > 0) {
                html += `
               <div class="question-block">
                    <h4>${q.displayNumber}. ${q.questionText}</h4>
               </div>
            `;

                q.subQuestions.forEach(sub => {
                    html += gauravQuestion.renderSubQuestionTable(sub);
                });
            }
        });

        $("#questionsContainer").html(html);
    },
    renderSubQuestionTable: function (sub) {

        let html = `
    <div class="activity-block border rounded p-3 mb-3">
        <h5 class="mb-3">${sub.displayNumber}. ${sub.questionText}</h5>
        <div class="row g-2">
    `;

        // Convert columns into Bootstrap form elements
        sub.subColumn.forEach((col, index) => {

            // COLUMN WIDTH Decide — you can change if needed
            let colWidth = "col-md-4";
            if (sub.subColumn.length == 2) colWidth = "col-md-6";
            if (sub.subColumn.length == 1) colWidth = "col-md-12";

            html += `
        <div class="${colWidth}">
            <label class="form-label">${col.questionText}</label>
            <input type="text" 
                   class="form-control" 
                   placeholder="अपना उत्तर यहाँ लिखें..."
                   id="subq_${sub.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 9999)}">
        </div>`;
        });

        // Add / Remove buttons
        html += `
        <div class="col-md-12 d-flex justify-content-end mt-2">
            <button class="btn btn-success btn-sm me-2 add-block">+</button>
            <button class="btn btn-danger btn-sm remove-block">−</button>
        </div>
    </div>
    `;

        return html;
    },



    saveGauravQuestionsAnswers: function (guid) {

        var resultData = [];
        // ✅ 1. Get answers from textarea-based questions
        $('textarea[id^="q_"]').each(function () {
            const $textarea = $(this);
            const questionId = $textarea.attr('id').replace('q_', '');
            const value = $textarea.val().trim();

            if (value) {
                resultData.push({
                    questionId: questionId,
                    answers: [value]
                });
            }
        });

        // ✅ 2. Get answers from simple subColumn-based tables (no subQuestions)
        $('.nested-table').each(function () {
            const $table = $(this);
            const $thead = $table.find('thead');
            const $tbody = $table.find('tbody');

            // Only proceed if this table has input rows
            if ($tbody.length > 0) {
                $tbody.find('tr').each(function (rowIndex) {
                    const $row = $(this);
                    const rowId = rowIndex + 1;   // 1-based row number; or just use rowIndex
                    let rowAnswers = [];

                    $row.find('input[type="text"]').each(function () {
                        const inputId = $(this).attr('id'); // e.g., "subq_8_1_2559"
                        const parts = inputId.split('_');   // ["subq", "8", "1", "2559"]
                        const mainQuestionId = parts[1];
                        const subQuestionId = parts[2];
                        const value = $(this).val().trim();

                        rowAnswers.push({
                            mainQuestionId: mainQuestionId,
                            subQuestionId: subQuestionId,
                            answer: value,
                            rowId           // <-- include the row’s index/ID here
                        });
                    });

                    if (rowAnswers.length > 0 && rowAnswers.some(ans => ans !== '')) {
                        resultData.push({
                            questionId: gauravQuestion.getClosestQuestionId($table),
                            answers: rowAnswers
                        });
                    }
                });
            }
        });

        console.log("Final Saved Data:", resultData);
    
        //alert(resultData);
        resultData = resultData.map(entry => {
            if (entry.questionId) {
                entry.answers = entry.answers.map(ans => ({
                    mainQuestionId: entry.questionId,
                    subQuestionId: null,
                    answer: ans
                }));
                entry.questionId = null;
            }
            return entry;
        });

        var model = {
            gauravId: guid,//$('#Gaurav').val(),
            model: resultData
        }
        console.log("Final Saved Data:", resultData);
        //common.ShowLoader();

        //gauravQuestion.saveGauravprofile(model);  
        return gauravQuestion.saveGauravprofile(model); // Return the AJAX call
    },

    getPrintPreview: function (guid) {
        
        //alert("in");
        var status = true;
        var District = $('#District option:selected').val();
        if (District == "-1") {
            $('label[for="error"]').show();
            errorDistrict.textContent = "Please Select  District";
            $('#District').addClass('colordiv');
            $('#District').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#District').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorDistrict').html('');
        }
        //if (!status) {
        //    return false;
        //}
        //common.ShowLoader();
        gauravQuestion.getStep2_FinalStatus(guid);


      
        ajax.doGetAjax(`${domain.getdomain()}/Indicator/GetGauravQuestion?gauravId=${guid}&&districtId=${District}`, function (r) {
            console.log(r);
            //common.HideLoader();
            if (r.status) {
                $('#myTable').html('');
                var htmlstring = "";
                if (r.data.length > 0) {
                    var k = 1;
                    if (r.status && r.data.length > 0) {
                        console.log(r);

                        // Example: Replace this with your actual `data` variable from AJAX
                        const response = r; // Replace with actual object from API

                        let questions = response.data;
                        let tbody = $('#myTable');
                        tbody.empty();
                        questions.forEach(q => {
                            // If this question has sub-questions, render nested table
                            if (q.subQuestions && q.subQuestions.length > 0) {
                                // Question header row
                                let questionRow = $('<tr>').append(
                                    $('<td>').text(q.displayNumber + ". " + q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);

                                // Create nested table for subQuestions
                                let nestedTable = $('<table>').addClass('table table-bordered').css({
                                    'border': '1px solid #000'
                                });

                                // Loop through subQuestions and add rows to the nested table
                                q.subQuestions.forEach(sub => {

                                    if (sub.subColumn && sub.subColumn.length > 0) {
                                        // Sub-question header row
                                        let subRow = $('<tr>').append(
                                            $('<td colspan="100%">').text(sub.displayNumber + ". " + sub.questionText)
                                                .css({ 'padding-left': '10px', 'font-weight': 'bold' })
                                        );
                                        nestedTable.append(subRow);

                                        let tableId = 'nestedTable_' + sub.guid;

                                        // Create nested table with empty thead and tbody
                                        let nestedTableHtml = $(`<table id="${tableId}"  data-qid="${sub.questionMasterId}" class="nested-table tbl_class">
                                <thead><tr></tr></thead>
                                <tbody></tbody>
                            </table>`);

                                        nestedTable.append($('<tr>').append($('<td colspan="100%">').append(nestedTableHtml)));

                                        const $headerRow = nestedTableHtml.find('thead tr');
                                        const $body = nestedTableHtml.find('tbody');

                                        // Populate column headers
                                        sub.subColumn.forEach(col => {
                                            $headerRow.append($('<th>').text(col.questionText).css({
                                                'border': '1px solid #ccc',
                                                'padding': '8px',
                                                'background': '#f9f9f9'
                                            }));
                                        });

                                        // Function to create a data row
                                        function createDataRow() {
                                            let $dataRow = $('<tr>');

                                            sub.subColumn.forEach(col => {
                                                let safeId = `subq_${sub.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 10000)}`;
                                                $dataRow.append(
                                                    $('<td>').append(
                                                        $('<span>')
                                                            .attr('id', safeId)
                                                            .css({
                                                                'display': 'block',
                                                                'min-height': '34px',
                                                                'border': '1px solid #ccc',
                                                                'padding': '6px'
                                                            })
                                                            .text('') // Set dynamically later
                                                    )
                                                );
                                            });

                                            return $dataRow;
                                        }

                                        // Add initial row
                                        $body.append(createDataRow());
                                    }

                                    else {
                                        // Sub-question header
                                        let subRow = $('<tr>').append(
                                            $('<td>').text(sub.displayNumber + ". " + sub.questionText)
                                                .css({ 'padding-left': '5px', 'font-weight': 'bold' })
                                        );
                                        nestedTable.append(subRow);

                                        // Sub-question input
                                        let subInputRow = $('<tr>').append(
                                            $('<td>').append(
                                                $('<textarea>')
                                                    .attr({
                                                        rows: 3,
                                                        cols: 150,
                                                        placeholder: "अपना उत्तर यहाँ लिखें...",
                                                        name: `q_${sub.questionMasterId}`,
                                                        id: `q_${sub.questionMasterId}`
                                                    })
                                                    .addClass('form-control')
                                            )
                                        );
                                        nestedTable.append(subInputRow);
                                    }

                                });

                                // Append the nested table row into the main table
                                let nestedTableRow = $('<tr>').append(
                                    $('<td>').append(nestedTable)
                                );
                                tbody.append(nestedTableRow);
                            }
                            else if (q.subQuestions && q.subQuestions.length == 0 && q.subColumn.length > 0) {
                                // Header row for question
                                let questionRow = $('<tr>').append(
                                    $('<td colspan="100%">')
                                        .text(q.displayNumber + ". " + q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);

                                // Nested table
                                let nestedTable = $('<table>')
                                    .addClass('table table-bordered nested-table tbl_class')
                                    .attr('data-qid', q.questionMasterId) // <-- Add this line
                                //.addClass('');
                                //.css({
                                //    'border': '1px solid #000',
                                //    'border-collapse': 'collapse',
                                //    'width': '100%',
                                //    'margin-left': '0px'
                                //});

                                // Table head
                                let thead = $('<thead>');
                                let headerRow = $('<tr>');

                                // SubColumn headers
                                q.subColumn.forEach(col => {
                                    let th = $('<th>').text(col.questionText).css({
                                        'border': '1px solid #000',
                                        'padding': '8px',
                                        'background-color': '#f1f1f1'
                                    });
                                    headerRow.append(th);
                                });
                                thead.append(headerRow);

                                // Table body
                                let subTbody = $('<tbody>');

                                // Row creation function with Add & Remove buttons
                                function createDataRow() {
                                    let dataRow = $('<tr>');

                                    q.subColumn.forEach(col => {
                                        let safeId = `subq_${q.questionMasterId}_${col.subQuestionMasterId}_${Math.floor(Math.random() * 10000)}`;
                                        let td = $('<td>').html(
                                            $('<span>')
                                                .attr('id', safeId)
                                                .css({
                                                    'display': 'block',
                                                    'min-height': '34px',
                                                    /*'border': '1px solid #ccc',*/
                                                    'padding': '6px'
                                                })
                                                .text('') // You can populate this later dynamically
                                        );
                                        dataRow.append(td);
                                    });

                                    return dataRow;
                                }

                                // Initial row
                                subTbody.append(createDataRow());

                                // Append to table
                                nestedTable.append(thead).append(subTbody);
                                let nestedRow = $('<tr>').append($('<td colspan="100%">').append(nestedTable));
                                tbody.append(nestedRow);
                            }
                            else {
                                // Question header row
                                let questionRow = $('<tr>').append(
                                    $('<td>').text(q.displayNumber + ". " + q.questionText)
                                        .addClass('font_text')
                                    //.css({ 'font-weight': 'bold', 'padding-top': '15px' })
                                );
                                tbody.append(questionRow);
                                // Render a span row for normal question (non-editable view)
                                let inputRow = $('<tr>').append(
                                    $('<td>').append(
                                        $('<span>')
                                            .attr({
                                                id: `q_${q.questionMasterId}`
                                            })
                                            .text('') // Optionally populate this dynamically
                                            .css({
                                                'display': 'block',
                                                'min-height': '40px',
                                                /*'border': '1px solid #ccc',*/
                                                'padding': '6px',
                                                /*'background-color': '#f9f9f9'*/
                                            })
                                    )
                                );
                                tbody.append(inputRow);
                            }
                        });

                    } else {
                        tbody.append('<tr><td colspan="4">No data found</td></tr>');
                    }

                }
                else {
                    toast.showToast('error', r.message, 'error');
                }

                setTimeout(gauravQuestion.getfillAnswer(guid), 6000);
            }
            else {
                $('#myTable').html('');
            }
        });
        //common.HideLoader();

        //gauravQuestion.GetGauravVivranPrint();
        //gauravQuestion.getMainGauravProfilePrint();
    },

    getfillAnswer: function (guid) {
        //alert("in");
        //var Gaurav = $('#Gaurav').val();
        var District = $('#District').val();
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetAnswersForEdit?gauravId=${guid}&districtId=${District}`, function (r) {
            console.log(r);
            common.HideLoader();
            if (r.status) {
                const answers = r.data;
                answers.forEach(block => {
                    // Main Question without sub-question
                    if (block.answers.length === 1 && block.answers[0].subQuestionId === "") {
                        const textareaId = `#q_${block.questionId}`;
                        $(textareaId).text(block.answers[0].answer);
                        /*$(textareaId).val(block.answers[0].answer);*/
                    }
                    else {
                        // find the table for this questionId
                        const $table = $(`.nested-table[data-qid="${block.questionId}"]`);
                        const $tbody = $table.find('tbody');
                        // 1) filter down to only question "13"
                        //
                        const qid = block.questionId;
                        const only13 = block.answers.filter(e => e.mainQuestionId === qid);

                        // 2) map to the rowId (default to 1 if somehow missing)
                        const rowIds = only13.map(e => e.rowId || 1);

                        // 3) take the max (or 1 if there are no entries)
                        const maxRowId = rowIds.length > 0
                            ? Math.max(...rowIds)
                            : 1;

                        console.log(maxRowId);  // → 2

                        const subQuestionMap = {};

                        // Group subQuestionIds by rowId
                        block.answers.forEach(item => {
                            const rowId = item.rowId;

                            if (!subQuestionMap[rowId]) {
                                subQuestionMap[rowId] = [];
                            }

                            if (!subQuestionMap[rowId].includes(item.subQuestionId)) {
                                subQuestionMap[rowId].push(item.subQuestionId);
                            }
                        });

                        // Output the result
                        console.log(subQuestionMap);

                        // assume maxRowId already computed, and createEmptyRow(qid) returns a new <tr> for that question
                        if (maxRowId > 1) {
                            for (let i = 1; i < maxRowId; i++) {
                                // append one empty row for this question
                                $tbody.append(createEmptyRow(qid));
                            }
                        }

                        block.answers.forEach(sub => {
                            const rowIndex = (sub.rowId || 1) - 1;

                            // Find the specific row based on rowId
                            const $row = $table.find('tbody tr').eq(rowIndex);

                            // Find the <span> element with ID matching subq_{main}_{sub}_
                            const $span = $row.find(
                                `span[id^="subq_${sub.mainQuestionId}_${sub.subQuestionId}_"]`
                            );

                            // Set the text inside the span
                            $span.text(sub.answer);
                        });

                        // helper: create one new empty <tr> for a given question
                        function createEmptyRow(qid) {
                            const colIds = subQuestionMap[1]; // Adjust this if needed for dynamic mapping
                            const $tr = $('<tr>');

                            colIds.forEach(subQId => {
                                const rnd = Math.floor(Math.random() * 1e4);
                                const id = `subq_${qid}_${subQId}_${rnd}`;

                                $tr.append(
                                    $('<td>').html(
                                        `<span id="${id}"></span>`
                                    ).css({ border: '1px solid #ccc', padding: '6px' })
                                );
                            });

                            // No action buttons here — remove the +/− buttons

                            return $tr;
                        }
                    }
                });
            }
        });
    },

    GetGauravVivranPrint: function () {
        var District = $('#District option:selected').val();
        // Get Gaurav Vivran
       
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetGauravVivran?districtId=${District}`, function (r) {
            console.log(r);
            
            common.HideLoader();
            if (r.status && r.data.data.length > 0) {
                r.data.data.forEach(item => {
                    
                    const name = item.gauravName.toLowerCase();
                    const gauravDistName = item.gauravDistname.toLowerCase();
                    const mangal = item.mangalName || "";
                    const vivran = item.vivran || "";

                    switch (name) {
                        case 'crop':
                            $('#crop_name').text(gauravDistName);
                            $('#crop_vivran').text(vivran);
                            break;
                        case 'botanical species':
                            $('#botanical_name').text(gauravDistName);
                            $('#botanical_vivran').text(vivran);
                            break;
                        case 'product':
                            $('#product_name').text(gauravDistName);
                            $('#product_vivran').text(vivran);
                            break;
                        case 'tourist place':
                            $('#tourist_name').text(gauravDistName);
                            $('#tourist_vivran').text(vivran);
                            break;
                        case 'sports':
                            $('#sports_name').text(gauravDistName);
                            $('#sports_vivran').text(vivran);
                            break;
                        default:
                            console.warn('Unrecognized Gaurav:', item.gauravName);
                    }
                });
            } else {
                alert('No data found.');
            }
        });
    },

    getMainGauravProfilePrint: function () {
        // get Gauravmainprofile
        var District = $('#District option:selected').val();
        common.ShowLoader();
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetMainGauravProfile?districtId=${District}`, function (r) {
            console.log(r);
            
            common.HideLoader();

            if (r.status && r.data) {
                const data = r.data.data[0];  // ✅ Use r.data directly (not r.data.forEach)

                $('#prastwana').text(data.introduction || '');
                $('#distprofile1').text(data.districtProfile_One || '');
                $('#distprofile2').text(data.districtProfile_Two || '');
            } else {
                alert('No data found.');
            }
        });
    },

    finalSave: function (guid) {
        //let gauravId = $('#Gaurav').val();

        //common.ShowLoader();
        Swal.fire({
            title: 'Are you sure?',
            text: "Do you really want to final submit?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                // ✅ Perform your action here
                ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/Profile_step2_Final?gauravId=${guid}`, function (r) {
                    if (r.status) {
                        Swal.fire(
                            'Done!',
                            'Your action has been completed.',
                            'success'
                        );
                        window.location.href = `${domain.getdomain()}/GauravProfile/PrintPreview?guid=${guid}`;
                    } else {
                        alert('Failed to save.');
                    }
                    common.HideLoader();
                });
            }
            else {
                common.HideLoader();
            }
        });
    },

    getStep2_FinalStatus: function (guid) {
        
        //alert("in");
        var status = true;
        //var Gaurav = $('#Gaurav option:selected').val();
        //if (Gaurav == "-1") {
        //    $('label[for="error"]').show();
        //    errorGaurav.textContent = "Please Select  Gaurav";
        //    $('#Gaurav').addClass('colordiv');
        //    $('#Gaurav').removeClass('sucess-highlight').addClass('errr-highlight');
        //    status = false;
        //}
        //else {
        //    $('#Gaurav').removeClass('errr-highlight').addClass('sucess-highlight');
        //    $('#errorGaurav').html('');
        //}
        var District = $('#District option:selected').val();
        if (District == "-1") {
            $('label[for="error"]').show();
            errorDistrict.textContent = "Please Select  District";
            $('#District').addClass('colordiv');
            $('#District').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#District').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorDistrict').html('');
        }
        if (!status) {
            return false;
        }
        // check final submit status
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetStep2_Status?gauravId=${guid}&&districtId=${District}`, function (r) {
            console.log(r);
            //common.HideLoader();
            if (r.status) {
                //$('.row.justify-content-end').remove(); // Removes the whole button section
                $('.row.justify-content-end').hide();
            }
            else {
                $('.row.justify-content-end').show();
            }
        });
    },

    CheckProfile: function (gauravId, guid) {
        //alert(gauravId);
        let District;
        ajax.doGetAjax(`${domain.getdomain()}/GauravProfile/GetStep2_Status?gauravId=${guid}&&districtId=${District}`, function (r) {
            console.log(r);
            common.HideLoader();
            if (r.status) {
                setTimeout(window.location.href = `${domain.getdomain()}/GauravProfile/PrintPreview?guid=${guid}`, 300);
                //$('.row.justify-content-end').remove(); // Removes the whole button section
            }
            else {
                common.ShowLoader();
                $('.row.justify-content-end').show();
                setTimeout(window.location.href = `${domain.getdomain()}/GauravProfile/Profiles?guid=${guid}`, 300);
            }
        });
    },

    printprofile: function (guid,districtId) {

    }

}