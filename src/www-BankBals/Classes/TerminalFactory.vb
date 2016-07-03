'Imports System
'Imports System.IO
'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.Threading

'Namespace Classes

'    Public Class TerminalFactory
'        Private _terminals As New List(Of Terminal)
'        Private CleanerThread As Thread

'        Public Sub New()
'            Me.CleanerThread = New Thread(New ThreadStart(AddressOf CleanTerminals))
'            CleanerThread.Start()
'        End Sub

'        Public Function GetTerminal(ByVal command As String) As Terminal
'            Dim T As New Terminal(command)
'            _terminals.Add(T)
'            Return T
'        End Function

'        Public Function GetByID(ByVal ID As Guid) As Terminal
'            For Each T As Terminal In _terminals
'                If T.ID = ID Then
'                    Return T
'                End If
'            Next
'            Return Nothing
'        End Function

'        Private Sub CleanTerminals()
'            Do While True
'                For Each T As Terminal In _terminals
'                    If T.Finished Then
'                        _terminals.Remove(T)
'                        T = Nothing
'                        Exit For
'                    End If
'                Next
'                Thread.Sleep(60000)
'            Loop
'        End Sub

'        Public Class Terminal
'            Private Process As New Process()
'            Private T As Thread

'            Public ID As Guid = GUID.NewGuid()
'            Public Command As String
'            Public Result As New List(Of String)
'            Public ErrorMsg As String
'            Public Started As Boolean = False
'            Public Finished As Boolean = False

'            Public Sub New(ByVal Command As String)
'                Me.Command = Command
'                Me.T = New Thread(New ThreadStart(AddressOf WorkBody))
'                T.Start()
'            End Sub

'            Private Sub WorkBody()
'                Command = Trim(Command)
'                Dim CommandCore As String = Split(Command, " ")(0)
'                Dim CommandArgs As String = Trim(Right(Command, Command.Length - CommandCore.Length))

'                Dim startInfo As New ProcessStartInfo(CommandCore)
'                startInfo.CreateNoWindow = True
'                startInfo.UseShellExecute = False
'                startInfo.RedirectStandardOutput = True
'                startInfo.RedirectStandardError = True
'                startInfo.Arguments = CommandArgs
'                Me.Process.StartInfo = startInfo
'                AddHandler Me.Process.OutputDataReceived, AddressOf ProcessDataHandler
'                AddHandler Me.Process.ErrorDataReceived, AddressOf ProcessDataHandler

'                Try
'                    Me.Process.Start()
'                    Started = True
'                    Me.Process.BeginOutputReadLine()
'                    Me.Process.BeginErrorReadLine()
'                    While Not Me.Process.HasExited
'                        Thread.Sleep(500)
'                    End While
'                Catch e As Exception
'                    ErrorMsg = e.Message
'                End Try
'                Finished = True
'            End Sub

'            Private Sub ProcessDataHandler(ByVal sendingProcess As Object, ByVal outLine As DataReceivedEventArgs)
'                If Not String.IsNullOrEmpty(outLine.Data) Then
'                    Me.Result.Add(outLine.Data)
'                End If
'            End Sub

'        End Class

'    End Class

'End Namespace