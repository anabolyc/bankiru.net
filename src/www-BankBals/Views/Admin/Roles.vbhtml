@ModelType String()

@Code
    ViewData("Title") = "Roles"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    Roles
</div>

<h3>Roles</h3>

<table>
<tr>
    <th>Name</th>
    <th>Actions</th>
</tr>                
@Code
    For Each Role As String In Model
        @<tr>
            <td>@Role</td>
            <td>
                @Html.ActionLink("Delete", "DeleteRole", New With {.Role = Role})
            </td>
        </tr>                
    Next
End Code
</table>

<br />
@Html.ActionLink("Create New", "CreateRole")