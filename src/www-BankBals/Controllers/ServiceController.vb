Imports www.BankBals.Classes
Imports www.BankBals.Data

Namespace Controllers

    <CompressFilter()> _
    Public Class ServiceController
        Inherits BaseController

        <OutputCache(Duration:=CACHE_LONG)> _
        Function Index() As ActionResult
            Return View((New Repository()).GetAggsOnly().ToList())
        End Function

        <OutputCache(Duration:=CACHE_LONG)> _
        Function Sources(Optional ByVal ID As Integer = 101) As ActionResult
            Dim _data As SourcesData = New SourcesData(ID)
            ViewBag.Form = ID
            ViewBag.Dates = _data.Dates
            Return View(_data.GetFormData())
        End Function

        <OutputCache(Duration:=CACHE_LONG)> _
        Function AggsList(Optional ByVal AggID As Integer = -1) As ActionResult
            Dim R As New Repository()
            ViewBag.AggName = R.GetAggsOnly().First(Function(A) A.AggID = AggID).FullNameRUS
            Return View(R.GetAggMembers(AggID).ToList())
        End Function

        Function IP() As ActionResult
            Return Content("<pre>" & Request.UserHostAddress & "</pre>")
        End Function

        <CompressFilter()> _
        <HttpGet()> _
        Function Help(Optional ByVal Form As Integer = 101, Optional ByVal AggItemID As Integer = 1620) As ActionResult
            Return View((New Export).GetHelpData(Form, AggItemID))
        End Function

        '<CompressFilter()> _
        '<HttpPost()> _
        'Function GetHelp(ByVal Form As Integer, ByVal AggItemID As Integer) As ActionResult
        '    Return Json((New Export).GetHelpData(Form, AggItemID))
        'End Function

    End Class

End Namespace
