Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

Namespace Models

    Public Class ChangePasswordModel
        Private oldPasswordValue As String
        Private newPasswordValue As String
        Private confirmPasswordValue As String

        <Required()> _
        <DataType(DataType.Password)> _
        <Display(Name:="Текущий пароль")> _
        Public Property OldPassword() As String
            Get
                Return oldPasswordValue
            End Get
            Set(ByVal value As String)
                oldPasswordValue = value
            End Set
        End Property

        <Required()> _
        <StringLength(100, ErrorMessage:="{0} должен быть не менее {2} символов.", MinimumLength:=6)> _
        <DataType(DataType.Password)> _
        <Display(Name:="Новый пароль")> _
        Public Property NewPassword() As String
            Get
                Return newPasswordValue
            End Get
            Set(ByVal value As String)
                newPasswordValue = value
            End Set
        End Property

        <DataType(DataType.Password)> _
        <Display(Name:="Еще раз")> _
        <Compare("NewPassword", ErrorMessage:="Не совпадает")> _
        Public Property ConfirmPassword() As String
            Get
                Return confirmPasswordValue
            End Get
            Set(ByVal value As String)
                confirmPasswordValue = value
            End Set
        End Property
    End Class

    Public Class LogOnModel
        Private userNameValue As String
        Private passwordValue As String
        Private rememberMeValue As Boolean

        <Required()> _
        <Display(Name:="Имя пользователя")> _
        Public Property UserName() As String
            Get
                Return userNameValue
            End Get
            Set(ByVal value As String)
                userNameValue = value
            End Set
        End Property

        <Required()> _
        <DataType(DataType.Password)> _
        <Display(Name:="Пароль")> _
        Public Property Password() As String
            Get
                Return passwordValue
            End Get
            Set(ByVal value As String)
                passwordValue = value
            End Set
        End Property

        <Display(Name:="Запомнить")> _
        Public Property RememberMe() As Boolean
            Get
                Return rememberMeValue
            End Get
            Set(ByVal value As Boolean)
                rememberMeValue = value
            End Set
        End Property
    End Class

    Public Class RegisterModel
        Private userNameValue As String
        Private passwordValue As String
        Private confirmPasswordValue As String
        Private emailValue As String

        <Required()> _
        <Display(Name:="Имя пользователя")> _
        Public Property UserName() As String
            Get
                Return userNameValue
            End Get
            Set(ByVal value As String)
                userNameValue = value
            End Set
        End Property

        <Required()> _
        <DataType(DataType.EmailAddress)> _
        <Display(Name:="Email")> _
        Public Property Email() As String
            Get
                Return emailValue
            End Get
            Set(ByVal value As String)
                emailValue = value
            End Set
        End Property

        <Required()> _
        <StringLength(100, ErrorMessage:="Пароль {0} должен быть не менее {2} символов", MinimumLength:=6)> _
        <DataType(DataType.Password)> _
        <Display(Name:="Пароль")> _
        Public Property Password() As String
            Get
                Return passwordValue
            End Get
            Set(ByVal value As String)
                passwordValue = value
            End Set
        End Property

        <DataType(DataType.Password)> _
        <Display(Name:="Подтвердите пароль")> _
        <Compare("Password", ErrorMessage:="Пароли не совпадают")> _
        Public Property ConfirmPassword() As String
            Get
                Return confirmPasswordValue
            End Get
            Set(ByVal value As String)
                confirmPasswordValue = value
            End Set
        End Property
    End Class

    Public Class ResetPassModel
        Private userNameValue As String

        <Required()> _
        <Display(Name:="Имя пользователя или e-mail")> _
        Public Property UserName() As String
            Get
                Return userNameValue
            End Get
            Set(ByVal value As String)
                userNameValue = value
            End Set
        End Property
    End Class

    Public Class TryResetPassModel
        Private passwordValue As String
        Private confirmPasswordValue As String

        <Required()> _
        <StringLength(100, ErrorMessage:="Пароль {0} должен быть не менее {2} символов", MinimumLength:=6)> _
        <DataType(DataType.Password)> _
        <Display(Name:="Новый пароль")> _
        Public Property Password() As String
            Get
                Return passwordValue
            End Get
            Set(ByVal value As String)
                passwordValue = value
            End Set
        End Property

        <DataType(DataType.Password)> _
        <Display(Name:="Подтвердите пароль")> _
        <Compare("Password", ErrorMessage:="Пароли не совпадают")> _
        Public Property ConfirmPassword() As String
            Get
                Return confirmPasswordValue
            End Get
            Set(ByVal value As String)
                confirmPasswordValue = value
            End Set
        End Property
    End Class

End Namespace