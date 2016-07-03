@ModelType ChangePasswordModel

@Code
    ViewData("Title") = "Change Password"
End Code

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

@Using Html.BeginForm()
    @Html.ValidationSummary(True, "Пароль не изменен. Исправьте ошибки и попробуйте снова.")
    @<div>
        <fieldset>
            <legend>Изменить пароль</legend>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.OldPassword)
            </div>
            <div class="editor-field">
                @Html.PasswordFor(Function(m) m.OldPassword)
                @Html.ValidationMessageFor(Function(m) m.OldPassword)
            </div>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.NewPassword)
            </div>
            <div class="editor-field">
                @Html.PasswordFor(Function(m) m.NewPassword)
                @Html.ValidationMessageFor(Function(m) m.NewPassword)
            </div>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.ConfirmPassword)
            </div>
            <div class="editor-field">
                @Html.PasswordFor(Function(m) m.ConfirmPassword)
                @Html.ValidationMessageFor(Function(m) m.ConfirmPassword)
            </div>

            <p>
                <input type="submit" value="Сменить" />
            </p>
        </fieldset>
    </div>
End Using
