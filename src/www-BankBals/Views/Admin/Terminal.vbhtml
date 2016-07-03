@Code
    ViewData("Title") = "Terminal"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    Terminal
</div>

<h3>Terminal</h3>

@Using Html.BeginForm()
    @<div>
    
    <input type="hidden" name="ID" value="@ViewData("ID")" />
    <textarea rows=1 cols=100 name="Command">@ViewData("Command")</textarea>
    <p>
        @Html.AntiForgeryToken()
        <input type="submit" value="@IIf(ViewData("ID").Equals(String.Empty), "Send", "Refresh")" />
    </p>
    </div>
End Using

