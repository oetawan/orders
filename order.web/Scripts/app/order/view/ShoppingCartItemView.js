/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/eventAggregator', 'app/order/model/ChangeQtyCommand', 'app/order/model/RemoveOrderItemCommand'], function ($, _, Backbone, EA, ChangeQtyCommand, RemoveOrderItemCommand) {
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
        toggleSaved: function () {
            $('a.update-qty', this.$el).removeClass('btn-warning');
            $('a.update-qty', this.$el).addClass('btn-success');
            $('a.update-qty', this.$el).html('Saved');
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            $('input.inputQty', this.$el).css('width', '60px');
            this.toggleButton();
            return this;
        },
        events: {
            'change input.inputQty': 'toggleButton',
            'click a.update-qty': 'changeQty',
            'click a.remove-item': 'removeItem'
        },
        changeQty: function (e) {
            e.preventDefault();
            var that = this;
            if (this.model.get("Qty") != $('input.inputQty', this.$el).val()) {
                var that = this;
                var itemId = this.model.get('ItemId');
                var qty = parseInt($('input.inputQty', this.$el).val());
                var cmd = new ChangeQtyCommand({
                    ItemId: itemId,
                    Qty: qty,
                    beforeSend: function () {
                        $('input.inputQty', that.$el).attr('disabled', 'disabled');
                        $('a.update-qty', that.$el).hide();
                        $('a.update-qty', that.$el).after('<div class="progress progress-indeterminate progress-update-qty"><div class="win-ring small"></div></div>');
                    },
                    complete: function () {
                        $('input.inputQty', that.$el).removeAttr('disabled');
                        $('a.update-qty', that.$el).show();
                        $('div.progress-update-qty', that.$el).remove();
                    },
                    success: function (model, response, options) {
                        if (response.success === true) {
                            that.toggleSaved();
                            EA.trigger('order:changeqty-success', response);
                        } else {
                            bootbox.modal(response.errorMessage, 'Error');
                        }
                    }
                });
                cmd.execute();
            }
        },
        removeItem: function () {
            var that = this;
            var itemId = this.model.get('ItemId');
            var cmd = new RemoveOrderItemCommand({
                ItemId: itemId,
                beforeSend: function () {
                    $('input.inputQty', that.$el).attr('disabled', 'disabled');
                    $('a.update-qty', that.$el).hide();
                    $('a.remove-item', that.$el).hide();
                    $('a.remove-item', that.$el).after('<div class="progress progress-indeterminate progress-remove-item"><div class="win-ring small"></div></div>');
                },
                complete: function () {
                    $('input.inputQty', that.$el).removeAttr('disabled');
                    $('a.update-qty', that.$el).show();
                    $('a.remove-item', that.$el).show();
                    $('div.progress-remove-item', that.$el).remove();
                },
                success: function (model, response, options) {
                    if (response.success === true) {
                        EA.trigger('order:removeorderitemsuccess', response);
                        that.remove();
                    } else {
                        bootbox.modal(response.errorMessage, 'Error');
                    }
                }
            });
            cmd.execute();
        },
        toggleButton: function (e) {
            var enabled = this.model.get("Qty") != $('input.inputQty', this.$el).val();
            if (enabled) {
                $('a.update-qty', this.$el).removeAttr('disabled');
                $('a.update-qty', this.$el).removeClass('btn-success');
                $('a.update-qty', this.$el).addClass('btn-warning');
                $('a.update-qty', this.$el).html('Update');
            } else {
                $('a.update-qty', this.$el).attr('disabled', 'disabled');
            }
        }
    });
});