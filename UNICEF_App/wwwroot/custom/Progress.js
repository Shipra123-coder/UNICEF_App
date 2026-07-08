var progress = {
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


        ajax.doGetAjax(`${domain.getdomain()}/Indicator/GetProgressReport?Id=${Gaurav}&&districtId=${District}`, function (r) {
            console.log(r);
            common.HideLoader();
            if (r.status) {
                var readonlyAttr = '';
                var readonlyclass = '';
                //var readonlyAttr = (r.source.toLowerCase() !== 'nodal officer') ? 'readonly' : '';
                //var readonlyclass = (r.source.toLowerCase() !== 'nodal officer') ? 'readonly_text' : '';
                $('#myTable').html('');
                var htmlstring = "";
                if (r.data.length > 0) {
                    var k = 1;
                    if (r.status && r.data.length > 0) {
                        $.each(r.data, function (i, item) {
                            // Main indicator row
                            let inputClass = progress.getInputClass(item.unit);
                            if (item.indicatortype != 'group_main') {
                                htmlstring += `<tr><th hidden>${item.indicatorId}</th><th hidden>${item.districtId}</th><th hidden>${item.gauravId}</th><th hidden>${item.gauravIdistId}</th>
                                            <th>${k}</th>
                                            <th>${item.indicatorname}</th>
                                            <th>${item.unit}</th>'/'
                                            <th><input type="text" class="form-control form-control-sm mb-3 ${inputClass} ${readonlyclass}" value="${item.basevalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 ${inputClass} ${readonlyclass}" value="${item.shortTermTargetvalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 ${inputClass} ${readonlyclass}" value="${item.midTermTargetvalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 ${inputClass} ${readonlyclass}" value="${item.longTermTargetvalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 alphanumeric ${readonlyclass}" value="${item.sourceOfData}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                         </tr>`;
                            }
                            else {
                                htmlstring += `<tr style="background-color: #aebee8;"><th hidden>${item.indicatorId}</th><th hidden>${item.districtId}</th><th hidden>${item.gauravId}</th><th hidden>${item.gauravIdistId}</th>                                            
                                <th>${k}</th>
                                <th colspan="7">${item.indicatorname}</th>                                            
                         </tr>`;
                            }
                            //tbody.append(mainRow);

                            // Sub-indicators (if any)
                            if (item.subIndicators && item.subIndicators.length > 0) {
                                $.each(item.subIndicators, function (j, sub) {
                                    htmlstring += `<tr><th hidden>${sub.indicatorId}</th><th hidden>${sub.districtId}</th><th hidden>${sub.gauravId}</th><th hidden>${sub.gauravIdistId}</th>
                                            <th hidden>${k} </th>
                                            <th colspan='2'>${sub.indicatorname}</th>
                                            <th>${sub.unit}</th>'/'
                                            <th><input type="text" class="form-control form-control-sm mb-3 alphanumeric ${readonlyclass}" value="${sub.basevalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 alphanumeric ${readonlyclass}" value="${sub.shortTermTargetvalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 alphanumeric ${readonlyclass}" value="${sub.midTermTargetvalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 alphanumeric ${readonlyclass}" value="${sub.longTermTargetvalue}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                            <th><input type="text" class="form-control form-control-sm mb-3 alphanumeric ${readonlyclass}" value="${sub.sourceOfData}" placeholder="" style="border:1px solid #000" ${readonlyAttr}></th>
                                    </tr>`;
                                    //htmlstring += '<tr>' +
                                    //    '<td style="padding-left: 30px;">↳ ' + sub.indicatorname + '</td>' +
                                    //    '<td>' + sub.unit + '</td>' +
                                    //    '<td>' + sub.basevalue + '</td>' +
                                    //    '<td>' + sub.targetvalue + '</td>' +
                                    //    '</tr>';
                                    //tbody.append(subRow);
                                    k++;
                                });
                            }
                            k++;
                        });

                        $('#myTable').html(htmlstring);
                        $("#btnSave").show();
                        //$('#divresultnew').find('.form-control.test').addClass('readonly_text');  // Adds your class
                    } else {
                        tbody.append('<tr><td colspan="8">No data found</td></tr>');
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
            else {
                $('#myTable').html('');
                htmlstring = `<tr class="text_align_center"><td colspan="5">No data found</td></tr>`;
                $('#myTable').html(htmlstring);
            }
        });
    },

    getInputClass: function (unit) {
        if (!unit) return 'alphanumeric';

        unit = unit.toLowerCase();

        // 🔤 Status & Yes/No
        if (unit.includes('status')) return 'only-text';
        if (unit.includes('yes/no')) return 'yes-no';

        // 🔢 Integer types
        if (
            unit.includes('number') ||
            unit.includes('no.') ||
            unit.includes('buyers') ||
            unit.includes('farmers') ||
            unit.includes('engagement')
        ) {
            return 'only-number';
        }

        // 🔢 Decimal types
        if (
            unit.includes('hectare') ||
            unit.includes('lakh') ||
            unit.includes('metric') ||
            unit.includes('ton') ||
            unit.includes('percentage')
        ) {
            return 'decimal-number';
        }

        return 'alphanumeric';
    },

    savedata: function () {
        debugger;
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
                    ShortTermTargetValue: $(ths[8]).find("input").val().trim(),
                    MidTermTargetValue: $(ths[9]).find("input").val().trim(),
                    LongTermTargetValue: $(ths[10]).find("input").val().trim(),
                    SourceOfData: $(ths[11]).find("input").val().trim(),
                    //Remark: $(ths[9]).find("input").val().trim(),
                };
                tableData.push(rowData);
            }
        });
        debugger;
        console.log(tableData);
        common.ShowLoader();
        ajax.AjaxPost(`${domain.getdomain()}/Indicator/SaveIndicatorTargetMaster`, JSON.stringify(tableData), function (r) {
            common.HideLoader();
            if (r.status) {
                toast.showToast('success', r.message, 'success');
                setTimeout(function () { progress.getdata(); }, 2000); 
                // Additional success logic...
            } else {
                toast.showToast('error', r.message, 'error');
            }
        });
    }
}

// 🔢 Integer only
$(document).on('input', '.only-number', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

// 🔢 Decimal allowed
$(document).on('input', '.decimal-number', function () {
    this.value = this.value.replace(/[^0-9.]/g, '');

    // Only one decimal point
    if ((this.value.match(/\./g) || []).length > 1) {
        this.value = this.value.slice(0, -1);
    }
});

// 🔤 Text only (Status)
$(document).on('input', '.only-text', function () {
    this.value = this.value.replace(/[^a-zA-Z ]/g, '');
});

// 🔤 Yes/No only
$(document).on('input', '.yes-no', function () {
    this.value = this.value.replace(/[^a-zA-Z]/g, '');

    let val = this.value.toLowerCase();
    if (!"yes".startsWith(val) && !"no".startsWith(val)) {
        this.value = '';
    }
});