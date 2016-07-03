'© Malyshenko A., 2013
Option Explicit

Public L
Public fso

'===================================== CODE =================================================

Include ".\include.vbs\log.vbs"
Include ".\include.vbs\db.vbs"
WScript.Quit RunMain()

'============================================================================================
Public Function RunMain()
	Set L = new Logger
	Set fso = CreateObject("Scripting.FileSystemObject")
	L.SetVerboseLevel 1
	If WScript.Arguments.Count < 1 Then 
		L.Info "RunMain", "Filename not found. Pass filename as parameter."
	Else
		UploadFile WScript.Arguments(0)
	End If
	RunMain = L.GetState
End Function 

Private function UploadFile(ArchiveName)
	L.Debug "UploadFile", "Enter"
	if fso.FileExists(ArchiveName) then
		GetFormManual fso.GetFile(ArchiveName)
	else
		Log.Err "UploadFile", "File not found: " & fileName
	end if
end function

Private function GetFormManual(file)
	Dim DT, fld
	DT = DateSerial(Right(Left(file.Name, 8), 4), Right(Left(file.Name, 10), 2), 1)

	Dim tmpDir:	tmpDir = ".\temp\"
	if fso.FolderExists(tmpDir) then
		set fld = fso.GetFolder(tmpDir)
		fld.Delete true
	end if
	L.Debug "GetFormManual", "Creating folder"
	fso.CreateFolder tmpDir

	Dim shell:	Set shell = CreateObject("WScript.Shell")
	Dim hresult:	hresult = -1
	select case right(file.Name, 3) 
	case "rar":
		L.Info "GetFormManual", "Unpacking file: " & file.Name
		hresult = shell.Run(".\bin\unrar.exe e -o+ """ + file.Path + """ " + tmpDir, 1, true)
	case "zip":
		L.Info "GetFormManual", "Unpacking file: " & file.Name
		hresult = shell.Run(".\bin\unzip.exe -o """ + file.Path + """ -d " + tmpDir, 1, true)
	case else:
		L.Err "GetFormManual", "Unknown archive format"
	end select
	if hresult <> 0 then L.Err "GetFormManual", "Failed to unpack file"
	
	Dim dbfFile 
	Dim between: between = "BETWEEN -9223372036854775808 AND 9223372036854775807"
	for each dbfFile in fso.GetFolder(tmpDir).Files
		Dim fileName
		fileName = UCase(dbfFile.Name)
		Select Case Left(file.Name, 3)
		Case 101:
			Select Case Right(filename, 6)
			case "ES.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "S1.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "_S.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "N1.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "_N.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "_B.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, NULL AS VR, NULL AS VV, NULL AS VITG, NULL AS ORA, NULL AS OVA, NULL AS OITGA, NULL AS ORP, NULL AS OVP, NULL AS OITGP, NULL AS IR, NULL AS IV, ITOGO AS IITG", "", 0
				'"WHERE (ITOGO " + between + ")", 0
			case "B1.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, VR, VV, VITG, ORA, OVA, OITGA, ORP, OVP, OITGP, IR, IV, IITG", "", 0
				'"WHERE (IITG " + between + ") AND (OITGA " + between + ") AND (OITGP " + between + ") AND (VITG " + between + ")", 0
			case else:
				L.Warn "GetFormManual", "Unknown file: " + fileName
			End Select
		Case 102:
			Select Case Right(filename, 7)
			case "AV1.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "_SP.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "SP1.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "_NP.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "NP1.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "_P1.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, SIM_R AS R, SIM_V AS V, SIM_ITOGO AS ITOGO", "", DT
			case else:
				If Right(filename, 6) = "_P.DBF" Then
					L.Info "GetFormManual", "Processing file: " + fileName
					ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, NULL AS R, NULL AS V, ITOGO AS ITOGO", "", DT
				Else
					L.Warn "GetFormManual", "Unknown file: " + fileName
				End IF
			End Select
		Case 123:
			Select Case Right(filename, 8)
			case "123D.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F123_DATA_TEMP", "REGN AS BankID, C1 AS [ID], C3 AS [Value]", "", DT
			case "123N.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "123B.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case else
				L.Warn "GetFormManual", "Unknown file: " + fileName
			End Select
			
		Case 134:
			Select Case Right(filename, 8)
			case "134D.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F134_DATA_TEMP", "REGN AS BankID, C1 AS [ID], C3 AS [Value]", "", DT
			case "134N.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "134B.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case else
				L.Warn "GetFormManual", "Unknown file: " + fileName
			End Select
		Case 135:
			Select Case Right(filename, 8)
			case "35_1.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_ACC", "REGN AS BankID, C1_1 AS [ID], C2_1 AS [Value]", "", DT
			case "35_2.DBF":
				L.Info "GetFormManual", "Processing file: " + fileName
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_FIG", "REGN AS BankID, C1_2 AS [ID], C2_2 AS [Value]", "", DT
			case "35_3.DBF":
				If DT <= DateSerial(2011, 1, 1) Then
					ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_3 AS [RowNum], C2_3 AS [ID], C3_3 AS [Xnum], C4_3 AS [XDate]", "", DT
				Else
					ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_NORM", "REGN AS BankID, C1_3 AS [ID], C2_3 AS [Value]", "", DT
				End if
			case "35_4.DBF":
				If DT <= DateSerial(2011, 1, 1) Then
					ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_4 AS [Holiday]", "", DT
				Else
					ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_4 AS [RowNum], C2_4 AS [ID], C3_4 AS [Xnum], C4_4 AS [XDate]", "", DT
				End if
			case "35_5.DBF":
				ImportDBFtoSQLServer dbfFile, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_5 AS [Holiday]", "", DT
			case "35_6.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case "135B.DBF":
				L.Info "GetFormManual", "Skipping file: " + fileName
			case else
				L.Warn "GetFormManual", "Unknown file: " + fileName
			End Select
		Case Else:
			L.Warn "GetFormManual", "Unknown Form: " & FormID
		End Select

	next
	
	L.Debug "GetFormManual", "Deleting folder"
	set fld = fso.GetFolder(tmpDir)
	On Error Resume Next
	fld.Delete true
	On Error Goto 0
End Function

Private function ImportDBFtoSQLServer(file, tableName, columnsList, whereClause, date)
	Dim tblName:	tblName = Split(file.Name, ".")(0)
	if Len(tblName) > 8 Then
		file.Copy file.ParentFolder + "\" + Right(file.Name, 8 + Len(".dbf")), true
		tblName = Right(tblName, 8)
	End If
				
	Dim obj:	Set obj = CreateObject("ADODB.DataTransfer")
	obj.ConnectionStringFrom = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=dBase IV;Data Source=" & file.ParentFolder
	
	obj.ColumnsList = columnsList
	obj.WhereClause = whereClause
	obj.TableName = tblName
	L.Debug "ImportDBFtoSQLServer", "Loading data from: " & file.Name
	if obj.DownloadData() then
		L.Info "ImportDBFtoSQLServer", "Done!"
		obj.ConnectionStringTo = "Data Source=10.2.200.54;Integrated Security=True;Initial Catalog=BankBals;"
		obj.DestinationTable = tableName
		if date <> 0 then obj.AssignDate = date
		L.Debug "ImportDBFtoSQLServer", "Uploading data"
		if obj.UploadData() then
			L.Debug "ImportDBFtoSQLServer", "Done!"
		else
			L.Err "ImportDBFtoSQLServer", "Failed to upload data: " & obj.LastException().Message
		end if	
	else
		L.Err "ImportDBFtoSQLServer", "Failed to get data from DBF: " & obj.LastException().Message
	end if
End Function

Private Sub Include(fileName)
    With CreateObject("Scripting.FileSystemObject")
		executeGlobal .openTextFile(fileName).readAll()
    End With
End Sub
