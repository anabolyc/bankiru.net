@Code
    ViewData("Title") = "API Index"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    API &#8594; 
    @Html.ActionLink("BankbalsTool", "BankbalsTool", "API")
</div>

<table class='table-datatable'>
<thead>
    <tr><th> Name </th><th> Parameters </th><th> links </th></tr>
</thead>
<tbody>
   <tr><td> 
   Banks List
   </td><td>
   -
   </td><td>
   @Html.ActionLink("text", "BanksList", New With {.type = "text"}) |
   @Html.ActionLink("html", "BanksList", New With {.type = "html"}) |
   @Html.ActionLink("xml", "BanksList", New With {.type = "xml"}) |
   @Html.ActionLink("json", "BanksList", New With {.type = "json"})
   </td></tr>

   <tr><td> 
   Dates List
   </td><td>
   -
   </td><td>
   @Html.ActionLink("text", "DatesList", New With {.type = "text"}) |
   @Html.ActionLink("html", "DatesList", New With {.type = "html"}) |
   @Html.ActionLink("xml", "DatesList", New With {.type = "xml"}) |
   @Html.ActionLink("json", "DatesList", New With {.type = "json"})
   </td></tr>
   
   <tr><td> 
   Views List
   </td><td>
   -
   </td><td>
   @Html.ActionLink("text", "ViewsList", New With {.type = "text"}) |
   @Html.ActionLink("html", "ViewsList", New With {.type = "html"}) |
   @Html.ActionLink("xml", "ViewsList", New With {.type = "xml"}) |
   @Html.ActionLink("json", "ViewsList", New With {.type = "json"})
   </td></tr>
   
   <tr><td> 
   Viewitems List 
   </td><td>
   ViewID
   </td><td>
   @Html.ActionLink("text", "ViewitemsList", New With {.type = "text", .ViewID = 1}) |
   @Html.ActionLink("html", "ViewitemsList", New With {.type = "html", .ViewID =1}) |
   @Html.ActionLink("xml", "ViewitemsList", New With {.type = "xml", .ViewID =1}) |
   @Html.ActionLink("json", "ViewitemsList", New With {.type = "json", .ViewID =1})
   </td></tr>
  
   <tr><td> 
   Params List
   </td><td>
   FormID
   </td><td>
   @Html.ActionLink("text", "ParamsList", New With {.type = "text", .FormID = 101}) |
   @Html.ActionLink("html", "ParamsList", New With {.type = "html", .FormID = 101}) |
   @Html.ActionLink("xml", "ParamsList", New With {.type = "xml", .FormID = 101}) |
   @Html.ActionLink("json", "ParamsList", New With {.type = "json", .FormID = 101})
   </td></tr>

   <tr><td> 
   Curitems List
   </td><td>
   -
   </td><td>
   @Html.ActionLink("text", "CuritemsList", New With {.type = "text"}) |
   @Html.ActionLink("html", "CuritemsList", New With {.type = "html"}) |
   @Html.ActionLink("xml", "CuritemsList", New With {.type = "xml"}) |
   @Html.ActionLink("json", "CuritemsList", New With {.type = "json"})
   </td></tr>

   <tr><td> 
   One Bank Data
   </td><td>
   BankID, ViewID
   </td><td>
   @Html.ActionLink("html", "GetExcelWebService", "Bank", New With {.BankID = 1000, .ViewID = 1}, Nothing)
   </td></tr>

   <tr><td> 
   One Viewitem Data
   </td><td>
   ViewID, ViewItemID
   </td><td>
   @Html.ActionLink("html", "GetExcelWebService", "View", New With {.ViewID = 1, .ViewItemID = 1}, nothing)
   </td></tr>

   <tr><td> 
   One Date Data
   </td><td>
   DateID, ViewID
   </td><td>
   @Html.ActionLink("html", "GetOneDateData", "API", New With {.DateID = 508, .ViewID = 1}, Nothing)
   </td></tr>

</tbody>
</table>

<br />
