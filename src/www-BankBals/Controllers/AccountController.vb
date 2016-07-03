Imports www.BankBals.Classes
Imports www.BankBals.Data
Imports www.BankBals.Models
Imports NLog

Namespace Controllers

    Public Class AccountController
        Inherits BaseController

        Public Function LogOn() As ActionResult
            Return View()
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken()> _
        Public Function LogOn(ByVal model As LogOnModel, ByVal returnUrl As String) As ActionResult
            If ModelState.IsValid Then
                If Membership.ValidateUser(model.UserName, model.Password) Then
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe)
                    If Url.IsLocalUrl(returnUrl) AndAlso returnUrl.Length > 1 AndAlso returnUrl.StartsWith("/") _
                       AndAlso Not returnUrl.StartsWith("//") AndAlso Not returnUrl.StartsWith("/\\") Then
                        Return Redirect(returnUrl)
                    Else
                        Return RedirectToAction("Index", "Home")
                    End If
                Else
                    ModelState.AddModelError("", "Неправильное имя пользователя или пароль")
                End If
            End If

            ' If we got this far, something failed, redisplay form
            Return View(model)
        End Function

        Public Function ResetPasswordSent() As ActionResult
            Return View()
        End Function

        Public Function TryResetPassword(ByVal Key As Guid) As ActionResult
            Dim model As New TryResetPassModel
            Dim context As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
            Dim rpk As aspnet_ResetPasswordKey = context.aspnet_ResetPasswordKeys.FirstOrDefault(Function(K) K.Key = Key)
            If rpk Is Nothing Then
                ModelState.AddModelError("", "Этот ключ сброса не зарегистрирован")
            End If
            Return View(model)
        End Function

        <HttpPost()> _
        Public Function TryResetPassword(ByVal model As TryResetPassModel, ByVal Key As Guid) As ActionResult
            If ModelState.IsValid Then
                Dim context As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
                Dim currentUser As MembershipUser
                Dim rpk As aspnet_ResetPasswordKey = context.aspnet_ResetPasswordKeys.FirstOrDefault(Function(K) K.Key = Key)
                Dim changePasswordSucceeded As Boolean
                If rpk Is Nothing Then
                    currentUser = Nothing
                    ModelState.AddModelError("", "Этот ключ сброса не зарегистрирован")
                Else
                    currentUser = Membership.GetUser(rpk.UserId)
                    changePasswordSucceeded = currentUser.ChangePassword(currentUser.ResetPassword(), model.Password)
                End If

                If changePasswordSucceeded Then
                    context.aspnet_ResetPasswordKeys.DeleteOnSubmit(rpk)
                    context.SubmitChanges()
                    FormsAuthentication.SetAuthCookie(currentUser.UserName, False)
                    Return RedirectToAction("Index", "Home")
                Else
                    ModelState.AddModelError("", "Произошла ошибка при смене пароля :(")
                End If
            End If

            Return View(model)
        End Function

        Public Function ResetPassword() As ActionResult
            Return View()
        End Function

        <HttpPost()> _
        Public Function ResetPassword(ByVal model As ResetPassModel, ByVal returnUrl As String) As ActionResult
            If ModelState.IsValid Then
                Dim u As MembershipUser
                u = Membership.GetUser(model.UserName)
                If u Is Nothing Then
                    Dim username As String = Membership.GetUserNameByEmail(model.UserName)
                    If username <> String.Empty Then u = Membership.GetUser(username)
                End If
                If Not u Is Nothing Then
                    Dim ID As Guid = GUID.NewGuid()
                    Try
                        Dim context As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
                        Dim user As aspnet_User = context.aspnet_Users.SingleOrDefault(Function(Us) Us.UserId = u.ProviderUserKey)

                        If context.aspnet_ResetPasswordKeys.Where(Function(Us) Us.UserId = u.ProviderUserKey).Count = 0 Then
                            Dim key As New aspnet_ResetPasswordKey
                            key.Key = ID
                            key.ApplicationId = user.ApplicationId
                            key.aspnet_User = user
                            context.aspnet_ResetPasswordKeys.InsertOnSubmit(key)
                        Else
                            Dim key As aspnet_ResetPasswordKey = context.aspnet_ResetPasswordKeys.First(Function(Us) Us.UserId = u.ProviderUserKey)
                            key.Key = ID
                            key.ApplicationId = user.ApplicationId
                        End If
                        context.SubmitChanges()
                        Common.MailNotifier.SendResetPasswordMessage(u.Email, ID)
                        Return (RedirectToAction("ResetPasswordSent"))
                    Catch e As Exception
                        ModelState.AddModelError("", "Произошла внутренняя ошибка :(")
#If DEBUG Then
                        ModelState.AddModelError("", e.Message)
#End If
                    End Try
                Else
                    ModelState.AddModelError("", "Нет пользователья с такими данными")
                End If
            End If
            Return View(model)
        End Function

        Public Function LogOff() As ActionResult
            FormsAuthentication.SignOut()
            Return RedirectToAction("Index", "Home")
        End Function

        Public Function Register() As ActionResult
            Return View()
        End Function

        <HttpPost()> _
        Public Function Register(ByVal model As RegisterModel) As ActionResult
            If ModelState.IsValid Then
                ' Attempt to register the user
                Dim createStatus As MembershipCreateStatus
                Membership.CreateUser(model.UserName, model.Password, model.Email, Nothing, Nothing, True, Nothing, createStatus)

                If createStatus = MembershipCreateStatus.Success Then
                    FormsAuthentication.SetAuthCookie(model.UserName, False)
                    Return RedirectToAction("Index", "Home")
                Else
                    ModelState.AddModelError("", ErrorCodeToString(createStatus))
                End If
            End If

            ' If we got this far, something failed, redisplay form
            Return View(model)
        End Function

        <Authorize()> _
        Public Function ChangePassword() As ActionResult
            Return View()
        End Function

        <Authorize()> _
        <HttpPost()> _
        Public Function ChangePassword(ByVal model As ChangePasswordModel) As ActionResult
            If ModelState.IsValid Then
                ' ChangePassword will throw an exception rather
                ' than return false in certain failure scenarios.
                Dim changePasswordSucceeded As Boolean

                Try
                    Dim currentUser As MembershipUser = Membership.GetUser(User.Identity.Name, True)
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword)
                Catch ex As Exception
                    changePasswordSucceeded = False
                End Try

                If changePasswordSucceeded Then
                    Return RedirectToAction("ChangePasswordSuccess")
                Else
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.")
                End If
            End If

            ' If we got this far, something failed, redisplay form
            Return View(model)
        End Function

        Public Function ChangePasswordSuccess() As ActionResult
            Return View()
        End Function

#Region "Status Code"
        Public Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
            ' See http://go.microsoft.com/fwlink/?LinkID=177550 for
            ' a full list of status codes.
            Select Case createStatus
                Case MembershipCreateStatus.DuplicateUserName
                    Return "Такой пользователь уже зарегестрирован. Выберите другое имя."

                Case MembershipCreateStatus.DuplicateEmail
                    Return "Пользователь с этим адресом email уже зарегестрирован."

                Case MembershipCreateStatus.InvalidPassword
                    Return "Неверный пароль. Проверьте и попробуйте снова"

                Case MembershipCreateStatus.InvalidEmail
                    Return "Неверный e-mail адрес. Проверьте и попробуйте снова."

                Case MembershipCreateStatus.InvalidAnswer
                    Return "The password retrieval answer provided is invalid. Please check the value and try again."

                Case MembershipCreateStatus.InvalidQuestion
                    Return "The password retrieval question provided is invalid. Please check the value and try again."

                Case MembershipCreateStatus.InvalidUserName
                    Return "Такого пользователя нет. Проверьте и попробуйте снова."

                Case MembershipCreateStatus.ProviderError
                    Return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator."

                Case MembershipCreateStatus.UserRejected
                    Return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."

                Case Else
                    Return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator."
            End Select
        End Function
#End Region

    End Class
End Namespace