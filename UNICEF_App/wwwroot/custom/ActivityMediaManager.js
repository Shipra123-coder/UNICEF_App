
// ========================================================================
// ACTIVITY MEDIA SHOWCASE & VISUAL BUILDER MANAGEMENT FRAMEWORK
// ========================================================================

var activityMediaManager = {

    // Core Engine Initializer Event Listeners
    init: function () {
        this.bindEvents();
        // Initial load: चूंकि डिफ़ॉल्ट रूप से "CoverPage" सेलेक्टेड रहता है
        this.loadUploadedCovers();
    },
    bindEvents: function () {
        var self = this;

        // 1. MASTER WORK MODE DROPDOWN TOGGLE LISTENER
        $('#LayoutActionMode').on('change', function () {
            var selectedMode = $(this).val(); // CoverPage OR InnerImage

            if (selectedMode === "CoverPage") {
                $('#coverPageSection').removeClass('d-none');
                $('#innerImageSection').addClass('d-none');
                $('#galleryPanelTitle').text("Uploaded Cover Pages Gallery");

                // Live refresh covers list
                self.loadUploadedCovers();
            }
            else if (selectedMode === "InnerImage") {
                $('#coverPageSection').addClass('d-none');
                $('#innerImageSection').removeClass('d-none');
                $('#galleryPanelTitle').text("Uploaded Inner Media Assets");

                // Clear gallery until parent cover is selected
                $('#liveMediaGalleryContainer').html(`
    <div class="col-12 text-center py-4 text-muted">
        <i class="fas fa-arrow-left fa-2x mb-2 text-success animate-bounce"></i>
        <p class="mb-0 small">Please select a Target Parent Cover Page above to view its inner assets.</p>
    </div>`);
                $('#galleryCounterBadge').text("0 Items Found");

                // Trigger dropdown options fetch
                self.populateParentCoverDropdown();
            }
        });

        // 2. 🌟 NEW: PARENT COVER SELECTION CHANGE LISTENER (FOR INNER MEDIA)
        $('#ParentCoverId').on('change', function () {
            var parentId = $(this).val();
            if (parentId && parentId !== "") {
                self.loadUploadedInnerMedia(parentId);
            } else {
                $('#liveMediaGalleryContainer').html(`
                        <div class="col-12 text-center py-4 text-muted">
                            <p class="mb-0 small">Please select a valid parent cover page to pull nested files.</p>
                        </div>`);
                $('#galleryCounterBadge').text("0 Items Found");
            }
        });

        // 3. NESTED SUB-MEDIA FORMAT INJECTION SELECTOR LISTENER
        $('#MediaTypeSelector').on('change', function () {
            var selectedType = $(this).val(); // Image, Video, Audio
            $('.media-input-node').addClass('d-none');
            $('#mediaBox_' + selectedType).removeClass('d-none');
        });

        // 4. EXECUTE FORM DATA SUBMISSION SAVE BUTTON TRIGGER
        $('#btnSaveMediaData').on('click', function () {
            self.saveMediaConfiguration();
        });

        // 5. RESET SYSTEM CONTROL FIELDS TRIGGER FLOW
        $('#btnResetForm').on('click', function () {
            self.resetFormControls();
        });
    },    
    loadUploadedCovers: function () {
        var self = this;
        var activityGuid = $('#ActivityGuid').val() || "0"; // 0 मतलब 'ALL'

        // टेबल फ्रेंडली लोडिंग इंडिकेटर
        $('#liveMediaGalleryContainer').html('<div class="col-12 text-center py-4"><i class="bx bx-loader-alt bx-spin text-primary fs-3"></i> Loading cover data...</div>');

        // केवल sectorId पेलोड में भेजें
        ajax.doPostAjax('/Management/GetCoverPagesList', { guid: activityGuid }, function (response) {
            if (!response || response.length === 0) {
                $('#liveMediaGalleryContainer').html(`
                <div class="col-12 text-center py-4 text-muted">
                    <i class="bx bx-folder-open fs-2 mb-2 opacity-50"></i>
                    <p class="mb-0 small">No cover pages found for this configuration.</p>
                </div>`);
                $('#galleryCounterBadge').text("0 Covers Found");
                return;
            }

            // मास्टर टेबल हेडर संरचना
            var html = `
        <div class="col-12">
            <div class="table-responsive bg-white rounded border shadow-xs">
                <table class="table table-hover align-middle mb-0">
                    <thead class="table-light text-uppercase font-monospace fs-7 text-secondary">
                        <tr>
                            <th scope="col" class="text-center" style="width: 70px;">Order</th>
                            <th scope="col" class="text-center" style="width: 80px;">Preview</th>
                            <th scope="col">Heading Title</th>
                            <th scope="col" style="width: 140px;">Document</th>
                            <th scope="col" class="text-center" style="width: 90px;">Actions</th>
                        </tr>
                    </thead>
                    <tbody>`;

            // डेटा रोज़ (Rows) जनरेशन
            response.forEach(function (item) {
                var coverId = item.coverPageId || item.CoverPageId;
                var heading = item.headingTitle || "";
                var orderNo = item.displayOrder ?? "1";
                var imgUrl = item.coverImageUrl || '/public/img/default-thumbnail.jpg';
                var pdfUrl = item.descriptionPdfUrl || "";

                html += `
            <tr id="coverCard_${coverId}">
                <td class="text-center">
                    <span class="badge bg-soft-primary text-primary fw-bold px-2 py-1 font-monospace">${orderNo}</span>
                </td>
                
                <td class="text-center">
                    <div class="rounded border bg-light overflow-hidden d-inline-block shadow-xs img-width-height" style="width: 50px; height: 35px; border-color: #e2e8f0 !important;">
                        <img src="${imgUrl}" class="w-100 h-100" alt="Cover" style="object-fit: cover; object-position: center;" />
                    </div>
                </td>
                
                <td>
                    <h6 class="fw-bold text-dark mb-0 text-truncate" style="max-width: 450px;" title="${heading}">
                        ${heading}
                    </h6>
                </td>
                
                <td>
                    ${pdfUrl ? `
                        <a href="${pdfUrl}" target="_blank" class="btn btn-sm btn-link text-danger font-monospace px-0 py-0 fw-semibold text-decoration-none">
                            <i class="bx bxs-file-pdf fs-5 align-middle me-1"></i>View PDF
                        </a>` : `
                        <span class="text-muted small italic fs-7">No Attachment</span>
                    `}
                </td>
                
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-outline-danger btn-icon rounded-circle btn-delete-cover" 
                            data-id="${coverId}" title="Delete Cover" style="width: 30px; height: 30px; padding: 0; line-height: 30px;">
                        <i class="bx bx-trash-alt align-middle fs-6"></i>
                    </button>
                </td>
            </tr>`;
            });

            html += `
                    </tbody>
                </table>
            </div>
        </div>`;

            $('#liveMediaGalleryContainer').html(html);
            $('#galleryCounterBadge').text(`${response.length} Covers Found`);
        });
    },
    loadUploadedInnerMedia: function (parentCoverId) {
        if (!parentCoverId) return;
        var selectedSectorId = $('#InnerSector').val() || "0";

        // टेबल फ्रेंडली लोडिंग इंडिकेटर
        $('#liveMediaGalleryContainer').html('<div class="col-12 text-center py-4"><i class="bx bx-loader-alt bx-spin text-success fs-3"></i> Synchronizing inner assets...</div>');

        ajax.doPostAjax('/Management/GetInnerMediaList', { coverPageId: parentCoverId, sectorId: selectedSectorId }, function (response) {
            if (!response || response.length === 0) {
                $('#liveMediaGalleryContainer').html(`
                <div class="col-12 text-center py-4 text-muted">
                    <i class="bx bx-images fs-2 mb-2 opacity-50"></i>
                    <p class="mb-0 small">No sub-media assets mapped to this specific cover yet.</p>
                </div>`);
                $('#galleryCounterBadge').text("0 Assets Found");
                return;
            }

            // इनर मीडिया टेबल संरचना
            var html = `
        <div class="col-12">
            <div class="table-responsive bg-white rounded border shadow-xs">
                <table class="table table-hover align-middle mb-0">
                    <thead class="table-light text-uppercase font-monospace fs-7 text-secondary">
                        <tr>
                            <th scope="col" class="text-center" style="width: 80px;">Format</th>
                            <th scope="col" class="text-center" style="width: 80px;">Preview</th>
                            <th scope="col">File / Asset Name</th>
                            <th scope="col">Description Notes</th>
                            <th scope="col" class="text-center" style="width: 90px;">Actions</th>
                        </tr>
                    </thead>
                    <tbody>`;

            // रोज़ (Rows) जनरेशन और कंडीशनल रेंडरिंग
            response.forEach(function (item) {
                var innerId = item.innerMediaId || item.InnerMediaId;
                var mediaType = item.mediaType || "Image";
                var assetUrl = item.mediaAssetUrl || "";
                var fileName = assetUrl.split('/').pop();
                var notes = item.descriptionNotes || '<span class="text-muted fs-7 italic">No notes added</span>';

                var previewHtml = '';
                var badgeClass = 'bg-dark';

                // फॉर्मेट के अनुसार सही आइकन और रंग सेट करें
                if (mediaType === "Image") {
                    badgeClass = 'bg-success';
                    previewHtml = `<img src="${assetUrl}" class="w-100 h-100" style="object-fit: cover; object-position: center;" alt="Preview" />`;
                }
                else if (mediaType === "Video") {
                    badgeClass = 'bg-info text-dark';
                    previewHtml = `
                    <div class="d-flex align-items-center justify-content-center bg-dark text-white w-100 h-100 fs-5">
                        <i class="bx bx-video"></i>
                    </div>`;
                }
                else if (mediaType === "Audio") {
                    badgeClass = 'bg-warning text-dark';
                    previewHtml = `
                    <div class="d-flex align-items-center justify-content-center bg-secondary text-white w-100 h-100 fs-5">
                        <i class="bx bx-volume-full"></i>
                    </div>`;
                }

                html += `
            <tr id="innerMediaCard_${innerId}">
                <td class="text-center">
                    <span class="badge ${badgeClass} font-monospace px-2 py-1 small" style="min-width: 55px;">${mediaType}</span>
                </td>
                
                <td class="text-center">
                    <div class="rounded border bg-light overflow-hidden d-inline-block shadow-xs img-width-height" style="width: 50px; height: 35px; border-color: #e2e8f0 !important;">
                        ${previewHtml}
                    </div>
                </td>
                
                <td>
                    <a href="${assetUrl}" target="_blank" class="fw-semibold text-primary text-decoration-none text-truncate d-block font-monospace small" style="max-width: 250px;" title="${fileName}">
                        <i class="bx bx-link-external me-1 align-middle fs-7"></i>${fileName}
                    </a>
                </td>
                
                <td>
                    <p class="text-secondary mb-0 text-truncate small" style="max-width: 350px;" title="${item.descriptionNotes || ''}">
                        ${notes}
                    </p>
                </td>
                
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-outline-danger btn-icon rounded-circle btn-delete-inner" 
                            data-id="${innerId}" data-parent-id="${parentCoverId}" title="Delete Asset" style="width: 30px; height: 30px; padding: 0; line-height: 30px;">
                        <i class="bx bx-trash-alt align-middle fs-6"></i>
                    </button>
                </td>
            </tr>`;
            });

            html += `
                    </tbody>
                </table>
            </div>
        </div>`;

            $('#liveMediaGalleryContainer').html(html);
            $('#galleryCounterBadge').text(`${response.length} Assets Found`);
        });
    },   
    populateParentCoverDropdown: function () {
        var activityGuid = $('#ActivityGuid').val() || $('#hiddenActivityGuid').val();
        if (!activityGuid || activityGuid === "0") return;

        if (typeof common !== 'undefined' && common.BindDropdown) {
            common.BindDropdown(`/Management/GetActiveCoversByActivity?guid=${activityGuid}`, "ParentCoverId", "Parent Canvas Layout", "");
        }
    },
    saveMediaConfiguration: function () {
        var self = this;
        var selectedMode = $('#LayoutActionMode').val();
        var activityGuid = $('#ActivityGuid').val() || $('#hiddenActivityGuid').val();
        var descriptionNotes = $('#SharedDescription').val().trim();

        if (!activityGuid || activityGuid === "0") {
            toast.showToast('Validation Warning', 'Please finalize core activity master parameters before embedding showcase metrics.', 'error');
            return false;
        }

        // 🔷 फ़ाइल साइज़ चेक करने का उदाहरण (Max 5MB For Image, 50MB For Video)
        if (selectedMode === "CoverPage") {
            var imgFile = $('#CoverImageFile')[0].files[0];

            if (imgFile) {
                var maxImgSize = 5 * 1024 * 1024; // 5 MB इन बाइट्स
                if (imgFile.size > maxImgSize) {
                    toast.showToast('Size Error', 'Cover image size must be less than 5MB.', 'error');
                    return false; // अपलोड रोकें
                }
            }
        }
        else if (selectedMode === "InnerImage") {
            var mediaType = $('#MediaTypeSelector').val();
            var fileInput = null;

            if (mediaType === "Image") fileInput = $('#InnerImageFile')[0];
            else if (mediaType === "Video") fileInput = $('#InnerVideoFile')[0];
            else if (mediaType === "Audio") fileInput = $('#InnerAudioFile')[0];

            if (fileInput && fileInput.files[0]) {
                var file = fileInput.files[0];

                // वीडियो के लिए 50MB की लिमिट
                if (mediaType === "Video" && file.size > (50 * 1024 * 1024)) {
                    toast.showToast('Size Error', 'Video file size cannot exceed 50MB.', 'error');
                    return false;
                }
                // इमेज के लिए 5MB की लिमिट
                if (mediaType === "Image" && file.size > (5 * 1024 * 1024)) {
                    toast.showToast('Size Error', 'Inner image size cannot exceed 5MB.', 'error');
                    return false;
                }
            }
        }

        var formData = new FormData();
        formData.append("ActivityGuid", activityGuid);
        formData.append("SubmissionMode", selectedMode);
        formData.append("Description", descriptionNotes);

        if (selectedMode === "CoverPage") {
            var heading = $('#CoverHeading').val().trim();
            var orderNo = $('#CoverOrderNo').val().trim();
            var imgFile = $('#CoverImageFile')[0].files[0];
            var pdfFile = $('#CoverPdfFile')[0].files[0];

            // 🌟 नए ड्रॉपडाउंस की वैल्यूज को कैप्चर करें
            var subActivityId = $('#ddlSubActivity').val() || "0";
            var taskId = $('#ddlTask').val() || "0";
            var geoLocationId = $('#ddlGeoLocation').val() || "0";

            if (!heading) {
                toast.showToast('Input Required', 'Please supply a main title header name for your cover layout.', 'error');
                return false;
            }
            if (!imgFile) {
                toast.showToast('File Required', 'An active primary banner cover illustration image must be uploaded.', 'error');
                return false;
            }

            formData.append("Heading", heading);
            formData.append("OrderNumber", orderNo || "1");
            formData.append("CoverImage", imgFile);
            if (pdfFile) formData.append("DescriptionPdf", pdfFile);

            // 🌟 नए पैरामीटर्स को FormData में अपेंड करें (कंट्रोलर में रिसीव करने के लिए)
            formData.append("SubActivityId", subActivityId);
            formData.append("TaskId", taskId);
            formData.append("GeoLocationId", geoLocationId);
        }
        else if (selectedMode === "InnerImage") {
            var parentId = $('#ParentCoverId').val();
            var mediaType = $('#MediaTypeSelector').val();
            var targetFileField = null;

            if (!parentId) {
                toast.showToast('Selection Required', 'Please map this secondary child asset component item to a target parent cover card row node.', 'error');
                return false;
            }

            formData.append("ParentCoverId", parentId);
            formData.append("MediaType", mediaType);

            if (mediaType === "Image") targetFileField = $('#InnerImageFile')[0];
            else if (mediaType === "Video") targetFileField = $('#InnerVideoFile')[0];
            else if (mediaType === "Audio") targetFileField = $('#InnerAudioFile')[0];

            if (targetFileField && targetFileField.files[0]) {
                formData.append("MediaFile", targetFileField.files[0]);
            } else {
                toast.showToast('Upload Required', `Please browse and append a valid transactional source asset file matching your chosen ${mediaType} selection.`, 'error');
                return false;
            }
        }

        Swal.fire({
            title: 'Confirm Save Matrix?',
            text: "Are you sure you want to commit these localized canvas layout parameters?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#0d6efd',
            cancelButtonColor: '#6c757d',
            confirmButtonText: '<i class="fas fa-check-circle me-1"></i> Yes, Securely Save'
        }).then((result) => {
            if (result.isConfirmed) {
                if (typeof common !== 'undefined' && common.ShowLoader) common.ShowLoader();

                $.ajax({
                    url: '/Management/SaveActivityMediaShowcase',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (typeof common !== 'undefined' && common.HideLoader) common.HideLoader();

                        if (response.status || response.success) {
                            toast.showToast('Success', response.message || 'Media structural nodes compiled successfully.', 'success');

                            // 🌟 RE-LOAD GALLERY INSTANTLY UPON SUCCESSFUL SAVE
                            if (selectedMode === "CoverPage") {
                                self.loadUploadedCovers();
                            } else {
                                self.loadUploadedInnerMedia(parentId);
                            }

                            self.resetFormControls();
                        } else {
                            toast.showToast('Transaction Rejected', response.message || 'System failed to parse properties cleanly.', 'error');
                        }
                    },
                    error: function () {
                        if (typeof common !== 'undefined' && common.HideLoader) common.HideLoader();
                        toast.showToast('Network Error', 'A critical framework routing lifecycle fault prevented background dataset syncing.', 'error');
                    }
                });
            }
        });
    },
    resetFormControls: function () {
        $('#CoverHeading').val("");
        $('#CoverOrderNo').val("1");
        $('#CoverImageFile').val("");
        $('#CoverPdfFile').val("");
        // Note: We don't clear ParentCoverId here so that the inner gallery stays open for multi-file additions.
        $('#InnerImageFile').val("");
        $('#InnerVideoFile').val("");
        $('#InnerAudioFile').val("");
        $('#SharedDescription').val("");
        $('#MediaTypeSelector').val("Image").trigger('change');
    },
    executeMediaDeletion: function (mode, targetId, parentId) {
        var self = this;
        var msgText = mode === 'CoverPage'
            ? "Warning: Deleting this cover will also delete all of its inner images/videos permanently!"
            : "This inner multimedia asset file will be deleted permanently.";

        Swal.fire({
            title: 'Are you sure?',
            text: msgText,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: '<i class="fas fa-trash-alt me-1"></i> Yes, Delete It!'
        }).then((result) => {
            if (result.isConfirmed) {
                if (typeof common !== 'undefined' && common.ShowLoader) common.ShowLoader();

                $.ajax({
                    url: '/Management/DeleteActivityMediaAsset',
                    type: 'POST',
                    data: { mode: mode, id: targetId },
                    success: function (r) {
                        if (typeof common !== 'undefined' && common.HideLoader) common.HideLoader();

                        if (r.status || r.success) {
                            toast.showToast('Deleted', r.message || 'Asset removed successfully.', 'success');

                            // लिस्ट को लाइव रिफ्रेश करें
                            if (mode === 'CoverPage') {
                                self.loadUploadedCovers();
                            } else {
                                self.loadUploadedInnerMedia(parentId);
                            }
                        } else {
                            toast.showToast('Execution Failed', r.message || 'Error deleting item.', 'error');
                        }
                    },
                    error: function () {
                        if (typeof common !== 'undefined' && common.HideLoader) common.HideLoader();
                        toast.showToast('Network Error', 'Could not reach server destination routine.', 'error');
                    }
                });
            }
        });
    },
};

// jQuery $(document).ready की जगह DOMContentLoaded का उपयोग
document.addEventListener('DOMContentLoaded', function () {
    if (typeof activityMediaManager !== "undefined" && typeof activityMediaManager.init === "function") {
        activityMediaManager.init();
    }
});

// =======================================================
// EVENT DELEGATION (डायनेमिक बटन्स के क्लिक को हैंडल करने के लिए)
// =======================================================
document.addEventListener('click', function (event) {

    // 5. DELETE COVER PAGE EVENT
    // यह चेक करता है कि क्या क्लिक किए गए एलिमेंट या उसके पैरेंट पर '.btn-delete-cover' क्लास है
    const deleteCoverBtn = event.target.closest('.btn-delete-cover');
    if (deleteCoverBtn) {
        // jQuery के .data('id') की जगह native dataset.id का उपयोग
        var coverId = deleteCoverBtn.dataset.id;

        if (typeof activityMediaManager !== "undefined" && typeof activityMediaManager.executeMediaDeletion === "function") {
            activityMediaManager.executeMediaDeletion('CoverPage', coverId, 0);
        }
        return; // मैच होने पर आगे चेक करने की जरूरत नहीं
    }

    // 6. DELETE INNER MEDIA ASSET EVENT
    // यह चेक करता है कि क्या क्लिक किए गए एलिमेंट या उसके पैरेंट पर '.btn-delete-inner' क्लास है
    const deleteInnerBtn = event.target.closest('.btn-delete-inner');
    if (deleteInnerBtn) {
        // jQuery के .data() की जगह dataset प्रॉपर्टी का उपयोग
        var innerId = deleteInnerBtn.dataset.id;
        var parentId = deleteInnerBtn.dataset.parentId; // HTML में data-parent-id ऑटोमैटिकली parentId बन जाता है

        if (typeof activityMediaManager !== "undefined" && typeof activityMediaManager.executeMediaDeletion === "function") {
            activityMediaManager.executeMediaDeletion('InnerImage', innerId, parentId);
        }
        return;
    }
});
