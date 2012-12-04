/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator','app/order/view/ShoppingCartItemView'], function ($, _, Backbone, EA, ShoppingCartItemView) {
    return Backbone.View.extend({
        tagName: 'table',
        className: 'table table-striped table-hover table-checkout',
        render: function () {
            this.addAll();
            $('td.currency', this.$el).formatCurrency({ colorize: true, region: 'id-ID' });
            return this;
        },
        addAll: function () {
            this.$el.empty();
            this.renderHeader();
            this.$el.append('<tbody></tbody>');
            _.each(this.model.get('Items'), function (item) {
                this.addItem(item);
            }, this);
        },
        renderHeader: function () {
            var html = "<thead>\
                            <tr>\
                            <td>Item Code</td>\
                            <td>Item Name</td>\
                            <td class='number'>Qty</td>\
                            <td>Unit</td>\
                            <td class='number'>Price</td>\
                            <td class='number'>Total Amount</td>\
                            </tr>\
                        </thead>";
            this.$el.append(html);
        },
        addItem: function (item) {
            var scItemModel = new Backbone.Model(item);
            var view = new ShoppingCartItemView({ model: scItemModel });
            $('tbody', this.$el).append(view.render().el);
        }
    });
});