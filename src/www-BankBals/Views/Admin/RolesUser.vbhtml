@ModelType Object

@Code
    ViewData("Title") = "Set Roles: " + Model(0)
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    @Html.ActionLink("Users", "Users", "Admin") &#8594; 
    Set User Roles
</div>

<h3>Set Roles: @Model(0)</h3>

<table>
    <tr>
        <th>Role</th>
        <th>In it</th>
        <th>Actions</th>
    </tr>
    @Code
        For Each Role In System.Web.Security.Roles.GetAllRoles()
            @<tr>
                <td> @Role </td>
                <td> @IIf( System.Web.Security.Roles.IsUserInRole(Model(0), Role), "YES", "NO")</td>
                <td> 
                    @Using Html.BeginForm()
                    @<input type="hidden" name="UserName" value="@Model(0)"/>
                    @<input type="hidden" name="RoleName" value="@Role"/>
                    @Html.AntiForgeryToken()
                    @<input type="submit" value="Toggle" />
                    End Using
                </td>
            </tr>
        Next
    End Code
</table>



