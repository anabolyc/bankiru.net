Imports System.Reflection
Imports System.IO
Imports NLog
Imports SevenZip

Public Class FileUploader
    Implements IDisposable

    Private Shared between As String = "BETWEEN " & Integer.MinValue.ToString & " AND " & Integer.MaxValue.ToString
    Private Const DEFAULT_DATE As Date = #1/1/1900#

    Public Shared Sub Upload(ByVal TempFolder As String, ByVal FileName As String, ByVal fileStream As Stream, ByRef L As Logger)
        Dim DT As DateTime
        Try
            DT = DateSerial(Right(Left(FileName, 8), 4), Right(Left(FileName, 10), 2), 1)
        Catch ex As Exception
            L.Error("Cannot get date from file: {0}", FileName)
            Exit Sub
        End Try

        '@@ need to resolve path problem
        'Dim SevenZipDllPath As String = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "7z.dll")

        SevenZipExtractor.SetLibraryPath("c:\Inetpub\wwwroot\bankbals\bin\7z.dll")
        Using ext As SevenZipExtractor = New SevenZipExtractor(fileStream)
            For i As Integer = 0 To ext.FilesCount - 1
                Dim dbfName As String = ext.ArchiveFileNames.Item(i).ToUpper
                Dim dbfFile As New FileStream(Path.Combine(TempFolder, dbfName), FileMode.Create, FileAccess.Write, FileShare.Read)
                ext.ExtractFile(i, dbfFile)
                dbfFile.Close()

                Select Case Left(FileName, 3)
                    Case "101"
                        Select Case Right(dbfName, 6)
                            Case "ES.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "S1.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "_S.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "N1.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "_N.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "_B.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, NULL AS VR, NULL AS VV, NULL AS VITG, NULL AS ORA, NULL AS OVA, NULL AS OITGA, NULL AS ORP, NULL AS OVP, NULL AS OITGP, NULL AS IR, NULL AS IV, ITOGO AS IITG", "")
                                '"WHERE (ITOGO " + between + ")", 0
                            Case "B1.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, VR, VV, VITG, ORA, OVA, OITGA, ORP, OVP, OITGP, IR, IV, IITG", "")
                                '"WHERE (IITG " + between + ") AND (OITGA " + between + ") AND (OITGP " + between + ") AND (VITG " + between + ")", 0
                            Case Else
                                L.Warn("Unknown file: {0}", dbfName)
                        End Select
                    Case "102"
                        Select Case Right(dbfName, 7)
                            Case "AV1.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "_SP.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "SP1.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "_NP.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "NP1.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "_P1.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, SIM_R AS R, SIM_V AS V, SIM_ITOGO AS ITOGO", "", DT)
                            Case Else
                                If Right(FileName, 6) = "_P.DBF" Then
                                    L.Info("Processing file: {0}", dbfName)
                                    ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, NULL AS R, NULL AS V, ITOGO AS ITOGO", "", DT)
                                Else
                                    L.Warn("Unknown file: {0}", dbfName)
                                End If
                        End Select
                    Case "134"
                        Select Case Right(dbfName, 8)
                            Case "134D.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F134_DATA_TEMP", "REGN AS BankID, C1 AS [ID], C3 AS [Value]", "", DT)
                            Case "134N.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "134B.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case Else
                                L.Warn("Unknown file: {0}", dbfName)
                        End Select
                    Case "135"
                        Select Case Right(dbfName, 8)
                            Case "35_1.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_ACC", "REGN AS BankID, C1_1 AS [ID], C2_1 AS [Value]", "", DT)
                            Case "35_2.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_FIG", "REGN AS BankID, C1_2 AS [ID], C2_2 AS [Value]", "", DT)
                            Case "35_3.DBF"
                                If DT <= DateSerial(2011, 1, 1) Then
                                    L.Info("Processing file: {0}", dbfName)
                                    ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_3 AS [RowNum], C2_3 AS [ID], C3_3 AS [Xnum], C4_3 AS [XDate]", "", DT)
                                Else
                                    L.Info("Processing file: {0}", dbfName)
                                    ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_NORM", "REGN AS BankID, C1_3 AS [ID], C2_3 AS [Value]", "", DT)
                                End If
                            Case "35_4.DBF"
                                If DT <= DateSerial(2011, 1, 1) Then
                                    L.Info("Processing file: {0}", dbfName)
                                    ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_4 AS [Holiday]", "", DT)
                                Else
                                    L.Info("Processing file: {0}", dbfName)
                                    ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_4 AS [RowNum], C2_4 AS [ID], C3_4 AS [Xnum], C4_4 AS [XDate]", "", DT)
                                End If
                            Case "35_5.DBF"
                                L.Info("Processing file: {0}", dbfName)
                                ImportDBFtoSQLServer(L, TempFolder, dbfName, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_5 AS [Holiday]", "", DT)
                            Case "35_6.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case "135B.DBF"
                                L.Info("Skipping file: {0}", dbfName)
                            Case Else
                                L.Warn("Unknown file: {0}", dbfName)
                        End Select
                    Case Else
                        L.Warn("Unknown Form: " & Left(FileName, 3))
                End Select

                File.Delete(Path.Combine(TempFolder, dbfName))
            Next
        End Using
    End Sub

    Private Shared Sub ImportDBFtoSQLServer(ByRef L As Logger, ByVal TempFolder As String, ByVal fileName As String, ByVal tableName As String, ByVal columnsList As String, ByVal whereClause As String, Optional ByVal DT As Date = DEFAULT_DATE)
        Dim tblName : tblName = Split(fileName, ".")(0)
        Dim cpyFile As String = String.Empty
        If Len(tblName) > 8 Then
            cpyFile = Path.Combine(TempFolder, Right(fileName, 8 + ".dbf".Length))
            File.Copy(Path.Combine(TempFolder, fileName), cpyFile)
            tblName = Right(tblName, 8)
        End If

        Dim obj As New ADODB.DataTransfer
        obj.ConnectionStringFrom = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=dBase IV;Data Source=" & TempFolder

        obj.ColumnsList = columnsList
        obj.WhereClause = whereClause
        obj.TableName = tblName
        L.Debug("Loading data from: " & fileName)
        If obj.DownloadData() Then
            L.Info("Done!")
            obj.ConnectionStringTo = "Data Source=192.168.1.12;Integrated Security=True;Initial Catalog=BankBals;"
            obj.DestinationTable = tableName
            If DT <> DEFAULT_DATE Then obj.AssignDate = DT
            L.Debug("Uploading data")
            If obj.UploadData() Then
                L.Debug("Done!")
            Else
                L.Error("Failed to upload data: " & obj.LastException().Message)
            End If
        Else
            L.Error("Failed to get data from DBF: " & obj.LastException().Message)
        End If
        If cpyFile <> String.Empty Then
            File.Delete(cpyFile)
        End If
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
