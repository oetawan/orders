/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator','app/order/view/ShoppingCartItemView'], function ($, _, Backbone, EA, ShoppingCartItemView) {
    return Backbone.View.extend({
        className: 'row-fluid',
        events: {
            'click button.checkout-command': 'checkout'
        },
        initialize: function () {
            this.model.on('change', function () {
                this.showAmount();
            }, this);
        },
        render: function () {
            this.renderContainer();
            this.renderTable();
            this.showAmount();
            this.addAllItems();
            $('.currency', this.$el).formatCurrency({ colorize: true, region: 'id-ID' });
            return this;
        },
        renderContainer: function () {
            var html = '<div class="span8 checkout-view-table"></div>\
                        <div class="span4">\
                            <div class="checkout-view-amount"></div>\
                            <div class="checkout-view-action"><button class="btn btn-large btn-success checkout-command">Checkout</button></div>\
                        </div>';
            this.$el.html(html);
        },
        renderTable: function () {
            var html = "<table class='table table-striped table-hover table-checkout'>\
                            <thead>\
                                <tr>\
                                    <td>No</td>\
                                    <td>Item Code</td>\
                                    <td>Item Name</td>\
                                    <td class='number'>Qty</td>\
                                    <td></td>\
                                    <td>Unit</td>\
                                    <td class='number'>Price</td>\
                                    <td class='number'>Sub Total</td>\
                                    <td></td>\
                                </tr>\
                            </thead>\
                            <tbody></tbody>\
                        </table>";
            $('div.checkout-view-table', this.$el).html(html);
        },
        showAmount: function () {
            var html = _.template('<h2 class="currency" style="color: green;"><%= TotalAmountAfterDiscount %></h2>', this.model.toJSON());
            $('div.checkout-view-amount', this.$el).html(html);
            $('.currency', this.$el).formatCurrency({ colorize: true, region: 'id-ID' });
        },
        addAllItems: function () {
            var index = 1;
            $('div.checkout-view-table tbody', this.$el).empty();
            _.each(this.model.get('Items'), function (item) {
                item.Index = index++;
                this.addItem(item);
            }, this);
        },
        addItem: function (item) {
            var scItemModel = new Backbone.Model(item);
            var view = new ShoppingCartItemView({ model: scItemModel, shoppingCart: this.model });
            $('div.checkout-view-table tbody', this.$el).append(view.render().el);
        },
        checkout: function () {
            var html = $(_.template('<div class="well checkout-view-confirm">' +
                                    '<h2>Total ' + this.model.get('Items').length + ' item(s):</h2><h2 id="total-amount"><%= TotalAmountAfterDiscount %></h2>' +
                                    '</div><div class="confirm-checkout-label"><i>Click <b>OK</b> to checkout, or <b>Cancel</b> to continue shopping</i></div>', this.model.toJSON()));
            $('#total-amount', html).formatCurrency({ colorize: true, region: 'id-ID' });
            bootbox.confirm(html, function (isOK) {
                
            });
        }
    });
});