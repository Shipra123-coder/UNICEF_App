

document.addEventListener("DOMContentLoaded", function () {

    const modal = document.getElementById("AnnouncemenModalManual");
    //const modalImg = document.getElementById("modalAnnouncement");
    const modalContent = document.getElementById("modalContent");

    const closeBtn = document.querySelector(".manual-close");
    const btnCancel = document.getElementById("btnCancelAnnouncement");
    const btnSave = document.getElementById("btnAnnouncement");
    document.addEventListener("click", function (e) {

        const btn = e.target.closest(".download-Announcement-btn");
        if (!btn) return;

        const base64Data = btn.dataset.file;

        // Create temporary link
        const link = document.createElement("a");
        link.href = base64Data;
        link.download = "announcement.pdf";

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    });

    // Close on X
    closeBtn.addEventListener("click", function () {
        modal.style.display = "none";
        // modalImg.src = "";
        modalContent.innerHTML = "";

    });

    // Close on background click
    modal.addEventListener("click", function (e) {
        if (e.target === modal) {
            modal.style.display = "none";
            //modalImg.src = "";
            modalContent.innerHTML = "";

        }
    });

    // ESC key close
    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            modal.style.display = "none";
            //modalImg.src = "";
            modalContent.innerHTML = "";

        }
    });

    document.addEventListener("click", function (e) {
        const btn = e.target.closest(".edit-btn-Announcement");
        if (!btn) return;

        // Fill form fields
        document.getElementById("Id").value = btn.dataset.id;
        document.getElementById("Title").value = btn.dataset.title;
        document.getElementById("DisplayOrder").value = btn.dataset.order;
        document.getElementById("IsActive").checked =
            btn.dataset.active === "True" || btn.dataset.active === "true";
        document.getElementById("IsNew").checked =
            btn.dataset.isnew === "True" || btn.dataset.isnew === "true";
        document.getElementById("ExistingImage").value = btn.dataset.image;

        // Change button text
        document.getElementById("btnAnnouncement").innerText = "Update";
        document.getElementById("btnAnnouncement").classList.remove("btn-primary");
        document.getElementById("btnAnnouncement").classList.add("btn-success");
        document.getElementById("btnCancelAnnouncement").classList.remove("d-none");
        // Scroll to top
        window.scrollTo({ top: 0, behavior: "smooth" });
    });

    document.querySelectorAll(".del-btn-Announcement").forEach(function (btn) {
        btn.addEventListener("click", function () {
            const id = this.getAttribute("data-sliderid");
            deleteAnnouncementRow(id);
        });

    });
    document.querySelectorAll(".toggle-status-btn-Announcement").forEach(btn => {
        btn.addEventListener("click", function () {

            const sliderId = this.dataset.id;
            const isActive = this.dataset.status === "True";

            if (!confirm("Are you sure you want to change status?")) return;

            fetch('/WebsiteMaster/ToggleAnnouncementStatus', {
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

    document.querySelectorAll(".toggle-IsNew-btn-Announcement").forEach(btn => {
        btn.addEventListener("click", function () {

            const sliderId = this.dataset.id;
            const isnew = this.dataset.isnew === "True";

            if (!confirm("Are you sure you want to change this?")) return;

            fetch('/WebsiteMaster/ToggleAnnouncementIsNew', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken':
                        document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    id: sliderId,
                    IsNew: !isnew
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

    btnCancel.addEventListener("click", function () {

        // 🔹 Clear text fields
        document.getElementById("Title").value = "";
        document.getElementById("DisplayOrder").value = "";

        // 🔹 Uncheck checkbox
        document.getElementById("IsActive").checked = false;

        document.getElementById("IsNew").checked = false;
        // 🔹 Clear file input (important)
        const fileInput = document.querySelector("input[type='file']");
        if (fileInput) fileInput.value = "";

        // 🔹 Clear hidden Id (MOST IMPORTANT)
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

});

function deleteAnnouncementRow(id) {
    //if (!confirm("Are you sure to delete?")) return;
    //alert(index);
    //alert(gauravGuid);
    //alert(savedRows[index].id);
    Swal.fire({
        title: "Are you sure?",
        text: "You want to delete this",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, proceed"
    }).then(result => {

        if (!result.isConfirmed) return;
        $.ajax({
            url: `/WebsiteMaster/deleteAnnouncementRow`,
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