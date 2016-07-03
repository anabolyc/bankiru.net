$.ajax({
    url: "@Url.Content("~/Bank/ListJSON")",
    dataType: "json",
    type: "POST",
    error: function () {
        window.log("ajax failed")
    },
    success: function (response) {

        var bankslist = response
        var data = []
        for (var i = 0; i < bankslist.length; i++) {
            var item = {}
            item.BankID = bankslist[i].BankID
            item.BankName = bankslist[i].BankName
            item.label = bankslist[i].BankID + ' - ' + bankslist[i].BankName
            data.push(item)
        }

        function banksList(filter) {
            text = ""
            for (var i = 0; i < bankslist.length; i++) {
                if ((bankslist[i].BankName.toLowerCase().indexOf(filter) >= 0) || (filter == bankslist[i].BankID)) {
                    text += "<tr><td>" + bankslist[i].BankID + "</td><td><a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=" + bankslist[i].BankID + "&ViewID=1'>" + bankslist[i].BankName + "</a></td></tr>"
                }
            }
            return "<table class='texttable' id='table-list'>" + text + "</table>"
        }
               
        $("#bank-srch")
        .autocomplete({ 
            autoFocus: true,
            delay: 200,
            select: function( event, ui) {
                window.location = "@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=" + ui.item.BankID + "&ViewID=1"
            },
            source: data
        })
    }
});

@*
$.ajax({    
    url: "@Url.Content("~/Bank/ListJSON")",
    dataType: "json",
    type: "POST",
    error: function () {
        window.log("ajax failed")
    },
    success: function (response) {
        window.log("got data: " + response.ErrorCode + ": " + response.ErrorMessage)

        if (response.ErrorCode != 0) {
            return;
        }

        window.datafactory.bankslist = response.BanksList
        var bankslist = window.datafactory.bankslist
        window.datafactory.bankslist.GetByID = function(ID) {
            for (var i = 0; i < bankslist.length; i++)
                if (bankslist[i].BankID == ID)
                    return bankslist[i]
        }

        var data = []
        for (var i = 0; i < window.datafactory.bankslist.length; i++) {
            var item = {}
            item.BankID = bankslist[i].BankID
            item.BankName = bankslist[i].BankName
            item.label = bankslist[i].BankID + " - " + bankslist[i].BankName
            item.value = bankslist[i].BankName
            data.push(item)
        }

        function bankContent() {
            var s = "<input class='sw' id='srch' type='text'/>"
            s += banksListString("")
            var $s = $("<div>" + s + "</div>")
            var last_srch = ""
            $s.children("#srch").focus(function () {
                intervalID = window.setInterval(function () {
                    s = $("#srch").attr("value")
                    if (s != last_srch) {
                        $("#div-bank").replaceWith(banksListString(s.toLowerCase()))
                        last_srch = s
                    }
                }, 250)
            });
            $s.children("#srch").blur(function () {
                clearInterval(intervalID)
                last_s = '' 
            });
            return $s
        }
            
        function banksListString(filter) {
            var s = ""
            var last = " "
            for (var i = 0; i < bankslist.length; i++) {
                if ((bankslist[i].BankName.toLowerCase().indexOf(filter) >= 0) || (filter == bankslist[i].BankID)) {
                    if (last.substr(0, 1) != bankslist[i].BankName.substr(0, 1))
                        s += "<div class='div-hr'><strong>" + bankslist[i].BankName.substr(0, 1) + "</strong><hr></div>"
                    s += "<div class='div-bank'><a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=" + bankslist[i].BankID + "&ViewID=@(ViewBag.View.ViewID)&SetID=@(Model.Set.SetID)'>" + bankslist[i].BankName + "</a></div>"
                    last = bankslist[i].BankName
                }
            }
            if (s == "")
                s = " No banks found "
            return "<div id='div-bank'>" + s + "</div>"
        }
            
        var text = "<span class='span-ui-dropdown'><input class='sw' id='bank-srch' type='text' value='@ViewBag.Bank.NameRus'/></span>"
        $("#bankslist").html(text)
        $("#bank-srch").autocomplete({ 
            autoFocus: true,
            delay: 200,
            minLength: 0,
            select: function(event, ui) {
                if (ui.item.BankID != @ViewBag.Bank.BankID) 
                    window.location = "@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=" + ui.item.BankID + "&ViewID=@(ViewBag.View.ViewID)&SetID=@(Model.Set.SetID)"
            },
            source: data,
            close: function(event, ui) {
                if ($("#bank-srch").attr("value") == "")
                    $("#bank-srch").attr("value", "@ViewBag.Bank.NameRus")
            }
        })
            
        var select = $("#bank-srch"),
            wrapper = select.parent();

        $("<a>")
		.attr("tabIndex", -1)
		.attr("title", "Show All Items")
		.appendTo( wrapper )
		.button({
			icons: {
				primary: "ui-icon-triangle-1-s"
			},
			text: false
		})
		.removeClass( "ui-corner-all" )
		.addClass( "ui-corner-right ui-button-icon" )
		.click(function() {
            $("#view-srch").autocomplete("close");
            // SHOW BOX
            bankvisible = $("<div id='bank'><img id='img-bankclose' title='close' src='@Url.Content("~/Content/close.png")' alt='close'/> </div>").appendTo($("body"));
            $("<div id='fade'></div>").css("opacity", 0.5).appendTo($("body")).fadeIn("fast")
            $("#fade").click(function () {
                // HIDE BOX
                bankvisible = !$("#bank").fadeOut("fast", function () {
                    $("#bank").remove()
                    $("#fade").remove()
                });
            });
            $("#img-bankclose").click(function () {
                // HIDE BOX
                bankvisible = !$("#bank").fadeOut("fast", function () {
                    $("#bank").remove();
                    $("#fade").remove();
                });
            });
            bankContent().appendTo($("#bank")).fadeIn("fast")
		})
    } // ~success
}); // ~ajax

*@