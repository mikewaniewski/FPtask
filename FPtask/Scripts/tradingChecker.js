$(function () {

    var PurchasedAmountTextBox = $("#TradedSharesAmount");

   
    PurchasedAmountTextBox.keyup(function () {
        checkAmount();
    });

    function checkAmount() {

        var res = true;
        if (/^[0-9]+$/.test(PurchasedAmountTextBox.val()) == false) {
            PurchasedAmountTextBox.val("0");
            $("#AmountInfo").html("Only digits are allowed in this field");
            res = false;
        }

        if (parseInt($("#AmountAvailable").val()) < parseInt(PurchasedAmountTextBox.val())) {

            PurchasedAmountTextBox.val($("#AmountAvailable").val());
            $("#AmountInfo").html("You cannot purchase more shares than available");
            res = false;

        }

        callPriceUpdate();

        return res
    }


    $("#TradeButton").click(function () {
         
        var isAmountOk = checkAmount();


        if ($("#connected").val() === "1" && isAmountOk === true) {
            $("#TradeForm").submit();

        }  

    });


});


 