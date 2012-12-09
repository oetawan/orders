/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/model/Group', 'app/order/model/OrderItem'], function ($, _, Backbone, Group, OrderItem) {
    return Backbone.Collection.extend({
        url: 'order/orderitems',
        model: OrderItem
    });
});