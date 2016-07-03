Imports System.IO.Compression
Imports System.Web
Imports System.Web.Mvc

Namespace Controllers

    Public Class CompressFilter
        Inherits ActionFilterAttribute

        Public Overrides Sub OnActionExecuting(ByVal filterContext As ActionExecutingContext)
            Dim request As HttpRequestBase = filterContext.HttpContext.Request

            Dim acceptEncoding As String = request.Headers("Accept-Encoding")

            If String.IsNullOrEmpty(acceptEncoding) Then
                Return
            End If

            acceptEncoding = acceptEncoding.ToUpperInvariant()

            Dim response As HttpResponseBase = filterContext.HttpContext.Response

            If acceptEncoding.Contains("GZIP") Then
                response.AppendHeader("Content-encoding", "gzip")
                response.Filter = New GZipStream(response.Filter, CompressionMode.Compress)
            ElseIf acceptEncoding.Contains("DEFLATE") Then
                response.AppendHeader("Content-encoding", "deflate")
                response.Filter = New DeflateStream(response.Filter, CompressionMode.Compress)
            End If
        End Sub

    End Class


End Namespace