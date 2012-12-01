/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery',
        'underscore',
        'backbone',
        'bootbox',
        'app/eventAggregator',
        'app/order/model/AddToOrderCommand'],
function ($, _, Backbone, bootbox, EA, AddToOrderCommand) {
    return Backbone.View.extend({
        className: 'mediumListIconTextItem zain-listviewitem',
        template: _.template('<div class="mediumListIconTextItem-Detail">\
            <h3 class="zain-heading" aria-hidden="true" data-icon="="> <%= Name %> - <%= Code %></h3>\
            <span class="label label-info"><%= CurrencyId %> <%= Price %></span>\
            <span>/</span>\
            <span class="label label-info"><%= UnitCode %></span><br/>\
          </div>'),
        events: {
            'click button.add-to-order': 'addToOrder'
        },
        initialize: function () {
            this.options.shoppingCart.on('change', function () {
                this.render();
            }, this);
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            this.renderOrderItem();
            return this;
        },
        renderOrderItem: function () {
            var that = this;
            var orderItem = _.find(this.options.shoppingCart.get('Items'), function (itm) {
                return itm.ItemId === that.model.get('Id');
            });
            var html;
            if (orderItem) {
                html = _.template('<div class="control-group input-container">\
                                    <label class="control-label" for="inputQty">Qty</label>\
                                    <div class="controls">\
                                        <input type="number" id="inputQty" value="<%= Qty %>">\
                                    </div>\
                                   </div>\
                                   <div class="control-group input-container">\
                                    <label class="control-label" for="inputPrice">Price</label>\
                                    <div class="controls">\
                                        <input type="text" id="inputPrice" class="format-currency" readonly="readonly" value="<%= Price %>">\
                                    </div>\
                                   </div>\
                                   <div class="control-group input-container">\
                                    <label class="control-label" for="inputAmount">Amount</label>\
                                    <div class="controls">\
                                        <input type="text" id="inputAmount" class="format-currency" readonly="readonly" value="<%= AmountAfterDiscount %>">\
                                    </div>\
                                   </div>\
                                   <div class="zain-action-group">\
                                    <button class="btn btn-warning add-to-order">Change Qty</button>\
                                   </div>', orderItem);
            } else {
                html = _.template('<div class="control-group inputqty-container">\
                                        <label class="control-label" for="inputQty">Qty</label>\
                                        <div class="controls">\
                                            <input type="number" id="inputQty" value="1">\
                                        </div>\
                                       </div>\
                                       <div class="zain-action-group">\
                                        <button class="btn btn-primary add-to-order">Add to order</button>\
                                       </div>', orderItem);
            }
            $('div.mediumListIconTextItem-Detail', this.$el).append(html);
            $('input.format-currency').formatCurrency({ colorize: true, region: 'id-ID', symbol: ' ' });
        },
        addToOrder: function () {
            var itemId = this.model.get('Id');
            var price = this.model.get('Price');
            var qty = parseInt($('input#inputQty', this.$el).val());
            var cmd = new AddToOrderCommand({
                ItemId: itemId,
                Qty: qty,
                Price: price
            });
            cmd.execute();
        }
    });
});