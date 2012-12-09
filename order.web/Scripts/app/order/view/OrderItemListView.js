/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator', 'app/order/view/OrderItemView'], function ($, _, Backbone, EA, OrderItemView) {
    return Backbone.View.extend({
        initialize: function () {
            this.collection.on('reset', this.addAllOrderItems, this);
        },
        render: function () {
            this.$el.empty();
            this.renderHeader();
            this.renderTable();
            this.addAllOrderItems();

            return this;
        },
        events: {
            'click a.back-button': 'back'
        },
        back: function () {
            this.options.parent.render();
        },
        addAllOrderItems: function () {
            $('tbody', this.$el).empty();
            var index = 0;
            this.collection.forEach(function (item) {
                item.set('No', ++index);
                this.addOrderItem(item);
            }, this);
        },
        renderHeader: function () {
            this.$el.append(_.template("<div style='position: relative;margin-bottom: 5px;'><h5><em>Order Detail: <%= OrderNumber %></em></h5><a class='back-button' href='#' aria-hidden='true' data-icon=''></a></div>", this.model.toJSON()));
            $('a.back-button', this.$el).css({'position': 'absolute', 'top':'0', 'right': '0', 'font-size': '24px'});
        },
        renderTable: function () {
            this.$el.append("<table class='table table-striped table-hover'>\
                                <thead>\
                                    <tr>\
                                        <th class='number'>No</th>\
                                        <th>Code</th>\
                                        <th>Name</th>\
                                        <th class='number'>Qty</th>\
                                        <th class='number'>Price</th>\
                                        <th class='number'>Total</th>\
                                    </tr>\
                                </thead>\
                                <tbody>\
                                </tbody>\
                             </table>");
        },
        addOrderItem: function (item) {
            var view = new OrderItemView({ model: item });
            $('tbody',this.$el).append(view.render().el);
        }
    });
});