window.verification = {

    init: function (savedRows) {

        if (savedRows) {
            this.loadSavedTable(savedRows);
        }

        //document.getElementById("btnRevert")
        //    ?.addEventListener("click", () => this.submitAction("revert"));

        //document.getElementById("btnVerify")
        //    ?.addEventListener("click", () => this.submitAction("verify"));
    },

    loadSavedTable: function (savedRows) {

        let tbody = document.querySelector("#savedTable tbody");
        if (!tbody || !savedRows) return;

        tbody.innerHTML = "";

        let grouped = {};
        savedRows.forEach(item => {
            grouped[item.rowId] ??= [];
            grouped[item.rowId].push(item);
        });

        Object.keys(grouped).forEach(rowId => {

            let columns = {};
            grouped[rowId].forEach(i => {
                columns[i.subQuestionMasterId] = i.answerValue;
            });

            tbody.insertAdjacentHTML("beforeend", `
                <tr class="subtable-row">
                    <td>${rowId}</td>
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
                </tr>
            `);
        });
    },

    submitAction: function (type) {

        let remark = document.getElementById("txtRemark").value.trim();

        if (type === "revert" && remark === "") {
            Swal.fire("Required", "Please enter remark", "warning");
            return;
        }

        Swal.fire({
            title: "Are you sure?",
            text: type === "verify"
                ? "You want to verify this profile"
                : "You want to revert this profile",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Yes, proceed"
        }).then(result => {

            if (!result.isConfirmed) return;

            fetch(type === "verify"
                ? "/Profile/VerifyProfile"
                : "/Profile/RevertBackVerification", {

                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken":
                        document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    GauravGuid: document.getElementById("hdnGauravGuid").value,
                    DistrictId: document.getElementById("hdnDistrictId").value,
                    FinancialYearId: document.getElementById("hdnFinancialYearId").value,
                    Remark: remark
                })
            })
                .then(res => res.json())
                .then(data => {

                    if (!data.status) {
                        Swal.fire("Error", data.message, "error");
                        return;
                    }

                    Swal.fire({
                        icon: "success",
                        title: "Success",
                        text: data.message,
                        confirmButtonText: "OK"
                    }).then(() => {

                        // Close modal
                        const modalEl = document.getElementById("profileModal");
                        const modal = bootstrap.Modal.getInstance(modalEl);
                        if (modal) modal.hide();

                        // Reload page
                        location.reload();
                    });
                });
        });

    }
}

// 🔥 EVENT DELEGATION (WORKS ALWAYS)
document.addEventListener("click", function (e) {

    if (e.target.id === "btnRevert") {
        verification.submitAction("revert");
    }

    if (e.target.id === "btnVerify") {
        verification.submitAction("verify");
    }

});
