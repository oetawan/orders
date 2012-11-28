/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/model/Group'], function ($, _, Backbone, Group) {
    return Backbone.Collection.extend({
        url: 'order/groups',
        model: Group
    });
});