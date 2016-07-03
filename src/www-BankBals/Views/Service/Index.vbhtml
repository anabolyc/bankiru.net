@Imports www.BankBals.Data
@ModelType List(Of A_AGG)

@Code
    ViewData("Title") = "Service Index"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    Service
</div>


<h3>Sources map</h3>
<ul>
<li>    
    @Html.ActionLink("Form 101", "Sources", New With {.ID = "101"})
</li>
<li>    
    @Html.ActionLink("Form 102", "Sources", New With {.ID = "102"})
</li>
<li>    
    @Html.ActionLink("Form 123", "Sources", New With {.ID = "123"})
</li>
<li>    
    @Html.ActionLink("Form 134", "Sources", New With {.ID = "134"})
</li>
<li>    
    @Html.ActionLink("Form 135", "Sources", New With {.ID = "135"})
</li>
</ul>


<h3>Aggregates Members List</h3>
<ul>
@Code
For Each item As A_AGG In Model
    @<li>
        @Html.ActionLink(item.FullNameRUS, "AggsList", New With {.AggID = item.AggID})
    </li>
Next
End Code
</ul>
