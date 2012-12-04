define(['jquery', 'underscore', 'backbone', 'bootbox', 'app/eventAggregator'], function ($, _, Backbone, bootbox, EA) {
    return Backbone.Model.extend({
        url: 'Order/AddItemToOrder',
        defaults: {
            ItemId: 0,
            Qty: 0,
            Price: 0,
            ItemCode: '',
            ItemName: '',
            UnitCode: ''
        },
        execute: function () {
            this.save({}, {
                cache: false,
                beforeSend: this.get('beforeSend'),
                complete: this.get('complete'),
                success: function (model, response, options) {
                    if (response.success === true) {
                        EA.trigger('order:addtoordersuccess', response);
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