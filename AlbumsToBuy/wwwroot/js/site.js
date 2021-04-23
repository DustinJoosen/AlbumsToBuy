$(document).ready(function () {
    $(".shop_album").on("click", function () {
        window.location = "/Shop/Album/" + $(this).attr("value");
    })
})