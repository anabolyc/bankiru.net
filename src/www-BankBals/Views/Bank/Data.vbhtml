@Imports Newtonsoft.Json
@Imports www.BankBals.Data

@ModelType BankQuery.Result

@Code
    ViewData("Title") = ViewBag.Bank.NameRus + " - " + ViewBag.View.ViewNameRus
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Банки", "List", "Bank") &#8594; 
    <span id='bankslist' class='span-ui-dropdown'>  
        <input class='sw' id='bank-srch' type='text' value='@ViewBag.Bank.NameRus' />
    </span>
</div>

<ul class='ul-views'>
@Code
    For Each OV As A_VIEW In ViewBag.Views
        If OV.ViewID = ViewBag.View.ViewID Then
            @<li class="selected"><a name="tab">@(OV.ViewNameRus)</a></li>    
        Else
            @<li><a title="@OV.ViewNameRus" href="@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=@(ViewBag.Bank.BankID)&ViewID=@(OV.ViewID)">@(Left(OV.ViewNameRus, 12) & IIf(Len(OV.ViewNameRus) > 12, "..", ""))</a></li>
        End If
    Next
End Code
</ul>

@Code
    If Common.Placement() = Common.Placements.PRODUCTION Then
@<div class='div-toplinks'>
    <a class='a-save' href="@VirtualPathUtility.ToAbsolute("~/Bank/GetExcel")?BankID=@(ViewBag.Bank.BankID)&ViewID=@(ViewBag.View.ViewID)&SetID=@(Model.Set.SetID)" > Сохранить обновляемый</a>
    <a class='a-save' href="@VirtualPathUtility.ToAbsolute("~/Bank/GetExcel2")?BankID=@(ViewBag.Bank.BankID)&ViewID=@(ViewBag.View.ViewID)&SetID=@(Model.Set.SetID)" > необновляемый</a>
    <a class='a-save' href="@VirtualPathUtility.ToAbsolute("~/API/BankBalsTool")" > BankbalsTool </a>
</div> 
End If
End Code

<div class='div-datatable-cont' id='content'>
    <div id='div-col'>
        <table class="table-datatable tablesorter">
            <thead>
                <tr>
                    <th colspan="3" class="th-selectors nosort">
                        @For Each item In ViewBag.Sets
                            Dim itemName As String = (If(item.A_PARAM1, item.A_PARAM)).Ticker
                            If item.SetID = Model.Set.SetID Then
                                @<span class="span-switch span-switch-selected">@itemName</span>
                            Else
                                @<span class="span-switch">
                                    <a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=@(ViewBag.Bank.BankID)&ViewID=@(ViewBag.View.ViewID)&SetID=@(item.SetID)'>@itemName</a>
                                </span>
                            End If
                        Next
                    </th>
                </tr>
                <tr>
                    <th></th>
                    <th>Ticker</th>
                    <th>Desc</th>
                </tr>
            </thead>
            <tbody>
                @Code
                    Dim IsRatio(0) As Boolean
                    Dim IsHeader(0) As Boolean
                    Dim rowIndex As Integer = 0
                    For Each item As BankQuery.Dictionary In Model.Dictionary
                        ReDim Preserve IsRatio(rowIndex)
                        ReDim Preserve IsHeader(rowIndex)
                        IsRatio(rowIndex) = If(item.IsRatio, False)
                        IsHeader(rowIndex) = item.IsHeader
                        If item.IsHeader Then
                            @<tr class="tr-header"><td colspan="3"> @item.Name </td></tr>
                        Else
                            @<tr> 
                                <td>
                                @If ViewBag.View.DescFilterRow <> "" Then
                                      @<span aggitemid='@item.AggItemID' class='span-more span-plus'>&nbsp;</span>      
                                End If
                                </td>
                                <td>
                                    @If item.ISWithSub Then
                                        @<span class="span-row-dropdown">&gt;&nbsp;@item.Ticker</span>
                                    Else
                                        @:&nbsp;&nbsp;@item.Ticker
                                    End If
                                </td>
                                <td>
                                    <img form='@ViewBag.View.Form' aggitemid='@item.AggItemID' class='img-help' src='@Url.Content("~/Content/help.png")' title='show help' />
                                    <img viewitemid='@item.ViewItemID' class='img-chart' src='@Url.Content("~/Content/chart.png")' alt='show chart' />
                                    @Html.Raw(Space(Math.Max(item.LevelDepth - 1, 0) * 3).Replace(" ", "&nbsp;"))
                                    <a href='@VirtualPathUtility.ToAbsolute("~/View/Data/")?ViewID=@(ViewBag.View.ViewID)&ViewItemID=@(item.ViewItemID)'>
                                    @item.Name
                                    </a>
                                </td>
                            </tr>    
                        End If
                        rowIndex += 1
                    Next
                End Code
            </tbody>
        </table>
    </div>
    <div id='div-body'>
        <table class="table-datatable tablesorter">
            <thead>
            <tr>
                @Code
                    Dim year As Integer = 0
                    Dim cols As Integer = 2
                    For Each item In ViewBag.Dates
                        If year <> item.Date.Year Then
                            If year <> 0 then 
                                @<th class="nosort" colspan="@cols"> @year </th>
                            End If
                            year = item.Date.Year
                            cols = 2
                        Else
                            cols += 2
                        End If
                    Next
                    @<th class="nosort" colspan="@cols"> @year </th>
                End Code
            </tr><tr>
                @Code
                    Dim colsCount As Integer
                    For Each item In ViewBag.Dates
                        @<th colspan="2"> @(Format(item.Date, "d MMM")) </th>    
                        colsCount += 1
                    Next
                End Code
            </tr>
            </thead>
            <tbody>
                @If IsHeader(0) Then
                @:</tr><tr class="tr-header">
                Else
                @:</tr><tr>    
                End If
                @Code
                    Dim cellIndex As Integer = 0
                    rowIndex = 0
                    For Each item As BankQuery.Data In Model.Data
                        If cellIndex Mod colsCount = 0 And cellIndex <> 0 Then
                            rowIndex += 1
                            If IsHeader(rowIndex) Then
                            @:</tr><tr class="tr-header">
                            Else
                            @:</tr><tr>    
                            End If
                        End If
                        If Model.Set.ParID2 Is Nothing Then
                            @<td colspan="2"> 
                                @If item.Value0.HasValue Then
                                    If IsRatio(rowIndex) Then
                                        If item.Value0 <> 0 Then
                                            @((item.Value0.Value / 100).ToString("N2"))    
                                        End If
                                    Else
                                            @item.Value0.Value.ToString("N0")
                                    End If
                                Else
                                            @:&nbsp;
                                End If
                            </td>
                        Else
                            @<td>
                                @If item.Value0.HasValue Then
                                    If IsRatio(rowIndex) Then
                                        If item.Value0 <> 0 Then
                                            @((item.Value0.Value / 100).ToString("N2"))    
                                        End If
                                    Else
                                            @item.Value0.Value.ToString("N0")
                                    End If
                                Else
                                            @:&nbsp;
                                End If
                            </td>
                            @<td>
                                @If item.Value1.HasValue Then
                                    @<span class="span-diff @(IIf(item.Value1.Value > 0, "span-green", IIf(item.Value1.Value < 0, "span-red", "")))">
                                    @If IsRatio(rowIndex) And item.Value0 = 0 Then
                                        @:&nbsp;
                                    Else
                                        If item.Value1.Value <= -99999 Then
                                            @:▼▼▼   
                                        ElseIf item.Value1.Value >= 99999 Then
                                            @:▲▲▲
                                        Else
                                            @((item.Value1.Value / 10000).ToString("P2"))
                                        End If
                                    End If
                                    </span>
                                    End If
                            </td>
                        End If
                        cellIndex += 1
                    Next
                End code
                </tr>
            </tbody>
        </table>
    </div>
</div>

<script type="text/javascript">
    window.datafactory = {
        ddreports: {},
        exreports: {}
    }
    @Html.Partial("_builder.js")
    @Html.Include("~/Scripts/applet.fixedhead.js")
    @Html.Include("~/Scripts/applet.toolbox.js")
</script>        

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

<script>
    $(document).ready(function () {
        $("li#li-tab2").addClass("selected")

        $("#content").find("img.img-chart").click(function () {
            var href = "/Bank/Chart/?ViewID=@(ViewBag.View.ViewID)&BankID=@(ViewBag.Bank.BankID)&SetID=@(Model.Set.SetID)&ViewItemID=" + $(this).attr("viewitemid");
            newwindow = window.open(href, 'name', 'height=600, width=800, resizable=true');
        });

        $("#content").find("img.img-help").click(function () {
            var url = "/Service/Help/?Form=" + $(this).attr("form") + "&AggItemID=" + $(this).attr("aggitemid")
            lightbox(url)
        });

        $("#content").find("span.span-row-dropdown").click(function () {
            var caller = this
            dropDownEvent(caller)
        });

        $("#content").find("span.span-more").click(function () {
            var caller = this
            exDataEvent(caller)
        });

        document.fixhead.apply($("#div-body").find("table.table-datatable"), $("#div-body"))
    }) 
</script>

<script type="text/javascript">
    @Html.Partial("applet.bank-search.js")
    @Html.Partial("applet.lightbox.js")
    @Html.Partial("_dropDownEvent.js")
    @Html.Partial("_exDataEvent.js")
</script>


