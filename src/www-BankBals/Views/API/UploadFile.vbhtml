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
        Dim log As Logger = ViewBag.Log
        Dim tgt As Targets.MemoryTarget = ViewBag.Target
        For each s As string In tgt.Logs
            @<text>
                @s
                <br />
            </text>
        Next
    End If
End Code

@Html.ValidationSummary(false)

@Using Html.BeginForm("UploadFile", "API", FormMethod.Post, New With {.enctype = "multipart/form-data"})
    @<div>
        <fieldset>
            <legend>Specify path: (like /credit/forms/101-20141001.rar for example)</legend>
            <br />
            <input type="text" name="fileUrl" id="fileUrl" />
            <p>
                <input type="submit" value="Upload" />
            </p>
        </fieldset>
    </div>
End Using 


