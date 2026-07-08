

document.addEventListener("DOMContentLoaded", function () {

    const modal = document.getElementById("imageModalManual");
    const modalImg = document.getElementById("modalImage");
    const closeBtn = document.querySelector(".manual-close");
    const btnCancel = document.getElementById("btnCancel");
    const btnSave = document.getElementById("btnSave");
    // Open modal
    document.addEventListener("click", function (e) {
        const btn = e.target.closest(".view-image-btn");
        if (!btn) return;

        modalImg.src = btn.dataset.image;
        modal.style.display = "flex";
    });

    // Close on X
    //closeBtn.addEventListener("click", function () {
    //    modal.style.display = "none";
    //    modalImg.src = "";
    //});
    if (closeBtn) {
        closeBtn.addEventListener("click", function () {
            modal.style.display = "none";
            modalImg.src = "";
        });
    }
    // Close on background click
    //modal.addEventListener("click", function (e) {
    //    if (e.target === modal) {
    //        modal.style.display = "none";
    //        modalImg.src = "";
    //    }
    //});
    if (modal && modalImg) {
        modal.addEventListener("click", function (e) {
            if (e.target === modal) {
                modal.style.display = "none";
                modalImg.src = "";
            }
        });
    }
    // ESC key close
    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            modal.style.display = "none";
            modalImg.src = "";
        }
    });

    document.addEventListener("click", function (e) {
        const btn = e.target.closest(".edit-btn");
        if (!btn) return;

        // Fill form fields
        document.getElementById("Id").value = btn.dataset.id;
        document.getElementById("Title").value = btn.dataset.title;
        document.getElementById("DisplayOrder").value = btn.dataset.order;
        document.getElementById("IsActive").checked =
            btn.dataset.active === "True" || btn.dataset.active === "true";

        document.getElementById("ExistingImage").value = btn.dataset.image;

        // Change button text
        document.getElementById("btnSave").innerText = "Update";
        document.getElementById("btnSave").classList.remove("btn-primary");
        document.getElementById("btnSave").classList.add("btn-success");
        document.getElementById("btnCancel").classList.remove("d-none");
        // Scroll to top
        window.scrollTo({ top: 0, behavior: "smooth" });
    });

    document.querySelectorAll(".del-btn").forEach(function (btn) {
        btn.addEventListener("click", function () {
            const id = this.getAttribute("data-sliderid");
            deleteSliderRow(id);
        });

    });
    document.querySelectorAll(".toggle-status-btn").forEach(btn => {
        btn.addEventListener("click", function () {

            const sliderId = this.dataset.id;
            const isActive = this.dataset.status === "True";

            if (!confirm("Are you sure you want to change status?")) return;

            fetch('/WebsiteMaster/ToggleSliderStatus', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken':
                        document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    id: sliderId,
                    isActive: !isActive
                })
            })
                .then(r => r.json())
                .then(res => {
                    if (res.status) {
                        toast.showToast('success', res.message, 'success');
                        location.reload();
                    } else {
                        toast.showToast('error', res.message, 'error');
                    }
                });
        });
    });

    //btnCancel.addEventListener("click", function () {

    //    // 🔹 Clear text fields
    //    document.getElementById("Title").value = "";
    //    document.getElementById("DisplayOrder").value = "";

    //    // 🔹 Uncheck checkbox
    //    document.getElementById("IsActive").checked = false;

    //    // 🔹 Clear file input (important)
    //    const fileInput = document.querySelector("input[type='file']");
    //    if (fileInput) fileInput.value = "";

    //    // 🔹 Clear hidden Id (MOST IMPORTANT)
    //    const idField = document.querySelector("input[name='Id']");
    //    if (idField) idField.value = "";

    //    // 🔹 Clear ExistingImage hidden field
    //    const imgField = document.querySelector("input[name='ExistingImage']");
    //    if (imgField) imgField.value = "";

    //    // 🔹 Change button text back to Save
    //    btnSave.innerText = "Save";

    //    // 🔹 Hide cancel button
    //    btnCancel.classList.add("d-none");
    //});
    if (btnCancel && btnSave) {

        btnCancel.addEventListener("click", function () {

            // 🔹 Clear text fields
            const title = document.getElementById("Title");
            const displayOrder = document.getElementById("DisplayOrder");

            if (title) title.value = "";
            if (displayOrder) displayOrder.value = "";

            // 🔹 Uncheck checkbox
            const isActive = document.getElementById("IsActive");
            if (isActive) isActive.checked = false;

            // 🔹 Clear file input
            const fileInput = document.querySelector("input[type='file']");
            if (fileInput) fileInput.value = "";

            // 🔹 Clear hidden Id
            const idField = document.querySelector("input[name='Id']");
            if (idField) idField.value = "";

            // 🔹 Clear ExistingImage hidden field
            const imgField = document.querySelector("input[name='ExistingImage']");
            if (imgField) imgField.value = "";

            // 🔹 Change button text back to Save
            btnSave.innerText = "Save";

            // 🔹 Hide cancel button
            btnCancel.classList.add("d-none");

        });

    }

});

function deleteSliderRow(id) {
    //if (!confirm("Are you sure to delete?")) return;
    //alert(index);
    //alert(gauravGuid);
    //alert(savedRows[index].id);
    Swal.fire({
        title: "Are you sure?",
        text: "You want to delete this profileImage",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, proceed"
               }).then(result => {

        if (!result.isConfirmed) return;
        $.ajax({
            url: `/WebsiteMaster/deleteSliderRow`,
            type: 'POST',
            data: { rowId: id },
            success: function (r) {
                if (r.status) {
                    toast.showToast('success', r.message, 'success');
                    location.reload();
                }
                else {
                    toast.showToast('error', r.message, 'error');
                }
            }
        });
    });
}
document.addEventListener("click", function (e) {

    const btn = e.target.closest(".edit-btn-nodal");

    if (!btn) return;

    // fill form fields

    document.getElementById("Id").value = btn.dataset.id;
    document.getElementById("Name").value = btn.dataset.name;
    document.getElementById("Designation").value = btn.dataset.designation;
    document.getElementById("Mobile").value = btn.dataset.mobile;
    document.getElementById("PhoneNumber").value = btn.dataset.phone;
    document.getElementById("Email").value = btn.dataset.email;

    // change button text

    document.getElementById("btnSaveNodal").innerText = "Update";
    document.getElementById("btnSaveNodal").classList.remove("btn-primary");
    document.getElementById("btnSaveNodal").classList.add("btn-success");

    // show cancel button

    document.getElementById("btnCancelNodal").classList.remove("d-none");

    // scroll top

    window.scrollTo({ top: 0, behavior: "smooth" });

});
document.getElementById("btnCancelNodal").addEventListener("click", function () {

    document.getElementById("Id").value = "";

    document.getElementById("Name").value = "";
    document.getElementById("Designation").value = "";
    document.getElementById("Mobile").value = "";
    document.getElementById("PhoneNumber").value = "";
    document.getElementById("Email").value = "";

    // reset button

    const saveBtn = document.getElementById("btnSaveNodal");

    saveBtn.innerText = "Save";
    saveBtn.classList.remove("btn-success");
    saveBtn.classList.add("btn-primary");

    // hide cancel button

    this.classList.add("d-none");

});
document.addEventListener("click", function (e) {

    const btn = e.target.closest(".delete-btn-nodal");

    if (!btn) return;
    //const id = this.getAttribute("data-id");
    let id = btn.dataset.id;
    Swal.fire({
        title: "Are you sure?",
        text: "You want to delete this profileImage",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, proceed"
    }).then(result => {

        if (!result.isConfirmed) return;
        $.ajax({
            url: `/WebsiteMaster/Delete`,
            type: 'POST',
            data: { id: id },
            success: function (r) {
                if (r.status) {
                    toast.showToast('success', r.message, 'success');
                    location.reload();
                }
                else {
                    toast.showToast('error', r.message, 'error');
                }
            }
        });
    });

});