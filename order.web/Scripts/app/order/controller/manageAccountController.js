define(['jquery',
        'underscore',
        'backbone',
        'app/order/view/ChangePasswordView'],
function ($, _, Backbone, ChangePasswordView) {

    return function () {
        var showChangePasswordForm = function (e) {
            e.preventDefault();
            var changePasswordView = new ChangePasswordView();
            changePasswordView.render();
            var box = bootbox.modal(changePasswordView.el, 'Change password');
            changePasswordView.options.parent = box;
        }

        var start = function () {
            $('button.btn-change-password').click(showChangePasswordForm);
        }

        return {
            start: start
        }
    }
});