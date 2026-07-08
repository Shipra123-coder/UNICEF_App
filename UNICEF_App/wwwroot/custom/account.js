var account = {
    login: function () {
        var flag = true;
        if ($("#Username").val() == '') {
            $('span[for="Username"]').text('Username required');
            flag = false;
        }
        if ($("#Password").val() == '') {
            $('span[for="Password"]').text('Password required');
            flag = false;
        }
        if (!flag) {
            return false;
        }

        const password = $("#Password").val();
        //const hashedPass = account.hashPasswordSHA256(password);
        //var encryptedPass = account.encryptPassword($("#Password").val());
        const salted = $("#Password").val(); //+ $('#nonce_id').val();
        const hashedPass = account.sha256(salted);
        var postData = {
            username: $("#Username").val(),
            password: hashedPass,
        };
        $(`#btn-login`).html('');
        $(`#btn-login`).html('<i class="fa fa-circle-o-notch fa-spin" style="font-size:24px"></i>');
        $(`#btn-login`).attr('disabled', true);
        common.ShowLoader();

        $.ajax({
            type: "POST",
            url: `/Account/SignIn`,
            data: postData,
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            // Add `no-referrer` directive
            referrerPolicy: "no-referrer", // Prevents sending Referer header
            success: function (data) {
                console.log(data);
                common.HideLoader();
                try {
                    if (data.isAuthenticate == true) {

                        //var returnUrl = $('#hdnReturnUrl').val();
                        //toast.showToast('success', data.message, 'success');
                        //if (returnUrl == "") {
                        //returnUrl = `/Indicator/Dashboard`;
                        //}
                        setTimeout(function () { location.href = "/Indicator/Dashboard" }, 3000);

                    }
                    else {
                        toast.showToast('error', data.message, 'error');
                        $(`#btn-login`).html('');
                        $(`#btn-login`).html('Log In');
                        $(`#btn-login`).attr('disabled', false);
                    }

                } catch (e) {

                }


            }
        });
    },
    ClearVal: function (name) {
        $(`span[for= "${name}"]`).text('');
    },   
    sha256: function (input) {
        return CryptoJS.SHA256(input).toString(CryptoJS.enc.Hex);
    },
}

