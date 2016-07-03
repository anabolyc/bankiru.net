Imports System
Imports System.IO
Imports System.Windows.Forms
Imports BankBals_common.Classes

Module Main

#Region "CONST"

    Private Const HOST As String = "http://balbase.mbkcentre.ru"
    Private Const SQL_DATE_FORMAT As String = "yyyy-MM-dd"
    Private DEFAULT_DATE As New DateTime(1970, 1, 1)

    Public Const SQL_DB_DEV As String = "[BankBals-dev].dbo."
    Public Const SQL_DB_WEB As String = "[BankBals-web].dbo."
    Public Log As New Logger()
    Public ConnectionString As New System.Data.SqlClient.SqlConnectionStringBuilder()

    Public options As ProgramOptions

    Public Structure ProgramOptions
        Public FullLoad As Boolean
        Public ForceLoad As Boolean
        Public uploadfile As String
    End Structure

#End Region

    Public Function Main(ByVal Args() As String) As Integer
        Dim runable As Boolean = False
        ParseCmdLineParams(Args, runable)

        If (runable) Then
            If options.uploadfile <> String.Empty Then
                getContentManual(options.uploadfile)
            Else
                getContent()
            End If
            Return Log.GetResult()
        Else
            Log.Write("Nothing to do! Use --help to see keys", "Main")
            Return 0
        End If
    End Function

    Private Sub ParseCmdLineParams(ByVal args As String(), ByRef runable As Boolean)
        runable = False
        Dim i As Integer = 0

        ConnectionString.DataSource = "(local)"
        ConnectionString.IntegratedSecurity = True
        ConnectionString.InitialCatalog = "BankBals-dev"

        For Each arg As String In args
            If arg = "--help" Then
                PrintHelp()
                runable = False
                Return
            End If
            'SQL Server
            If (arg = "--server") AndAlso (i < args.Length - 1) Then
                ConnectionString.DataSource = args(i + 1)
            End If
            If (arg = "--username") AndAlso (i < args.Length - 1) Then
                ConnectionString.UserID = args(i + 1)
            End If
            If (arg = "--password") AndAlso (i < args.Length - 1) Then
                ConnectionString.Password = args(i + 1)
            End If
            If (arg = "--database") AndAlso (i < args.Length - 1) Then
                ConnectionString.InitialCatalog = args(i + 1)
            End If
            'options
            If (arg = "--upload-file") AndAlso (i < args.Length) Then
                options.uploadfile = args(i + 1)
                runable = True
            End If
            If arg = "--auto" Then
                runable = True
            End If
            If arg = "--full-load" Then
                options.FullLoad = True
                runable = True
            End If
            If arg = "--force-load" Then
                options.ForceLoad = True
                runable = True
            End If
            i += 1
        Next

        Log.Write("Using server: " + ConnectionString.DataSource, "ParseCmdLineParams")
        If (ConnectionString.UserID <> [String].Empty) AndAlso (ConnectionString.Password <> [String].Empty) Then
            ConnectionString.IntegratedSecurity = False
            Log.Write("IntegratedSecurity set to False, UserID = " + ConnectionString.UserID + ", password = ******", "ParseCmdLineParams")
        Else
            ConnectionString.IntegratedSecurity = True
            Log.Write("IntegratedSecurity set to True", "ParseCmdLineParams")
        End If
    End Sub

    Private Sub PrintHelp()
        Dim filepath As String = Common.GetAppFolder() + "\help.txt"
        If File.Exists(filepath) Then
            Dim helps As String() = File.ReadAllLines(filepath)
            For Each s As String In helps
                Console.WriteLine(s)
            Next
        Else
            Console.WriteLine("Help file is missing: './help.txt'")
        End If
    End Sub

    Private Sub getContentManual(ByVal filename As String)
        Log.Write("getContentManual", "Not implemented yet", True, True)
    End Sub

    Private Sub getContent()

        Dim bankslistLink As String = HOST & "/analyzer.php?mail=&bank=&D1=2008&D2=" & Year(DateTime.Now)
        'Dim bankslistLink As String = HOST & "/analyzer.php?mail=&bank=&D1=2012&D2=2012"         '***

        Dim DirectoryPath As String = Common.GetTmpPath() & "\mbkbanks\"
        If System.IO.Directory.Exists(DirectoryPath) Then
            My.Computer.FileSystem.DeleteDirectory(DirectoryPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(DirectoryPath)


        Dim browser As New WebBrowser
        browser.ScriptErrorsSuppressed = True
        BrowserLoop(browser, bankslistLink)

        Log.Write("Loading Banks List", "getContent")
        Dim linksToLoad As New List(Of String)
        For Each link As HtmlElement In browser.Document.GetElementsByTagName("a")
            If (link.DomElement.href Like "*/base*regn=*") Then
                'If link.DomElement.href Like "*/base*regn=18&*" Then '***
                linksToLoad.Add(link.DomElement.href)
            End If
        Next

        Log.Write("List loaded: " & linksToLoad.Count & " banks", "getContent")
        ProcessLinks(browser, linksToLoad, DirectoryPath)

        My.Computer.FileSystem.DeleteDirectory(DirectoryPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End Sub

    Private Sub ProcessLinks(ByRef wb As WebBrowser, ByVal linksToLoad As List(Of String), ByVal DirectoryPath As String)
        For Each link As String In linksToLoad
            Log.Write("Loading bank link: " & link, "ProcessLinks")
            BrowserLoop(wb, link)
            For Each href As HtmlElement In wb.Document.GetElementsByTagName("a")
                If href.DomElement.href Like "*.zip" Then
                    loadOneFile(href.DomElement.href, DirectoryPath)
                End If
            Next
        Next
    End Sub

    Private Sub loadOneFile(ByVal link As String, ByVal DirectoryPath As String)
        Dim s As String = link.Split("/")(4)
        Dim FormID As String = Split(s, ".")(1)
        s = s.Split(".")(0)
        Dim BankID As Integer = Left(s, 4)
        s = Right(s, Len(s) - 4)
        Dim Y As Integer = Left(s, 3)
        Dim M As Integer
        Select Case Right(s, 1)
            Case "a" : M = 10
            Case "b" : M = 11
            Case "c" : M = 12
            Case Else : M = CInt(Right(s, 1))
        End Select

        Dim DT As Date = DateSerial(Y, M, 1)
        Dim FileName0 As String = link.Split("/")(4)

        'If ShouldLoad(DT, BankID, FormID) Then
        If ShouldLoad(FileName0) Then
            If GetFromUrl.DownLoadFile0(link, DirectoryPath & FileName0) = Result.RESULT_OK Then
                If Common.Unpack(DirectoryPath & FileName0, DirectoryPath, Packer.ZIP) = Result.RESULT_OK Then
                    System.IO.File.Delete(DirectoryPath & FileName0)
                    For Each FileName As String In System.IO.Directory.GetFiles(DirectoryPath)
                        Select Case Right(FileName, 3)
                            Case "101"
                                ParseFileF101(FileName, BankID, DT)
                            Case "102"
                                ParseFileF102(FileName, BankID, DT)
                            Case "134"
                                ParseFileF134(FileName, BankID, DT)
                            Case "135"
                                ParseFileF135_1(FileName, BankID, DT)
                                ParseFileF135_3(FileName, BankID, DT)
                                ParseFileF135_5(FileName, BankID, DT)
                            Case Else
                                Log.Write("Unrecognised file: " & FileName, "loadOneFile", True)
                        End Select
                        System.IO.File.SetAttributes(FileName, FileAttributes.Normal)
                        System.IO.File.Delete(FileName)
                    Next
                    DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'; INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', -1, 1)")
                Else
                    Log.Write("Failed to unzip file", "loadOneFile", True)
                End If
            Else
                Log.Write("Failed to download file", "loadOneFile", True)
            End If
        Else
            Log.Write("Skipping, already loaded", "loadOneFile")
        End If

    End Sub

#Region "PARSER"

    Public Function ParseFileF101(ByVal FileName As String, ByVal BankID As Integer, ByVal DT As Date) As Result
        Dim result As Result
        Dim DU = New DataUploader(BankID, DT, "101")
        Dim RowsSucessed As Integer = 0
        Dim RowsFailed As Integer = 0

        Using sr As New StreamReader(FileName, System.Text.Encoding.Default)
            Dim line As String = String.Empty
            Dim prevline As String = String.Empty
            Do
                prevline = line
                line = sr.ReadLine()
                If Not (line Is Nothing) Then
                    line = CleanString(line)

                    If Not ((line Like "*[a-z]*") Or (line Like "*[а-я]*") Or (line.Length = 0)) Then
                        'skipping text lines completely
                        If line Like "##### *" Then
                            'line should start with 5-digit 
                            If line Like "##### #* #* #* #* #* #* #* #* #* #* #* *#" Then
                                RowsSucessed += 1
                                DU.AddRow(line)
                            ElseIf line Like "98### *" Then

                            ElseIf LCase(prevline) Like "*итого*" And line Like "#* #* #* #* #* #* #* #* #* #* #* *#" Then

                            Else
                                RowsFailed += 1
                            End If
                        End If
                    End If

                End If
            Loop Until line Is Nothing

            Dim FileName0 As String = System.IO.Path.GetFileName(FileName)
            DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'")
            If (RowsFailed > 10) Or (RowsSucessed / RowsFailed < 5) Then
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', 0, 0)")
                result = result.RESULT_FAIL
            Else
                result = DU.Upload
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', " + DU.Rows.Count.ToString + ", 1)")
            End If

        End Using

        Return result
    End Function

    Public Function ParseFileF102(ByVal FileName As String, ByVal BankID As Integer, ByVal DT As Date) As Result
        Dim result As Result
        Dim DU = New DataUploader(BankID, DT, "102")
        Dim RowsSucessed As Integer = 0
        Dim RowsFailed As Integer = 0

        Using sr As New StreamReader(FileName, System.Text.Encoding.Default)
            Dim line As String = String.Empty
            Dim prevline As String = String.Empty
            Do
                prevline = line
                line = sr.ReadLine()
                If Not (line Is Nothing) And (line <> String.Empty) Then
                    If ((LCase(line) Like "*Итого*") Or (prevline Like "*€в®Ј®*")) And Not prevline Like "*#*" Then
                        'skip line
                    Else

                        If Not (LCase(line) Like "*итого*") And Not (line Like "*€в®Ј®*") Then
                            line = CleanString(line, True)

                            If line <> String.Empty Then
                                If line Like "##### *" Then
                                    'line should start with 5-digit 
                                    If line Like "##### #* #* *#" Then
                                        RowsSucessed += 1
                                        DU.AddRow(line)
                                    Else
                                        RowsFailed += 1
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Loop Until line Is Nothing

            Dim FileName0 As String = System.IO.Path.GetFileName(FileName)
            DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'")
            If (RowsFailed > 10) Or (RowsSucessed / RowsFailed < 5) Or (RowsSucessed < 50) Then
                Log.Write(line, "ParseFileF102", False, False, ConsoleColor.Cyan)
                Log.Write("", "ParseFileF102")
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', 0, 0)")
                result = result.RESULT_FAIL
            Else
                result = DU.Upload
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', " + DU.Rows.Count.ToString + ", 1)")
            End If

        End Using

        Return result
    End Function

    Public Function ParseFileF134(ByVal FileName As String, ByVal BankID As Integer, ByVal DT As Date) As Result
        Dim result As Result
        Dim DU = New DataUploader(BankID, DT, "134")
        Dim RowsSucessed As Integer = 0
        Dim RowsFailed As Integer = 0

        Using sr As New StreamReader(FileName, System.Text.Encoding.Default)
            Dim line As String = String.Empty
            Dim prevline As String = String.Empty
            Do
                prevline = line
                line = sr.ReadLine()
                If Not (line Is Nothing) And (line <> String.Empty) Then
                    line = CleanString(line, , True)

                    If line <> String.Empty Then
                        If ((line Like "### *") Or (line Like "###.# *")) And Not (line Like "*#.#.#*") Then
                            If (line Like "### *#") Or (line Like "###.# *#") Then
                                RowsSucessed += 1
                                DU.AddRow(line)
                            Else
                                RowsFailed += 1
                            End If
                        End If
                    End If

                End If
            Loop Until line Is Nothing

            Dim FileName0 As String = System.IO.Path.GetFileName(FileName)
            DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'")
            If (RowsFailed > 10) Or (RowsSucessed / RowsFailed < 5) Or (RowsSucessed < 10) Then
                Log.Write(line, "ParseFileF134", False, False, ConsoleColor.Cyan)
                Log.Write("", "ParseFileF134")
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', 0, 0)")
                result = result.RESULT_FAIL
            Else
                result = DU.Upload
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', " + DU.Rows.Count.ToString + ", 1)")
            End If

        End Using

        Return result
    End Function

    Public Function ParseFileF135_1(ByVal FileName As String, ByVal BankID As Integer, ByVal DT As Date) As Result
        Dim result As Result
        Dim DU = New DataUploader(BankID, DT, "135_1")
        Dim RowsSucessed As Integer = 0
        Dim RowsFailed As Integer = 0

        Using sr As New StreamReader(FileName, System.Text.Encoding.Default)
            Dim line As String = String.Empty
            Dim prevline As String = String.Empty
            Do
                prevline = line
                line = sr.ReadLine()
                If Not (line Is Nothing) And (line <> String.Empty) Then
                    line = CleanString(line)

                    If line <> String.Empty Then
                        If (line Like "88## #*") Or (line Like "89## #*") Then
                            RowsSucessed += 1
                            DU.AddRow(line)
                        Else
                            'RowsFailed += 1
                        End If
                    End If

                End If
            Loop Until line Is Nothing

            Dim FileName0 As String = System.IO.Path.GetFileName(FileName)
            DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'")
            If (RowsSucessed = 0) Then
                Log.Write(line, "ParseFileF135_1", False, False, ConsoleColor.Cyan)
                Log.Write("", "ParseFileF135_1")
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', 0, 0)")
                result = result.RESULT_FAIL
            Else
                result = DU.Upload
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', " + DU.Rows.Count.ToString + ", 1)")
            End If

        End Using

        Return result
    End Function

    Public Function ParseFileF135_3(ByVal FileName As String, ByVal BankID As Integer, ByVal DT As Date) As Result
        Dim result As Result
        Dim DU = New DataUploader(BankID, DT, "135_3")
        Dim RowsSucessed As Integer = 0
        Dim RowsFailed As Integer = 0

        Using sr As New StreamReader(FileName, System.Text.Encoding.Default)
            Dim line As String = String.Empty
            Dim prevline As String = String.Empty
            Do
                prevline = line
                line = sr.ReadLine()
                If Not (line Is Nothing) And (line <> String.Empty) Then
                    line = CleanString(line, , , True)

                    If line <> String.Empty Then
                        If (line Like "h# #*") Or (line Like "h## #*") Or (line Like "h#.# #*") Or (line Like "h##.# #*") Or (line Like "t# #*") Then
                            RowsSucessed += 1
                            DU.AddRow(line)
                        Else
                            'RowsFailed += 1
                        End If
                    End If

                End If
            Loop Until line Is Nothing

            Dim FileName0 As String = System.IO.Path.GetFileName(FileName)
            DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'")
            If (RowsSucessed = 0) Then
                Log.Write(line, "ParseFileF135_3", False, False, ConsoleColor.Cyan)
                Log.Write("", "ParseFileF135_3")
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', 0, 0)")
                result = result.RESULT_FAIL
            Else
                result = DU.Upload
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', " + DU.Rows.Count.ToString + ", 1)")
            End If

        End Using

        Return result
    End Function

    Public Function ParseFileF135_5(ByVal FileName As String, ByVal BankID As Integer, ByVal DT As Date) As Result
        Dim result As Result
        Dim DU = New DataUploader(BankID, DT, "135_5")
        Dim RowsSucessed As Integer = 0
        Dim RowsFailed As Integer = 0

        Using sr As New StreamReader(FileName, System.Text.Encoding.Default)
            Dim line As String = String.Empty
            Dim prevline As String = String.Empty
            Do
                prevline = line
                line = sr.ReadLine()
                If Not (line Is Nothing) And (line <> String.Empty) Then
                    line = CleanString(line, , , True)

                    If line <> String.Empty Then

                        If (line Like "h# #*") Or (line Like "h## #*") Or (line Like "h#.# #*") Or (line Like "h##.# #*") Or (line Like "t# #*") Then
                            RowsSucessed += 1
                            DU.AddRow(line)
                        Else
                            'RowsFailed += 1
                        End If
                    End If

                End If
            Loop Until line Is Nothing

            Dim FileName0 As String = System.IO.Path.GetFileName(FileName)
            DBQuery("DELETE FROM " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" + FileName0 + "'")
            If (RowsSucessed = 0) Then
                Log.Write(line, "ParseFileF135_5", False, False, ConsoleColor.Cyan)
                Log.Write("", "ParseFileF135_5")
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', 0, 0)")
                result = result.RESULT_FAIL
            Else
                result = DU.Upload
                DBQuery("INSERT INTO " & SQL_DB_DEV & "W_LOAD_FILES_MBK (FileName, RowsCount, Accepted) VALUES ('" + FileName0 + "', " + DU.Rows.Count.ToString + ", 1)")
            End If

        End Using

        Return result
    End Function

#End Region

#Region "HELPERS"

    Public Function CleanString(ByVal s As String, Optional ByVal RemoveText As Boolean = False, Optional ByVal CutText As Boolean = False, Optional ByVal ReplaceUnderscore As Boolean = False) As String
        Dim result As String = s
        result = LCase(result)
        result = Replace(result, "|", " ")
        result = Replace(result, "+", "")
        result = Replace(result, "-", "")
        result = Replace(result, "=", " ")
        result = Replace(result, vbTab, " ")
        result = Replace(result, " х ", " 0 ")
        result = Replace(result, "ў в.з.", "")
        result = Replace(result, "в т.ч.", "")

        If ReplaceUnderscore Then
            result = Replace(result, "_", ".")
        End If

        If CutText Then
            If Not result = String.Empty Then
                For i As Integer = result.Length To 1 Step -1
                    If Not (Mid(result, i, 1) Like "#" Or Mid(result, i, 1) = " " Or Mid(result, i, 1) = ".") Then
                        result = Left(result, i - 1) & Right(result, result.Length - i)
                    End If
                Next
            End If
        ElseIf RemoveText Then
            If Not result = String.Empty Then
                For i As Integer = result.Length To 1 Step -1
                    If Not (Mid(result, i, 1) Like "#" Or Mid(result, i, 1) Like " ") Then
                        result = Right(result, result.Length - i)
                        Exit For
                    End If
                Next
            End If
        End If

        result = Trim(result)
        Do While result Like "*  *"
            result = Replace(result, " . ", "  ")
            result = Replace(result, "  ", " ")
        Loop

        Return result
    End Function

    Private Function ShouldLoad(ByVal FileName As String) As Boolean
        Dim rcd As New ADODB.Recordset
        rcd = DBQuery(SQLText:="SELECT COUNT(*) FROM  " & SQL_DB_DEV & "W_LOAD_FILES_MBK WHERE FileName = '" & FileName & "'", Quiet:=True)
        If Not rcd Is Nothing Then
            If Not rcd.EOF Then
                If rcd.Fields(0).Value > 0 Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Public Sub BrowserLoop(ByVal wb As WebBrowser, ByVal link As String, Optional ByVal exitByTimeout As Boolean = False)
        Const TIME_OUT As Integer = 20
        Dim startTime As DateTime = DateTime.Now
        Try
            If link <> String.Empty Then
                wb.Navigate(link)
            End If
            While wb.ReadyState <> WebBrowserReadyState.Complete
                Application.DoEvents()
                If (exitByTimeout) AndAlso (DateTime.Now > startTime.AddSeconds(TIME_OUT)) Then
                    Return
                End If
            End While
        Catch
            Return
        End Try
    End Sub

    Private Function ShouldLoad(ByVal DT As Date, ByVal BankID As Integer, ByVal FormID As String) As Boolean
        Dim rcd As New ADODB.Recordset
        rcd = DBQuery("SELECT  COUNT(*) FROM " & SQL_DB_DEV & "DI_MBK_BALS_F" & FormID & "_DATA WHERE BankID= " & BankID & " AND Date= '" & DT.ToString(SQL_DATE_FORMAT) & "'")
        If Not rcd Is Nothing Then
            If Not rcd.EOF Then
                If rcd.Fields(0).Value > 0 Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

#End Region

    Public Class DataUploader

        Public BankID As Integer
        Public DT As Date
        Public Rows() As String
        Public FormID As String

        Public Sub New(ByVal BankID As Integer, ByVal DT As Date, ByVal FormID As String)
            Me.BankID = BankID
            Me.DT = DT
            Me.FormID = FormID
            ReDim Rows(0)
        End Sub

        Public Sub AddRow(ByVal s As String)
            Me.Rows(UBound(Me.Rows)) = s
            ReDim Preserve Me.Rows(UBound(Me.Rows) + 1)
        End Sub

        Public Function Upload() As Result
            Dim rcd As New ADODB.Recordset
            rcd = DBQuery("DELETE FROM " & TableName() & " WHERE BankID=" & Me.BankID & " AND Date='" & Me.DT.ToString(SQL_DATE_FORMAT) & "'")

            Dim columns As String = Me.Columns

            For k As Integer = UBound(Rows) - 1 To 0 Step -1
                Dim Row As String = Rows(k)
                rcd = DBQuery(SQLText:="SELECT * FROM " & TableName() & " WHERE BankID=" & Me.BankID & " AND Date='" & Me.DT.ToString(SQL_DATE_FORMAT) & "' AND ID='" & Split(Row, " ")(0) & "'", Quiet:=True)

                If rcd.EOF Then rcd.AddNew()
                rcd.Fields("BankID").Value = Me.BankID
                rcd.Fields("Date").Value = Me.DT.ToString(SQL_DATE_FORMAT)
                Try
                    For i As Integer = 0 To UBound(Split(columns, ", "))
                        rcd.Fields(Split(columns, ", ")(i)).Value = Split(Row, " ")(i)
                    Next
                    rcd.Update()
                Catch e As Exception
                    Log.Write("Failed to upload row: " & e.Message, "DataUploader.Upload")
                End Try
            Next
            Log.Write(Rows.Count & " rows uploaded", "DataUploader.Upload")
            Return Result.RESULT_OK
        End Function

        Private Function Columns() As String
            Select Case FormID
                Case "101"
                    Return "ID, VR, VV, VITG, ORA, OVA, OITGA, ORP, OVP, OITGP, IR, IV, IITG"
                Case "102"
                    Return "ID, R, V, ITOGO"
                Case "134"
                    Return "ID, Value"
                Case "135_1", "135_3", "135_4", "135_5"
                    Return "ID, Value"
                Case Else
                    Return ""
            End Select
        End Function

        Private Function TableName() As String
            Select Case FormID
                Case "101", "102", "134"
                    Return SQL_DB_DEV & "DI_MBK_BALS_F" & FormID & "_DATA"
                Case "135_1"
                    Return SQL_DB_DEV & "DI_MBK_BALS_F135_DATA_ACC"
                Case "135_2"
                    Return SQL_DB_DEV & "DI_MBK_BALS_F135_DATA_CAL"
                Case "135_3"
                    Return SQL_DB_DEV & "DI_MBK_BALS_F135_DATA_FIG"
                Case "135_4"
                    Return SQL_DB_DEV & "DI_MBK_BALS_F135_DATA_FOUL"
                Case "135_5"
                    Return SQL_DB_DEV & "DI_MBK_BALS_F135_DATA_NORM"
                Case Else
                    Return ""
            End Select
        End Function

    End Class

End Module
