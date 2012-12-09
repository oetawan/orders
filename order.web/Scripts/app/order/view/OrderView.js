/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/model/OrderItemList', 'app/order/view/OrderItemListView', 'app/order/view/Progress', 'app/eventAggregator'], function ($, _, Backbone, OrderItemList, OrderItemListView, Progress, EA) {
    return Backbone.View.extend({
        tagName: 'tr',
        initialize: function () {
            var that = this;
            this.model.on('change', this.render, this);
            this.orderItemList = new OrderItemList();
        },
        events: {
            'click a': 'showDetail'
        },
        render: function () {
            var template = _.template("<td><a href='#'><%= OrderDateString %></a></td>\
                                       <td><a href='#'><%= OrderNumber %></a></td>\
                                       <td class='number'><a href='#'><%= TotalAmountAfterDiscount %></a></td>");
            this.$el.html(template(this.model.toJSON()));
            $('td.number', this.$el).formatCurrency({ colorize: true, region: 'id-ID' });

            return this;
        },
        showDetail: function () {
            var that = this;
            var progress = new Progress();
            this.$el.html("<td colspan='3'><div class='detail-container well'></div></td>");
            var orderItemListView = new OrderItemListView({
                model: this.model, collection: this.orderItemList, parent: that
            });
            $('div.detail-container', this.$el).html(orderItemListView.render().el);
            
            this.orderItemList.fetch({
                beforeSend: function () {
                    $('div.detail-container', this.$el).append(progress.render().el);
                },
                complete: function () {
                    $('div.progress', this.$el).remove();
                },
                error: function (model, xhr) {
                    showError(new Backbone.Model({ errorMessage: xhr.statusText + " (" + xhr.status + ")" }));
                },
                async: false,
                data: {
                    orderId: this.model.get("Id")
                }
            });
        }
    });
});