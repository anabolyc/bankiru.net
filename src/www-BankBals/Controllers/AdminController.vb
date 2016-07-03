Imports www.BankBals.Classes
Imports www.BankBals.Data
Imports System.Security.Principal
Imports NLog
Imports System.IO

Namespace Controllers

#If DEBUG Then
    Partial Public Class AdminController
#Else
    <Authorize(Roles:="Administrator", Users:="bankiru.net")> _
    Public Class AdminController
#End If
        Inherits BaseController

        Function Index() As ActionResult
            Return View()
        End Function

        Function Users() As ActionResult
            Return View(System.Web.Security.Membership.GetAllUsers().Cast(Of MembershipUser).OrderByDescending(Function(U) U.CreationDate).ToList())
        End Function

        Function Roles() As ActionResult
            Return View(System.Web.Security.Roles.GetAllRoles())
        End Function

        Function CreateRole() As ActionResult
            Return View()
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken()> _
        Function CreateRole(ByVal Role As String) As ActionResult
            Try
                System.Web.Security.Roles.CreateRole(Role)
                Return RedirectToAction("Roles")
            Catch
                Return View()
            End Try
        End Function

        Function DeleteRole(ByVal Role As Object) As ActionResult
            Return View(Role)
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken()> _
        Function DeleteRole(ByVal Role As String, ByVal Confirm As String) As ActionResult
            Try
                System.Web.Security.Roles.DeleteRole(Role)
                Return RedirectToAction("Roles")
            Catch
                Return View(Role)
            End Try
        End Function

        Function EditUser(ByVal UserName As Object) As ActionResult
            Return View(UserName)
        End Function

        Function DeleteUser(ByVal UserName As Object) As ActionResult
            Return View(UserName)
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken()> _
        Function DeleteUser(ByVal UserName As String, ByVal Confirm As String) As ActionResult
            Try
                System.Web.Security.Membership.DeleteUser(UserName)
                Return RedirectToAction("Users")
            Catch
                Return View(UserName)
            End Try
        End Function

        Function RolesUser(ByVal UserName As Object) As ActionResult
            Return View(UserName)
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken()> _
        Function RolesUser(ByVal UserName As String, ByVal RoleName As String) As ActionResult
            Try
                If System.Web.Security.Roles.IsUserInRole(UserName, RoleName) Then
                    System.Web.Security.Roles.RemoveUserFromRole(UserName, RoleName)
                Else
                    System.Web.Security.Roles.AddUserToRole(UserName, RoleName)
                End If
                Return RedirectToAction("Users")
            Catch
                Return View(UserName)
            End Try
        End Function

        <CompressFilter()> _
        <FilterIP(AllowedSingleIPs:="127.0.0.1,195.225.38.17,95.31.4.54", AllowedMaskedIPs:="192.168.1.0;255.255.255.0")> _
        Function DBConsole() As ActionResult
            Return View()
        End Function

        <CompressFilter()> _
        <FilterIP(AllowedSingleIPs:="127.0.0.1,195.225.38.17,95.31.4.54", AllowedMaskedIPs:="192.168.1.0;255.255.255.0")> _
        <HttpPost()> _
        <ValidateAntiForgeryToken()> _
        Function DBConsole(ByVal Command As String) As ActionResult
            ViewData("Command") = Command
            Dim export As New Export
            Dim result As Object = export.AsText(Command, Data.Export.Format.HTMLTABLE)
            Return View(result)
        End Function

#Region "depricated"
        'Function Terminal() As ActionResult
        '    ViewData("Command") = String.Empty
        '    ViewData("ID") = String.Empty
        '    Return View()
        'End Function

        '<HttpPost()> _
        '<ValidateAntiForgeryToken()> _
        'Function Terminal(ByVal Command As String, ByVal ID As Nullable(Of Guid)) As ActionResult
        '    Dim T As TerminalFactory.Terminal
        '    If Not ID.HasValue() Then
        '        T = TermFactory.GetTerminal(Command)
        '        Dim d As DateTime = Now.AddSeconds(0.5)
        '        While Now < d
        '        End While
        '    Else
        '        T = TermFactory.GetByID(ID.Value)
        '    End If
        '    ViewData("Command") = Command
        '    If T Is Nothing Then
        '        ViewData("ID") = String.Empty
        '    Else
        '        ViewData("ID") = T.ID
        '    End If
        '    Return View("TerminalResult", T)
        'End Function

        '<HttpPost()> _
        'Function TerminalUpdateJSON(ByVal ID As Guid, ByVal RowNumber As Integer) As ActionResult
        '    Dim T As TerminalFactory.Terminal = TermFactory.GetByID(ID)
        '    Dim Rows As New List(Of String)
        '    Dim i As Integer
        '    For i = RowNumber To T.Result.Count - 1
        '        Rows.Add(T.Result(i))
        '    Next
        '    Return Json(New With {.ErrorCode = 0, .ErrorMessage = "", .Finished = T.Finished, .Rows = Rows, .RowNumber = i})
        '    'Return Json()
        'End Function

        'Function CacheManager() As ActionResult
        '    ViewBag.StateString = CM.GetCurrnetState()
        '    Return View()
        'End Function

        '<HttpPost()> _
        'Function ClearCache() As ActionResult
        '    CM.ClearCache()
        '    Return RedirectToAction("CacheManager")
        'End Function

        '<HttpPost()> _
        'Function RebuildCache() As ActionResult
        '    CM.RebuildCache()
        '    Return RedirectToAction("CacheManager")
        'End Function

        '<HttpPost()> _
        'Function StopRebuild() As ActionResult
        '    CM.StopCache()
        '    Return RedirectToAction("CacheManager")
        'End Function
#End Region

    End Class
End Namespace

