//var ajax = {
//    doPostAjax: function (url, data, callback) {
//        $.ajax({
//            type: 'POST',
//            url: url,
//            data: data,
//            success: function (result) {
//                return callback(result);
//            }
//        });
//    },
//    doGetAjax: function (url, callback) {
//        $.ajax({
//            type: 'Get',
//            url: url,
//            success: function (result) {
//                return callback(result);
//            }
//        });
//    },
//    AjaxPost: function (url, data, callback) {
//        $.ajax({
//            type: 'POST',
//            url: url,
//            data: data,
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            async: false,
//            success: function (result) {
//                return callback(result);
//            }
//        });
//    },

//}
//var common = {
//    isNumberKey: function (evt) {
//        var charCode = (evt.which) ? evt.which : evt.keyCode
//        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
//            return false;
//        }
//        return true;
//    },
//    SelectAllCheckBox: function (id) {
//        var checked = $(`#` + id).prop(`checked`);
//        $('input[type=checkbox]').each(function () {
//            if (checked) {
//                $(this).prop(`checked`, true);
//            }
//            else {
//                $(this).prop(`checked`, false);
//            }
//        });
//    },
//    OpenPopup: function () {
//        //$(`#exampleModal`).addClass('show');
//        $("#exampleModal").modal();

//    },
//    CloseModal: function () {
//        $('#closepopup').trigger("click");
//    },

//    LoadModelValidation: function () {
//        $.validator.unobtrusive.parse("form");
//    },
//    ChangeUrl: function (controller, action) {

//        var new_url = "/" + controller + "/" + action;
//        window.history.pushState("data", "Title", new_url);
//        document.title = action;

//    },
//    BindDropdown: function (url, id, select, seletedid,callback) {
//        ajax.doGetAjax(url, function (result) {
//            //debugger;
//            var selectOption = "";

//            if (select != undefined) {
//                selectOption = select;
//            }
//            $('#' + id).html("");
//            if (selectOption !="")
//            $('#' + id).append('<option value="-1">--Select ' + selectOption + '--</option>');
//            if (result.status) {

//                $(result.data).each(function (index, val) {
//                    //var opt = new Option(val.name, val.id);

//                    if (seletedid == val.id) {
//                        $('#' + id).append('<option value=' + val.id + ' selected>' + val.name + '</option>');
//                    }
//                    else {
//                        $('#' + id).append('<option value=' + val.id + '>' + val.name + '</option>');
//                    }
                    
//                });

//                if (seletedid == "All")
//                    $('#' + id).append('<option value="99">All</option>');

//                // Execute callback if provided
//                if (typeof callback === 'function') {
//                    callback();
//                }
//            }
//        });
//    },

//    BindDropdownbyVlaue: function (url, id, select, seletedid) {
//        ajax.doGetAjax(url, function (result) {
//            //debugger;
//            var selectOption = "";

//            if (select != undefined) {
//                selectOption = select;
//            }
//            $('#' + id).html("");
//            $('#' + id).append('<option value="-1">--Select ' + selectOption + '--</option>');
//            if (result.status) {

//                $(result.data).each(function (index, val) {
//                    //var opt = new Option(val.name, val.id);

//                    if (seletedid.toLowerCase() == val.name.toLowerCase()) {
//                        $('#' + id).append('<option value=' + val.id + ' selected>' + val.name + '</option>');
//                    }
//                    else {
//                        $('#' + id).append('<option value=' + val.id + '>' + val.name + '</option>');
//                    }

//                });
//                //$('#' + id + 'option[value=' + seletedid + ']').attr('selected', 'selected');
//            }
//        });
//    },

//    BindDropdownmultiselect: function (url, id, select, seletedid) {
//        ajax.doGetAjax(url, function (result) {
//            //debugger;
//            var selectOption = "";

//            if (select != undefined) {
//                selectOption = select;
//            }
//            $('#' + id).html("");
//            $('#' + id).append('<option value="-1">--Select ' + selectOption + '--</option>');
//            if (result.status) {

//                $(result.data).each(function (index, val) {
//                    //var opt = new Option(val.name, val.id);

//                    if (seletedid == val.id) {
//                        $('#' + id).append('<option value=' + val.id + ' selected>' + val.name + '</option>');
//                    }
//                    else {
//                        $('#' + id).append('<option value=' + val.id + '>' + val.name + '</option>');
//                    }

//                });

//                //var str = "2,3,4";
//                var arr = seletedid.split(',');
//                if (seletedid != "")
//                    $('#' + id).val(arr).trigger('change');
//                //$('#' + id + 'option[value=' + seletedid + ']').attr('selected', 'selected');
//            }
//        });
//    },
//    BindRole: function () {
//        ajax.doGetAjax(`/Account/GetRole`, function (result) {

//            if (result.status) {
//                $('#UserRole').html("");
//                $('#UserRole').append('<option value="0" selected>Select Role</option>');
//                $(result.results).each(function (index, val) {
//                    var opt = new Option(val.roleName, val.id);
//                    $('#UserRole').append(opt);
//                })
//            }
//        });
//    },

//    ShowLoader: function () {
//        //$('#cover-spin').show(0);
//        /*$('#dvLoader').removeClass("d-none");*/
//        //$('#btnloader').click();
//        //$('#dvLoader').addClass("show");
//        $('#dvLoader').show();
//    },
//    HideLoader: function () {
//        //$('#cover-spin').hide(0);
//        /*$('#dvLoader').addClass("d-none");*/
//        //$('#btnclose').click();
//        /* $('#dvLoader').removeClass("show");*/
//        $('#dvLoader').hide();
//    },
//    AddEditLayout: function (item, type, area) {
//        //debugger;
//        if (type == `Side`) {
//            var data = { "SidebarClass": $(item).attr(`data-class`), "Type": type }
//        }
//        if (type == `Top`) {
//            var data = { "TopbarClass": $(item).attr(`data-class`), "Type": type }
//        }
//        ajax.doPostAjax(`/` + area + `/Home/AddEditLayout`, data, function (result) {
//            if (result.status) {

//            }
//        });
//    },
//    GetLayout: function (area) {
//        ajax.doGetAjax(`/` + area + `/Home/GetLayout`, function (result) {
//            if (result.status) {
//                $(`.app-header`).addClass(result.results.topbarClass);
//            }
//        });
//    },
//    Filter: function (area, controller, action, data, tblId, IsButtonClick = 0, isMember = 0) {

//        var start;
//        var end;
//        var days = isMember == 0 ? 29 : 365;
//        start = moment().subtract(days, 'days');
//        end = moment();
//        function cb(start, end) {

//            if (IsButtonClick == 0) {
//                $('#reportrange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));

//                data["fromDate"] = start.format('D-MMM-YYYY');
//                data["toDate"] = end.format('D-MMM-YYYY');

//            }

//            table.BindPostTable(`/${area}/${controller}/${action}`, tblId, data);

//        }
//        $('#reportrange').daterangepicker({
//            startDate: start,
//            endDate: end,
//            ranges: {
//                'Today': [moment(), moment()],
//                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
//                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
//                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
//                'This Month': [moment().startOf('month'), moment().endOf('month')],
//                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
//            },

//        }, cb);
//        cb(start, end);
//    },
//    printDiv: function (divId) {
//        var printContents = document.getElementById(divId).innerHTML;
//        var originalContents = document.body.innerHTML;

//        document.body.innerHTML = printContents;

//        window.print();

//        document.body.innerHTML = originalContents;
//    },

//    gethost: function () {
//        host = window.location.host;
//        //host = window.location.href;
//        //host = "/Master";
//        return host;
//        //="";
//        //hostname = window.location.hostname;
//    },
//    distroy: function () {
//        ajax.doGetAjax(`${domain.getdomain()}/Account/DestroySession`, function (r) {
//            console.log(); 
//                //$("#frmNameBackTOSSO").submit();
//                $('#btnsubmit').click();
//                return true;           
//        });
//    },
//    distroyLogOut: function () {
//        ajax.doGetAjax(`${domain.getdomain()}/Account/DestroySession`, function (r) {
//            console.log();
//            //$("#frmNameBackTOSSO").submit();
//            $('#btnLogOut').click();
//            return true;
//        });
//    }
//}
var ajax = {
    doPostAjax: function (url, data, callback) {
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            "headers": {
                "RequestVerificationToken": $('meta[name="csrf-token"]').attr('content')
            },
            success: function (result) {
                return callback(result);
            },
            error: function (xhr) {
                let message = "Something went wrong";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    message = xhr.responseJSON.message;
                }
                callback({ status: false, message: message }); // fallback for BadRequest
            }
        });
    },
    doPostAjaxHtml: function (url, data, callback) {
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            dataType: "html",   // ⭐ FIX
            headers: {
                "RequestVerificationToken": $('meta[name="csrf-token"]').attr('content')
            },
            success: function (result) {
                callback(result);
            },
            error: function (xhr) {
                let message = "Something went wrong";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    message = xhr.responseJSON.message;
                }
                callback({ status: false, message: message });
            }
        });
    },
    doPostheaderAjax: function (url, data, header, callback) {
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            headers: {
                'RequestVerificationToken': header
            },
            success: function (result) {
                return callback(result);
            }
        });
    },
    doGetAjax: function (url, callback) {
        $.ajax({
            type: 'Get',
            url: url,
            success: function (result) {
                return callback(result);
            }
        });
    },
    doGetAjaxVetting: function (url,  data, callback) {
        $.ajax({
            type: 'GET',
            url: url,
            data: data,
            success: function (result) {

                if (typeof callback === "function") {
                    callback(result);
                }
            }
        });
    },
    AjaxPost: function (url, data, callback) {
        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {
                return callback(result);
            }
        });
    },

}
var common = {
    isNumberKey: function (evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    },
    SelectAllCheckBox: function (id) {
        var checked = $(`#` + id).prop(`checked`);
        $('input[type=checkbox]').each(function () {
            if (checked) {
                $(this).prop(`checked`, true);
            }
            else {
                $(this).prop(`checked`, false);
            }
        });
    },
    OpenPopup: function () {
        //$(`#exampleModal`).addClass('show');
        $("#exampleModal").modal();

    },
    CloseModal: function () {
        $('#closepopup').trigger("click");
    },

    LoadModelValidation: function () {
        $.validator.unobtrusive.parse("form");
    },
    ChangeUrl: function (controller, action) {

        var new_url = "/" + controller + "/" + action;
        window.history.pushState("data", "Title", new_url);
        document.title = action;

    },
    BindDropdown: function (url, id, select, seletedid) {
        ajax.doGetAjax(url, function (result) {
            
            var selectOption = "";

            if (select != undefined) {
                selectOption = select;
            }
            $('#' + id).html("");
            $('#' + id).append('<option value="">Select ' + selectOption + '</option>');
            if (result.status) {

                $(result.data).each(function (index, val) {
                    //var opt = new Option(val.name, val.id);

                    if (seletedid == val.id) {
                        $('#' + id).append('<option value=' + val.id + ' selected>' + val.name + '</option>');
                    }
                    else {
                        $('#' + id).append('<option value=' + val.id + '>' + val.name + '</option>');
                    }

                });
                //$('#' + id + 'option[value=' + seletedid + ']').attr('selected', 'selected');
            }
        });
    },
    GetPendingDistrict: function (url, data, id, select, seletedid) {

        ajax.doGetAjaxVetting(url, data, function (result) {

            var selectOption = "";

            if (select != undefined) {
                selectOption = select;
            }

            $('#' + id).html("");
            $('#' + id).append('<option value="">--Select ' + selectOption + '--</option>');

            if (result.status) {

                $(result.data).each(function (index, val) {

                    if (seletedid == val.id) {
                        $('#' + id).append('<option value="' + val.id + '" selected>' + val.name + '</option>');
                    }
                    else {
                        $('#' + id).append('<option value="' + val.id + '">' + val.name + '</option>');
                    }

                });

            }

        });

    },

    BindRole: function () {
        ajax.doGetAjax(`/Account/GetRole`, function (result) {

            if (result.status) {
                $('#UserRole').html("");
                $('#UserRole').append('<option value="0" selected>Select Role</option>');
                $(result.results).each(function (index, val) {
                    var opt = new Option(val.roleName, val.id);
                    $('#UserRole').append(opt);
                })
            }
        });
    },

    ShowLoader: function () {
        //$('#cover-spin').show(0);
        $('#dvLoader').show();
    },
    HideLoader: function () {
        //$('#cover-spin').hide(0);
        $('#dvLoader').hide();
    },
    AddEditLayout: function (item, type, area) {
        debugger;
        if (type == `Side`) {
            var data = { "SidebarClass": $(item).attr(`data-class`), "Type": type }
        }
        if (type == `Top`) {
            var data = { "TopbarClass": $(item).attr(`data-class`), "Type": type }
        }
        ajax.doPostAjax(`/` + area + `/Home/AddEditLayout`, data, function (result) {
            if (result.status) {

            }
        });
    },
    GetLayout: function (area) {
        ajax.doGetAjax(`/` + area + `/Home/GetLayout`, function (result) {
            if (result.status) {
                $(`.app-header`).addClass(result.results.topbarClass);
            }
        });
    },
    Filter: function (area, controller, action, data, tblId, IsButtonClick = 0, isMember = 0) {

        var start;
        var end;
        var days = isMember == 0 ? 29 : 365;
        start = moment().subtract(days, 'days');
        end = moment();
        function cb(start, end) {

            if (IsButtonClick == 0) {
                $('#reportrange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));

                data["fromDate"] = start.format('D-MMM-YYYY');
                data["toDate"] = end.format('D-MMM-YYYY');

            }

            table.BindPostTable(`/${area}/${controller}/${action}`, tblId, data);

        }
        $('#reportrange').daterangepicker({
            startDate: start,
            endDate: end,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },

        }, cb);
        cb(start, end);
    },
    printDiv: function (divId) {
        var printContents = document.getElementById(divId).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    },

    distroy: function () {
        ajax.doGetAjax(`/Home/DistroySession`, function (r) {
            console.log(r.data);
            if (r.status) {
                // destroy success → now submit form
                document.getElementById("backToSsoForm").submit();
            }
            else {
                return false;
            }
        });
    }
}
var SweetAlert = {
    SwalSuccessAlert: function (title, text, showCancelButton, confirmButtonText, allowOutsideClick) {
        Swal.fire({
            title: title,
            text: text,
            icon: 'success',
            showCancelButton: showCancelButton,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: confirmButtonText,
            allowOutsideClick: allowOutsideClick
        })
    },
    SwalAlert: function (title, text, icon, showCancelButton, confirmButtonColor, cancelButtonColor, confirmButtonText, allowOutsideClick) {
        Swal.fire({
            title: title,
            text: text,
            icon: icon,
            showCancelButton: showCancelButton,
            confirmButtonColor: confirmButtonColor,
            cancelButtonColor: cancelButtonColor,
            confirmButtonText: confirmButtonText,
            allowOutsideClick: allowOutsideClick
            //title: 'Are you sure?',
            //text: "You won't be able to revert this!",
            //icon: 'warning',
            //showCancelButton: true,
            //confirmButtonColor: '#3085d6',
            //cancelButtonColor: '#d33',
            //confirmButtonText: 'Yes, delete it!',
            //allowOutsideClick: false
        })
    },

}

var print = {
    printdv: function (id) {
        // Retrieve the inner HTML of the specified element
        var divContents = document.getElementById(id).innerHTML;


        //// Create a new row element with page-break class
        //var pageBreakRow = `<tr style="border-collapse: collapse;" border="1">
        //                            <td colspan="1"> <b  style="padding:0px;">Indicator No</b> </td>
        //                            <td style="" colspan="1"> <b>Detail </b> </td>
        //                            <td style="" colspan="5"> <b>Year </b> </td>
        //                        </tr>`;

        //// Append the page break row to the div contents (e.g., at the end of the table)
        //divContents += pageBreakRow;

        var a = window.open('', '', 'height=800, width=800');
        a.document.write('<html><head>');
        //Print the Table CSS.
        var table_style = document.getElementById("table_style").innerHTML;
        a.document.write('<style type = "text/css">');
        a.document.write(table_style);
        a.document.write('</style>');
        a.document.write('</head>');
        a.document.write('<body>');
        a.document.write(divContents);
        a.document.write('</body></html>');
        a.document.close();
        a.print();
    },

    //printdv: function (id) {

    //    var divContents = document.getElementById(id).innerHTML;

    //    var a = window.open('', '', 'height=800,width=900');

    //    a.document.write('<html><head><title>Print</title>');

    //    /* Bootstrap */
    //    a.document.write(`
    //    <link rel="stylesheet"
    //          href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css">
    //`);

    //    /* ✅ External print CSS */
    //    a.document.write(`
    //          <link href="~/css/custompdf.css" rel="stylesheet" asp-append-version="true" />
    //`);

    //    a.document.write('</head><body>');
    //    a.document.write(divContents);
    //    a.document.write('</body></html>');

    //    a.document.close();

    //    a.onload = function () {
    //        a.focus();
    //        a.print();
    //    };
    //}

}

var domain = {
    getdomain: function () {
        //var hostorigion = window.location.origin;
        //alert(hostorigion);  
        return "";
        //return "/SDGV2";
    }
}

document.addEventListener("DOMContentLoaded", function () {

    // Only Alphabets + Space, no space at start or end while typing
    document.querySelectorAll(".alphaspace").forEach(function (el) {
        el.addEventListener("keypress", function (e) {
            var regex = /^[a-zA-Z ]$/;
            var char = String.fromCharCode(e.which || e.keyCode);

            // If character is not a letter or space, prevent it
            if (!regex.test(char)) {
                e.preventDefault();
                return;
            }

            if (char === " ") {
                var value = el.value;
                var cursorPos = el.selectionStart;

                // Prevent space at the beginning
                if (cursorPos === 0) {
                    e.preventDefault();
                    return;
                }

                // Prevent space at the end if last char is space
                if (cursorPos === value.length && value.endsWith(" ")) {
                    e.preventDefault();
                    return;
                }
            }
        });

        // Optional: Trim spaces on blur (when user leaves the field)
        el.addEventListener("blur", function () {
            el.value = el.value.trim();
        });
    });


    // Alphanumeric (0-9 + a-zA-Z + space)
    document.querySelectorAll(".alphanumeric").forEach(function (el) {
        el.addEventListener("keypress", function (e) {
            var regex = /^[0-9a-zA-Z ]+$/;
            var str = String.fromCharCode(e.which || e.keyCode);
            if (!regex.test(str)) {
                e.preventDefault();
            }

            if (str === " ") {
                var value = el.value;
                var cursorPos = el.selectionStart;

                // Prevent space at the beginning
                if (cursorPos === 0) {
                    e.preventDefault();
                    return;
                }

                // Prevent space at the end if last char is space
                if (cursorPos === value.length && value.endsWith(" ")) {
                    e.preventDefault();
                    return;
                }
            }
        });

        // Optional: Trim spaces on blur (when user leaves the field)
        el.addEventListener("blur", function () {
            el.value = el.value.trim();
        });
    });

    // Number only (0-9)
    document.querySelectorAll(".numberonly").forEach(function (el) {
        el.addEventListener("keypress", function (e) {
            var charCode = e.which || e.keyCode;
            var str = String.fromCharCode(charCode);

            // Allow only digits 0-9, block everything else including spaces
            if (!/[0-9]/.test(str)) {
                e.preventDefault();
            }
        });

        // Optional: Trim spaces if any pasted or left in the field
        el.addEventListener("blur", function () {
            el.value = el.value.replace(/\s+/g, '');
        });

        // Also prevent pasting spaces or non-digits
        el.addEventListener("paste", function (e) {
            var pasteData = (e.clipboardData || window.clipboardData).getData('text');
            if (!/^\d+$/.test(pasteData)) {
                e.preventDefault();
            }
        });
    });


});
