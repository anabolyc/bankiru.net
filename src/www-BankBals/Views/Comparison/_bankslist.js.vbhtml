@ModelType String

<script type="text/javascript">
(function() {
    $.ajax({
        url: "@Url.Content("~/Bank/ListJSON")",
        dataType: "json",
        type: "POST",
        data: "LoadComparables=true",
        error: function () {
            window.log("ajax failed")
        },
        success: function (response) {
            window.datafactory.bankslist = response
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
                item.BankIDC1 = bankslist[i].BankIDC1
                item.BankIDC2 = bankslist[i].BankIDC2
                data.push(item)
                if (bankslist[i].BankID == BankID)
                    window.datafactory.thisbank = bankslist[i]
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
                            @If Model = "TABLE" Then
                            @<text>
                            s += "<div class='div-bank'><a href='@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=@(ViewBag.ViewID)&BankID0=" + bankslist[i].BankID + "&BankID1=" + bankslist[i].BankIDC1 + "&BankID2=" + bankslist[i].BankIDC2 + "'>" + bankslist[i].BankName + "</a></div>"
                            </text>
                            End If
                            @If Model = "CHART" Then
                            @<text>
                            s += "<div class='div-bank'><a href='@VirtualPathUtility.ToAbsolute("~/Comparison/CompareBubbles/")?BankID=" + bankslist[i].BankID + "'>" + bankslist[i].BankName + "</a></div>"
                            </text>
                            End If
                        last = bankslist[i].BankName
                    }
                }
                if (s == "")
                    s = " No banks found "
                return "<div id='div-bank'>" + s + "</div>"
            }
            
            $("#bank-srch").autocomplete({ 
                autoFocus: true,
                delay: 200,
                minLength: 0,
                select: function(event, ui) {
                    if (ui.item.BankID != BankID) 
                        @If Model = "TABLE" Then
                        @<text>
                        window.location = "@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=@(ViewBag.ViewID)&BankID0=" + ui.item.BankID + "&BankID1=" + ui.item.BankIDC1 + "&BankID2=" + ui.item.BankIDC2 
                        </text>
                        End If
                        @If Model = "CHART" Then
                        @<text>
                        window.location = "@VirtualPathUtility.ToAbsolute("~/Comparison/CompareBubbles/")?BankID=" + ui.item.BankID
                        </text>
                        End If
                },
                source: data,
                close: function(event, ui) {
                    if ($("#bank-srch").attr("value") == "")
                        $("#bank-srch").attr("value", window.datafactory.thisbank.BankName)
                }
            })
            
            $("input.input-bank-c").autocomplete({ 
                autoFocus: true,
                delay: 200,
                minLength: 0,
                select: function(event, ui) {
                    if (ui.item.BankID != $(this).attr("bankid")) {
                            $(this).attr("bankid", ui.item.BankID)
                            window.location = "@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=@(ViewBag.ViewID)&BankID0=@(ViewBag.Bank.BankID)&BankID1=" + $("#input-bank-c1").attr("bankid") + "&BankID2=" + $("#input-bank-c2").attr("bankid")
                        }
                },
                source: data/*,
                close: function(event, ui) {
                    if ($("#bank-srch").attr("value") == "")
                        $("#bank-srch").attr("value", window.datafactory.thisbank.BankName)
                }*/
            })
            @*
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
            if ((window.datafactory.thisbank) && (window.datafactory.thisview)) {
                document.title = window.datafactory.thisbank.BankName + ": " + window.datafactory.thisview.ViewName
            } *@
        } // ~success
    }); // ~ajax
})();

</script>