Imports NLog
Imports NLog.Config
Imports NLog.Targets
Imports System.Web.Hosting
Imports System.IO

Partial Public Class MvcApplication
    Private Shared Msg As String = "Unhandled exception: {0}" & vbCrLf & _
                                    "Method: {1}" & vbCrLf & _
                                    "Url: {2}" & vbCrLf & _
                                    "Code: {5}" & vbCrLf & _
                                    "User-Agent: {3}" & vbCrLf & _
                                    "Stack trace: {4}"

    Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim log As Logger = LogManager.GetLogger("Global.asax")
        If Not Server Is Nothing Then
            Dim ex As Exception = Server.GetLastError()
            If Response.StatusCode <> 404 Then
                If Not WhiteList.Match(Request.UserAgent) And Not BlackList.Match(Request.UserAgent) Then
                    log.Error(String.Format(Msg, _
                        ex.Message, _
                        Request.HttpMethod, _
                        Request.Url, _
                        Request.UserAgent, _
                        ex.StackTrace, _
                        Response.StatusCode), ex)
                End If
            End If
        End If
    End Sub
End Class

Public Class BaseController
    Inherits System.Web.Mvc.Controller

    Private Shared Msg As String = "Unhandled exception: {0}" & vbCrLf & _
                                    "Method: {1}" & vbCrLf & _
                                    "Url: {2}" & vbCrLf & _
                                    "User-Agent: {3}" & vbCrLf & _
                                    "Stack trace: {4}"

    Protected Overrides Sub OnActionExecuting(ByVal filterContext As System.Web.Mvc.ActionExecutingContext)
        If BlackList.Match(filterContext.HttpContext.Request.UserAgent) Then
            filterContext.Result = New EmptyResult
        End If
        MyBase.OnActionExecuting(filterContext)
    End Sub

    Protected Overrides Sub OnActionExecuted(ByVal filterContext As ActionExecutedContext)
        Dim log As Logger = LogManager.GetLogger(filterContext.Controller.GetType().FullName)

        If filterContext.Exception IsNot Nothing Then
            If Not WhiteList.Match(filterContext.HttpContext.Request.UserAgent) And Not BlackList.Match(Request.UserAgent) Then
                log.Error(String.Format(Msg, _
                    filterContext.Exception.Message, _
                    filterContext.HttpContext.Request.HttpMethod, _
                    filterContext.HttpContext.Request.Url, _
                    filterContext.HttpContext.Request.UserAgent, _
                    filterContext.Exception.StackTrace), filterContext.Exception)
            End If
        End If

        MyBase.OnActionExecuted(filterContext)
    End Sub

End Class

#Region "LISTS"

Public Class WhiteList
    Friend Shared _agents As New List(Of String)

    Shared Sub New()
        Dim path = HostingEnvironment.MapPath("/useragent.whitelist")
        For Each line As String In File.ReadAllLines(path)
            If Not line.StartsWith("#") And Trim(line) <> String.Empty Then _agents.Add(line)
        Next
    End Sub

    Friend Shared Function Match(ByVal userAgent As String) As Boolean
        For Each line As String In _agents
            If userAgent Like line Then
                Return True
            End If
        Next
        Return False
    End Function
End Class

Public Class BlackList
    Friend Shared _agents As New List(Of String)

    Shared Sub New()
        Dim path = HostingEnvironment.MapPath("/useragent.blacklist")
        For Each line As String In File.ReadAllLines(path)
            If Not line.StartsWith("#") Then _agents.Add(Trim(line))
        Next
    End Sub

    Friend Shared Function Match(ByVal userAgent As String) As Boolean
        For Each line As String In _agents
            If userAgent Like line Then
                Return True
            End If
        Next
        Return False
    End Function
End Class

#End Region