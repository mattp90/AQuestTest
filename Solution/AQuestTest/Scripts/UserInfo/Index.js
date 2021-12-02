var goBack = false;
$(document).ready(function () {
    
    $('#btnBackToOrderDetail').on('click', function () {
        goBack = true;
    });

    $('#form-user-info').submit(function () {
        if (goBack) {
            return true;
        }

        if (!$('#PrivacyPolicyReaded').is(":checked")) {

            $('#alert-privacy-policy').show();
            return false;
        }
        return true;            
    });
});
