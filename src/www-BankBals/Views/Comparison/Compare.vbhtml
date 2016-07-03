@Imports www.BankBals.Data
@ModelType IEnumerable(Of CompQuery.Result)()

@Code
    ViewData("Title") = ViewBag.Bank.NameRus & " - Сравнение"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Сравнить банки", "Index", "Comparison") &#8594; 
    <span id='bankslist' class='span-ui-dropdown'>  
        <input class='sw' id='bank-srch' type='text' value='@ViewBag.Bank.NameRus' />
    </span>
</div>

<ul class='ul-views'>
@Code
    For Each item As A_VIEWS_BANKSPIC In ViewBag.Views
        If item.ViewID = ViewBag.ViewID Then
            @<li class="selected"><a name="tab">@(item.ViewNameRus)</a></li>    
        Else
            @<li>
            <a href="@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=@(item.ViewID)&BankID0=@(ViewBag.Bank.BankID)&BankID1=@(ViewBag.Bank.BankIDC1)&BankID2=@(ViewBag.Bank.BankIDC2)">@item.ViewNameRus</a>
            </li>
        End If
    Next
    @<li>
        <a href="@VirtualPathUtility.ToAbsolute("~/Comparison/CompareBubbles/")?BankID=@(ViewBag.Bank.BankID)">Картинкой</a>
    </li>
End Code
</ul>

<script type="text/javascript">
    @Html.Include("~/Scripts/applet.fixedhead.js")
    @Html.Include("~/Scripts/applet.toolbox.js")
</script>        

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")
@Html.Partial("_bankslist.js", "TABLE")

<script type="text/javascript">
    var BankID = @ViewBag.Bank.BankID
    window.datafactory = new Object()
</script>

<div class='div-datatable-cont' id='cont'>
<table class='table-datatable table-comparison'>
<thead>
    <tr>
        <th rowspan='2'>NameRus</th>
        <th colspan='3'>@ViewBag.Bank.NameRus</th>
        <th colspan='3'><input bankid="@ViewBag.BankC1.BankID" class="input-bank-c" id="input-bank-c1" type="text" value="@ViewBag.BankC1.NameRus" /></th>
        <th colspan='3'><input bankid="@ViewBag.BankC2.BankID" class="input-bank-c" id="input-bank-c2" type="text" value="@ViewBag.BankC2.NameRus" /></th>
        <th colspan='2'>@ViewBag.BankC3.NameRus</th>
        <th colspan='2'>@ViewBag.BankC4.NameRus</th>
    </tr>
    <tr>
        <th> RUB ths </th><th>#</th><th> Chg</th>
        <th> RUB ths </th><th>#</th><th> Chg</th>
        <th> RUB ths </th><th>#</th><th> Chg</th>
        <th> RUB ths </th><th>Chg</th>
        <th> RUB ths </th><th>Chg</th>
    </tr>
</thead>
<tbody>
@Code
    Dim i As Integer = 0
    For Each item As A_VIEWITEMS_COMPARE In ViewBag.ViewItems
    @<tr class='@(IIf(item.IsHeader = 1, "tr-header" & item.LevelID, ""))'>
        
        <td class='td-leftaligned' colspan='@(IIf(item.IsHeader = 1, "14", "1"))'>
            @Html.Raw(Space(IIf(item.LevelID>0, item.LevelID, 0)).Replace(" ", "&nbsp;&nbsp;"))
            @item.NameRus 
        </td>
        
        @For j As Integer = LBound(Model) To UBound(Model)
                If item.IsHeader = 0 Then
                @: <td>
                If Not IsNothing(Model(j)(i).Value) Then
                    If item.IsRatio Then
                        @: @CDbl(Model(j)(i).Value / 100).ToString("F")
                    Else
                        @: @CLng(Model(j)(i).Value).ToString("N0")
                    End If
                Else
                    @: -
                End If
                @: </td>
                If j < 3 Then
                @: <td class='td-img-bars'>
                If Not IsNothing(Model(j)(i).Rank) Then
                    @<img src="@Url.Content("~/Content/bars/" & Model(j)(i).Rank & ".png")" alt="#@(Model(j)(i).Rank)" />
                End If
                @: </td>
                End If
                @: <td>
                If Not IsNothing(Model(j)(i).Chg) Then
                    @: <span class='span-change'>
                    @: @Model(j)(i).Chg.ToString()
                    @: </span>
                End If
                @: </td>
            End If
        Next
    </tr>
    i += 1
    Next
End Code
</tbody>
</table>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $("li#li-tab5").addClass("selected")
window.log("start changes")
        $("span.span-change").each(function (index, e) {
            $(e).replaceWith(window.toolbox.getChangeElement($(e).text()))
        })
window.log("finish changes")
    })
</script>

<script src="@Url.Content("~/Scripts/Highcharts-3.0.0/js/highcharts.js")" type="text/javascript"></script>
