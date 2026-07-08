var toast = {
    showToast: function (heading,msg,icon) {
        'use strict';
        toast.resetToastPosition();
        $.toast({
            heading: heading,
            text: msg,
            showHideTransition: 'slide',
            icon: icon,
            loaderBg: '#f96868',
            position: 'top-right' // Changed from 'mid-center' to 'top-right'
        })
    },
    resetToastPosition:function () {
        $('.jq-toast-wrap').removeClass('bottom-left bottom-right top-left mid-center'); // to remove previous position class
        $(".jq-toast-wrap").css({
            "top": "",
            "left": "",
            "bottom": "",
            "right": ""
        }); //to remove previous position style
    }
}