var mpermission = {
    getPermission: function () {
        var groupId = $("#GroupId").val();
        if (groupId == -1) {
            $("#errorgroupId").text("Please select a role");
            return;
        }
        $("#errorgroupId").text("");

        var model = {
            GroupId: groupId
        }
        ajax.doPostAjax(`GetGroupPermissions`, model, function (r) {
            console.log(r);
            common.HideLoader();
            mpermission.buildTreeView(r);
        });

        //$.ajax({
        //    url: "/api/GetMenuPermissions?roleId=" + roleId,
        //    type: "GET",
        //    dataType: "json",
        //    success: function (data) {
        //        mpermission.buildTreeView(data);
        //    },
        //    error: function () {
        //        alert("Error fetching menu permissions.");
        //    }
        //});
    },

    buildTreeView: function (menuData) {
        let html = `<ul class="tree-root">`;
        html += mpermission.buildTreeNodes(menuData);
        html += `</ul>`;

        $("#treeContainer").html(html);
    },
    buildTreeNodes: function (nodes) {
        let html = "";

        nodes.forEach(node => {

            let hasChildren = node.children && node.children.length > 0;

            // safe checked values
            let isChecked = node.isChecked ? 'checked' : '';
            let canAdd = node.canAdd ? 'checked' : '';
            let canList = node.canList ? 'checked' : '';
            let canEdit = node.canEdit ? 'checked' : '';
            let canDelete = node.canDelete ? 'checked' : '';
            let canActive = node.canActiveDeactive ? 'checked' : '';

            html += `
        <li>
            <div class="tree-item">

                ${hasChildren ? `<button class="toggle-btn">+</button>` : ""}

                <input type="checkbox" 
                       class="tree-checkbox" 
                       data-id="${node.id}" ${isChecked}>

                <label>${node.name}</label>

                <div class="permission-box">

                    <label>
                        <input type="checkbox" class="perm-add" data-id="${node.id}" ${canAdd}>
                        Add
                    </label>

                    <label>
                        <input type="checkbox" class="perm-list" data-id="${node.id}" ${canList}>
                        List
                    </label>

                    <label>
                        <input type="checkbox" class="perm-edit" data-id="${node.id}" ${canEdit}>
                        Edit
                    </label>

                    <label>
                        <input type="checkbox" class="perm-delete" data-id="${node.id}" ${canDelete}>
                        Delete
                    </label>

                    <label>
                        <input type="checkbox" class="perm-active" data-id="${node.id}" ${canActive}>
                        Active/DeActive
                    </label>

                </div>

            </div>

            ${hasChildren ? `
                <ul class="child-list">
                    ${mpermission.buildTreeNodes(node.children)}
                </ul>
            ` : ""}

        </li>
        `;
        });

        return html;
    },

    //buildTreeNodes: function (nodes) {
    //    let html = "";
    //    nodes.forEach(node => {
    //        let hasChildren = node.children && node.children.length > 0;
    //        let isChecked = node.isChecked ? 'checked="checked"' : '';

    //        html += `
    //        <li>
    //            <div class="tree-item">
    //                ${hasChildren ? `<button class="toggle-btn">+</button>` : ""}
    //                <input type="checkbox" class="tree-checkbox" data-id="${node.id}" ${isChecked}>
    //                <label>${node.name}</label>
                    
    //                <!-- Permission checkboxes -->
    //                <div class="permission-box">
    //                    <label><input type="checkbox" class="perm-add" data-id="${node.id}" ${node.canAdd ? 'checked' : ''}> Add</label>
    //                    <label><input type="checkbox" class="perm-list" data-id="${node.id}" ${node.canList ? 'checked' : ''}> List</label>
    //                    <label><input type="checkbox" class="perm-edit" data-id="${node.id}" ${node.canEdit ? 'checked' : ''}> Edit</label>
    //                    <label><input type="checkbox" class="perm-delete" data-id="${node.id}" ${node.canDelete ? 'checked' : ''}> Delete</label>
    //                    <label><input type="checkbox" class="perm-active" data-id="${node.id}" ${node.canActiveDeactive ? 'checked' : ''}> Active/DeActive</label>
    //                </div>
    //            </div>
    //            ${hasChildren ? `<ul class="child-list">${mpermission.buildTreeNodes(node.children)}</ul>` : ""}
    //        </li>
    //    `;
    //    });
    //    return html;
    //},


    //buildTreeNodes: function (nodes) {
    //    let html = "";
    //    console.log(nodes);
    //    nodes.forEach(node => {
    //        let hasChildren = node.children && node.children.length > 0;
    //        let isChecked = node.isChecked ? 'checked="checked"' : '';  // Assuming node has `isChecked` property

    //        html += `
    //            <li>
    //                <div class="tree-item">
    //                    ${hasChildren ? `<button class="toggle-btn">+</button>` : ""}
    //                    <input type="checkbox" class="tree-checkbox" data-id="${node.id}" ${isChecked}>
    //                    <label>${node.name}</label>

    //                </div>
    //                ${hasChildren ? `<ul class="child-list">${mpermission.buildTreeNodes(node.children)}</ul>` : ""}
    //            </li>
    //        `;
    //    });
    //    return html;
    //},

    //saveFinal: function () {
    //    let selectedMenus = [];
    //    $(".tree-checkbox:checked").each(function () {
    //        selectedMenus.push($(this).data("id"));
    //    });

    //    let groupId = $("#GroupId").val();

    //    var model = {
    //        GroupId: groupId,
    //        menuIds : selectedMenus
    //    }

    //    console.log(model);
    //    //common.ShowLoader();
    //    Swal.fire({
    //        title: 'Are you sure?',
    //        text: "Are you sure You want to save!",
    //        icon: 'warning',
    //        showCancelButton: true,
    //        confirmButtonColor: '#3085d6',
    //        cancelButtonColor: '#d33',
    //        confirmButtonText: 'Yes',
    //        cancelButtonText: 'No',
    //    }).then((result) => {
    //        if (result.isConfirmed) {
    //            common.ShowLoader();
    //            ajax.doPostAjax(`saveGroupPermissions`, model, function (r) {
    //                console.log(r);
    //                common.HideLoader();
    //                if (r.status) {
    //                    toast.showToast('success', r.message, 'success');
    //                    setTimeout(function () {
    //                        mpermission.getPermission();
    //                    }, 2000);
    //                }
    //                else {
    //                    toast.showToast('error', r.message, 'error');

    //                }
    //            });
    //        }
    //    });
    //},

    saveFinal: function () {

        let permissions = [];

        // Loop through every menu row
        $(".tree-item").each(function () {

            let menuId = $(this).find(".tree-checkbox").data("id");

            // Parent menu selected or not
            let isParentChecked = $(this).find(".tree-checkbox").is(":checked");

            // Get child permissions
            let permAdd = $(this).find(".perm-add").is(":checked");
            let permList = $(this).find(".perm-list").is(":checked");
            let permEdit = $(this).find(".perm-edit").is(":checked");
            let permDelete = $(this).find(".perm-delete").is(":checked");
            let permActive = $(this).find(".perm-active").is(":checked");

            // Only push if parent menu OR any permission selected
            if (isParentChecked || permAdd || permList || permEdit || permDelete || permActive) {

                permissions.push({
                    MenuId: menuId,
                    CanAdd: permAdd,
                    CanList: permList,
                    CanEdit: permEdit,
                    CanDelete: permDelete,
                    CanActiveDeactive: permActive
                });
            }
        });

        let groupId = $("#GroupId").val();

        var model = {
            GroupId: groupId,
            Permissions: permissions
        };

        console.log("Final Model => ", model);

        Swal.fire({
            title: 'Are you sure?',
            text: "You want to save group permissions?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doPostAjax(`saveGroupPermissions`, model, function (r) {
                    common.HideLoader();
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(() => mpermission.getPermission(), 1500);
                    } else {
                        toast.showToast('error', r.message, 'error');
                    }
                });
            }
        });
    },

};
document.addEventListener('DOMContentLoaded', function () {
    // Toggle tree nodes
    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('toggle-btn')) {
            var btn = e.target;
            var childList = btn.parentElement.nextElementSibling; // assuming sibling structure
            if (childList && childList.classList.contains('child-list')) {
                if (childList.style.display === 'none' || childList.style.display === '') {
                    childList.style.display = 'block';
                } else {
                    childList.style.display = 'none';
                }
            }
            btn.classList.toggle('open');
            btn.textContent = btn.textContent.trim() === '-' ? '+' : '-';
        }
    });

    // Parent-Child Checkbox Behavior
    document.addEventListener('change', function (e) {
        if (e.target.classList.contains('tree-checkbox')) {
            var checkbox = e.target;
            var isChecked = checkbox.checked;

            // Handle children
            var li = checkbox.closest('li');
            if (li) {
                var childCheckboxes = li.querySelectorAll('.child-list .tree-checkbox');
                childCheckboxes.forEach(function (child) {
                    child.checked = isChecked;
                });
            }

            // Handle parent
            if (!isChecked) {
                // Uncheck parent if all siblings are unchecked
                var parentUl = checkbox.closest('ul');
                if (parentUl) {
                    var allUnchecked = parentUl.querySelectorAll('.tree-checkbox:checked').length === 0;
                    if (allUnchecked) {
                        var parentLi = parentUl.closest('li');
                        if (parentLi) {
                            var parentCheckbox = parentLi.querySelector('.tree-item .tree-checkbox');
                            if (parentCheckbox) {
                                parentCheckbox.checked = false;
                            }
                        }
                    }
                }
            } else {
                // Check parent if this checkbox is checked
                var parentUl = checkbox.closest('ul');
                if (parentUl) {
                    var parentLi = parentUl.closest('li');
                    if (parentLi) {
                        var parentCheckbox = parentLi.querySelector('.tree-item .tree-checkbox');
                        if (parentCheckbox) {
                            parentCheckbox.checked = true;
                        }
                    }
                }
            }
        }
    });
});

//// Toggle tree nodes
//$(document).on("click", ".toggle-btn", function () {
//    $(this).parent().siblings(".child-list").slideToggle();
//    $(this).toggleClass("open");
//    $(this).text($(this).text() === "-" ? "+" : "-");
//});

//// Parent-Child Checkbox Behavior
//$(document).on("change", ".tree-checkbox", function () {
//    var isChecked = $(this).prop("checked");

//    // If a parent checkbox is checked/unchecked, check/uncheck all its children
//    $(this).closest("li").find(".child-list .tree-checkbox").prop("checked", isChecked);

//    // If a child checkbox is checked, check its parent checkbox
//    if (!isChecked) {
//        // If unchecked, check if any sibling checkboxes are still checked
//        $(this).closest("ul").each(function () {
//            var allUnchecked = $(this).find(".tree-checkbox:checked").length === 0;
//            if (allUnchecked) {
//                $(this).closest("li").children(".tree-item").find(".tree-checkbox").prop("checked", false);
//            }
//        });
//    } else {
//        // If checked, make sure the parent checkbox is also checked
//        $(this).closest("ul").closest("li").children(".tree-item").find(".tree-checkbox").prop("checked", true);
//    }
//}); 
