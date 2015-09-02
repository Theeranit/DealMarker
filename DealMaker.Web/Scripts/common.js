function set_options(strName, strFirstOption, strFunction, callbackFn) {

    $.ajax({
        type: "POST",
        url: window.location.pathname + "/" + strFunction, //'<%= Page.ResolveClientUrl("FIEntryInfo.aspx/' + strFunction + '" ) %>',
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            strSelected = "";
            $("#" + strName).get(0).options.length = 0;
            if (strFirstOption != null)
                $("#" + strName).get(0).options[0] = new Option(strFirstOption, "-1");

            $.each(msg.d.Options, function (index, obj) {
                $("#" + strName).get(0).options[$("#" + strName).get(0).options.length] = new Option(obj.DisplayText, obj.Value);
                if (obj.Default != null && obj.Default == true)
                    strSelected = obj.Value;
            });

            if (strSelected != "")
                $("#" + strName).val(strSelected);

            if (callbackFn != null)
                callbackFn();
        },
        error: function () {
            $("#" + strName).get(0).options.length = 0;
            if (strFirstOption != null)
                $("#" + strName).get(0).options[0] = new Option(strFirstOption, "-1");
        }
    });
}

function Export2CSV() {
    $.ajax({
        type: "POST",
        url: window.location.pathname + "/Export2CSV",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            
        },
        error: function () {
            
        }
    });
}