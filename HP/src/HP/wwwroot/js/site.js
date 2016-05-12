// Write your Javascript code.

function  checkValue() {
    var element = document.getElementById('DocumentInput');
}

(function () {

    $(":file").change(function () {
       // alert("The text has been changed.");
        var obj = this;

        if (obj.value != "" || obj.value != null) {
            $('#upload').show();
            
        }
        else {
            $('#upload').hide();
        }
        
    });
}());