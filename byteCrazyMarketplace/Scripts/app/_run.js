﻿$(function () {
    app.initialize();

    //  Knockout
    ko.validation.init({ grouping: { observable: false } });
    ko.applyBindings(app, document.body);
});
