$.ajax({
    url: "@Url.Content("~/View/ListJSON")",
    dataType: "json",
    type: "POST",
    error: function () {
        window.log("ajax failed")
    },
    success: function (response) {
                
        var viewslist = response
        var data = []
        for (var i = 0; i < viewslist.length; i++) {
            for (j = 0; j < viewslist[i].ViewItems.length; j++) {
                var item = {}
                item.ViewID = viewslist[i].ViewID
                item.category = viewslist[i].ViewName
                item.ViewItemID = viewslist[i].ViewItems[j].ViewItemID
                item.label = viewslist[i].ViewItems[j].ViewItemName
                data.push(item)
            }
        }

        $.widget("custom.catcomplete", $.ui.autocomplete, {
		    _renderMenu: function(ul, items) {
			    var self = this, 
                currentCategory = "";

			    $.each(items, function(index, item) {
				    if (item.category != currentCategory) {
					    ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
					    currentCategory = item.category;
				    }
				    self._renderItem(ul, item);
			    });
		    }
	    });

        $("#view-srch").catcomplete({ 
            delay: 200,
            source: data,
            select: function(event, ui) {
                window.location = "@VirtualPathUtility.ToAbsolute("~/View/Data/")?ViewID=" + ui.item.ViewID + "&ViewItemID=" + ui.item.ViewItemID
            }
        })
        window.log("finished ui")
    }
});

@* // TODO: page looks messy and links does not works properly
    $("<a>")
	.attr("tabIndex", -1)
	.attr("title", "Show All Items")
	.appendTo( $("#viewslist") )
	.button({
		icons: {
			primary: "ui-icon-triangle-1-s"
		},
		text: false
	})
	.removeClass( "ui-corner-all" )
	.addClass( "ui-corner-right ui-button-icon" )
	.click(function() {
        $("#date-srch").autocomplete("close");
        // SHOW BOX
        bankvisible = $("<div id='div-frame'><img id='img-bankclose' title='close' src='@Url.Content("~/Content/close.png")' /> </div>").appendTo($("body"));
        $("<div id='div-fade'></div>").css("opacity", 0.5).appendTo($("body")).fadeIn("fast");
        $("#div-fade").click(function () {
            // HIDE BOX
            bankvisible = !$("#div-frame").fadeOut("fast", function () {
                $("#div-frame").remove();
                $("#div-fade").remove();
            });
        });
        $("#img-bankclose").click(function () {
            // HIDE BOX
            bankvisible = !$("#div-frame").fadeOut("fast", function () {
                $("#div-frame").remove();
                $("#div-fade").remove();
            });
        });

        $("<iframe src='/View/ListFrame/' />").appendTo($("#div-frame"));

	}) *@