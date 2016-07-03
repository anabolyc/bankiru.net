@ModelType LogOnModel

@Code
    ViewData("Title") = "Войти на сайт"
End Code

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

@Html.ValidationSummary(True, "")

@Using Html.BeginForm()
    @<div>
        <fieldset>
            <legend>Войти на сайт</legend>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.UserName)
            </div>
            <div class="editor-field">
                @Html.TextBoxFor(Function(m) m.UserName)
                @Html.ValidationMessageFor(Function(m) m.UserName)
            </div>

            <div class="editor-label">
                @Html.LabelFor(Function(m) m.Password)
            </div>
            <div class="editor-field">
                @Html.PasswordFor(Function(m) m.Password)
                @Html.ValidationMessageFor(Function(m) m.Password)
            </div>

            <div class="editor-label">
                @Html.CheckBoxFor(Function(m) m.RememberMe)
                @Html.LabelFor(Function(m) m.RememberMe)
            </div>

            <p>
                @Html.AntiForgeryToken()
                <input type="submit" value="Войти" />
            </p>
            <p>
                @Html.ActionLink("Регистрация", "Register") &nbsp;
                @Html.ActionLink("Сбросить пароль", "ResetPassword") &nbsp;
                @Html.ActionLink("Изменить пароль", "ChangePassword") &nbsp;
            </p>
        </fieldset>
    </div>
End Using
