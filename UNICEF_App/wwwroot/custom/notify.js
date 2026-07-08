//function OnBegin() {
//    common.ShowLoader();    
//}
function OnBegin() {
    common.ShowLoader();
    // Get password and nonce
    var passwordField = $("input[name='Password']");
    var rawPassword = passwordField.val();
    var nonce = $("#nonce").val();

    // Hash password + nonce using SHA-256
    var salted = rawPassword + nonce;
    //var hashedPassword = await sha256(salted).toString();
    var hashedPassword = CryptoJS.SHA256(salted).toString();

    // Replace password field value with hashed version
    //$('#Password').val(hashedPassword);
    passwordField.val(hashedPassword);

    return true; // allow the form to continue submission
}
async function sha256(text) {
    const encoder = new TextEncoder();
    const data = encoder.encode(text);
    const hashBuffer = await crypto.subtle.digest("SHA-256", data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
    return hashHex;
}
function OnFailure(response) {
    if (response.status) {
        $.when(toast.showToast('success', response.message, 'success')).then(function () {

            if (response.isRedirect) {
                setTimeout(function () {
                    location.replace(response.redirectUrl);
                }, 2000);
            }
            else {
                common.CloseModal();
                if (response.isPost) {
                    table.BindPostTable(response.redirectUrl, 'myTable');
                }
                else {
                    //table.BindTable(response.redirectUrl, 'myTable');                   
                }

            }
        });
    }
    else {
        toast.showToast('error', response.message, 'error');
    }
}
function OnSuccess(response) {
    if (response.status) {
        $.when(toast.showToast('success', response.message, 'success')).then(function () {

            if (response.isRedirect) {
                setTimeout(function () {
                    location.replace(response.redirectUrl);
                }, 2000);
            }
            else {
                common.CloseModal();
                if (response.isPost) {
                    table.BindPostTable(response.redirectUrl, 'myTable');
                }
                else {
                    //table.BindTable(response.redirectUrl, 'myTable');
                    // Clear any existing table
                    var tableContainer = document.getElementById('divtable');
                    tableContainer.innerHTML = ""; // Clear previous content

                    // Create the table
                    var table = document.createElement('table');
                    table.className = 'table table-bordered dataTable';
                    // Add an id to the table
                    table.id = 'dataTable';

                    // Create thead and tbody
                    var thead = document.createElement('thead');
                    var tbody = document.createElement('tbody');

                    // Generate header row
                    var headerRow = document.createElement('tr');
                    var headers = Object.keys(response.data[0]); // Get the headers from the first row
                    headers.forEach(function (header) {
                        var th = document.createElement('th');
                        th.textContent = header;
                        headerRow.appendChild(th);
                    });
                    thead.appendChild(headerRow); // Add header row to thead
                    table.appendChild(thead);     // Append thead to table

                    // Generate table body
                    response.data.forEach(function (row) {
                        var dataRow = document.createElement('tr');
                        headers.forEach(function (header) {
                            var td = document.createElement('td');
                            td.textContent = row[header] || ''; // Handle null or undefined values
                            dataRow.appendChild(td);
                        });
                        tbody.appendChild(dataRow);
                    });
                    table.appendChild(tbody);      // Append tbody to table
                    // Append the table to the container
                    tableContainer.appendChild(table);
                }

            }
        });
    }
    else {
        $('#errormsg').html(response.message);
        toast.showToast('error', response.message, 'error');
    }
}
function OnComplete() {
    common.HideLoader();
    $("input[name='Password']").val(""); // clear field for safety
}