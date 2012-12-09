/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator', 'app/order/view/OrderView'], function ($, _, Backbone, EA, OrderView) {
    return Backbone.View.extend({
        initialize: function () {
            this.collection.on('reset', this.addAllOrders, this);
        },
        render: function () {
            this.$el.empty();
            this.renderHeader();
            this.renderTable();
            this.addAllOrders();
            return this;
        },
        addAllOrders: function () {
            $('tbody', this.$el).empty();
            this.collection.forEach(function (item) {
                this.addOrderItem(item);
            }, this);
        },
        renderHeader: function () {
            this.$el.append("<h3>Order History</h3><hr/>");
        },
        renderTable: function () {
            this.$el.append("<table class='table table-striped table-hover'>\
                                <thead>\
                                    <tr>\
                                        <th>Date</th>\
                                        <th>Order Number</th>\
                                        <th class='number'>Amount</th>\
                                    </tr>\
                                </thead>\
                                <tbody>\
                                </tbody>\
                             </table>");
        },
        addOrderItem: function (item) {
            var view = new OrderView({ model: item });
            $('tbody',this.$el).append(view.render().el);
        }
    });
});