'© Malyshenko A., 2009-2012
Option Explicit

Const adOpenDynamic = 2
Const adOpenForwardOnly = 0
Const adOpenKeyset = 1
Const adOpenStatic = 3
Const adOpenUnspecified = -1

Const adLockBatchOptimistic = 4
Const adLockOptimistic = 3
Const adLockPessimistic = 2
Const adLockReadOnly = 1
Const adLockUnspecified = -1

Const adStateClosed = 0
Const adStateOpen = 1
Const adStateConnecting = 2
Const adStateExecuting = 4
Const adStateFetching = 8

Class DB
	Private conn
	
	Private Sub Class_Initialize 
		Set conn = CreateObject("ADODB.Connection")
	End Sub 
	
	Private Function Connect()
	    Connect = E_SUCCESS
	    If conn.State = adStateClosed Then
	        L.log "Connect", "Connecting to DB", False, False
	        Set conn = CreateObject("ADODB.Connection")
	        conn.CommandTimeout = 0
	        conn.connectionString = connectionString()
	        L.log "Connect", conn.connectionString, False, False
	        On Error Resume Next
	        conn.Open
	        If Err.Number <> 0 Then
		        L.log "Connect", "Cannot connect to DB", False, True
			    connect = E_FAIL
			Else
				L.log "Connect", "Connected", False, False 
	        End If
	    End If
	    On Error GoTo 0
	End Function

	Private Function connectionString()
	Dim fso
		Set fso = CreateObject("Scripting.FileSystemObject")
		connectionString = fso.OpenTextFile(".\" & Wscript.ScriptName & ".cfg").ReadAll
	End Function

	Public Function DBQuery(SQLText)
	    If connect = E_SUCCESS Then
	        L.log "DBQuery", "Running query: " & SQLText, False, False
	        Set DBQuery = CreateObject("ADODB.Recordset")
	        On Error Resume Next 
	        DBQuery.Open SQLText, conn, adOpenDynamic, adLockOptimistic
	        If Err.Number <> 0 Then
		        Set DBQuery = Nothing
			    L.log "DBQuery", "Failed to run query: " & Err.Description, False, True
	        Else
	        	'L.log "DBQuery", "Success", False, False
	        End If
	        On Error GoTo 0
	    End If
	End Function
End Class