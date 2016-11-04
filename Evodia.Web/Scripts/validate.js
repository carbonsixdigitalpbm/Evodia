(function($) {
    'use strict';

    var validateForms = function() {
        var $forms = $('.js-form');

        $forms.each(function() {
            $(this).validate({
                validClass: 'is-valid',
                errorClass: 'is-invalid',
                errorElement: "span",
                errorPlacement: function(error, element) {
                    if (element.is('select')) {
                        error.appendTo(element.closest('.o-form__field')).addClass('o-validation u-small');
                    } else {
                        error.appendTo(element.closest('.o-form__field')).addClass('o-validation u-small');
                    }
                },
                onfocusout: function(element) {
                    $(element).valid();
                },
                onclick: function(element) {
                    $(element).valid();
                    $('.content').css("height", "auto");
                },
                focusInvalid: true,
                rules: {
                    Name: "required",
                    Email: {
                        required: true,
                        email: true
                    },
                    Message: "required"
                },
                messages: {

                    Name: "Please enter your name",
                    Email: {
                        required: "Please enter your email address",
                        email: "Please enter a valid email address"
                    },
                    Message: "Please enter your message"
                },
                highlight: function(element, errorClass, validClass) {
                    $(element).closest('.o-form__field').addClass(errorClass).removeClass(validClass);
                },
                unhighlight: function(element, errorClass, validClass) {
                    $(element).closest('.o-form__field').removeClass(errorClass).addClass(validClass);
                }
            });
        });
    };

    /* ===========================================================
    # Init
    =========================================================== */

    validateForms();

})(jQuery);
