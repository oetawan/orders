/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator'], function ($, _, Backbone, EA) {
    return Backbone.View.extend({
        tagName: 'tr',
        template: _.template('<td><%= ItemCode %></td>\
                   <td><%= ItemName %></td>\
                   <td class="number"><%= Qty %></td>\
                   <td><%= UnitCode %></td>\
                   <td class="currency"><%= Price %></td>\
                   <td class="currency"><%= AmountAfterDiscount %></td>'),
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this;
        }
    });
});