@ModelType Object

@Code
    ViewData("Title") = "Edit User: " & Model(0)
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    @Html.ActionLink("Users", "Users", "Admin") &#8594; 
    Edit User
</div>

<h3>Edit User: @Model(0)</h3>

Not implemented!
