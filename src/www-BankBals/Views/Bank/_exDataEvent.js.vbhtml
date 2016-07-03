function exDataEvent(caller) {
    function applyData() {
        window.datafactory.exreports[aggitemid].expanded = true
        var bld = window.builder.exdata(window.datafactory.exreports[aggitemid])
        var RowNumber = caller.parentNode.parentNode.sectionRowIndex
        if ( $("#div-col").find("tbody > tr").eq(RowNumber + 1).hasClass("tr-chart") ){
            RowNumber += 1
        }
        $(bld.columns).insertAfter( $("#div-col").find("tbody > tr").eq(RowNumber) )
        $(bld.body).insertAfter( $("#div-body").find("tbody > tr").eq(RowNumber) )
        caller.id = window.datafactory.exreports[aggitemid].id
        $(caller).addClass("span-minus")
        $(caller).removeClass("span-plus")
    }
    
    var aggitemid
    var isAggItemID
    if ($(caller).attr("aggitemid")) {
        aggitemid = $(caller).attr("aggitemid")
        isAggItemID = true
    } else {
        aggitemid = $(caller).attr("itemid")
        isAggItemID = false
    }
    if ($(caller).hasClass("span-plus")) {
        if (!window.datafactory.exreports[aggitemid]) {
            // data already loaded
            $(caller).addClass("loading")
            $.ajax({
                url: "@Url.Content("~/Bank/MoreDataJSON")" ,
                dataType: "json",
                type: "POST",
                data: "Form=@(ViewBag.View.Form)&BankID=@(ViewBag.Bank.BankID)&ViewID=@(ViewBag.View.ViewID)&SetID=@(Model.Set.SetID)&IsAggItemID=" + isAggItemID + "&ViewItem=" + aggitemid,
                error: function () {
                    window.log("ajax failed")
                },
                success: function (response) {
                    window.datafactory.exreports[aggitemid] = {
                        data: response,
                        id  : window.toolbox.newid("EX")
                    }
                    applyData()
                    $(caller).removeClass("loading")
                }
            });
        } else {
            applyData()
        } 
    } else {
        $("." + caller.id).remove()
        window.datafactory.exreports[aggitemid].expanded = false
        $(caller).removeClass("span-minus")
        $(caller).addClass("span-plus")
    }
}