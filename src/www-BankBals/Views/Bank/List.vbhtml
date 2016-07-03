@Imports www.BankBals.Data
@ModelType List(Of A_BANKS_ALL)
    
@Code
    ViewData("Title") = "Список банков"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    Банки
</div>

<div class='div-datatable-cont' id='cont'> 
    <input class='sw' id='bank-srch' type='text'/><br/><br/>

    <table class='texttable' id='table-list'>  
    <tr><th>Рег. номер</th><th>Название</th></tr>  
    @Code
        For Each item As A_BANKS_ALL In Model
            @<tr>
                <td>@(IIf(item.BankID > 0, item.BankID, ""))</td>
                <td><a href='@VirtualPathUtility.ToAbsolute("~/Bank/Data/")?BankID=@(item.BankID)&ViewID=1'>@(item.NameRus)</a></td>
             </tr>
        Next
    End Code
    </table>
</div>

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

<script type="text/javascript">

    $(document).ready(function () {
        $("li#li-tab2").addClass("selected")
        
        @Html.Partial("applet.bank-search.js")
    });
</script>

