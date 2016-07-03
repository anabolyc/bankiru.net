@ModelType String

@Code
    ViewData("Title") = "Database Console"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    DB&nbsp;Console
</div>

@Using Html.BeginForm()
    @<div>

    <textarea rows=1 cols=100 name="Command">@ViewData("Command")</textarea>
    <p>
        @Html.AntiForgeryToken()
        <input type="submit" value="Send" />
    </p>

    </div>
End Using

@If Model <> "" Then

@<span class="span-terminal">
Command:<br />
    @ViewData("Command")<br /><br />
Result:<br /><br />
    @Html.Raw(Model)
</span>

End If
