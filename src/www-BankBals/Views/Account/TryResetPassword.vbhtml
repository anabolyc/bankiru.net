@ModelType TryResetPassModel

@Code
    ViewData("Title") = "Сброс пароля"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

 @Html.ValidationSummary(True, "")

@Using Html.BeginForm()
    @<div>
        <fieldset>
            <legend>Сброс пароля</legend>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.Password)
            </div>
            <div class="editor-field">
                @Html.PasswordFor(Function(m) m.Password)
                @Html.ValidationMessageFor(Function(m) m.Password)
            </div>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.ConfirmPassword)
            </div>
            <div class="editor-field">
                @Html.PasswordFor(Function(m) m.ConfirmPassword)
                @Html.ValidationMessageFor(Function(m) m.ConfirmPassword)
            </div>

            <p>
                <input type="submit" value="Сбросить" />
            </p>
            
        </fieldset>
    </div>
End Using


