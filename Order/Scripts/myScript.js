function currentMenu() {
    this.MenuID = ko.observable();
    this.Name = ko.observable();
    this.Count = ko.observable();
}

function orderViewModel() {
    var self = this;
    self.menus = ko.observableArray();
    self.selectedMenus = ko.observableArray();

    $.ajax({
        type: "GET",
        url: "home/menus/",
        dataType: "json",
        success: function (json) {
            self.menus(json);
        }
    });
    $.ajax({
        type: "GET",
        url: "home/selectedmenus/",
        dataType: "json",
        success: function (json) {
            self.selectedMenus(json);
        }
    });

    self.order = function (item) {
        self.selectedMenus().clear;
        $.ajax({
            type: "GET",
            data: { MenuID: item.ID },
            url: "home/order",
            dataType: "json",
            success: function (json) {
                self.selectedMenus(json);
            }
        });
    };

    self.remove = function (item) {
        self.selectedMenus().clear;
        $.ajax({
            type: "GET",
            data: { MenuID: item.MenuID },
            url: "home/remove",
            dataType: "json",
            success: function (json) {
                self.selectedMenus(json);
            }
        });
    };

    self.total = ko.computed( function () {
        var t = 0;
        ko.utils.arrayForEach(self.selectedMenus(),function (item) {
            t += item.Count;
        });
        return t;
    });

    self.amount = ko.computed(function () {
        var t = 0;
        ko.utils.arrayForEach(self.selectedMenus(), function (item) {
            t += item.Price * item.Count;
        });
        return t;
    });

    setInterval(function () {
        $.ajax({
            type: "GET",
            url: "home/selectedmenus/",
            dataType: "json",
            success: function (json) {
                self.selectedMenus(json);
            }
        });
    }, 10000);
}

$(document).ready(function () {
    ko.applyBindings(new orderViewModel());
});

