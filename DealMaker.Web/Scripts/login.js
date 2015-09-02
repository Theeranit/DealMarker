
$(document).ajaxStop($.unblockUI);

$(document).ready(function () {
    
    //$("#login-form").submit(function () {
    
    $("#login-button").click(function () {
        var isFormValid = true;

        $("#login-form .required").each(function () { // Note the :text
            if ($.trim($(this).val()).length == 0) {
                //$(this).parent().addClass("highlight");
                $(this).parent().children('label').addClass("highlight")
                isFormValid = false;
            } else {
                $(this).parent().children('label').removeClass("highlight");
            }
        });
        if (!isFormValid)
            alert("Please fill in all the required fields (highlighted in red)");
        else {

            $.blockUI({ message: '<h3><img src="Images/busy.gif" /> Just a moment...</h3>' });

            logon();
            //location.replace("main.aspx");
        }
    });

    $('.input').keypress(function (e) {
        if (e.which == 13) {
            $(this).blur();
            $('#login-button').focus().click();
        }
    });
});

function blockPage() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff' 
        },
        message: '<h1><img src="Images/busy.gif" /> Just a moment...</h1>'
    });
    $('.blockOverlay').attr('title', 'Click to unblock').click($.unblockUI); 
}

function unblockPage() {
    $.unblockUI();
}

function logon() {
    var _userName = $("#loginusername").val();
    var _password = encodePass($("#loginpassword").val());

    var _pdata = { username: _userName, password: _password };

    $.ajax(
    {
        Type: "GET",
        contentType: "application/xml; charset=utf-8",
        dataType: "xml",
        url: "Services/Logon.asmx/LogOn",
        data: _pdata,
        success: function (data) {
            logoncallback(data);
        },
        error: function (data) {
        }
    });
}

function logoncallback(xml) {
    var xmlDoc = $.parseXML(xml.childNodes[0].textContent),
    $xml = $(xmlDoc),
    $title = $xml.find("res_msg");
    $code = $xml.find("res_code");
    //alert($title.text());
    //alert($code.text());
    if ($code.text() == "101") {
        //alert($code.text());
        window.location.assign("Main.aspx?type=logon");
//        var href = $(this).children('a').first().attr('href');
//        var host = window.location.hostname;
//        window.location = 'http://' + host + '/' + href;
        return false;
        //GotoMainPage();
        //location.replace("Main.aspx?type=logon");
    }
    else {
        var $errorlabel = $("#error-label"), $infolabel = $("#info-label");
        $infolabel.hide();

        $errorlabel.text($title.text());
        $errorlabel.show();
    }
}

function GotoMainPage() {
    window.location.replace("Main.aspx?type=logon");
    return false;
}

function WaitingLogon() {
    $.blockUI({ message: '<h3><img src="Images/busy.gif" /> Just a moment...</h3>' });
    return false;
}
