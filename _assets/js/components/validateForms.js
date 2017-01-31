var ValidateForms = (function($) {
    'use strict';

    var _hasVal = function($el, className) {
        var $parent = $el.parent();

        if ($el.val()) {
            if (!$parent.hasClass(className)) {
                $parent.addClass(className);
            }
        } else {
            if ($parent.hasClass(className)) {
                $parent.removeClass(className);
            }
        }

    };

    var _init = function($forms) {

        $forms.each(function() {
            var $this = $(this);

            $this.find('textarea,input').each(function() {
                _hasVal($(this), 'has-value');
            });
        });

        var generalValidateSettings = {
            onfocusout: function(element) {
                var $el = $(element);

                _hasVal($el, 'has-value');
                $el.valid();
            },
            rules: {
                Attachment: {
                    extension: "pdf|doc|docx",
                    filesize: 10485760
                }
            },
            messages: {
                Attachment: {
                    extension: "The file must be a pdf, doc or docx format under 10MB",
                    filesize: "Please select a file under 10MB"
                }
            },
        };

        var unobtrusiveSettings = {
            validClass: 'is-valid',
            errorClass: 'is-invalid',
            errorElement: "span",
            errorPlacement: function(error, element) {
                var $inputWrap = element.closest('.o-form__field');

                error.appendTo($inputWrap).addClass('o-validation u-small');
                $inputWrap.removeClass("is-valid");
                $inputWrap.addClass("is-invalid");
            },
            success: function(element) {
                $(element).closest(".o-form__field").removeClass("is-invalid").addClass("is-valid");
            },
        };

        // var settings = {
        //     debug: true,
        //     validClass: 'is-valid',
        //     errorClass: 'is-invalid',
        //     errorElement: "span",
        //     errorPlacement: function(error, element) {
        //         error.appendTo(element.closest('.o-form__field')).addClass('o-validation u-small');

        //         element.parent().addClass("is-invalid");
        //     },
        //     onfocusout: function(element) {
        //         var $el = $(element);

        //         _hasVal($el, 'has-value');
        //         $el.valid();
        //     },
        //     onclick: function(element) {
        //         $(element).valid();
        //         _hasVal($(element), 'has-value');
        //     },
        //     focusInvalid: true,
        //     // highlight: function(element, errorClass, validClass) {
        //     //     $(element).closest('.o-form__field').addClass("is-invalid").removeClass("is-valid");
        //     // },
        //     // unhighlight: function(element, errorClass, validClass) {
        //     //     $(element).closest('.o-form__field').removeClass("is-invalid").addClass("is-valid");
        //     // },
        //     success: function(element) {
        //         _hasVal($(element), 'has-value');
        //         $(element).closest(".o-form__field").removeClass("is-invalid").addClass("is-valid");
        //     },
        // };

        $.validator.addMethod('filesize', function(value, element, param) {
            return this.optional(element) || (element.files[0].size <= param);
        });


        $.validator.unobtrusive.options = unobtrusiveSettings;

        $.validator.setDefaults(generalValidateSettings);

        $("input[type='file']").on("change", function() {
            var $this = $(this);
            var isValid = $this.valid();
            var $attachmentError = $this.parent().next('.js-file-message');
            var $formWrap = $this.closest('.js-form__field--file');

            if (!isValid) {
                $formWrap.removeClass('is-valid').addClass('is-invalid');
                $attachmentError.html("");
            } else {
                var fileName = $this.val().replace(/^.*\\/, "");

                $attachmentError.html("File " + fileName + " has been selected");
            }
        });
    };

    return {
        init: _init
    };

})(jQuery);
