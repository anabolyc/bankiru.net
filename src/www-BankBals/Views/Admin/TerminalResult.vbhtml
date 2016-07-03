@ModelType www.BankBals.Models.TerminalFactory.Terminal

@Code
    ViewData("Title") = "Terminal"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    Terminal
</div>

<h3>Terminal Result</h3>

@Html.Partial("~/Views/Shared/applet.log.js.vbhtml")

@If Not Model Is Nothing Then

@<pre>
Process ID   : @Model.ID
Error Message: @Model.ErrorMsg
</pre>    

@<pre>
<span class='span-terminal'> </span>
</pre>
    
@<script type="text/javascript">
     var terminal = $(".span-terminal")[0]
     var RowNumber = 0
     var fetchTerminalResult = function () { 
        $.ajax({
            url: "@Url.Content("~/Admin/TerminalUpdateJSON")",
        data: "ID=" + "@Model.ID" + "&RowNumber=" + RowNumber,
        dataType: "json",
        type: "POST",
        error: function () {
            window.log("ajax failed")
        },
        success: function (response) { 
            window.log("got data: " + response.ErrorCode + ": " + response.ErrorMessage)

                if (response.ErrorCode != 0) {
                    return;
                }
                RowNumber = response.RowNumber

                for (var i = 0; i < response.Rows.length; i++)
                    
                    terminal.innerText += response.Rows[i] + "\n"

                if (!response.Finished) {
                    window.setTimeout(fetchTerminalResult, 1000)
                }
            } // ~success
        }); // ~ajax
        }

     $(document).ready(function () {
         window.setTimeout(fetchTerminalResult, 1000)
     })
</script>
End If
