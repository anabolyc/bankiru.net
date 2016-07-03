Imports System
Imports System.Web.Mvc

Namespace HtmlHelpers

    Public Module HelpExtensions

        <System.Runtime.CompilerServices.Extension()> _
        Public Function Include(ByVal htmlHelper As HtmlHelper, ByVal path As String) As MvcHtmlString
            Dim abspath As String = System.Web.Hosting.HostingEnvironment.MapPath(path)
            If System.IO.File.Exists(abspath) Then
                Return MvcHtmlString.Create(System.IO.File.ReadAllText(abspath))
            Else
                Return MvcHtmlString.Create("//FILE NOT FOUND: " + path)
            End If
        End Function

        <System.Runtime.CompilerServices.Extension()> _
        Public Function DEBUG(ByVal page As System.Web.Mvc.WebViewPage) As Boolean

            Dim value = False
#If (DEBUG) Then
            value = True
#End If
            Return value
        End Function

    End Module
End Namespace
