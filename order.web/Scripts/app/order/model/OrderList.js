/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/model/Group', 'app/order/model/Order'], function ($, _, Backbone, Group, Order) {
    return Backbone.Collection.extend({
        url: 'order/all',
        model: Order
    });
});