@ModelType Object

@Code
    ViewData("Title") = "Delete User: " + Model(0)
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    @Html.ActionLink("Users", "Users", "Admin") &#8594; 
    Delete User
</div>

<h3>Delete User: @Model(0)</h3>

@Using Html.BeginForm()
    @<div>

    <input type="text" name="Role" value="@Model(0)"/>
    <input type="hidden" name="Confirm" value="true"/>
    
    <p>
        @Html.AntiForgeryToken()
        <input type="submit" value="Delete" />
    </p>

    </div>
End Using