@Imports NLog

@Code
    ViewData("Title") = "UploadFile"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class='div-bread'> 
    @Html.ActionLink("Главная", "Index", "Home") &#8594; 
    @Html.ActionLink("Admin", "Index", "Admin") &#8594; 
    Upload File
</div>

@Code
If Not ViewBag.Log is Nothing Then
    Dim log As IEnumerable(Of String) = ViewBag.Log
    For Each s As String In log
        @<text>
            @s
            <br />
        </text>
    Next
End If

Dim FU As New FileUploader2
For Each s As String In FU.GetMessages()
        @<text>
            @s
            <br />
        </text>
Next
End Code

@Html.ValidationSummary(false)

@Using Html.BeginForm("UploadFile2", "Admin", FormMethod.Post, New With {.enctype = "multipart/form-data"})
    @<div>
        <fieldset>
            <legend>Choose file</legend>
            <br />
            <select name="FormID">
                <option value="101">101</option>
                <option value="102">102</option>
                <option value="123">123</option>
                <option value="134">134</option>
                <option value="135">135</option>
            </select>
            
            <select name="DateID">
                @For Each D As A_DATE In ViewBag.Dates
                @<option value="@D.DateID">@D.Date.ToString("yyyy-MM-dd")</option>
                Next
            </select>
            <br /><br />
            <input type="file" name="postedFile" id="postedFile" />
            <p>
                <input type="submit" value="Upload" />
            </p>
        </fieldset>
    </div>
            End Using


