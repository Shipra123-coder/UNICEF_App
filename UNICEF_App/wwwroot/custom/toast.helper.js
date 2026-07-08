window.toast = {
    showToast: function (heading, message, icon) {
        // 1. Forcefully remove any existing toast containers to reset the position
        $('.jq-toast-wrap').remove();

        // 2. Safe text parsing as handled before
        var toastText = message;
        if (typeof message === 'object' && message !== null) {
            toastText = message.message || message.Message || "Operation completed.";
        } else if (!message) {
            toastText = "No message details provided.";
        }

        // 3. Trigger fresh toast with top-right setup
        $.toast({
            heading: heading || 'Notification',
            text: toastText,
            icon: icon || 'info',
            position: 'bottom-right', // This will now work perfectly
            loaderBg: '#000',
            hideAfter: 4000,
            showHideTransition: 'slide'
        });
    }
};