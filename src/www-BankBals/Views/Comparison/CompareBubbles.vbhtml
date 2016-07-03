@Imports www.BankBals.Data
@ModelType List(Of A_2X2CHARTS_VIEW)

@Code
    ViewData("Title") = ViewBag.Bank.NameRus & " - Сравнение в кружочках"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Сравнить банки", "Index", "Comparison") &#8594; 
        <span id='bankslist' class='span-ui-dropdown'>  
        <input class='sw' id='bank-srch' type='text' value='@ViewBag.Bank.NameRus' />
    </span>
</div>

<a name='top'></a>
<ul class='ul-views'>
@Code
    For Each item As A_VIEWS_BANKSPIC In ViewBag.Views
        @<li>
        <a href="@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=@(item.ViewID)&BankID0=@(ViewBag.Bank.BankID)&BankID1=@(ViewBag.Bank.BankIDC1)&BankID2=@(ViewBag.Bank.BankIDC2)">@item.ViewNameRus</a>
        </li>
    Next
    @<li class="selected"><a name="tab">Картинкой</a></li>    
End Code
</ul>

<script type="text/javascript">
    @Html.Include("~/Scripts/applet.fixedhead.js")
    @Html.Include("~/Scripts/applet.toolbox.js")
</script>        

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")
@Html.Partial("_bankslist.js", "CHART")

<script type="text/javascript">
    var BankID = @ViewBag.Bank.BankID
    window.datafactory = new Object()
</script>

<ul class='ul-contents'>
@For Each item As A_2X2CHARTS_VIEW In Model
    @<li><a class='a-anchor' href="#chart@(item.ChartID)">@item.NameRus</a></li>
Next
</ul>

@For Each item As A_2X2CHARTS_VIEW In Model
    @<p><a name="chart@(item.ChartID)"></a>
    <h3>@item.NameRus</h3>
    <div class='div-highchart-bubbles' id='div-highchart-@(item.ChartID)' chartid='@(item.ChartID)' xcaption='@item.X_AggItemNameRus' xmax='@item.X_MaxValue' ycaption='@item.Y_AggItemNameRus' ymax='@item.Y_MaxValue'>
    <img src="@Url.Content("~/Content/loading_big.gif")" alt="loading..."/>
    </div>
    <a class='a-to-contents' href="#top">К содержанию</a>
    </p>
Next

<script type="text/javascript">
$(document).ready(function () {
    checkCharts()
    $(window).scroll(function(){
        checkCharts()
    }) 
})

var checkCharts = function () {
    $("div.div-highchart-bubbles").each(function (index, Element) {
        if (window.toolbox.visible(Element) && !$(Element).attr("processed")) {
            $(Element).attr("processed", "1")
            $.ajax({
            url: "@Url.Content("~/Comparison/CompareBubblesJSON")?BankID=@(ViewBag.Bank.BankId)&ChartID=" + $(Element).attr("chartid"),
            type: "POST",
            dataType: "json",
            error: function () {
                window.log("ajax failed")
            },
            success: function (response) {
                
                var data = [{
                    type: 'bubble',
                    animation: false, 
                    shadow: false,
                    name: "data", 
                    data: [],
                    color: "#6699CC"
                }]
            
                var RD = response
                for (var i = 0; i < RD.length; i++) {
                    data[0].data.push({
                        x: parseFLOAT(RD[i].X),
                        y: parseFLOAT(RD[i].Y),
                        //color: ,
                        marker: { 
                            fillColor: parseInt(RD[i].BankID) == @(ViewBag.Bank.BankID) ? "#FF6C11": null,
                            lineColor: parseInt(RD[i].BankID) == @(ViewBag.Bank.BankID) ? "#FF6C11": null
                        },
                        z: Math.sqrt(RD[i].TotAss),
                        assets: RD[i].TotAss,
                        name: RD[i].BankName,
                        rank: RD[i].RN
                    })
                }

                $(Element).highcharts({
	                chart: {
                        animation: false,
	                    zoomType: 'xy'
                    },
	                title: {
	    	            text: ''
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
		                formatter: function(){
			                var s = ""
			                s += "<b>" + this.point.name + "</b> (#" + this.point.rank + " по акт. = " + (this.point.assets / 1e6).toFixed(2) + " млрд. руб.)<br />"
                            s += $(Element).attr("xcaption") + ": <b>" + this.x.toFixed(2) + "</b><br />"
                            s += $(Element).attr("ycaption") + ": <b>" + this.y.toFixed(2) + "</b>"
			                return "<span class='span-hc-tooltip'>" + s + "<span>"
		                }
	                },
                    yAxis: {
		                min: 0,
                        max: $(Element).attr("ymax"),
		                gridLineWidth: 1,
		                gridLineDashStyle: "ShortDot",
		                color: "#C0C0C0",
		                title: { text: $(Element).attr("ycaption") },
                        labels: {
                            style: { color: "#555", fontFamily: 'Consolas, "Ubuntu Mono", UbuntuMono, "Courier New", monospace' }
                        }
	                },
	                xAxis: {
                        min: 0,
                        max: $(Element).attr("xmax"),
		                gridLineWidth: 1,
		                gridLineDashStyle: "ShortDot",
                        color: "#C0C0C0",
                        title: { text: $(Element).attr("xcaption") },
                        labels: {
                            style: { color: "#555", fontFamily: 'Consolas, "Ubuntu Mono", UbuntuMono, "Courier New", monospace' }
                        }
	                },
	                series: data
	            })
            
            } // ~success
        }); // ~ajax
    }
    }) 
}
</script>

<script src="@Url.Content("~/Scripts/Highcharts-3.0.4/js/highcharts.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/Highcharts-3.0.4/js/highcharts-more.js")" type="text/javascript"></script>