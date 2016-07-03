@ModelType List(Of System.Web.Security.MembershipUser)

@Code
    ViewData("Title") = "Users"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    Users
</div>

<h3>Users</h3>

<table>
<tr>
    <th>CreationDate</th>
    <th>UserName</th>
    <th>Email</th>
    <th>Admin</th>
    <th>Approved</th>
    <th>Locked</th>
    <th>Online</th>
    <th>LastLoginDate</th>
    <th>Actions</th>
</tr>                
@Code
    For Each User As System.Web.Security.MembershipUser In Model
        @<tr>
            <td>@User.CreationDate</td>
            <td>@User.UserName</td>
            <td>@User.Email</td>
            <td>@IIf(System.Web.Security.Roles.IsUserInRole(User.UserName, "Administrator"), "YES", "NO")</td>
            <td>@Iif(User.IsApproved, "YES", "NO")</td>
            <td>@IIf(User.IsLockedOut, "YES", "NO")</td>
            <td>@Iif(User.IsOnline, "YES", "NO")</td>
            <td>@User.LastLoginDate</td>
            <td>
                @Html.ActionLink("Edit", "EditUser", New With {.UserName = User.UserName})
                @Html.ActionLink("Roles", "RolesUser", New With {.UserName = User.UserName})
                @Html.ActionLink("Delete", "DeleteUser", New With {.UserName = User.UserName})
            </td>
        </tr>                
    Next
End Code
</table>