define(['jquery', 'underscore', 'backbone', 'bootbox', 'app/eventAggregator'], function ($, _, Backbone, bootbox, EA) {
    return Backbone.Model.extend({
        url: 'Order/ChangeQty',
        defaults: {
            ItemId: 0,
            Qty: 0
        },
        execute: function () {
            this.save({}, {
                cache: false,
                beforeSend: this.get('beforeSend'),
                complete: this.get('complete'),
                success: this.get('success'),
                error: function (model, xhr, options) {
                    bootbox.modal(xhr.responseText, 'Error');
                }
            });
        }
    });
});