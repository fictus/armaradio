$(document).ready(function () {
    $("#dvPopupUpdateSessionCookie").modal();

    $("#lnkUpdateSessionCookie").on("click", function (e) {
        e.preventDefault();

        armaradio.masterPageWait(true);

        armaradio.masterAJAXGet({}, "Home", "GetSessionCookie")
            .then(function (response) {
                $("#txtSessionCookie").val(response.cookie || "");

                armaradio.masterPageWait(false);

                $("#dvPopupUpdateSessionCookie").modal("show");
            });
    });

    $("#btnUpdateSessionCookie").on("click", function () {
        let cookieVal = $.trim($("#txtSessionCookie").val());

        if (cookieVal == "") {
            armaradio.warningMsg({
                msg: "Cookie cannot be Null!",
                captionMsg: "Cookie Error",
                typeLayout: "red"
            });

            return;
        }

        armaradio.masterPageWait(true);

        armaradio.masterAJAXPost({
            Cookie: cookieVal
        }, "Home", "SaveSessionCookie")
            .then(function (response) {
                $("#dvPopupUpdateSessionCookie").modal("hide");
                $("#txtSessionCookie").val("");

                armaradio.masterPageWait(false);
            });
    });
});