    function helpClickEvent(caller) {
        $.ajax({
            url: "@Url.Content("~/Bank/GetHelp")",
            data: "Form=" + $(caller).attr("form") + "&AggItemID=" + $(caller).attr("aggitemid"),
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

                var helpdata = response.Data
            
                //$(".content").click(function() {
                    // SHOW BOX
                    bankvisible = $("<div id='div-frame'><img id='img-bankclose' title='close' src='@Url.Content("~/Content/close.png")' /> </div>").appendTo($("body"));
                    $("<div id='div-fade'></div>").css("opacity", 0.1).appendTo($("body")).fadeIn("fast");
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
                    bankContent().appendTo($("#div-frame")).fadeIn("fast");
                    $(".ul-list").hide()

                    var last_srch = ""
                    $("#srch").focus(function () {
                        intervalID = window.setInterval(function () {
                            s = $("#srch").attr("value");
                            if (s != last_srch) {
                                $("#ul-help").replaceWith(viewListString(s.toLowerCase()))
                                last_srch = s
                            }
                        }, 250)
                    });
                    $("#srch").blur(function () { clearInterval(intervalID); last_s = ''; });
			    //})

                function bankContent() {
                    var s = "<br/><input class='sw' id='srch' type='text'/><br>" + 
                        "<span class='pseudolink' id='span-expand'>Раскрыть</span>&nbsp;<span class='pseudolink' id='span-collapse'>Скрыть</span><br /><br />"
                    var $s = $("<div>" + s + "</div>").append(viewListString(""))
                    $s.find("#span-collapse").click(function () {
                        $(".ul-list").hide();
                    });
                    $s.find("#span-expand").click(function () {
                        $(".ul-list").show();
                    });
                    return $s;
                }
            
                function viewListString(filter) {
                    function space(num) {
                        return Array(num).join("&nbsp;&nbsp;&nbsp;")
                    }
                    var text = ""
                    var LevelID = helpdata[0].LevelID
                    var id = window.toolbox.newid('g')
                    for (var i = 0; i < helpdata.length; i++) {
                        if (helpdata[i].LevelID != LevelID) {
                            for(var j = helpdata[i].LevelID; j < LevelID; j++)
                                text += "<ul class='ul-list " + id + "'>"
                            for(var j = LevelID; j < helpdata[i].LevelID; j++)
                                text += "</ul>"
                            LevelID = helpdata[i].LevelID
                        }
                        id = window.toolbox.newid('g')
                        if ((helpdata[i].Ticker + ": " + helpdata[i].Name).toLowerCase().indexOf(filter) >= 0) {
                            if (helpdata[i].Ticker == '') {
                                text +=  "<li class='li-header' GroupID='" + id + "'><span>" + helpdata[i].Name + "</span></li>"
                            } else {
                                text +=  "<li " + (LevelID == 0 ? "" : "class='li-pseudolink'") + " GroupID='" + id + "'><span>" + helpdata[i].Ticker + (helpdata[i].Type == "" ? "" : " (" + helpdata[i].Type + ")") + ": " + helpdata[i].Name + "</span></li>"
                            }
                        }
                    }
                    var $result = $("<div id='div-bank'><ul id='ul-help'>" + text + "</ul></div>")

                    $result.find("li").click(function () {
                        window.log($(this).attr('GroupID'))
                        $(this).parent().find("." + $(this).attr('GroupID')).toggle("fast");
                    });
                    return $result
                }
            } // ~success
        }); // ~ajax
    }
