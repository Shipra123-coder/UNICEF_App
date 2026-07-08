var group = {
    ToggleActive : function(guid)
    {
    alert("in");
    var data = {
        guid: guid,
    }
    Swal.fire({
        title: 'Are you sure?',
        text: "Are you sure you want to change status?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
    }).then((result) => {
        if (result.isConfirmed) {
            common.ShowLoader();
            ajax.doPostAjax(`${domain.getdomain()}/GroupMaster/ToggleActive`, data, function (r) {
                common.HideLoader();
                if (r.status) {
                    location.reload();
                    toast.showToast('success', r.message, 'success');
                }
                else {
                    toast.showToast('error', r.message, 'error');
                }
            });
        }
    });
}
}