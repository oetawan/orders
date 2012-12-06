define(['jquery', 'underscore', 'backbone', 'bootbox', 'app/eventAggregator'], function ($, _, Backbone, bootbox, EA) {
    return Backbone.Model.extend({
        url: 'Order/RemoveItem',
        defaults: {
            ItemId: 0
        },
        execute: function () {
            var success = this.get('success') || function (model, response, options) {
                if (response.success === true) {
                    EA.trigger('order:removeorderitemsuccess', response);
                } else {
                    bootbox.modal(response.errorMessage, 'Error');
                }
            };
            this.save({}, {
                cache: false,
                beforeSend: this.get('beforeSend'),
                complete: this.get('complete'),
                success: success,
                error: function (model, xhr, options) {
                    bootbox.modal(xhr.responseText, 'Error');
                }
            });
        }
    });
});