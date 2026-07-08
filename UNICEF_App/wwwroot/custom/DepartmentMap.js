var departmentMaster = {
    saveDepartmentMapping: function () {
        var status = true;

        // 1. Activity ID Validation (Pichle step se milna zaroori hai)
        var ActivityId = $("#currentActivityId").val();
        if (!ActivityId || ActivityId === "0") {
            toast.showToast('error', 'Main Activity not found. Please save Step 1 first.', 'error');
            return false;
        }

        // 2. Nodal Department Validation
        var NodalDept = $("#nodalDept").val();
        if (!NodalDept || NodalDept === "") {
            $('#nodalDept').addClass('errr-highlight').removeClass('sucess-highlight');
            status = false;
        } else {
            $('#nodalDept').removeClass('errr-highlight').addClass('sucess-highlight');
        }

        // 3. Collect Supporting Departments from Chips
        // Hum un IDs ko collect karenge jo selectedDepts array ya chips ke data-id mein hain
        var SupportingDepts = [];
        $(".btn-remove-dept").each(function () {
            var deptId = $(this).attr("data-id");
            if (deptId) {
                SupportingDepts.push(deptId);
            }
        });

        // Agar validation fail hota hai
        if (!status) {
            toast.showToast('error', 'Please select the Nodal Department', 'error');
            return false;
        }

        // 4. Final Model Construction
        var model = {
            ActivityId: ActivityId,
            NodalDepartment: NodalDept,
            SupportingDepartments: SupportingDepts // Yeh List<string> ki tarah controller mein jayega
        };

        console.log("Department Mapping Model:", model);

        // 5. Swal Confirmation
        Swal.fire({
            title: 'Confirm Mapping?',
            text: "Do you want to map these departments to the activity?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Map it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();

                // 6. AJAX Post Call
                ajax.doPostAjax(`SaveDepartmentMapping`, model, function (r) {
                    common.HideLoader();

                    if (r.status || r.success) {
                        toast.showToast('success', r.message || 'Departments mapped successfully!', 'success');

                        // Wizard Navigation: Move to Step 3 (Sector)
                        setTimeout(function () {
                            $("#content2").removeClass("active").hide();
                            $("#content3").addClass("active").show();

                            $("#s2").addClass("completed").removeClass("active");
                            $("#s3").addClass("active");

                            // Scroll to top for better UX
                            window.scrollTo(0, 0);
                        }, 1000);

                    } else {
                        toast.showToast('error', r.message || 'Error occurred while mapping', 'error');
                    }
                });
            }
        });
    }
};

// --- Event Listener ---
$(document).on("click", "#mapDepartment", function (e) {
    e.preventDefault();
    departmentMaster.saveDepartmentMapping();
});