var mmenu = {
    saveMenu: function () {
        var status = true;

        var Name = $('#Name').val();
        if (Name.length <= 0) {
            $('label[for="error"]').show();
            errorName.textContent = "Please enter name";
            $('#errorName').addClass('colordiv');
            $('#Name').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Name').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorName').html('');
        }

        var MangalName = $('#MangalName').val();
        if (MangalName.length <= 0) {
            $('label[for="error"]').show();
            errorMangalName.textContent = "Please enter mangal name";
            $('#errorMangalName').addClass('colordiv');
            $('#MangalName').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#MangalName').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorMangalName').html('');
        }

        var Controller = $('#Controller').val();
        if (Controller.length <= 0) {
            $('label[for="error"]').show();
            errorController.textContent = "Please enter controller name";
            $('#errorController').addClass('colordiv');
            $('#Controller').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Controller').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorController').html('');
        }

        var ActionName = $('#ActionName').val();
        if (ActionName.length <= 0) {
            $('label[for="error"]').show();
            errorActionName.textContent = "Please enter action name";
            $('#errorActionName').addClass('colordiv');
            $('#ActionName').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#ActionName').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorActionName').html('');
        }

        var OrderNumber = $('#OrderNumber').val();
        if (OrderNumber.length <= 0) {
            $('label[for="error"]').show();
            errorOrderNumber.textContent = "Please enter order number";
            $('#errorOrderNumber').addClass('colordiv');
            $('#OrderNumber').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#OrderNumber').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorOrderNumber').html('');
        }

        var Icon = $('#Icon').val();
        if (Icon.length <= 0) {
            $('label[for="error"]').show();
            errorIcon.textContent = "Please enter icon";
            $('#errorIcon').addClass('colordiv');
            $('#Icon').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Icon').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorIcon').html('');
        }

        var IsActive = $("#IsActive").prop("checked") ? true : false;

        var Guid = $('#Guid').val();
        //alert(checked);       
        if (!status) {
            return false;
        }

        var model = {
            Guid: Guid,
            Name: Name,
            MangalName: MangalName,
            Controller: Controller,
            ActionName: ActionName,
            Icon: Icon,
            OrderNumber: OrderNumber,
            IsActive: IsActive
        }
        console.log(model);
        //common.ShowLoader();
        Swal.fire({
            title: 'Are you sure?',
            text: "Are you sure You want to save!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doPostAjax(`saveMenu`, model, function (r) {
                    console.log(r);
                    common.HideLoader();
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(function () { window.location.href = `Menu`; }, 2000);
                        //setTimeout(function () {
                        //    compnay.getCompanyProfile();
                        //}, 2000);                
                    }
                    else {
                        toast.showToast('error', r.message, 'error');

                    }
                });
            }
        });
    },
    saveSubMenu: function () {
        var status = true;

        var HeaderMenu = $('#HeaderMenu option:selected').val();
        if (HeaderMenu == "") {
            $('label[for="error"]').show();
            errorHeaderMenu.textContent = "Please header menu";
            $('#errorHeaderMenu').addClass('colordiv');
            $('#HeaderMenu').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#HeaderMenu').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorHeaderMenu').html('');
        }

        var Name = $('#Name').val();
        if (Name.length <= 0) {
            $('label[for="error"]').show();
            errorName.textContent = "Please enter name";
            $('#errorName').addClass('colordiv');
            $('#Name').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Name').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorName').html('');
        }

        var MangalName = $('#MangalName').val();
        if (MangalName.length <= 0) {
            $('label[for="error"]').show();
            errorMangalName.textContent = "Please enter mangal name";
            $('#errorMangalName').addClass('colordiv');
            $('#MangalName').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#MangalName').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorMangalName').html('');
        }

        var Controller = $('#Controller').val();
        if (Controller.length <= 0) {
            $('label[for="error"]').show();
            errorController.textContent = "Please enter controller name";
            $('#errorController').addClass('colordiv');
            $('#Controller').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Controller').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorController').html('');
        }

        var ActionName = $('#ActionName').val();
        if (ActionName.length <= 0) {
            $('label[for="error"]').show();
            errorActionName.textContent = "Please enter action name";
            $('#errorActionName').addClass('colordiv');
            $('#ActionName').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#ActionName').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorActionName').html('');
        }

        var OrderNumber = $('#OrderNumber').val();
        if (OrderNumber.length <= 0) {
            $('label[for="error"]').show();
            errorOrderNumber.textContent = "Please enter order number";
            $('#errorOrderNumber').addClass('colordiv');
            $('#OrderNumber').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#OrderNumber').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorOrderNumber').html('');
        }

        var Icon = $('#Icon').val();
        if (Icon.length <= 0) {
            $('label[for="error"]').show();
            errorIcon.textContent = "Please enter icon";
            $('#errorIcon').addClass('colordiv');
            $('#Icon').removeClass('sucess-highlight').addClass('errr-highlight');
            status = false;
        }
        else {
            $('#Icon').removeClass('errr-highlight').addClass('sucess-highlight');
            $('#errorIcon').html('');
        }

        var IsActive = $("#IsActive").prop("checked") ? true : false;

        var Guid = $('#Guid').val();
        //alert(checked);       
        if (!status) {
            return false;
        }

        var model = {
            Guid: Guid,
            HeaderMenu: HeaderMenu,
            Name: Name,
            MangalName: MangalName,
            Controller: Controller,
            ActionName: ActionName,
            Icon: Icon,
            OrderNumber: OrderNumber,
            IsSubMenu: 1,
            IsActive: IsActive
        }
        console.log(model);
        //common.ShowLoader();
        Swal.fire({
            title: 'Are you sure?',
            text: "Are you sure You want to save!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doPostAjax(`saveSubMenu`, model, function (r) {
                    console.log(r);
                    common.HideLoader();
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(function () { window.location.href = `SubMenu`; }, 2000);
                        common.BindDropdown("BindDropDown_HeaderMenu", "HeaderMenu", "Header Menu", $('#hiddenHeaderMenu').val());
                        //setTimeout(function () {
                        //    compnay.getCompanyProfile();
                        //}, 2000);                
                    }
                    else {
                        toast.showToast('error', r.message, 'error');

                    }
                });
            }
        });
    },
    getdata: function () {
        //var GoalId = $('#Goal option:selected').val();
        var data = {
            //option1: GoalId
        };
        table.BindPostTable(`MenudataList`, 'data_list', data);
    },
    MenuIsActive: function (id) {
        //alert("in");
        Swal.fire({
            title: 'Are you sure?',
            text: "Are you sure You want to change status!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doGetAjax(`IsActiveMenu?MenuId=` + id, function (r) {
                    common.HideLoader();
                    console.log(r);
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(function () {
                            var data = {};
                            table.BindPostTable(`/Menu/MenudataList`, 'data_list', data);                           
                        }, 2000);                                  
                    }
                    else {
                        toast.showToast('error', r.message, 'error');
                    }
                });
            }
        });
    },
    SubMenuIsActive: function (id) {
        //alert("in");
        Swal.fire({
            title: 'Are you sure?',
            text: "Are you sure You want to change status!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doGetAjax(`IsActiveMenu?MenuId=` + id, function (r) {
                    common.HideLoader();
                    console.log(r);
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(function () {
                            var data = {};                            
                            table.BindPostTable(`/Menu/SubMenudataList`, 'data_list', data);
                        }, 2000);                                  
                    }
                    else {
                        toast.showToast('error', r.message, 'error');
                    }
                });
            }
        });
    },
    Delete: function (guid) {
        //alert("in");
        Swal.fire({
            title: 'Are you sure?',
            text: "Are you sure You want to delete!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doPostAjax(`DeleteMenu?guid=` + guid, function (r) {
                    common.HideLoader();
                    console.log(r);
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(function () {
                            var data = {};
                            table.BindPostTable(`/Menu/MenudataList`, 'data_list', data);
                        }, 2000);
                        //setTimeout(function () {
                        //    compnay.getCompanyProfile();
                        //}, 2000);                
                    }
                    else {
                        toast.showToast('error', r.message, 'error');
                    }
                });
            }
        });
    },
    SubDelete: function (guid) {
        //alert("in");
        Swal.fire({
            title: 'Are you sure?',
            text: "Are you sure You want to delete!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
        }).then((result) => {
            if (result.isConfirmed) {
                common.ShowLoader();
                ajax.doPostAjax(`DeleteMenu?guid=` + guid, function (r) {
                    common.HideLoader();
                    console.log(r);
                    if (r.status) {
                        toast.showToast('success', r.message, 'success');
                        setTimeout(function () {
                            var data = {};
                            table.BindPostTable(`/Menu/SubMenudataList`, 'data_list', data);
                        }, 2000);
                        common.HideLoader();
                    }
                    else {
                        common.HideLoader();
                        toast.showToast('error', r.message, 'error');
                    }
                });
            }
        });
    },
}

document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('delete-submenu')) {
            e.preventDefault();            
            const guid = e.target.getAttribute('data-guid');
            if (guid) {
                mmenu.SubDelete(guid);
            }
        }

        if (e.target.classList.contains('edit-submenu')) {
            e.preventDefault();
            if (!confirm("Are you sure you want to activate this record?")) return;
            const guid = e.target.getAttribute('data-guid');
            if (guid) {
                console.log("Redirecting to:", `${domain.getdomain()}/Menu/SubMenu?Guid=${guid}`);
                window.location.href = `${domain.getdomain()}/Menu/SubMenu?Guid=${guid}`;
            }
        }

        if (e.target.classList.contains('save-submenu')) {
            e.preventDefault();
            //if (!confirm("Are you sure you want to save this record?")) return;
            mmenu.saveSubMenu();
        }
    });

    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('delete-menu')) {
            e.preventDefault();
            //if (!confirm("Are you sure you want to delete this record?")) return;
            const guid = e.target.getAttribute('data-guid');
            if (guid) {
                mmenu.Delete(guid);
            }
        }

        if (e.target.classList.contains('edit-menu')) {
            e.preventDefault();
            if (!confirm("Are you sure you want to activate this record?")) return;
            const guid = e.target.getAttribute('data-guid');
            if (guid) {
                console.log("Redirecting to:", `${domain.getdomain()}/Menu/Menu?Guid=${guid}`);
                window.location.href = `${domain.getdomain()}/Menu/Menu?Guid=${guid}`;
            }
        }

        if (e.target.classList.contains('save-menu')) {
            e.preventDefault();
            //if (!confirm("Are you sure you want to save this record?")) return;
            mmenu.saveMenu();
        }
    });

    document.addEventListener('change', function (e) {
        if (e.target.classList.contains('submenu-active-toggle')) {
            const menuId = e.target.getAttribute('data-id');
            const isChecked = e.target.checked;
            mmenu.SubMenuIsActive(menuId, isChecked);
        }
        if (e.target.classList.contains('menu-active-toggle')) {
            const menuId = e.target.getAttribute('data-id');
            const isChecked = e.target.checked;
            mmenu.MenuIsActive(menuId, isChecked);
        }
    });
});

