Imports ADODB

Module db

    Private Const CONSOLE_COLOR As ConsoleColor = ConsoleColor.DarkBlue

    Public Structure SqlParameter
        Public ParamName As String
        Public ParamType As DataTypeEnum
        Public Direction As ParameterDirectionEnum
        Public Size As Long
        Public Value As Object
    End Structure

    Public Function SqlParameterToken(Optional ByVal ParamName$ = vbNullString, Optional ByVal ParamType As DataTypeEnum = DataTypeEnum.adEmpty, Optional ByVal Direction As ParameterDirectionEnum = ParameterDirectionEnum.adParamInput, Optional ByVal Size As Long = 0, Optional ByVal Value As Object = 0) As SqlParameter
        SqlParameterToken.ParamName = ParamName
        SqlParameterToken.ParamType = ParamType
        SqlParameterToken.Direction = Direction
        SqlParameterToken.Size = Size
        SqlParameterToken.Value = Value
    End Function

    Public Function CmdAppendParameter(ByVal cmd As ADODB.Command, ByVal Param As SqlParameter) As ADODB.Command
        cmd.Parameters.Append(cmd.CreateParameter(Param.ParamName, Param.ParamType, Param.Direction, CInt(Param.Size), Param.Value))
        CmdAppendParameter = cmd
    End Function

    Private Function connect(Optional ByVal ForceReconnect As Boolean = False, Optional ByVal CursorLocation As ADODB.CursorLocationEnum = CursorLocationEnum.adUseServer, Optional ByVal overrideConnectionString$ = vbNullString, Optional ByVal RunInContext As String = "master") As ADODB.Connection
        Static conn As ADODB.Connection
        If ForceReconnect Then conn = Nothing

        If (conn Is Nothing) Then
            Log.Write("Соединяюсь с БД", "connect", False, False, CONSOLE_COLOR)
            conn = New ADODB.Connection
            conn.CommandTimeout = 0
            If overrideConnectionString = vbNullString Then
                conn.ConnectionString = connectionString()
            Else
                conn.ConnectionString = overrideConnectionString
            End If
            conn.CursorLocation = CursorLocation
            On Error GoTo errExit
            conn.Open()
            conn.DefaultDatabase = RunInContext
            On Error GoTo 0
        ElseIf conn.State = 0 Then
            On Error GoTo errExit
            conn.Open()
            On Error GoTo 0
        End If
        connect = conn
        conn.DefaultDatabase = RunInContext
        Exit Function
errExit:
        Log.Write("Не могу соединиться с БД", "connect", True)
        connect = Nothing
        On Error GoTo 0
    End Function

    Private Function connectionString() As String
        connectionString = Main.ConnectionString.ConnectionString
    End Function

    Public Function DBQuery(ByVal SQLText As String, Optional ByVal cursorType As ADODB.CursorTypeEnum = CursorTypeEnum.adOpenDynamic, Optional ByVal lockType As ADODB.LockTypeEnum = LockTypeEnum.adLockOptimistic, Optional ByVal CursorLocation As ADODB.CursorLocationEnum = CursorLocationEnum.adUseServer, Optional ByVal ForceReconnect As Boolean = False, Optional ByVal overrideConnectionString$ = vbNullString, Optional ByVal Quiet As Boolean = False, Optional ByVal RunInContext As String = "master") As ADODB.Recordset
        Dim TRY_COUNT%
        TRY_COUNT = 3
start:
        If Not Quiet Then
            Log.Write(SQLText, "DBQuery", False, False, CONSOLE_COLOR)
        End If

        DBQuery = New ADODB.Recordset
        On Error GoTo errExit
        If RunInContext <> vbNullString Then

        End If
        DBQuery.Open(SQLText, connect(ForceReconnect, CursorLocation, overrideConnectionString, RunInContext), cursorType, lockType)
        On Error GoTo 0
        Exit Function
errExit:
        'try reconnect
        If (Err.Number = -2147467259) And (TRY_COUNT > 0) Then
            connect(True, CursorLocation)
            TRY_COUNT = TRY_COUNT - 1
            On Error GoTo 0
            GoTo start
        End If

        DBQuery = Nothing
        Log.Write(Err.Description, "DBQuery", True)
        On Error GoTo 0
    End Function

    Private Function readFileContents$(ByVal path$, Optional ByVal lineBreak$ = vbNullString)
        'считывает файл в стринг
        Dim result$ = vbNullString
        Dim s$ = vbNullString
        Dim x% = FreeFile()
        If Dir(path) <> vbNullString Then
            FileOpen(x, path, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
            Do While Not EOF(x)
                Input(x, s)
                result = result & lineBreak & s
            Loop
            FileClose(x)
        End If
        Return result
    End Function

    Public Function DBCommand(ByVal SQLText$, Optional ByVal CursorLocation As ADODB.CursorLocationEnum = CursorLocationEnum.adUseServer, Optional ByVal ForceReconnect As Boolean = False, Optional ByVal overrideConnectionString$ = vbNullString, Optional ByVal Quiet As Boolean = False) As ADODB.Command
        Dim TRY_COUNT%
        TRY_COUNT = 3
start:
        If Not Quiet Then
            Log.Write(SQLText, "DBCommand", False, False, CONSOLE_COLOR)
        End If
        DBCommand = New ADODB.Command
        On Error GoTo errExit
        DBCommand.ActiveConnection = connect(ForceReconnect, CursorLocation, overrideConnectionString)
        DBCommand.CommandText = SQLText
        On Error GoTo 0
        Exit Function
errExit:
        'try reconnect
        If (Err.Number = -2147467259) And (TRY_COUNT > 0) Then
            connect(True)
            TRY_COUNT = TRY_COUNT - 1
            On Error GoTo 0
            GoTo start
        End If

        DBCommand = Nothing
        Log.Write(Err.Description, "DBCommand", False, True)
        On Error GoTo 0
    End Function

    Public Function DBQueryP(ByVal SQLText$, ByVal params() As SqlParameter, Optional ByVal CursorLocation As ADODB.CursorLocationEnum = CursorLocationEnum.adUseServer, Optional ByVal ForceReconnect As Boolean = False, Optional ByVal overrideConnectionString$ = vbNullString, Optional ByVal Quiet As Boolean = False) As ADODB.Recordset
        'Optional cursorType As ADODB.CursorTypeEnum = adOpenDynamic, Optional lockType As ADODB.LockTypeEnum = adLockOptimistic,
        Dim cmd As ADODB.Command, i%, j%
        cmd = DBCommand(SQLText, CursorLocation, ForceReconnect, overrideConnectionString, Quiet)
        On Error GoTo errExit
        For i = LBound(params) To UBound(params)
            CmdAppendParameter(cmd, params(i))
        Next
        DBQueryP = cmd.Execute
        'DBQueryP.cursorType = cursorType
        'DBQueryP.lockType = lockType
        Exit Function
errExit:
        DBQueryP = Nothing
        Log.Write(Err.Description, "DBQueryP", False, True)
        On Error GoTo 0
    End Function

End Module
