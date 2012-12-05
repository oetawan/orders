/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator','app/order/model/ChangeQtyCommand'], function ($, _, Backbone, EA, ChangeQtyCommand) {
    return Backbone.View.extend({
        tagName: 'tr',
        template: _.template('<td class="number"><%= Index %></td>\
                   <td><%= ItemCode %>\
                   <td><%= ItemName %></td>\
                   <td class="number"><input class="inputQty" type="number" min="1" value="<%= Qty %>"></td>\
                   <td><a href="#" class="btn btn-warning btn-mini update-qty">Update</a></td>\
                   <td><%= UnitCode %></td>\
                   <td class="currency"><%= Price %></td>\
                   <td class="currency"><%= AmountAfterDiscount %></td>\
                   <td><a href="#" class="btn btn-mini remove-item">Remove</a></td>'),
        initialize: function () {
            this.options.shoppingCart.on('change', function () {
                var that = this;
                var orderItem = _.find(this.options.shoppingCart.get('Items'), function (itm) {
                    return itm.ItemId === that.model.get('ItemId');
                });
                if (orderItem && orderItem.Qty == $('input.inputQty', this.$el).val()) {
                    this.model.set(orderItem);
                    this.toggleButton();
                }
            }, this);
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            $('input.inputQty', this.$el).css('width', '60px');
            this.toggleButton();
            return this;
        },
        events: {
            'change input.inputQty': 'toggleButton',
            'click a.update-qty': 'changeQty'
        },
        changeQty: function (e) {
            if (this.model.get("Qty") != $('input.inputQty', this.$el).val()) {
                var that = this;
                var itemId = this.model.get('ItemId');
                var qty = parseInt($('input.inputQty', this.$el).val());
                var cmd = new ChangeQtyCommand({
                    ItemId: itemId,
                    Qty: qty,
                    beforeSend: function () {
                        that.$el.mask('Please wait while updating...');
                    },
                    complete: function () {
                        that.$el.unmask();
                    }
                });
                cmd.execute();
            }
        },
        toggleButton: function (e) {
            var enabled = this.model.get("Qty") != $('input.inputQty', this.$el).val();
            if (enabled) {
                $('a.update-qty', this.$el).removeAttr('disabled');
            } else {
                $('a.update-qty', this.$el).attr('disabled', 'disabled');
            }
        }
    });
});