$(function () {
    //flag = true;
    for (i = 0; i < $('.reg_no').length; i++) {
        $($('.reg_no')[i]).html(i + 1);

        //if (i % 5 == 0) 
        //	flag = !flag;
        //if (flag)
        //	$($('.reg_no')[i]).closest('tr').attr("style", "background-color: aliceblue;");

        //if (i % 2 == 1)
        //	$($('.reg_no')[i]).closest('tr').attr("style", "background-color: aliceblue;");
    }

    for (i = 0; i < $('.cancel_no').length; i++) {
        $($('.cancel_no')[i]).html(i + 1);
    }   
});

function copySmsRecipient() {
    $('#clipBoard').select();

    if (document.execCommand('copy')) {
    }
    else {
        alert("IE에서 시도해 주세요!");
    }
}
