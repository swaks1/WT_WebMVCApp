

function ShowLoader(selector) {
    //id
    $("#" + selector).prepend("<div class='load-overlay'></div>");

    //class
    $.each($("." + selector), function (index, item) {
        $(item).prepend("<div class='load-overlay'></div>");
    });
}

function HideLoader() {
    $.each($(".load-overlay"), function (index, item) {
        item.remove();
    });
}
