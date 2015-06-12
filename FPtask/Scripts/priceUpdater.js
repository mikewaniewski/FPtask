$(function () {

    callPriceUpdate();

});



function ajaxCall() {

    $.ajax({
        method: "GET",
        url: $("#WebSocketURI").val(),
        data: {
            op: "0"
        },

        async: true,
        headers: {
            Accept: "application/json; charset=utf-8"
        },
        success: function (response) {

            displayData(response);
        },
        error: function (e) {
            stopProcessing();
        }

    });


}




function webSocketCall() {



    var ws = new WebSocket($("#WebSocketURI").val());
    ws.onopen = function () {

    };
    ws.onmessage = function (msg) {
        displayData(msg);
    };
    ws.onerror = function () {

    }

}



function callPriceUpdate() {


    if (Modernizr.websockets) {
        webSocketCall();

    }
    else {
        ajaxCall();
        setInterval(function () {
            ajaxCall();

        }, 15000);
    }

}


function stopProcessing() {

    $("#connected").val("0");

    $("#mainErrorNotifier").css('display', 'block');
    $("#mainErrorNotifier").html("Connection with Stock Exchange Server failed! All financial transactions are disabled!").effect("highlight", { color: '#6e3e7c' }, 500);

}


function displayData(msg) {


    $("#mainErrorNotifier").css('display', 'none');
    $("#mainErrorNotifier").html();

    GetWalletData();

    var jsonData = JSON.parse(msg.data);

    var c = 0;
    if ($("#PublicationDate").length > 0) {

        var d = new Date(jsonData["PublicationDate"]);


        $("#PublicationDate").text(d).effect("highlight", { color: '#6e3e7c' }, 500);
    }



    $.ajax({
        url: "/Operations/SaveHistoryPrices",
        type: "POST",
        data: JSON.stringify({ 'pricesJson': msg.data }),
        dataType: "json",
        traditional: true,
        contentType: "application/json; charset=utf-8"

    }).done(function (data) {



        $("#udpateResult").html(data["message"]);

        if (data["status"] === "Success") {
            $("#udpateResult").css('color', 'green').effect("highlight", { color: '#6e3e7c' }, 500);
        } else {
            $("#udpateResult").css('color', 'red').effect("highlight", { color: '#6e3e7c' }, 500);
        }

    });




    jsonData.Items.forEach(function () {

        $("#" + jsonData.Items[c]["Code"] + "_price").html(jsonData.Items[c]["Price"])
            .parent().parent().effect("highlight", { color: '#6e3e7c' }, 500);


        if ($("#TotalValue").length > 0) {
            if ($("#CurrentCode").val() == jsonData.Items[c]["Code"]) {

                $("#Price").val(jsonData.Items[c]["Price"]);

                $("#TotalValue").text(
                    parseFloat(
                    parseFloat($("#" + jsonData.Items[c]["Code"] + "_price").text()) * parseInt($("#TradedSharesAmount").val())
                    )
                ).effect("highlight", { color: '#6e3e7c' }, 500);
            }
        }

        c++;
    });


}


function GetWalletData() {

    if ($("#topWalletInfo").length > 0) {

        $.ajax({
            url: "/Account/GetWalletData",
            type: "POST",
            dataType: "json",

            traditional: true,
            contentType: "application/json; charset=utf-8"


        }).done(function (data) {

            $("#topWalletInfo").html(data["MoneyAvailable"]).effect("highlight", { color: '#6e3e7c' }, 500);

        });
    }

}