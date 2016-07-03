    window.builder = new Object()
    window.builder.cell = function (container, PriValue, SecValue, isRatio, IsHeader) {
        var td0 = document.createElementEx2("td", (PriValue == "") ? " " : (isRatio ? PriValue / 100. : window.toolbox.addCommas(PriValue)))
        @If Model.Set.ParID2 Is Nothing Then
            @<text>
            td0.colSpan = 2
            container.appendChild(td0)
            </text>
        Else
            @<text>
            container.appendChild(td0)
            var td1 = document.createElement("td")
            td1.appendChild(window.toolbox.getChangeElement(SecValue))
            container.appendChild(td1)
            </text>
        End If
    }
    
    window.builder.dropdown = function (BuildData) {
        var result = {
            body: [],
            columns: []
        }
        if (!BuildData.expanded) 
            return result
        var data = BuildData.data
        var j = 0
        var ViewItemTicker = ""
        for (var i = 0; i < data.Items.length; i++) {
            ViewItemTicker = data.Items[i].Ticker
            var tr0 = document.createElementEx("tr", "", BuildData.id)
            $(tr0).attr("level", BuildData.level)
            $(tr0).addClass("tr-dropdown")
            var tr1 = document.createElementEx("tr", "", BuildData.id)
            $(tr1).attr("level", BuildData.level)
            $(tr1).addClass("tr-dropdown")
            var td1 = document.createElement("td")
            @If ViewBag.View.DescFilterRow <> "" Then
            @<text>
            var span = document.createElementEx("span", "&nbsp;", "span-more span-plus")
            if (data.Items[i].AggItemID)
                span.setAttribute("aggitemid", data.Items[i].AggItemID)
            else
                span.setAttribute("itemid", data.Items[i].ID)
            td1.appendChild(span)
            </text>
            End If
            tr1.appendChild(td1)
            td1 = document.createElement("td")
            td1.innerHTML = window.toolbox.space(parseInt(BuildData.level) + 1)
            if (data.Items[i].Expandable)
                td1.appendChild( document.createElementEx("span", "&gt;&nbsp;" + data.Items[i].Ticker, "span-row-dropdown") )
            else
                td1.appendChild( document.createElementEx("span", "&nbsp;&nbsp;" + data.Items[i].Ticker, "span-row-nodropdown") )
            
            tr1.appendChild(td1)
            //"<img class='img-chart' src='@Url.Content("~/Content/chart.png")' alt='show chart' />"
            var td2 = document.createElementEx("td", "")
            td2.innerHTML += "&nbsp;" + data.Items[i].Name

            tr1.appendChild(td2)
            result.columns.push(tr1)

            for (; j < (data.Values.length / data.Items.length) * (i + 1); j++) {
            @If ViewBag.View.Form = 102 then
                @<text>window.builder.cell(tr0, data.Values[j][0], null, false)</text>
            Else
                @<text>window.builder.cell(tr0, data.Values[j][0], data.Values[j][1], false)</text>
            End If
            }
            result.body.push(tr0)
            
            if (window.datafactory.exreports[ViewItemTicker]) {
                // если в середке что-то раскрыто уже
                if (window.datafactory.exreports[ViewItemTicker].expanded) {
                    var bld = window.builder.exdata(window.datafactory.exreports[ViewItemTicker])
                    for (var k = 0; k < bld.body.length; k++) {
                        result.body.push(bld.body[k])
                        result.columns.push(bld.columns[k])
                    }
                }
            }

            if (window.datafactory.ddreports[ViewItemTicker]) {
                //это только если справа раскрыто несолько уровней и после этого раскрывается сверху.
                if (window.datafactory.ddreports[ViewItemTicker].expanded) {
                    var bld = window.builder.dropdown(window.datafactory.ddreports[ViewItemTicker])
                    for (var k = 0; k < bld.body.length; k++) {
                        result.body.push(bld.body[k])
                        result.columns.push(bld.columns[k])
                    }
                }
            }

        }
        return result
    }

    window.builder.exdata = function (BuildData) {
        var result = {
            body: [],
            columns: []
        }
        if (!BuildData.expanded) 
            return result
        var data = BuildData.data
        var j = 0
        for (var i = 0; i < data.Items.length; i++) {
            var tr0 = document.createElementEx("tr", "", BuildData.id + " tr-moredata")
            var tr = document.createElementEx("tr", "", BuildData.id)
            $(tr).addClass("tr-moredata")
            $(tr0).addClass("tr-moredata-" + i)
            $(tr).addClass("tr-moredata-" + i)
            //<img class='img-chart' src='@Url.Content("~/Content/chart.png")' alt='show chart' />
            var td = document.createElementEx("td", "&nbsp;" + data.Items[i].Name)
            td.colSpan = 3
            tr.appendChild(td)
            result.columns.push(tr)
            for (; j < (data.Values.length / data.Items.length) * (i + 1); j++) {
            @If ViewBag.View.Form = 102 then
                @<text>window.builder.cell(tr0, data.Values[j][0], null, data.Items[i].IsRatio)</text>
            Else
                @<text>window.builder.cell(tr0, data.Values[j][0], data.Values[j][1], data.Items[i].IsRatio)</text>
            End If
            }
            result.body.push(tr0)
        }
        return result
    }