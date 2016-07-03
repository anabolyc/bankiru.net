@Code
    ViewData("Title") = "Admin"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    Admin
</div>

<h3>Admin pages</h3>
<ul>
@*
<li>    
    @Html.ActionLink("Cache Manager", "CacheManager")
</li>
*@
<li>    
    @Html.ActionLink("Roles", "Roles")
</li>
<li>    
    @Html.ActionLink("Users", "Users")
</li>
<li>    
    @Html.ActionLink("DB Console", "DBConsole")
</li>
<li>    
    @Html.ActionLink("Upload file (CBR archive)", "UploadFile", "API", Nothing, Nothing)
</li>

@*
<li>    
    @Html.ActionLink("Terminal", "Terminal")
</li>
*@
</ul>
    
    