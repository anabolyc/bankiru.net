@ModelType ResetPassModel

@Code
    ViewData("Title") = "Сбросить пароль"
End Code

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

 @Html.ValidationSummary(True, "")

@Using Html.BeginForm()
    @<div>
        <fieldset>
            <legend>Сбросить пароль</legend>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.UserName)
            </div>
            <div class="editor-field">
                @Html.TextBoxFor(Function(m) m.UserName)
                @Html.ValidationMessageFor(Function(m) m.UserName)
            </div>

            <p>
                <input type="submit" value="Послать" />
            </p>

        </fieldset>
    </div>
End Using
