function dropDownEvent(caller) {

    function applyData() {
        window.datafactory.ddreports[ViewItemTicker].expanded = true
        var bld = window.builder.dropdown(window.datafactory.ddreports[ViewItemTicker])
        $(bld.columns).find("span.span-row-dropdown").click( function () {
            var caller = this
            dropDownEvent(caller)
        })
        $(bld.columns).find("span.span-more").click( function () {
            var caller = this
            exDataEvent(caller)
        });
        $(bld.columns).find("img.img-chart").click( function () {
            var caller = this
            chartClickEvent(caller)
        });
        var RowNumber = caller.parentNode.parentNode.sectionRowIndex
        if ( (window.datafactory.exreports[ViewItemTicker]) && (window.datafactory.exreports[ViewItemTicker].expanded) ){
            RowNumber += 5
        }
        if ( $("#div-col").find("tbody > tr").eq(RowNumber + 1).hasClass("tr-chart") ){
            RowNumber += 1
        }
        $(bld.columns).insertAfter( $("#div-col").find("tbody > tr").eq(RowNumber) )
        $(bld.body).insertAfter( $("#div-body").find("tbody > tr").eq(RowNumber) )
        caller.id = window.datafactory.ddreports[ViewItemTicker].id
        $(caller).html( $(caller).html().replace("&gt;", "&lt;") ).removeClass("loading")
    }

    var ViewItemTicker = $(caller).text()
    ViewItemTicker = ViewItemTicker.substring(2, ViewItemTicker.length)
    var level = $(caller.parentNode.parentNode).attr("level")
    if (!level) level = 1
    if ($(caller).text().substring(0, 1) == ">") {
        if (!window.datafactory.ddreports[ViewItemTicker]) {
            $(caller).addClass("loading")
            window.log(ViewItemTicker)
            $.ajax({
                url: "@Url.Content("~/Bank/DataJSONAggItem")",
                dataType: "json",
                type: "POST",
                data: "BankID=@(ViewBag.Bank.BankID)&ViewID=@(ViewBag.View.ViewID)&SetID=@(Model.Set.SetID)&ViewItemTicker=" + encodeURIComponent(ViewItemTicker),
                error: function () {
                    window.log("ajax failed")
                },
                success: function (response) {
                    window.datafactory.ddreports[ViewItemTicker] = {
                        data : response,
                        id   : window.toolbox.newid("DD"),
                        level: parseInt(level) + 1
                    }
                    applyData()
                    $(caller).removeClass("loading")
                }
            });
        } else {
            applyData()
        }
    } else {
        $("." + caller.id).each(function(index, element) {
            if ($(element).find("span.span-more").hasClass("span-minus"))
                $(element).find("span.span-more").click()
            if ($(element).find(".span-row-dropdown").text().substring(0, 1) == "<") {
                $(element).find(".span-row-dropdown").click()
            }
        })
        $("." + caller.id).remove()
        window.datafactory.ddreports[ViewItemTicker].expanded = false
        $(caller).html( $(caller).html().replace("&lt;", "&gt;") )                }
}