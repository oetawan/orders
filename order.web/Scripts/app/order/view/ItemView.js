/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery',
        'underscore',
        'backbone',
        'bootbox',
        'app/eventAggregator',
        'app/order/model/AddToOrderCommand',
        'app/order/model/RemoveOrderItemCommand'],
function ($, _, Backbone, bootbox, EA, AddToOrderCommand, RemoveOrderItemCommand) {
    return Backbone.View.extend({
        className: 'mediumListIconTextItem zain-listviewitem',
        template: _.template('<div class="mediumListIconTextItem-Detail">\
            <h3 class="zain-heading" aria-hidden="true" data-icon="="> <%= Name %> - <%= Code %></h3>\
            <span class="label label-info"><%= CurrencyId %> <%= Price %></span>\
            <span>/</span>\
            <span class="label label-info"><%= UnitCode %></span><br/>\
          </div>'),
        events: {
            'click button.add-to-order': 'addToOrder',
            'click button.remove-item': 'removeItem'
        },
        initialize: function () {
            this.options.shoppingCart.on('change', function () {
                this.render();
            }, this);
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            this.renderOrderItem();
            /*$('input#inputQty', this.$el).validate({
                rules: {
                    inputQty: {
                        required: true,
                        min: 1
                    }
                }
            });*/
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
                                        <input id="inputQty" type="number" min="1" value="<%= Qty %>">\
                                    </div>\
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
                                    <button class="btn btn-danger remove-item">Remove Item</button>\
                                   </div>', orderItem);
            } else {
                html = _.template('<div class="control-group inputqty-container">\
                                        <label class="control-label" for="inputQty">Qty</label>\
                                        <div class="controls">\
                                            <input id="inputQty" type="number" min="1" value="1">\
                                        </div>\
                                       </div>\
                                       <div class="zain-action-group">\
                                        <button class="btn btn-primary add-to-order">Add to order</button>\
                                       </div>', orderItem);
            }
            $('div.mediumListIconTextItem-Detail', this.$el).append(html);
            $('input.format-currency').formatCurrency({ colorize: true, region: 'id-ID' });
        },
        addToOrder: function () {
            var that = this;
            var itemId = this.model.get('Id');
            var itemCode = this.model.get('Code');
            var itemName = this.model.get('Name');
            var unitCode = this.model.get('UnitCode');
            var price = this.model.get('Price');
            var qty = parseInt($('input#inputQty', this.$el).val());
            var cmd = new AddToOrderCommand({
                ItemId: itemId,
                Qty: qty,
                Price: price,
                ItemCode: itemCode,
                ItemName: itemName,
                UnitCode: unitCode,
                beforeSend: function () {
                    that.$el.mask('Adding item to order...');
                },
                complete: function () {
                    that.$el.unmask();
                }
            });
            cmd.execute();
        },
        removeItem: function () {
            var that = this;
            var itemId = this.model.get('Id');
            var cmd = new RemoveOrderItemCommand({
                ItemId: itemId,
                beforeSend: function () {
                    that.$el.mask('Removing item...');
                },
                complete: function () {
                    that.$el.unmask();
                }
            });
            cmd.execute();
        }
    });
});