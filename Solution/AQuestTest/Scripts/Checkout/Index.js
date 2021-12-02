var goBack = false;
$(document).ready(function () {

    $('#btnBackToUserInfo').on('click', function () {
        goBack = true;
    });

    $('#form-checkout').submit(function (e) {
        if (goBack) {
            return true;
        }

        if (e.target.id != "btnPayWithPaypal") {
            if (!$('#GeneralConditionsReaded').is(":checked")) {
            
                 $('#alert-general-contidionts-terms').show();
                 return false;
            }

            return true;
        }
    });
});
