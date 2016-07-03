Imports System.IO
Imports www.BankBals.Models

Namespace Classes

    Module GlobalVar

        Public Const IS_PRODUCTION As Boolean = True

#If DEBUG Then
        Public Const CACHE_LONG As Integer = 0
        Public Const CACHE_SHORT As Integer = 0
#Else
        Public Const CACHE_LONG As Integer = 3600
        Public Const CACHE_SHORT As Integer = 600
#End If

        'Public TermFactory As New TerminalFactory
    End Module

    Public Class Common

        Public Enum Placements
            PRODUCTION = 0
            TESTSERVER = 1
        End Enum

        Public Shared Function SiteHost() As String
            Return IIf(IS_PRODUCTION, "http://bankiru.net/", "http://localhost:1187/")
        End Function

        Public Shared Function SiteTitle() As String
            Return IIf(IS_PRODUCTION, "<span class='span-b'>B</span>ankiru.net", String.Empty)
        End Function

        Public Shared Function Placement() As Placements
            Return IIf(IS_PRODUCTION, Placements.PRODUCTION, Placements.TESTSERVER)
        End Function

        Public Shared Sub Timer(Optional ByVal Caption As String = "", Optional ByVal Reset As Boolean = False)
            Static StartTime As DateTime = DateTime.Now
            If Reset Then
                StartTime = DateTime.Now
            Else
                Debug.WriteLine((DateTime.Now.Ticks - StartTime.Ticks) / 10000.0 & " ms = " & Caption)
                StartTime = DateTime.Now
            End If
        End Sub

        Public Class MailNotifier
            Private Shared Function GetConfig() As MailSender.Config
                Dim cfg As New MailSender.Config
#If DEBUG Then
                cfg.EnableMail = True
                cfg.EnableSsl = False
                cfg.SmtpServer = "***"
                cfg.SmtpPort = 25
                cfg.SmtpReply = "***"
#Else
                cfg.EnableMail = True
                cfg.EnableSsl = False
                cfg.SmtpServer = "***"
                cfg.SmtpPort = 25
                cfg.SmtpUserName = "***"
                cfg.SmtpPassword = "***"
                cfg.SmtpReply = "***"
#End If
                cfg.SmtpReplyFullName = "Служба поддержки bankiru.net"
                Return (cfg)
            End Function

            Public Shared Sub SendResetPasswordMessage(ByVal email As String, ByVal Key As Guid)
                MailSender.MailSender.SendMail(GetConfig(), email, "Сброс пароля на сайте bankiru.net", "Перейдите по ссылке<br/> http://bankiru.net/Account/TryResetPassword?Key=" & Key.ToString & "<br/> чтобы сбросить пароль на сайте bankiru.net<br/>Если вы не запрашивали сброс пароля на нашем сайте, проигнорируйте письмо, пароль останется тем же. Видимо кто-то <i>случайно</i> пытался сбросить ваш пароль.")
            End Sub

        End Class
    End Class

End Namespace
