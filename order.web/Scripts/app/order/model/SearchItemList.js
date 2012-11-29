/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/model/Item'], function ($, _, Backbone, Item) {
    return Backbone.Collection.extend({
        url: 'order/searchitem',
        model: Item
    });
});