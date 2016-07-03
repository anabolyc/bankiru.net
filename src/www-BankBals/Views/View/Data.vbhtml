@Imports www.BankBals.Data

@ModelType ViewQuery.Result

@Code
    ViewData("Title") = ViewBag.View.ViewNameRus + ": " + ViewBag.AggItem.NameRus
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Рэнкинги", "List", "View") &#8594; 
    <span id='viewslist' class='span-ui-dropdown'>
        <input class='sw' id='view-srch' type='text' value='@ViewBag.AggItem.NameRus'/>
    </span>
</div>
@* <link href="@Url.Content("~/Content/tablesorter.css")" rel="stylesheet" type="text/css" /> *@

@Code
    If Common.Placement() = Common.Placements.PRODUCTION Then
@<div class='div-toplinks'>
    <a class='a-save' href="@VirtualPathUtility.ToAbsolute("~/View/GetExcel")?ViewID=@(ViewBag.View.ViewID)&ViewItemID=@(ViewBag.ViewItem.ViewItemID)&SetID=@(Model.Set.SetID)" > Сохранить обновляемый</a>
    <a class='a-save' href="@VirtualPathUtility.ToAbsolute("~/View/GetExcel2")?ViewID=@(ViewBag.View.ViewID)&ViewItemID=@(ViewBag.ViewItem.ViewItemID)&SetID=@(Model.Set.SetID)" > необновляемый</a>
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
                                    <a href='@VirtualPathUtility.ToAbsolute("~/View/Data/")?ViewID=@(ViewBag.View.ViewID)&ViewItemID=@(ViewBag.ViewItem.ViewItemID)&SetID=@(item.SetID)'>@itemName</a>
                                </span>
                            End If
                        Next
                    </th>
                </tr>
                <tr>
                    <th>#TA</th>
                    <!--
                    if (document.state.sortedColIndex == 0)
                    th.className = (!document.state.sortedDirection[document.state.sortedColIndex] ? "headerSortUp" : "headerSortDown")
                    -->
                    <th>Reg.Num</th>
                    <th>Name</th>
                </tr>
            </thead>
            <tbody>
                @Code
                    Dim index As Nullable(Of Integer) = Nothing
                    Dim i As Integer = 0
                    For Each item As ViewQuery.Dictionary In Model.Dictionary
                        If item.BankID > 0 Then
                            index = If(index, 0) + 1
                        End If
                        i += 1
                        @<tr> 
                            <td>@index</td>
                            <td>
                                <a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=@(item.BankID)&ViewID=@(ViewBag.View.ViewID)'>
                                @Iif(item.BankID >0, item.BankID, Nothing)
                                </a>
                            </td>
                            <td><img bankid='@item.BankID' class='img-chart' src='@Url.Content("~/Content/chart.png")' alt='show chart' />@item.Name</td>
                        </tr>    
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
                <tr>
                @Code
                    Dim cellIndex As Integer = 0
                    For Each item As ViewQuery.Data In Model.Data
                        If cellIndex Mod colsCount = 0 And cellIndex <> 0 Then
                                @:</tr><tr>
                            End If
                        If Model.Set.ParID2 Is Nothing Then
                            @<td colspan="2"> 
                                @If item.Value0.HasValue Then
                                    If ViewBag.IsRatio Then
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
                                    If ViewBag.IsRatio Then
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
                                    @If ViewBag.IsRatio And item.Value0 = 0 Then
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
    window.datafactory = {}
    @Html.Include("~/Scripts/applet.fixedhead.js")
    @Html.Include("~/Scripts/applet.toolbox.js")
</script>        

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

<script type="text/javascript">
    $(document).ready(function () {
        $("li#li-tab3").addClass("selected")

        $("#content").find("img.img-chart").click(function () {
            var href = "/View/Chart/?ViewID=@(ViewBag.View.ViewID)&ViewItemID=@(ViewBag.ViewItem.ViewItemID)&SetID=@(Model.Set.SetID)&BankID=" + $(this).attr("bankid");
            newwindow = window.open(href, 'name', 'height=600, width=800, resizable=true');
        });

        document.fixhead.apply($("#div-body").find("table.table-datatable"), $("#div-body"))

    });
</script>

<script type="text/javascript">
    @Html.Partial("applet.view-search.js")
</script>

