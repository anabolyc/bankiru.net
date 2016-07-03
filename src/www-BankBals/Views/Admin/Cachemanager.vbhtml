@Code
    ViewData("Title") = "Cache Manager"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    Cache&nbsp;Manager
</div>

<h3>Banks Data Cache </h3>
<pre>
@Html.Raw(ViewBag.StateString(0))
</pre>

<h3>Views Data Cache </h3>
<pre>
@Html.Raw(ViewBag.StateString(1))
</pre>

<h3>Actions </h3>

<table class='table-clear'>
<tr>
<td>
    <form action="/Admin/ClearCache/" method="post">        <input type="submit" value="Clear Cache" />    </form>
</td>
<td>
    <form action="/Admin/RebuildCache/" method="post">        <input type="submit" value="Rebuild Cache" />    </form>
</td>
<td>
    <form action="/Admin/StopRebuild/" method="post">        <input type="submit" value="Stop Rebuild" />    </form>
</td>
</tr>
</table>
