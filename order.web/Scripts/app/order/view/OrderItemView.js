/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator'], function ($, _, Backbone, EA) {
    return Backbone.View.extend({
        tagName: 'tr',
        initialize: function () {
            this.model.on('change', this.render, this);
        },
        render: function () {
            var template = _.template("<td class='number'><%= No %></td>\
                                       <td><%= ItemCode %></td>\
                                       <td><%= ItemName %></td>\
                                       <td class='number'><span><%= Qty %></span><span> <%= UnitCode %></span></td>\
                                       <td class='number currency'><%= Price %></td>\
                                       <td class='number currency'><%= AmountAfterDiscount %></td>");
            this.$el.html(template(this.model.toJSON()));
            $('.number.currency', this.$el).formatCurrency({ colorize: true, region: 'id-ID' });

            return this;
        }
    });
});