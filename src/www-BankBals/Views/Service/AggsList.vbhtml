@Imports www.BankBals.Data
@ModelType List(Of W_AGG_COMP)
    
@Code
    ViewData("Title") = "Agregate Members Data"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Service", "Index", "Service") &#8594; 
    Aggregate Members List (@ViewBag.AggName)
</div>

<table class="table-datatable table-aggsmembers">
<thead>
    <tr>
    <th>  Reg.Num.  </th>
    <th>  Name  </th>
    </tr>
</thead>

<tbody>
    @Code
    For Each item As W_AGG_COMP In Model
        @<tr>
        <td>@item.BankID</td>
        <td><a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=@(item.BankID)&ViewID=1'>@item.A_BANK.FullNameRUS</a></td>
        </tr>
    Next
    End Code
</tbody>
</table>