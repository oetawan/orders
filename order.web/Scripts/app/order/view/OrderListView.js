/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator', 'app/order/view/OrderView','app/order/model/OrderList'], function ($, _, Backbone, EA, OrderView, OrderList) {
    return Backbone.View.extend({
        initialize: function () {
            this.collection.on('reset', this.addAllOrders, this);
            this.collection.on('add', this.addOrderItem, this);
            this.count = 0;
            this.fetchingOrderCount = null;
            this.fetchOrderCount();
        },
        events: {
            'click .next-page-button': 'fetchOrder'
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
            this.showNextPageIfNecessary();
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
        },
        fetchOrderCount: function () {
            var that = this;
            if (this.fetchingOrderCount)
                this.fetchingOrderCount.abort();

            this.fetchingOrderCount = $.ajax('/Order/Count', {
                dataType: 'json',
                cache: false,
                beforeSend: function () {
                    $(this.el).mask("Fetching order history...");
                },
                complete: function () {
                    that.fetchingOrderCount = null;
                    $(that.el).unmask();
                },
                success: function (data) {
                    that.count = data.count;
                },
                error: function (xhr, status, err) {
                    bootbox.alert(xhr.responseText);
                }
            });
        },
        showNextPageIfNecessary: function () {
            $('div.next-page-button-container', this.$el).remove();
            if (this.collection.length < this.count) {
                this.$el.append('<div class="next-page-button-container"><a href="#" class="next-page-button" aria-hidden="true" data-icon=""></a></div>');
            }
        },
        fetchOrder: function () {
            var that = this;
            new OrderList().fetch({
                update: true,
                remove: false,
                data: {
                    skip: this.collection.length,
                    pageSize: 10
                },
                beforeSend: function () {
                    $(that.el).mask("Fetching order history...");
                },
                complete: function () {
                    $(that.el).unmask();
                },
                success: function (data) {
                    data.forEach(function (item) {
                        that.collection.add(item);
                    }, this);
                    that.showNextPageIfNecessary();
                },
                error: function (model, xhr) {
                    bootbox.alert(xhr.responseText);
                }
            });
        }
    });
});