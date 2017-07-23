(function () {
    $(function () {
        $("#AppsList .switchery").click(function () {
            var active = $(this).parent().find(".appChanger");
            if (active.is(":checked")) {
                if (active.attr("disabled")) return;
                else $.post("/platform/ChangeApp?appid=" + active.data("appid"), function () { window.location.href = "/"; });
            } return;
        });
    });
})()