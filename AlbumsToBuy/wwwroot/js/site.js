$(document).ready(function () {
    $(".shop_album").on("click", function () {
        window.location = "/Shop/Album/" + $(this).attr("value");
    })

    $(".show_order_delivered_checkbox").on("click", function () {
        var urlParams = new URLSearchParams(document.location.search);

        if (urlParams.has("showDelivered")) {
            urlParams.delete("showDelivered");
        }
        else {
            urlParams.append("showDelivered", true);
        }

        document.location.search = urlParams.toString();
    })
})

function SetPageIndex(index_updater) {
    var urlParams = new URLSearchParams(document.location.search);

    var page = urlParams.get("page");
    if (page === null) {
        page = 1;
    }

    var value = (parseInt(page) + index_updater).toString();
    urlParams.set("page", value);

    document.location.search = urlParams.toString();
}