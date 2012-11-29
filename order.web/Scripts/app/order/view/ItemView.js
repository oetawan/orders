/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'bootbox','app/eventAggregator'], function ($, _, Backbone, bootbox, EA) {
    return Backbone.View.extend({
        className: 'mediumListIconTextItem zain-listviewitem',
        template: _.template('<div class="mediumListIconTextItem-Detail">\
            <h3 class="zain-heading" aria-hidden="true" data-icon="="> <%= Name %> - <%= Code %></h3>\
            <span class="label label-info"><%= CurrencyId %> <%= Price %></span>\
            <span>/</span>\
            <span class="label label-info"><%= UnitCode %></span>\
            <div class="control-group inputqty-container">\
                <label class="control-label" for="inputQty">Qty</label>\
                <div class="controls">\
                    <input type="number" id="inputQty" value="1">\
                </div>\
            </div>\
            <div class="zain-action-group">\
                <button class="btn btn-primary add-to-order">Add to order</button>\
            </div>\
          </div>'),
        events: {
            'click button.add-to-order': 'addToOrder'
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this;
        },
        addToOrder: function () {
            this.model.set('qty', parseInt($('#inputQty', this.$el).val()));
            EA.trigger('order:additem', this.model);
        }
    });
});