// Write your Javascript code.

function  checkValue() {
    var element = document.getElementById('DocumentInput');
}

(function () {

    $(":file").change(function () {
       // alert("The text has been changed.");
        var obj = this;

        //Validate just txt types
        if (obj.value.lastIndexOf(".txt") == -1) {

            alert("Please upload only .txt extention file");
        }
        else {
            if (obj.value != "" || obj.value != null) {
                $('#upload').show();

            }
            else {
                $('#upload').hide();
            }
        }
        
    });



}());