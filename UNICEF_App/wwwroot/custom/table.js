
var table = {

    dataTable: function (tblId) {
        if ($.fn.dataTable.isDataTable('#' + tblId)) {
            $('#' + tblId).DataTable().destroy();
        }
        $('#' + tblId).DataTable(
            {
                "searching": true,
                "retrieve": true,
                "paging": true,
                "pageLength": 10,
                "destroy": true,
                "ordering": false,
                "dom": 'Bfrtip',
                buttons: ['pageLength', 'colvis',
                    {

                        extend: 'collection',
                        text: 'Export',
                        buttons: [
                            {
                                extend: 'copyHtml5',
                                exportOptions: {
                                    columns: [0, ':visible']
                                },
                                text: '<i class="fa fa-files-o text-info"></i>',
                                titleAttr: 'Copy'
                            },
                            {
                                extend: 'excelHtml5',
                                exportOptions: {
                                    columns: ':visible'
                                },
                                text: '<i class="fa fa-file-excel-o text-success"></i>',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'pdfHtml5',
                                exportOptions: {
                                    columns: [0, 1, 2, 5]
                                },
                                text: '<i class="fa fa-file-pdf-o text-danger"></i>',
                                titleAttr: 'PDF'
                            }

                        ]
                    }

                ]
            });

    },

    BindTable: function (url, tblId) {
        var column = [];
        var sringArray = [];
        $('#' + tblId + ' thead tr th').each(function () {
            if ($(this).text().toLowerCase().includes("image") || $(this).text().toLowerCase().includes("edit") || $(this).text().toLowerCase().includes("delete") || $(this).text().toLowerCase().includes("isactive") || $(this).text().toLowerCase().includes("action")) {
                var clmn = { "data": $(this).text().charAt(0).toLowerCase() + $(this).text().slice(1), sortable: false, };
                column.push(clmn);
            }
            else {
                var clmn = { "data": $(this).text().charAt(0).toLowerCase() + $(this).text().slice(1), sortable: true, };
                column.push(clmn);
            }

        });
        column.forEach(item => {
            if (item.data.toLowerCase() != "srno") item.sortable = false;
        });
        if ($.fn.dataTable.isDataTable('#' + tblId)) {
            $('#' + tblId).DataTable().destroy();
        }

        $('#' + tblId).DataTable(
            {
                "searching": true,
                "retrieve": true,
                "serverSide": true,
                "processing": true,
                "paging": true,
                "pageLength": 10,
                "destroy": true,
                "ajax":
                {
                    "url": url,
                    "contentType": "application/json",
                    "type": "GET",
                    "dataType": "JSON",
                    "data": function (d) {
                        return d;
                    },
                    "dataSrc": function (json) {
                        json.draw = json.draw;
                        json.recordsTotal = json.recordsTotal;
                        json.recordsFiltered = json.recordsFiltered;
                        json.data = json.data;
                        var return_data = json;
                        return return_data.data;
                    }
                },
                "columns": column,

                "dom": 'Bfrtip',
                //buttons: ['pageLength', 'colvis',
                //    {
                //        extend: 'copyHtml5',
                //        exportOptions: {
                //            columns: [0, ':visible']
                //        },
                //        text: '<i class="fa fa-files-o text-success"></i>',
                //        titleAttr: 'Copy'
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        exportOptions: {
                //            columns: ':visible'
                //        },
                //        text: '<i class="fa fa-file-excel-o text-success"></i>',
                //        titleAttr: 'Excel'
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        exportOptions: {
                //            columns: [0, 1, 2, 5]
                //        },
                //        text: '<i class="fa fa-file-pdf-o text-danger"></i>',
                //        titleAttr: 'PDF'
                //    }

                //]
                //buttons: ['pageLength', 'colvis',
                //    {

                //        extend: 'collection',
                //        text: 'Export',
                //        buttons: [
                //            {
                //                extend: 'copyHtml5',
                //                exportOptions: {
                //                    columns: [0, ':visible']
                //                },
                //                text: '<i class="fa fa-files-o text-info"></i>',
                //                titleAttr: 'Copy'
                //            },
                //            {
                //                extend: 'excelHtml5',
                //                exportOptions: {
                //                    columns: ':visible'
                //                },
                //                text: '<i class="fa fa-file-excel-o text-success"></i>',
                //                titleAttr: 'Excel'
                //            },
                //            {
                //                extend: 'pdfHtml5',
                //                exportOptions: {
                //                    columns: [0, 1, 2, 5]
                //                },
                //                text: '<i class="fa fa-file-pdf-o text-danger"></i>',
                //                titleAttr: 'PDF'
                //            }

                //        ]
                //    }

                //]
            });

    },
    BindPostTable: function (url, tblId, data) {
      
        var column = [];
        var sringArray = [];
        $('#' + tblId + ' thead tr th').each(function () {
            if ($(this).text().toLowerCase().includes("image") || $(this).text().toLowerCase().includes("edit") || $(this).text().toLowerCase().includes("delete") || $(this).text().toLowerCase().includes("isactive") || $(this).text().toLowerCase().includes("action")) {
                var clmn = { "data": $(this).text().charAt(0).toLowerCase() + $(this).text().slice(1), sortable: false, name: $(this).text() };
                column.push(clmn);
            }
            else {
                var clmn = { "data": $(this).text().charAt(0).toLowerCase() + $(this).text().slice(1), sortable: true, name: $(this).text() };
                column.push(clmn);
            }

        });
        column.forEach(item => {
            if (item.data.toLowerCase() != "srno") item.sortable = false;
        });

        if ($.fn.dataTable.isDataTable('#' + tblId)) {
            $('#' + tblId).DataTable().destroy();
        }

        //$('#' + tblId).DataTable({
        //    "searching": true,
        //    "retrieve": true,
        //    "serverSide": true,
        //    "processing": true,
        //    "paging": true,
        //    "pageLength": 10,
        //    "destroy": true, // Do not destroy the table
        //    "filter": true,
        //    "orderMulti": false,
        //    "lengthMenu": [[10, 50, 100, 500, 1000], [10, 50, 100, 500, 1000]],
        //    "ajax": {
        //        "url": url,
        //        "type": "POST",
        //        "datatype": "json",
        //        "data": function (d) {
        //            if (data != undefined) {
        //                return $.extend({}, d, data);
        //            } else { return d; }
        //        },
        //        "dataSrc": function (json) {
        //            json.draw = json.draw;
        //            json.recordsTotal = json.recordsTotal;
        //            json.recordsFiltered = json.recordsFiltered;
        //            json.data = json.data;
        //            return json.data; // Return data directly
        //        }
        //    },
        //    "columns": column,
        //    "dom": 'lBfrtip',
        //});

        $('#' + tblId).DataTable(
            {
                "searching": true,
                "retrieve": true,
                "serverSide": true,
                "processing": true,
                "paging": true,
                "pageLength": 10,
                "destroy": true,
                "filter": true, // this is for disable filter (search box)    
                "orderMulti": false,
                "lengthMenu": [[10, 50, 100, 500, 1000], [10, 50, 100, 500, 1000]],
                /*   "order": [[0, "desc"]],*/
                "ajax":
                {
                    "url": url,
                    "type": "POST",
                    "datatype": "json",
                    "headers": {
                        "RequestVerificationToken": $('meta[name="csrf-token"]').attr('content')
                    },
                    "data": function (d) {
                        if (data != undefined) {
                            return $.extend({}, d, data);
                        } else { return d; }

                    },
                    "dataSrc": function (json) {
                        console.log(json.data);
                        json.draw = json.draw;
                        json.recordsTotal = json.recordsTotal;
                        json.recordsFiltered = json.recordsFiltered;
                        json.data = json.data;
                        var return_data = json;
                        return return_data.data;
                    }
                },
                "columns": column,

                /*"dom": 'lBfrtip',*/
                "dom": '<"top d-flex justify-content-between"lBf>rtip',
                /* "dom": 'lfrtip',*/
                buttons: [
                    //{
                    //    extend: 'excelHtml5',
                    //    exportOptions: {
                    //        columns: ':visible'
                    //    },
                    //    text: '<i style="color:green;" class="mdi mdi-file-excel"></i>Excel',
                    //    titleAttr: 'Excel'
                    //},
                    //{
                    //    extend: 'pdfHtml5',
                    //    exportOptions: {
                    //        columns: ':visible'
                    //    },
                    //    text: '<i style="color:red;" class="mdi mdi-file-pdf"></i>PDF',
                    //    //className:"btn btn-outline-primary btn-sm",
                    //    titleAttr: 'PDF'
                    //}
                ]
            });
        //table.ajax.reload(); // Reload the table with new data
    }


}