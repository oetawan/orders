/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone','app/eventAggregator'], function ($, _, Backbone, EA) {
    return Backbone.View.extend({
        className: 'mediumListIconTextItem zain-listviewitem',
        template: _.template('<div class="mediumListIconTextItem-Detail">\
            <h3 class="zain-heading" aria-hidden="true" data-icon="="> <%= Name %> - <%= Code %></h3>\
            <span class="label label-info"><%= CurrencyId %> <%= Price %></span>\
            <span>/</span>\
            <span class="label label-info"><%= UnitCode %></span>\
            <div class="control-group">\
                <label class="control-label" for="inputQty">Qty</label>\
                <div class="controls">\
                    <input type="text" id="inputQty" value="1">\
                </div>\
            </div>\
            <div class="zain-action-group">\
                <button class="btn btn-primary">Add to order</button>\
            </div>\
          </div>'),
        events: {
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this;
        }
    });
});