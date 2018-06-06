

function ShowLoader(elementId) {
    debugger;
        $("#" + elementId).prepend("<div class='load-overlay'></div>");
    }

    function HideLoader() {
        $.each($(".load-overlay"), function (index, item) {
            item.remove();
        });
    }
