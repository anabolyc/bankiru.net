@Code
    ViewData("Title") = "Create Role"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    @Html.ActionLink("Roles", "Roles", "Admin") &#8594; 
    Create Role
</div>

<h3>Create Role</h3>

@Using Html.BeginForm()
    @<div>

    <input type="text" name="Role"/>
    <p>
        @Html.AntiForgeryToken()
        <input type="submit" value="Готово" />
    </p>

    </div>
End Using