@Imports www.BankBals.Data

@ModelType ViewQuery.Result2
    
@Code
    ViewData("Title") = "График:" + Model.Name + " - " + ViewBag.AggItem.NameRus
    Layout = "~/Views/Shared/_Chart.vbhtml"
End Code

<div id="chart"></div>
<script type="text/javascript">
    var categories = []
    var data = [{
        type: 'column',
        animation: false,
        shadow: false,
        name: "data",
        data: [],
        color: "#6699CC"
    }]

    @Code
        Dim i As Integer = 0
        Dim last As Double = 0
        For Each item As ViewQuery.Data3 In Model.Data
            last = item.Value / IIf(ViewBag.isRatio, 100.0, 1000000.0)
            @:data[0].data.push({x: @i, y: @last})
            @:categories.push('@(item.Date.ToString("MMM yy"))');
            i += 1
        Next
    End Code
        
    data[0].data[data[0].data.length - 1].color = '#3366CC';

    data.push({
		type: 'line',
		data: [[0, @last], [@(i - 1), @last]],
		animation: false,
		enableMouseTracking: false,
		color: "#3366CC",
		shadow: false
	})

    $("#chart").highcharts({
        chart: {
            animation: false
        },
        title: {
            text: '@Model.Name - @(ViewBag.AggItem.NameRus)@(IIf(ViewBag.IsRatio, "", ", млрд. руб."))',
            style: { color: "#555", fontSize: 14, fontFamily: 'Consolas, "Ubuntu Mono", UbuntuMono, "Courier New", monospace' }
        },
        credits: { enabled: false },
        legend: { enabled: false },
        tooltip: {
            animation: false,
            backgroundColor: "rgba(255, 255, 255, 0.85)",
            enabled: true,
            borderRadius: 0,
            borderWidth: 1,
            shadow: false,
            useHTML: true,
            formatter: function () {
                var s = ""
                s += "1 " + this.x + ": " + this.y.toFixed(2) + '@(IIf(ViewBag.IsRatio, " %", " млрд. руб."))'
                return "<span class='span-hc-tooltip'>" + s + "<span>"
            }
        },
        yAxis: {
            endOnTick: false,
            gridLineWidth: 1,
            gridLineDashStyle: "ShortDot",
            color: "#C0C0C0",
            title: {
                text: null
            },
            labels: {
                style: { color: "#555", fontFamily: 'Consolas, "Ubuntu Mono", UbuntuMono, "Courier New", monospace' }
            }
        },
        xAxis: {
            categories: categories,
            gridLineWidth: 1,
            gridLineDashStyle: "ShortDot",
            tickInterval: 4,
            labels: {
                style: { color: "#555", fontFamily: 'Consolas, "Ubuntu Mono", UbuntuMono, "Courier New", monospace' }
            }
        },
        plotOptions: {
            column: {
                pointPadding: 0.0,
                borderWidth: 0
            },
            line: {
                lineWidth: 0.5,
                marker: {
                    enabled: false
                }
            }
        },
        series: data
    })
</script>

