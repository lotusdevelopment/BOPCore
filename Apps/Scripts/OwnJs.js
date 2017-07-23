$(function () {
    checkStorage();
    disableAutoComplete();
    ChangeCompanyProperties();
    AutoLogOut();
    InitDatePicker();
    localInfo();
    $lf.ss = $lf.parseString(localStorage.sessions);
    $("input[type=submit]").click(function () {
        $.each($("input,textarea,select").filter('[required]:visible'), function () {
            if (IsNullOrEmptyString($(this).val()))
                $(this).addClass("invalidNew");
            else
                $(this).addClass("validNew");
        });
    });
    $('select').each(function () {
        //$(this).removeAttr('selected');
    });
});

var $lf = {
    parseObject: function (obj) {
        if (!IsNullOrEmptyString(obj))
            return JSON.stringify(obj);
    },
    parseString: function (obj) {
        if (!IsNullOrEmptyString(obj))
            return JSON.parse(obj);
    }
}

var $lq = {
    first: function (list) {

    },
    where: function (list, data, field) {
        debugger;
    }
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function IsNullOrEmptyString(data) {
    if (data === "" || data === null || data === " " || data == undefined)
        return true;
    return false;
}

function IsNullOrEmptyArray(data) {
    var val = false;
    $(data).each(function (i, j) {
        if (IsNullOrEmptyString(j))
            val = true;
    });
    return val;
}

function IsNullOrEmpty(name, type) {
    var val = false;
    switch (type) {
        case "class":
            $("." + name).each(function (i, j) {
                if (IsNullOrEmptyString(j.value)) {
                    val = true;
                    $(this).addClass("invalidNew");
                } else
                    $(this).addClass("validNew");
            });
            break;
    }
    return val;
}

function IsValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function validatePhone(txtPhone) {
    var filter = /^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/;
    if (filter.test(txtPhone))
        return true;
    return false;
}

function BuildModal(title, content) {
    $("#panel-modal .panel-title").html(title);
    $("#panel-modal .panel-body").html(content);
    FireModal();
}

function BuildModal(title, content, button) {
    $("#con-close-modal .modal-title").html(title);
    $("#con-close-modal .modal-body").html(content);
    $("#con-close-modal .modal-footer").html(button);
    FireModal2();
}

function FireModal() {
    $("#btnModalFire").trigger("click");
}

function FireModal2() {
    $("#btnModalFire2").trigger("click");
}

function HideModal() {
    $('#panel-modal').modal('hide');
}

function HideModal2() {
    $('#con-close-modal').modal('hide');
}

function InitDatePicker() {
    try {
        $('.datepicker').datepicker({
            autoclose: true,
            todayHighlight: true,
            showDropdowns: true,
            opens: 'left',
            drops: 'down',
            buttonClasses: ['btn', 'btn-sm'],
            applyClass: 'btn-success',
            cancelClass: 'btn-default'
        });
    } catch (e) {
        console.log(e);
    }
}

var localInfo = function () {
    if (localStorage.sessions == null)
        localStorage.setItem('sessions', $lf.parseObject(LotusSessions()));
}

function LotusSessions() {
    var $ss = {};
    if (localStorage.sessions != null)
        $ss = $lf.parseString(localStorage.sessions);
    var rtnLotus = {
        setConnections: function () {
            $.ajax({
                url: "/Platform/GetCs",
                success: function (data) {
                    rtnLotus.cs = data[0];
                },
                dataType: "json",
                error: function (xhr) {
                    console.log($lf.parseObject(xhr));
                    alert("Error: we had a problem loading the app properties, please reload the page.");
                },
                async: false
            });
        },
        setUser: function () {
            $.ajax({
                url: "/Platform/GetUs",
                success: function (data) {
                    rtnLotus.us = data;
                },
                dataType: "json",
                error: function (xhr) {
                    console.log($lf.parseObject(xhr));
                    alert("Error: we had a problem loading the app properties, please reload the page.");
                },
                async: false
            });
        },
        getConnectionsUrl: function () {
            if (!("cs" in $ss) || $ss.cs === null)
                rtnLotus.setConnections();
            else
                if ($ss.cs.length === 0)
                    rtnLotus.setConnections();
        },
        getUser: function () {
            if (!("us" in $ss) || $ss.us === null)
                rtnLotus.setUser();
        }
    }
    rtnLotus.getConnectionsUrl();
    rtnLotus.getUser();
    return rtnLotus;
}

function AutoLogOut() {
    setTimeout(function () {
        //localStorage.clear();
        localStorage.removeItem("sessions");
        alert("Your session time expired, you will be logged out");
        $.post("/Account/LogOff", function (data) {
            localStorage.clear();
            window.location.href = "/";
        });
    }, 1800000);
}

function checkStorage() {
    if (Storage === void (0)) {
        alert("The current browser is not compatible with LotusApp, please update or change the web browser.");
        location.href = "https://www.lotusodonto.com/";
    }
}

function isValidPassword(password) {
    var validLength = /.{8}/.test(password);
    var hasCaps = /[A-Z]/.test(password);
    var hasNums = /\d/.test(password);
    var hasSpecials = /[~!,@#%&_\$\^\*\?\-]/.test(password);
    var isValidP = validLength && hasCaps && hasNums && hasSpecials;
    return isValidP;
}

function disableAutoComplete() {
    $("input[type=text]").attr("autocomplete", "new-password");
    $("input[type=password]").attr("autocomplete", "new-password");
    $("input[type=date]").attr("autocomplete", "new-password");
    $("input[type=tel]").attr("autocomplete", "new-password");
    $("input[type=email]").attr("autocomplete", "new-password");
    $("input[type=number]").attr("autocomplete", "new-password");
}

function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
}

function ChangeCompanyProperties() {
    var currentUrl = window.location.protocol + "//" + window.location.host;
    var already = false;
    $.get("../Content/extras/companies.json", function (data) {
        $.each(data.Companies, function (i, j) {
            $.each(j.url, function (k, l) {
                if (already) return;
                if (currentUrl == l) {
                    $(".branding-changing").html(j.name + '.');
                    $(".branding-img-changing").attr("src", j.logo);
                    $("#favicon").attr("href", j.favicon);
                    already = true;
                }
            });
        });
    });
}