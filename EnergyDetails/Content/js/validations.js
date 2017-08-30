//$(document).ajaxStart(function () {
//    createLoaderPOP();
//});
//$(document).ajaxStop(function () {
//    closeLoaderPOP();
//});

$(document).ajaxError(function (event, request, settings) {
    if ((request.status == "401")) {
        var strHTML = "Whooops! Session expired! :( Please re-login to continue! <br/><br/>" +
                    "<input name=\"\" type=\"button\" value=\"Login Again\" class=\"btn-orng1\" onclick=\"javascript:createLoginPOP();\">";
        createGenericErrPOP(strHTML);

        $('#btnGenericOK, .ui-dialog-titlebar-close').hide();
        //window.location.href = "../../Home/logoff/500";
    }
    else if (request.status == "1205") {
        var customError = "We are unable to process this request at the moment. Please try again after some time. (This is not an error.)";
        createGenericErrPOP(customError);
    }
    else {
        createGenericErrPOP(request.status);
    }
});

function createLoginPOP() {
    $("#popErrGeneric").dialog('close');
    $('#btnGenericOK, .ui-dialog-titlebar-close').show();

    var rlDiv = document.getElementById("popLogin");
    var objDiv = null;
    if (rlDiv == null) {
        var strHTML = "<div id=\"popLogin\" class=\"popError sucBorder\" style=\"display: none;\">" +
                        "<div class=\"header sucMsg\" style=\"margin-bottom:10px;\"><img src=\"../../content/images/iconCheck.png\" alt=\"\">Login</div>" +
                        "<div style=\"line-height:45px;\">" +
                            "Please login to your account to access requested content. <br/>"+
                            "Username: &nbsp;&nbsp;<input id=\"txtPOPUsername\" type=\"text\" class=\"txtFieldType1\" /><br/>" +
                            "Password:&nbsp;&nbsp;&nbsp; <input id=\"txtPOPPassword\" type=\"password\" class=\"txtFieldType1\" />" +
                        "</div>" +
                        "<p class=\"clrMrgnBottom\" style=\"padding:10px 0px;\">" +
                            "<input id=\"btnPOPLogin\" type=\"button\" value=\"Login\" class=\"btn-orng1\" onclick=\"javascript:loginInternal();\"></p>" +
                       "</div>";
        objDiv = document.createElement('div');
        objDiv.setAttribute('id', 'LoginWrapper');
        objDiv.style.display = "none";
        objDiv.innerHTML = strHTML;
        document.body.appendChild(objDiv);
    }

    $("#popLogin").dialog({
        closeOnEscape: false,
        modal: true,
        width: 425
        /*,
        buttons: { 
        Ok: function () { $(this).dialog("close"); }
        }*/
    });
    $('.ui-dialog-titlebar-close').hide();
}

function loginInternal() {
    var uname = $.trim($('#txtPOPUsername').val());
    var pass = $.trim($('#txtPOPPassword').val());

    if (uname != "" && pass != "") {
        $.ajax({
            url: "/home/userLoginInternal",
            async: true,
            type: "POST",
            cache: false,
            dataType: "json",
            data: { username: uname, password: pass },
            success: function (resp) {
                if (!resp.success) {
                    if (resp.result == "0")
                        createGenericErrPOP("Invalid username or password.");
                    else
                        createGenericErrPOP("Whooops! Something went wrong :( Try again! <br/><br/>Error detail: " + resp.msg);
                }
                else {
                    var url = window.location.href;
                    window.location = url;
                }
            }
        });
    } 
}




function createErrPOP(strMsg) {
    var rlDiv = document.getElementById("popErrList");
    var objDiv = null;
    if (rlDiv == null) {
        var strHTML = "<div id=\"popErrList\" class=\"popError errBorder\" style=\"display: none;\">" +
                        "<div class=\"header\"><img src=\"../../content/images/iconError.png\" alt=\"\">Error</div>" +
                        "<div id=\"popErrListMsg\" class=\"errorList clear\">" + strMsg + "</div>" +
                        "<p class=\"txtRight clrMrgnBottom\">" +
                            "<input name=\"\" type=\"button\" value=\"Close\" class=\"btn-orng1\" onclick=\"javascript:$('#popErrList').dialog('close');\"></p>" +
                       "</div>";
        objDiv = document.createElement('div');
        objDiv.setAttribute('id', 'RLWrapper');
        objDiv.style.display = "none";
        objDiv.innerHTML = strHTML;
        document.body.appendChild(objDiv);
    }
    else {
        $('#popErrListMsg').html('');
        $('#popErrListMsg').html(strMsg);
    }

    $("#popErrList").dialog({
        closeOnEscape: false,
        modal: true,
        width: 425
        /*,
        buttons: { 
        Ok: function () { $(this).dialog("close"); }
        }*/
    });
}

function createGenericErrPOP(strMsg) {
    var rlDiv = document.getElementById("popErrGeneric");
    var objDiv = null;
    if (rlDiv == null) {
        var strHTML = "<div id=\"popErrGeneric\" class=\"popError errBorder\" style=\"display: none;\">" +
                        "<div class=\"header\"><img src=\"../../content/images/iconError.png\" alt=\"\">Error</div>" +
                        "<p id=\"popGenericMsg\">" + strMsg + "</p>" +
                        "<p><input id=\"btnGenericOK\" type=\"button\" value=\"Close\" class=\"btn-orng1\" onclick=\"javascript:$('#popErrGeneric').dialog('close');\" /></p>" +
                      "</div>";
        objDiv = document.createElement('div');
        objDiv.setAttribute('id', 'RLWrapper');
        objDiv.style.display = "none";
        objDiv.innerHTML = strHTML
        document.body.appendChild(objDiv);
    }
    else {
        $('#popGenericMsg').html('');
        $('#popGenericMsg').html(strMsg);
    }
    $("#popErrGeneric").dialog({
        closeOnEscape: false,
        modal: true,
        width: 425
    });
}

function createSuccessPOP(strMsg) {
    var rlDiv = document.getElementById("popSuccess");
    var objDiv = null;
    if (rlDiv == null) {
        var strHTML = "<div id=\"popSuccess\" class=\"popError sucBorder\" style=\"display: none;\">" +
                            "<div class=\"header sucMsg\"><img src=\"../../content/images/iconCheck.png\" alt=\"\">Success</div>" +
                            "<p id=\"popSucMsg\">" + strMsg + "</p>" +
                            "<p><input name=\"\" id=\"btnSuccessOK\" type=\"button\" value=\"Close\" class=\"btn-orng1\" onclick=\"javascript:$('#popSuccess').dialog('close');\"></p>" +
                      "</div>";
        objDiv = document.createElement('div');
        objDiv.setAttribute('id', 'RLWrapper');
        objDiv.style.display = "none";
        objDiv.innerHTML = strHTML;
        document.body.appendChild(objDiv);
    }
    else {
        $('#popSucMsg').html(strMsg);
    }
    $("#popSuccess").dialog({
        closeOnEscape: false,
        modal: true,
        width: 425
    });
}

//function createLoaderPOP() {
//    var rlDiv = document.getElementById("popLoader");
//    var objDiv = null;
//    if (rlDiv == null) {
//        var strHTML = "<div style='text-align:center; padding:15px;'>Please wait...<br/><img src='../../content/images/FSRPreloader.gif' alt='Please wait...' /></div>";
//        objDiv = document.createElement('div');
//        objDiv.setAttribute('id', 'popLoader');
//        objDiv.style.display = "none";
//        //objDiv.setAttribute('overflow', 'hidden');
//        objDiv.innerHTML = strHTML;
//        document.body.appendChild(objDiv);
//    }

//    $("#popLoader").dialog({
//        closeOnEscape: false,
//        resizable: false,
//        modal: true,
//        width: 180,
//        height: 100
//    });
//    $('.ui-dialog-titlebar').hide();
//    $('#popLoader').css('overflow', 'hidden');
//    $('#popLoader').css('height', 'auto');
//}

//function closeLoaderPOP(){
//    $("#popLoader").dialog('close');
//}