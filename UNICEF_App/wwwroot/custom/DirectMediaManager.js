// ========================================================================
// RE-CONFIGURED FOR SECTOR-BASED ARCHITECTURE ONLY (NO ACTIVITY ID)
// ========================================================================

var directMediaManager = {

    init: function () {
        this.bindEvents();
        // शुरुआत में ऑल कवर्स लोड करने के लिए कॉल करें
        this.loadUploadedCovers();
    },

    bindEvents: function () {
        var self = this;

        // 1. मास्टर एक्शन ड्रॉपडाउन (Cover Page या Inner Image)
        $('#LayoutActionMode').on('change', function () {
            var selectedMode = $(this).val();

            if (selectedMode === "CoverPage") {
                $('#coverPageSection').removeClass('d-none');
                $('#innerImageSection').addClass('d-none');
                $('#galleryPanelTitle').text("Uploaded Cover Pages Gallery");
                self.loadUploadedCovers();
            }
            else if (selectedMode === "InnerImage") {
                $('#coverPageSection').addClass('d-none');
                $('#innerImageSection').removeClass('d-none');
                $('#galleryPanelTitle').text("Uploaded Inner Media Assets");

                // इनर इमेज मोड में आते ही गैलरी को तब तक खाली रखें जब तक पैरेंट कवर न चुना जाए
                $('#liveMediaGalleryContainer').html(`
                    <div class="col-12 text-center py-4 text-muted">
                        <i class="bx bx-left-arrow-alt fs-3 mb-2 text-success animate-bounce"></i>
                        <p class="mb-0 small">Please select a Target Parent Cover Page above to view its inner assets.</p>
                    </div>`);
                $('#galleryCounterBadge').text("0 Items Found");

                // 🌟 अब पैरेंट कवर ड्रॉपडाउन भी सीधे चुनी हुई 'SectorId' के आधार पर लोड होगा
                self.populateParentCoverDropdown();
            }
        });

        // 2. 🌟 सेक्टर बदलने पर लाइव फ़िल्टर (जैसे ही सेक्टर बदलें, उस सेक्टर के कवर्स आ जाएँ)
        $('#Sector').on('change', function () {
            var currentMode = $('#LayoutActionMode').val();

            if (currentMode === "CoverPage") {
                self.loadUploadedCovers();
            } else if (currentMode === "InnerImage") {
                // यदि इनर इमेज मोड खुला है, तो सेक्टर बदलने पर पैरेंट कवर्स का ड्रॉपडाउन री-फ्रेश करें
                self.populateParentCoverDropdown();
                $('#ParentCoverId').val("").trigger('change');
            }
        });
        // 2. 🌟 सेक्टर बदलने पर लाइव फ़िल्टर (जैसे ही सेक्टर बदलें, उस सेक्टर के कवर्स आ जाएँ)
        $('#InnerSector').on('change', function () {
            var currentMode = $('#LayoutActionMode').val();
            if (currentMode === "InnerImage") {
                // यदि इनर इमेज मोड खुला है, तो सेक्टर बदलने पर पैरेंट कवर्स का ड्रॉपडाउन री-फ्रेश करें
                self.populateParentCoverDropdown();
                $('#ParentCoverId').val("").trigger('change');
            }
        });

        // 3. पैरेंट कवर ड्रॉपडाउन बदलने पर इनर मीडिया लिस्ट लोड करें
        $('#ParentCoverId').on('change', function () {
            var parentId = $(this).val();
            if (parentId) {
                self.loadUploadedInnerMedia(parentId);
            } else {
                $('#liveMediaGalleryContainer').html('<div class="col-12 text-center py-4 text-muted"><p class="mb-0 small">Please select a valid parent cover page.</p></div>');
                $('#galleryCounterBadge').text("0 Items Found");
            }
        });

        // 4. मीडिया फॉर्मेट टाइप चेंजर
        $('#MediaTypeSelector').on('change', function () {
            var selectedType = $(this).val();
            $('.media-input-node').addClass('d-none');
            $('#mediaBox_' + selectedType).removeClass('d-none');
        });

        // 5. सेव बटन इवेंट
        $('#btnSaveMediaData').on('click', function () {
            self.saveMediaConfiguration();
        });

        // 6. रीसेट बटन इवेंट
        $('#btnResetForm').on('click', function () {
            self.resetFormControls();
        });
    },

    // =========================================================
    // 🌟 LOAD COVER PAGES BASED ON SECTOR FILTER
    // =========================================================
    loadUploadedCovers: function () {
        var self = this;
        var selectedSectorId = $('#Sector').val() || "0"; // 0 मतलब 'ALL'

        // लोडिंग इंडिकेटर (टेबल के अनुकूल)
        $('#liveMediaGalleryContainer').html('<div class="col-12 text-center py-4"><i class="bx bx-loader-alt bx-spin text-primary fs-3"></i> Loading cover data...</div>');

        // केवल sectorId पेलोड में भेजें
        ajax.doPostAjax('/Management/GetDirectCoverPagesList', { sectorId: parseInt(selectedSectorId) }, function (response) {
            if (!response || response.length === 0) {
                $('#liveMediaGalleryContainer').html(`
                <div class="col-12 text-center py-4 text-muted">
                    <i class="bx bx-folder-open fs-2 mb-2 opacity-50"></i>
                    <p class="mb-0 small">No cover pages found for this configuration.</p>
                </div>`);
                $('#galleryCounterBadge').text("0 Covers Found");
                return;
            }

            // 🌟 स्टेप 1: टेबल का आउटर स्ट्रक्चर और हेडर तैयार करें
            var html = `
        <div class="col-12">
            <div class="table-responsive bg-white rounded border shadow-xs">
                <table class="table table-hover align-middle mb-0">
                    <thead class="table-light text-uppercase font-monospace fs-7 text-secondary">
                        <tr>
                            <th scope="col" class="text-center" style="width: 80px;">Order</th>
                            <th scope="col" style="width: 120px;">Preview</th>
                            <th scope="col">Heading Title</th>
                            <th scope="col" style="width: 150px;">Document</th>
                            <th scope="col" class="text-center" style="width: 100px;">Actions</th>
                        </tr>
                    </thead>
                    <tbody>`;

            // 🌟 स्टेप 2: लूप चलाकर हर रिकॉर्ड को टेबल रो (tr) के रूप में अपेंड करें
            response.forEach(function (item) {
                var coverId = item.coverPageId || item.CoverPageId;
                var heading = item.headingTitle || "";
                var orderNo = item.displayOrder ?? "1";
                var imgUrl = item.coverImageUrl || '/public/img/default-thumbnail.jpg';
                var pdfUrl = item.descriptionPdfUrl || "";

                html += `
            <tr id="coverCard_${coverId}">
                <td class="text-center">
                    <span class="badge bg-soft-primary text-primary fw-bold px-2.5 py-1.5 font-monospace">${orderNo}</span>
                </td>
                
                <td>
    <div class="rounded border bg-light overflow-hidden shadow-xs img-width-height" style="width: 50px; height: 35px; border-color: #e2e8f0 !important;">
        <img src="${imgUrl}" class="w-100 h-100" alt="Cover Preview" style="object-fit: cover; object-position: center;" />
    </div>
</td>
                
                <td>
                    <h6 class="fw-bold text-dark mb-0 text-truncate" style="max-width: 400px;" title="${heading}">
                        ${heading}
                    </h6>
                </td>
                
                <td>
                    ${pdfUrl ? `
                        <a href="${pdfUrl}" target="_blank" class="btn btn-sm btn-link text-danger font-monospace px-0 py-0 fw-semibold">
                            <i class="bx bxs-file-pdf fs-5 align-middle me-1"></i>View PDF
                        </a>` : `
                        <span class="text-muted small italic fs-7">No Attachment</span>
                    `}
                </td>
                
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-outline-danger btn-icon rounded-circle btn-delete-cover" 
                            data-id="${coverId}" title="Delete Cover" style="width: 32px; height: 32px; padding: 0; line-height: 32px;">
                        <i class="bx bx-trash-alt align-middle fs-6"></i>
                    </button>
                </td>
            </tr>`;
            });

            // 🌟 स्टेप 3: टेबल को क्लोज करें और डोम में इंजेक्ट करें
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
        var selectedSectorId = $('#InnerSector').val() || "0"; // 0 मतलब 'ALL'

        // लोडिंग इंडिकेटर (टेबल फ्रेंडली)
        $('#liveMediaGalleryContainer').html('<div class="col-12 text-center py-4"><i class="bx bx-loader-alt bx-spin text-success fs-3"></i> Synchronizing inner assets...</div>');

        ajax.doPostAjax('/Management/GetDirectInnerMediaList', { coverPageId: parentCoverId, sectorId: selectedSectorId }, function (response) {
            if (!response || response.length === 0) {
                $('#liveMediaGalleryContainer').html(`
                <div class="col-12 text-center py-4 text-muted">
                    <i class="bx bx-images fs-2 mb-2 opacity-50"></i>
                    <p class="mb-0 small">No sub-media assets mapped to this specific cover yet.</p>
                </div>`);
                $('#galleryCounterBadge').text("0 Assets Found");
                return;
            }

            // 🌟 स्टेप 1: इनर मीडिया टेबल का आउटर स्ट्रक्चर और हेडर तैयार करें
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

            // 🌟 स्टेप 2: लूप चलाकर रो (tr) जनरेट करें
            response.forEach(function (item) {
                var innerId = item.innerMediaId || item.InnerMediaId;
                var mediaType = item.mediaType || "Image";
                var assetUrl = item.mediaAssetUrl || "";
                var fileName = assetUrl.split('/').pop();
                var notes = item.descriptionNotes || '<span class="text-muted fs-7 italic">No notes added</span>';

                // 🌟 मैनेज्ड और फिक्स साइज प्रीव्यू कंटेनर ($50px x $35px)
                var previewHtml = '';
                var badgeClass = 'bg-dark';

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
                    <span class="badge ${badgeClass} font-monospace px-2 py-1 small img-width-height" style="min-width: 55px;">${mediaType}</span>
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

            // 🌟 स्टेप 3: टेबल को क्लोज करके डोम में पुश करें
            html += `
                    </tbody>
                </table>
            </div>
        </div>`;

            $('#liveMediaGalleryContainer').html(html);
            $('#galleryCounterBadge').text(`${response.length} Assets Found`);
        });
    },

    // 🌟 POPULATE DROPDOWN BASED ON SELECTED SECTOR ONLY
    populateParentCoverDropdown: function () {
        var selectedSectorId = parseInt($('#InnerSector').val()) || 0;
        if (typeof common !== 'undefined' && common.BindDropdown) {
            // एंडपॉइंट पर सीधे sectorId भेजें
            common.BindDropdown(`/Management/GetActiveCoversBySector?sectorId=${selectedSectorId}`, "ParentCoverId", "Parent Canvas Layout", "");
        }
    },

    // =========================================================
    // MASTER SAVE CONFIGURATION
    // =========================================================
    saveMediaConfiguration: function () {
        var self = this;
        var selectedMode = $('#LayoutActionMode').val();
       
        var descriptionNotes = $('#SharedDescription').val().trim();

        
        var formData = new FormData();
        formData.append("SubmissionMode", selectedMode);
        formData.append("Description", descriptionNotes);
        

        if (selectedMode === "CoverPage") {
            var heading = $('#CoverHeading').val().trim();
            var orderNo = $('#CoverOrderNo').val().trim();
            var imgFile = $('#CoverImageFile')[0].files[0];
            var pdfFile = $('#CoverPdfFile')[0].files[0];
            var sectorId = $('#Sector').val();

            // 🌟 अब वैलिडेशन सेक्टर ड्रॉपडाउन पर लगेगा
            if (!sectorId || sectorId === "") {
                toast.showToast('Selection Required', 'Please select a valid target Sector before saving data.', 'error');
                return false;
            }


            if (!heading) {
                toast.showToast('Input Required', 'Please supply a main title header name for your cover layout.', 'error');
                return false;
            }
            if (!imgFile) {
                toast.showToast('File Required', 'An active primary banner cover image must be uploaded.', 'error');
                return false;
            }

            formData.append("SectorId", sectorId); // Sector Id अपेंड करें
            formData.append("Heading", heading);
            formData.append("OrderNumber", orderNo || "1");
            formData.append("CoverImage", imgFile);
            if (pdfFile) formData.append("DescriptionPdf", pdfFile);
        }
        else if (selectedMode === "InnerImage") {
            var parentId = $('#ParentCoverId').val();
            var mediaType = $('#MediaTypeSelector').val();
            var targetFileField = null;
            var InnersectorId = $('#InnerSector').val();

            // 🌟 अब वैलिडेशन सेक्टर ड्रॉपडाउन पर लगेगा
            if (!InnersectorId || InnersectorId === "") {
                toast.showToast('Selection Required', 'Please select a valid target Sector before saving data.', 'error');
                return false;
            }

            if (!parentId) {
                toast.showToast('Selection Required', 'Please map this asset component to a target parent cover.', 'error');
                return false;
            }
            formData.append("SectorId", InnersectorId); // Sector Id अपेंड करें
            formData.append("ParentCoverId", parentId);
            formData.append("MediaType", mediaType);

            if (mediaType === "Image") targetFileField = $('#InnerImageFile')[0];
            else if (mediaType === "Video") targetFileField = $('#InnerVideoFile')[0];
            else if (mediaType === "Audio") targetFileField = $('#InnerAudioFile')[0];

            if (targetFileField && targetFileField.files[0]) {
                formData.append("MediaFile", targetFileField.files[0]);
            } else {
                toast.showToast('Upload Required', `Please browse and append a valid file matching your chosen ${mediaType} selection.`, 'error');
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
            confirmButtonText: '<i class="bx bx-check-circle me-1"></i> Yes, Securely Save'
        }).then((result) => {
            if (result.isConfirmed) {
                if (typeof common !== 'undefined' && common.ShowLoader) common.ShowLoader();

                $.ajax({
                    url: '/Management/SaveDirectMediaShowcase',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (typeof common !== 'undefined' && common.HideLoader) common.HideLoader();

                        if (response.status || response.success) {
                            toast.showToast('Success', response.message || 'Media structural nodes compiled successfully.', 'success');
                            self.loadUploadedCovers();
                            self.resetFormControls();
                        } else {
                            toast.showToast('Transaction Rejected', response.message || 'System failed to parse properties cleanly.', 'error');
                        }
                    },
                    error: function () {
                        if (typeof common !== 'undefined' && common.HideLoader) common.HideLoader();
                        toast.showToast('Network Error', 'A critical network failure prevented database syncing.', 'error');
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

// jQuery $(document).ready की जगह native DOMContentLoaded का उपयोग
document.addEventListener('DOMContentLoaded', function () {
    if (typeof directMediaManager !== "undefined" && typeof directMediaManager.init === "function") {
        directMediaManager.init();
    }
});

// =======================================================
// ग्लोबल इवेंट बाइंडिंग्स (Event Delegation)
// =======================================================
document.addEventListener('click', function (event) {

    // 1. DELETE COVER PAGE EVENT
    const deleteCoverBtn = event.target.closest('.btn-delete-cover');
    if (deleteCoverBtn) {
        // jQuery के $(this).data('id') की जगह dataset.id का उपयोग
        var coverId = deleteCoverBtn.dataset.id;

        if (typeof directMediaManager !== "undefined" && typeof directMediaManager.executeMediaDeletion === "function") {
            directMediaManager.executeMediaDeletion('CoverPage', coverId, 0);
        }
        return; // मैच होने पर यहीं से बाहर निकलें
    }

    // 2. DELETE INNER MEDIA ASSET EVENT
    const deleteInnerBtn = event.target.closest('.btn-delete-inner');
    if (deleteInnerBtn) {
        // jQuery के .data() की जगह dataset प्रॉपर्टी का उपयोग
        var innerId = deleteInnerBtn.dataset.id;
        var parentId = deleteInnerBtn.dataset.parentId; // HTML में data-parent-id अपने आप parentId बन जाता है

        if (typeof directMediaManager !== "undefined" && typeof directMediaManager.executeMediaDeletion === "function") {
            directMediaManager.executeMediaDeletion('InnerImage', innerId, parentId);
        }
        return;
    }
});