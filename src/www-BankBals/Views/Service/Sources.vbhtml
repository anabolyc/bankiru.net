@Imports www.Bankbals.Data
@ModelType IEnumerable(Of SourcesData.Data)

@Code
    ViewData("Title") = "Sources Map"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Service", "Index", "service") &#8594; 
    Sources Map (Form <b>@ViewBag.Form</b>)
</div>

<table class="table-datatable table-sources-legend">
<tr> <td class='td-blue-darker'>CBR</td><td>Данные с <a href="http://www.cbr.ru/">cbr.ru</a> с оборотами </td> </tr>
<tr> <td class='td-blue'>CBR</td><td>Данные с <a href="http://www.cbr.ru/">cbr.ru</a> без оборотов </td> </tr>
<tr> <td class='td-yellow-darker'>MBK</td><td>Данные с <a href="http://mbkcentre.ru/">mbkcentre.ru</a> с оборотами </td> </tr>
<tr> <td class='td-yellow'>MBK</td><td>Данные с <a href="http://mbkcentre.ru/">mbkcentre.ru</a> без оборотов </td> </tr>
<tr> <td>---</td><td>Данных нет</td> </tr>
</table>

<table class="table-datatable table-sources">
<thead>
    <tr>
    <th>Reg.Num.</th>
    <th>Name</th>
    @For Each item As A_DATE In ViewBag.Dates
        @<th>@item.Date.ToString("yyyy-MM-dd")</th>    
    Next
    </tr></thead>
    <tbody>
    @Code
        Dim BankID As Integer = 0
    For Each item As SourcesData.Data In Model
            If BankID <> item.BankID Then
                If BankID <> 0 Then
                    @:</tr>
                End If
                @:<tr>
                @<td><a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=@(item.BankID)&ViewID=1'>@item.BankID</a></td>
                @<td>@item.NameRus</td>
                BankID = item.BankID
            End If
            If Not item.Src.HasValue Then
                @<td>-</td>
            ElseIf item.Src.Value = 1 Then
                @<td class='td-blue'>CBR</td>
            ElseIf item.Src.Value = 11 Then
                @<td class='td-blue-darker'>CBR</td>
            ElseIf item.Src.Value = 0 Then
                @<td class='td-yellow'>MBK</td>
            ElseIf item.Src.Value = 10 Then
                @<td class='td-yellow-darker'>MBK</td>
            End If
        Next
    End Code
</tr>
</tbody>
</table>
