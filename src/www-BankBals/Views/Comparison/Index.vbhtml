@Imports www.BankBals.Data
@ModelType List(Of A_BANKS_ALL)

@Code
    ViewData("Title") = "Сравнение банков"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    Сравнить банки
</div>

<div class='div-datatable-cont' id='cont'> 
    <input class='sw' id='bank-srch' type='text'/><br/><br/>

    <table class='texttable' id='table-list'>  
    <tr><th>Рег. номер</th><th>Название</th></tr>  
    @Code
        For Each item As A_BANKS_ALL In Model
            If item.BankID > 0 Then
            @<tr>
                <td>@(item.BankID)</td>
                <td><a href='@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=1&BankID0=@(item.BankID)&BankID1=@(item.BankIDC1)&BankID2=@(item.BankIDC2)'>@(item.NameRus)</a>
                </td>
            </tr>
            End If
        Next
    End Code
    </table>
</div>

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

<script type="text/javascript">

    $(document).ready(function () {
        $("li#li-tab5").addClass("selected")
        window.log("sent request")
        $.ajax({
            url: "@Url.Content("~/Bank/ListJSON")",
            dataType: "json",
            data: "LoadComparables=true",
            type: "POST",
            error: function () {
                $("#bank-srch").remove()
                window.log("ajax failed")
            },
            success: function (response) {
                var bankslist = response
                var data = []
                for (var i = 0; i < bankslist.length; i++) {
                    var item = {}
                    item.BankID = bankslist[i].BankID
                    item.BankName = bankslist[i].BankName
                    item.label = bankslist[i].BankID + ' - ' + bankslist[i].BankName
                    item.BankIDC1 = bankslist[i].BankIDC1
                    item.BankIDC2 = bankslist[i].BankIDC2
                    data.push(item)
                }

                $("#bank-srch").autocomplete({ 
                    autoFocus: true,
                    delay: 200,
                    select: function( event, ui) {
                        window.location = "@VirtualPathUtility.ToAbsolute("~/Comparison/Compare/")?ViewID=1&BankID0=" + ui.item.BankID + "&BankID1=" + ui.item.BankIDC1 + "&BankID2=" + ui.item.BankIDC2 
                    },
                    source: data
                })
                window.log("finished ui")
            }
        });

    });
</script>

