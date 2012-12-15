define([
    'underscore',
    'backbone',
    'app/eventAggregator'],
function (_, Backbone, EA) {

        return Backbone.View.extend({
            initialize: function () {
            },
            render: function () {
                var html = '<form class="form-horizontal">\
                                <div class="control-group">\
                                    <label class="control-label" for="inputOldPassword">Old password</label>\
                                    <div class="controls">\
                                        <input type="password" id="inputOldPassword">\
                                    </div>\
                                </div>\
                                <div class="control-group">\
                                    <label class="control-label" for="newPassword">New password</label>\
                                    <div class="controls">\
                                        <input type="password" id="newPassword">\
                                    </div>\
                                </div>\
                                <div class="control-group">\
                                    <label class="control-label" for="confirmPassword">Confirm new password</label>\
                                    <div class="controls">\
                                        <input type="password" id="confirmPassword">\
                                    </div>\
                                </div>\
                                <div class="control-group">\
                                    <label class="control-label" for="confirmPassword"></label>\
                                    <div class="controls">\
                                        <a href="#" class="btn btn-success change-password-button">Change password</a>\
                                    </div>\
                                </div>\
                            </form>';
                this.$el.html(html);
                
                return this;
            },
            events: {
                'click a.change-password-button': 'changePassword'
            },
            validate: function () {
                /*$('div.input-error').remove();

                var oldPasswordInput = $('input#oldpassword');
                var passwordInput = $('input#password');
                var confirmPasswordInput = $('input#confirmPassword', this.$el);

                var validOldPassword = $.trim(oldPasswordInput.val()) !== "";
                var validPassword = $.trim(passwordInput.val()) !== "";
                var validConfirmPassword = $.trim(confirmPasswordInput.val()) !== "";
                var confirmPasswordMatch = $.trim(passwordInput.val()) === $.trim(confirmPasswordInput.val());

                if (!validOldPassword)
                    oldPasswordInput.after('<div class="input-error">Old password cannot be blank</div>');
                if (!validPassword)
                    passwordInput.after('<div class="input-error">Password cannot be blank</div>');
                if (!confirmPasswordMatch)
                    confirmPasswordInput.after("<div class='input-error'>Password doesn't match</div>");

                var isValid = validOldPassword && validPassword && validConfirmPassword && confirmPasswordMatch;

                if (!isValid) {
                    $('div.help-inline').css('display', 'none');
                } else {
                    $('div.help-inline').css('display', 'block');
                }
                return isValid;*/
            },
            changePassword: function (e) {
                e.preventDefault();
                var that = this;
                var data = {
                    OldPassword: $('input#inputOldPassword', this.$el).val(),
                    NewPassword: $('input#newPassword', this.$el).val(),
                    ConfirmPassword: $('input#confirmPassword', this.$el).val()
                };
                $.ajax('Account/ChangePassword', {
                    dataType: 'json',
                    type: 'post',
                    cache: 'false',
                    'data': data,
                    beforeSend: function () {
                        that.$el.mask("Changing password...");
                    },
                    complete: function () {
                        that.$el.unmask();
                    },
                    success: function (data) {
                        if (data.success === true) {
                            bootbox.alert('<div class="checkout-success-alert" aria-hidden="true" data-icon=""><h3>Password changed.</h3></div>');
                            $('a.close', this.$el).click();
                        } else {
                            if (data.errorMessage) {
                                var html = $('<div class="error-panel"></div>');
                                html.html(data.errorMessage);
                                bootbox.alert(html);
                            } else if (data.errors) {
                                var html = $('<div class="error-panel"><ul></ul></div>');
                                _.each(data.errors, function (err) {
                                    html.append("<li class='error-list-item'>" + err + "</li>");
                                }, this);
                                bootbox.alert(html);
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        var html = $('<div class="error-panel"></div>');
                        html.html(errorThrown);
                        bootbox.alert(html);
                    }
                });
            }
        });
});