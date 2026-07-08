// =============================================
// BEST PRACTICES MASTER
// =============================================

var bestPrecticesMaster = {

    // =========================================
    // SAVE BEST PRACTICE
    // =========================================

    saveBestPrectices: async function () {

        try {

            let formData = new FormData();

            formData.append(
                "ActivityGuid",
                $("#ActivityGuid").val()
            );

            formData.append(
                "SubActivityId",
                $("#ddlSubActivity").val()
            );

            formData.append(
                "TaskId",
                $("#ddlTask").val()
            );

            formData.append(
                "GeoId",
                $("#ddlGeoLocation").val()
            );

            formData.append(
                "Heading",
                $("#txtHeading").val()
            );

            formData.append(
                "Description",
                $("#txtDescription").val()
            );

            // =================================
            // MAIN IMAGE
            // =================================

            let mainImage =
                $("#mainImage")[0].files[0];

            if (mainImage) {

                formData.append(
                    "MainImage",
                    mainImage
                );

            }

            // =================================
            // PDF
            // =================================

            let pdfFile =
                $("#pdfFile")[0].files[0];

            if (pdfFile) {

                formData.append(
                    "PdfFile",
                    pdfFile
                );

            }

            // =================================
            // BUTTON LOADING
            // =================================

            $("#btnSaveBestPractice")
                .prop("disabled", true)
                .html(`
                    <span class="spinner-border spinner-border-sm"></span>
                    Saving...
                `);

            // =================================
            // AJAX
            // =================================

            let response =
                await $.ajax({

                    url: "/Management/SaveBestPrectices",

                    type: "POST",

                    data: formData,

                    processData: false,

                    contentType: false

                });

            // =================================
            // SUCCESS
            // =================================

            if (response.success) {

                alert(
                    "Best Practice Saved Successfully"
                );

                // =============================
                // SAVE ID
                // =============================

                $("#savedBestPracticeId")
                    .val(response.bestPracticeId);

                // =============================
                // SHOW GALLERY SECTION
                // =============================

                $("#gallerySection")
                    .removeClass("d-none");

            }
            else {

                alert(response.message);

            }

        }
        catch (ex) {

            console.log(ex);

            alert(
                "Something went wrong."
            );

        }
        finally {

            // =================================
            // RESET BUTTON
            // =================================

            $("#btnSaveBestPractice")
                .prop("disabled", false)
                .html(`
                    <i class="bx bx-save"></i>
                    Save Best Practice
                `);

        }

    },

    // =========================================
    // UPLOAD GALLERY IMAGES
    // =========================================

    uploadGalleryImages: async function () {

        try {

            let bestPracticeId =
                $("#savedBestPracticeId").val();

            if (bestPracticeId == "") {

                alert(
                    "Please save best practice first."
                );

                return;

            }

            // =================================
            // FILES
            // =================================

            let files =
                $("#galleryImages")[0].files;

            if (files.length == 0) {

                alert(
                    "Please select gallery images."
                );

                return;

            }

            // =================================
            // FORM DATA
            // =================================

            let formData = new FormData();

            formData.append(
                "BestPracticeId",
                bestPracticeId
            );

            // =================================
            // MULTIPLE FILES
            // =================================

            for (let i = 0; i < files.length; i++) {

                formData.append(
                    "GalleryImages",
                    files[i]
                );

            }

            // =================================
            // BUTTON LOADING
            // =================================

            $("#btnUploadGallery")
                .prop("disabled", true)
                .html(`
                    <span class="spinner-border spinner-border-sm"></span>
                    Uploading...
                `);

            // =================================
            // AJAX
            // =================================

            let response =
                await $.ajax({

                    url: "/Management/UploadGalleryImages",

                    type: "POST",

                    data: formData,

                    processData: false,

                    contentType: false

                });

            // =================================
            // SUCCESS
            // =================================

            if (response.success) {

                alert(
                    "Gallery Images Uploaded Successfully"
                );

                // =============================
                // RESET FILE INPUT
                // =============================

                $("#galleryImages").val("");

            }
            else {

                alert(response.message);

            }

        }
        catch (ex) {

            console.log(ex);

            alert(
                "Something went wrong."
            );

        }
        finally {

            // =================================
            // RESET BUTTON
            // =================================

            $("#btnUploadGallery")
                .prop("disabled", false)
                .html(`
                    Upload Images
                `);

        }

    }

};

// =============================================
// DOCUMENT READY
// =============================================

document.addEventListener(
    "DOMContentLoaded",
    function () {

        // =====================================
        // SAVE
        // =====================================

        document
            .getElementById(
                "btnSaveBestPractice"
            )
            .addEventListener(
                "click",
                async function () {

                    await bestPrecticesMaster
                        .saveBestPrectices();

                }
            );

        // =====================================
        // GALLERY
        // =====================================

        document
            .getElementById(
                "btnUploadGallery"
            )
            .addEventListener(
                "click",
                async function () {

                    await bestPrecticesMaster
                        .uploadGalleryImages();

                }
            );

    }
);