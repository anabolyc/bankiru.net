Imports www.BankBals.Classes
Imports www.BankBals.Data
Imports NLog
Imports System.IO


Namespace Controllers

    <CompressFilter()> _
Public Class APIController
        Inherits BaseController

        Public Const CBR_HOST As String = "http://cbr.ru"

        Function Index() As ActionResult
            Return View()
        End Function

        Function BankbalsTool() As ActionResult
            Return View()
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function BanksList(Optional ByVal type As String = "text") As ActionResult
            Dim SQLText As String = "SELECT BankID, NameRus FROM BB_BANKS_LIST(NULL)"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function DatesList(Optional ByVal type As String = "text") As ActionResult
            Dim SQLText As String = "SELECT DateID, Date FROM BB_DATES_LIST()"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        Function DatesList2(ByVal FormID As Integer, Optional ByVal type As String = "text") As ActionResult
            Dim SQLText As String = "SELECT DateID, Date FROM BB_DATES_LIST2(" & FormID & ")"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function CuritemsList(Optional ByVal type As String = "text") As ActionResult
            Dim SQLText As String = "SELECT * FROM BB_CURITEM_LIST(1)"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function ViewsList(Optional ByVal type As String = "text") As ActionResult
            Dim SQLText As String = "SELECT ViewID, NameRus, Form FROM BB_VIEWS_LIST()"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function ViewitemsList(Optional ByVal type As String = "text", Optional ByVal ViewID As Integer = 1) As ActionResult
            Dim SQLText As String = "SELECT ViewItemID, Ticker, NameRus, IsRatio, AggItemID FROM BB_VIEWITEMS_LIST2(" & ViewID & ")"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function ParamsList(Optional ByVal type As String = "text", Optional ByVal FormID As Integer = 101) As ActionResult
            Dim SQLText As String = "SELECT TOP 1 ParID, Ticker FROM BB_PARAMS_LIST() WHERE F" & FormID & " = 1 AND IsMain = 1"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function GetOneDateData(Optional ByVal DateID As Integer = 469, Optional ByVal ViewID As Integer = 1) As ActionResult
            Dim Result As New ContentResult()
            Dim export As New Export
            Result.Content = export.GetOneDateData(DateID, ViewID)
            Result.ContentEncoding = System.Text.Encoding.UTF8
            Result.ContentType = "text/html"
            Return Result
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function DataEx1(ByVal type As String, ByVal ViewID As Integer, ByVal BankID As Integer, ByVal AggItemID As Integer) As ActionResult
            Dim SQLText As String = "SELECT * FROM BB_DATA101_EX1(" & ViewID & ", " & BankID & ", " & AggItemID & ")"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function DataEx2(ByVal type As String, ByVal ViewID As Integer, ByVal BankID As Integer, ByVal ID As Integer) As ActionResult
            Dim SQLText As String = "SELECT * FROM BB_DATA101_EX2(" & ViewID & ", " & BankID & ", " & ID & ")"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function DataEx3(ByVal type As String, ByVal ViewID As Integer, ByVal DateID As Integer, ByVal AggItemID As Integer) As ActionResult
            Dim SQLText As String = "SELECT * FROM BB_DATA101_EX3(" & ViewID & ", " & DateID & ", " & AggItemID & ")"
            Dim export As New Export
            Select Case type
                Case "text"
                    Return export.AsText(SQLText)
                Case "html"
                    Return export.AsHTML(SQLText)
                Case "xml"
                    Return export.AsXML(SQLText)
                Case "json"
                    Return export.AsJSON(SQLText)
                Case Else
                    Return Content(String.Empty)
            End Select
        End Function

        <HttpPost()> _
        Public Function GetKey(ByVal UserName As String, ByVal Password As String) As ActionResult
            If Membership.ValidateUser(UserName, Password) Then
                Dim db As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
                Dim newkey As aspnet_AccessKey = New aspnet_AccessKey()
                Dim ID As Guid = Guid.NewGuid()
                newkey.UserID = db.aspnet_Users.First(Function(u) u.UserName = UserName).UserId
                newkey.Key = ID
                newkey.Valid = DateTime.Now.AddHours(1)
                db.aspnet_AccessKeys.InsertOnSubmit(newkey)
                db.SubmitChanges()
                Return Content(ID.ToString())
            Else
                Return New HttpStatusCodeResult(400, "Access denied")
            End If
        End Function

        <Authorize(Roles:="Dataadmin")> _
        Function UploadFile() As ActionResult
            Return View()
        End Function

        'Function UploadFile(ByVal postedFile As HttpPostedFileBase) As ActionResult
        <Authorize(Roles:="Dataadmin")> _
        <HttpPost()> _
        Function UploadFile(ByVal fileUrl As String) As ActionResult
            Dim url As String = CBR_HOST & fileUrl
            Dim target As Targets.MemoryTarget = New Targets.MemoryTarget
            target.Layout = "${message}"
            Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Info)

            Dim L As Logger = LogManager.GetLogger("UploadFile")

            Dim tempFolder As String = Path.Combine(HttpRuntime.AppDomainAppPath, "Content", "_temp")
            If Not Directory.Exists(tempFolder) Then Directory.CreateDirectory(tempFolder)
            FileUploader.Upload(tempFolder, url, L)
            ViewBag.Log = L
            ViewBag.Target = target
            Return View()
        End Function

        <HttpPost()>
        Function UploadFileWithKey(ByVal fileUrl As String, ByVal Key As Guid) As ActionResult
            Dim url As String = CBR_HOST & fileUrl

            Dim db As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
            Dim userID As Guid = CheckKey(Key)
            If userID = Nothing Then
                Return New HttpStatusCodeResult(400, "This key is not valid")
            Else
                If db.aspnet_UsersInRoles.FirstOrDefault(Function(UR) UR.aspnet_Role.LoweredRoleName = "dataadmin" And UR.UserId = userID) Is Nothing Then
                    Return New HttpStatusCodeResult(400, "User do not have right to change data")
                Else
                    Dim target As Targets.MemoryTarget = New Targets.MemoryTarget
                    target.Layout = "${message}"
                    Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Info)
                    Dim L As Logger = LogManager.GetLogger("UploadFile")

                    Dim tempFolder As String = Path.Combine(HttpRuntime.AppDomainAppPath, "Content", "_temp")
                    If Not Directory.Exists(tempFolder) Then Directory.CreateDirectory(tempFolder)
                    FileUploader.Upload(tempFolder, url, L)
                    Dim message As String = String.Empty
                    For Each s As String In target.Logs
                        message &= s & vbCrLf
                    Next
                    Return Content(message)
                End If
            End If
        End Function

        <HttpPost()>
        Function ProcessSQLTask(ByVal Command As String, ByVal Key As Guid) As ActionResult
            Dim db As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
            Dim userID As Guid = CheckKey(Key)
            If userID = Nothing Then
                Return New HttpStatusCodeResult(400, "This key is not valid")
            Else
                If db.aspnet_UsersInRoles.FirstOrDefault(Function(UR) UR.aspnet_Role.LoweredRoleName = "dataadmin" And UR.UserId = userID) Is Nothing Then
                    Return New HttpStatusCodeResult(400, "User do not have right to change data")
                Else
                    Return Content((New Export).AsText(Command, Data.Export.Format.TEXT))
                End If
            End If
        End Function

        Private Function CheckKey(ByVal Key As Guid) As Guid
            Dim db As New AspnetDbDataContext(Tools.ConnectionString(Tools.SQL_DB_ASP))
            db.ExecuteCommand("DELETE FROM aspnet_AccessKeys WHERE Valid < GETDATE()")

            Dim accesskey As aspnet_AccessKey = db.aspnet_AccessKeys.FirstOrDefault(Function(K) K.Key = Key)
            If accesskey Is Nothing Then
                Return Nothing
            Else
                If accesskey.Valid < DateTime.Now Then
                    Return Nothing
                Else
                    Return accesskey.UserID
                End If
            End If
        End Function

    End Class
End Namespace

