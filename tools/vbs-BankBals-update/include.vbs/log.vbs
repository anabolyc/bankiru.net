'©Malyshenko A., 2013
' Module version 1.3
Option Explicit

Public Const LOG_CLEAR_LOGS_AFTER_DAYS = 30
Public Const LOG_USE_HTML = 1

Public Const LOG_SEND_EMAILS = 0
Public Const LOG_SEND_ON_WARN = 1
Public Const LOG_SEND_ON_ERR = 1
Public Const LOG_SEND_ON_OK = 0
Public Const LOG_EMAIL_OPERATOR = "andriy.malyshenko@gazprombank.ru"
Public Const LOG_EMAIL_SENDER = "FACTSET@gazprombank.ru"
Public Const LOG_SMTP_SERVER = "10.200.98.6"
	
Class Logger

	Private VerboseLevel
	Private ErrorFlag
	Private WarningFlag
    Private objTextFile
	Private FileName
	Private fso

	Public Sub Class_Initialize()
		Set fso = CreateObject("Scripting.FileSystemObject")
		Dim FolderName	:	FolderName = ".\logs\"
		If Not fso.FolderExists(FolderName) Then
			fso.CreateFolder FolderName
		End If
		Dim i			:	i = 0
		Do
			If i = 0 Then
				FileName = ".\logs\" & WScript.ScriptName & "." & Replace(Replace(Replace(Replace(CStr(Now), ".", "-"), ":", "-"), "/", "-"), "\", "-") & ".log"
			Else
				FileName = ".\logs\" & WScript.ScriptName & "." & Replace(Replace(Replace(Replace(CStr(Now), ".", "-"), ":", "-"), "/", "-"), "\", "-") & "." & i & ".log"
			End If
			If Not fso.FileExists(FileName) Then
				Set objTextFile = fso.CreateTextFile(FileName, 1, 1)
				FileName = fso.GetAbsolutePathName(FileName)	
				If LOG_USE_HTML Then objTextFile.WriteLine "<html><body><pre>"
				Exit Do
			Else
				i = i + 1
			End If
		Loop
	End Sub
	
	Public Sub Class_Terminate
		If LOG_USE_HTML Then objTextFile.WriteLine "</pre></body></html>"
		objTextFile.Close
		If LOG_SEND_EMAILS Then
			If (ErrorFlag And LOG_SEND_ON_ERR) Or (WarningFlag And LOG_SEND_ON_WARN) Or LOG_SEND_ON_OK Then
				With CreateObject("WScript.Shell")
					Dim Command		:	Command = ".\bin\BLAT\blat.exe"
					If fso.FileExists(Command) Then
						Dim oEnv	:	Set oEnv = .Environment("PROCESS")
						oEnv("SEE_MASK_NOZONECHECKS") = 1
						Command = Command & " " & """" & FileName & """ -server " & LOG_SMTP_SERVER & " -to " & LOG_EMAIL_OPERATOR & " -f " & LOG_EMAIL_SENDER & " -subject ""Report from " & WScript.ScriptFullName & " @ " & CreateObject( "WScript.Network" ).ComputerName & """ "
						If LOG_USE_HTML Then Command = Command & " -html"
						Dim result	:	result = .Run(Command, 10 ,1)
						If result <> 0 Then
							WScript.Echo "FAILED to send message via blat.exe!"
						End If 
					Else
						WScript.Echo "blat.exe NOT FOUND!"
					End If
					
				End With
			End If
		End If
		If LOG_CLEAR_LOGS_AFTER_DAYS > 0 Then
			Dim files	:	Set files = fso.GetFolder(".\logs\").Files
			Dim file
			For Each file In files
				If file.DateCreated + LOG_CLEAR_LOGS_AFTER_DAYS < Now Then
					WScript.Echo file.Name & " deleted as part of cleanup process"
					file.Delete True
				End If
			Next
		End If
	End Sub
	
	Public Function GetState()
		If ErrorFlag Then
			GetState = -2
		Else
			If WarningFlag Then
				GetState = -1
			Else
				GetState = 0
			End If
		End If
	End Function 
	
	Public Function Debug(Who, byval msg)
		If VerboseLevel >= 1 Then
			Dim result
			result = TimeString() & vbtab & " [" + Who + "]: "
			result = result & msg
			WScript.Echo result
			If LOG_USE_HTML Then result = "<font color='gray' size='-1'>" & result & "</font>"
			objTextFile.WriteLine result
		End If
	End Function
	
	Public Function Info(Who, byval msg)
		If VerboseLevel >= 0 Then
			Dim result
			result = TimeString() & vbtab & " [" + Who + "]: "
			result = result & msg
			WScript.Echo result
			objTextFile.WriteLine result
		End If
	End Function
	
	Public Function Warn(Who, byval msg)
		WarningFlag = true
		If VerboseLevel >= -1 Then
			Dim result
			result = TimeString() & vbtab & " [" + Who + "]: "
			result = result & msg
			WScript.Echo " . . . . . . . . . . . . . . . W A R N I N G . . . . . . . . . . . . . . . . . "
			WScript.Echo result
			If LOG_USE_HTML Then result = "<font color='brown'>" & result & "</font>"
			objTextFile.WriteLine result
		End If
	End Function
	
	Public Function Err(Who, byval msg)
		ErrorFlag = true
		If VerboseLevel >= -2 Then
			Dim result
			result = TimeString() & vbtab & " [" + Who + "]: "
			result = result & msg
			WScript.Echo " * * * * * * * * * * * * * * * * * E R R O R * * * * * * * * * * * * * * * * * "
			WScript.Echo result
			If LOG_USE_HTML Then result= "<font color='red' size='+1'>" & result & "</font>"
			objTextFile.WriteLine result
		End If
	End Function
	
	Public Function SetVerboseLevel(level)
		VerboseLevel = level
	End Function
		
	Private Function TimeString() 
	Dim result
	Dim h:	h = DatePart("h", Now)
		if len(h) = 1 then h = " " & h
	    result = h & ":" 
	Dim n:	n = DatePart("n", Now)
		if len(n) = 1 then n = "0" & n
		result = result & n & ":" 
	Dim s:	s = DatePart("s", Now)
		if len(s) = 1 then s = "0" & s
		result = result & s
	TimeString = result
	End Function
End Class

