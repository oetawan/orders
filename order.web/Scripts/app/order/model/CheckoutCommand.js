define(['jquery', 'underscore', 'backbone', 'bootbox', 'app/eventAggregator'], function ($, _, Backbone, bootbox, EA) {
    return Backbone.Model.extend({
        url: 'Order/Checkout',
        execute: function () {
            this.save({}, {
                cache: false,
                beforeSend: function () {
                    $('body').mask('Please wait checking out...');
                },
                complete: function () {
                    $('body').unmask();
                },
                success: function (model, response, options) {
                    if (response.success === true) {
                        EA.trigger('order:checkout-success', response);
                    } else {
                        bootbox.modal(response.errorMessage, 'Error');
                    }
                },
                error: function (model, xhr, options) {
                    bootbox.modal(xhr.responseText, 'Error');
                }
            });
        }
    });
});