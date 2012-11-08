function currentMenu() {
    this.MenuID = ko.observable();
    this.Name = ko.observable();
    this.Count = ko.observable();
}

function orderViewModel() {
    var self = this;
    self.menus = ko.observableArray();
    self.selectedMenus = ko.observableArray();
    self.orderDetails = ko.observableArray();
    
    $.ajax({
        type: "GET",
        url: "home/menus/",
        dataType: "json",
        cache: false,
        success: function (json) {
            self.menus(json);
            $("#loading").hide();
        }
    });
    $.ajax({
        type: "GET",
        url: "home/selectedmenus/",
        dataType: "json",
        cache: false,
        success: function (json) {
            self.selectedMenus(json);
        }
    });
    $.ajax({
        type: "GET",
        url: "home/orderdetail/",
        dataType: "json",
        cache: false,
        success: function (json) {
            self.orderDetails(json);
        }
    });

    self.order = function (item) {
        $("#dialog-form").dialog({
            autoOpen: false,
            height: 200,
            width: 300,
            modal: true,
            buttons: {
                "OK": function () {
                    if ($("#nameForm").valid()) {
                        var name = $("#name").val();
                        self.selectedMenus().clear;
                        self.orderDetails().clear;
                        $.ajax({
                            type: "GET",
                            data: { MenuID: item.ID, FullName: name },
                            url: "home/order",
                            dataType: "json",
                            cache: false,
                            success: function (json) {
                                self.selectedMenus(json);
                                $.ajax({
                                    type: "GET",
                                    url: "home/orderdetail",
                                    dataType: "json",
                                    cache: false,
                                    success: function (json) {
                                        self.orderDetails(json);
                                    }
                                });
                                $("#name").val('');
                            }
                        });
                        
                        $(this).dialog("close");
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
        $("#dialog-form").dialog("open");
    };

    self.remove = function (item) {
        self.selectedMenus().clear;
        self.orderDetails().clear;
        $.ajax({
            type: "GET",
            data: { ID: item.ID },
            url: "home/remove",
            dataType: "json",
            cache: false,
            success: function (json) {
                self.selectedMenus(json);
                $.ajax({
                    type: "GET",
                    url: "home/orderdetail",
                    dataType: "json",
                    cache: false,
                    success: function (json) {
                        self.orderDetails(json);
                    }
                });
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
            cache: false,
            success: function (json) {
                self.selectedMenus(json);
                $.ajax({
                    type: "GET",
                    url: "home/orderdetail",
                    dataType: "json",
                    cache: false,
                    success: function (json) {
                        self.orderDetails(json);
                    }
                });
            }
        });
    }, 10000);
}

$(document).ready(function () {
    ko.applyBindings(new orderViewModel());
});

