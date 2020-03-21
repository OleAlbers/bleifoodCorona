/**
 * Submitting Form
 */
jQuery(document).ready(function ($) {
		var debug = false; //show system errors
    var sendingMessage = 'Sending...';


    $('.simpleForm').submit(function () {
        var $f = $(this);
        var $submit = $f.find('input[type="submit"]');

        //prevent double click
        if ($submit.hasClass('disabled')) {
            return false;
        }

        $submit.attr('data-value', $submit.val()).val(sendingMessage).addClass('disabled');

        $.ajax({
            url: $f.attr('action'),
            method: 'post',
            data: $f.serialize(),
            dataType: 'json',
            success: function (data) {

                if (data.errors) {
                    // error
		                var $errorMsg = jQuery($f).parent().find(".errorMsg");
				            jQuery($f).fadeOut(300,function(){
					            $errorMsg.fadeIn();
				            });

                } else {
		                // success
		                var $successMsg = jQuery($f).parent().find(".successMsg");
				            jQuery($f).fadeOut(300,function(){
					            $successMsg.fadeIn();
				            });
                }

                $submit.val($submit.attr('data-value')).removeClass('disabled');
            },
            error: function (data) {
                if (debug) {
                    alert(data.responseText);
                }
                $submit.val($submit.attr('data-value')).removeClass('disabled');
            }
        });

        return false;
    });
});