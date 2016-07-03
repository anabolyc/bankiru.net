Imports www.BankBals.Classes
Imports www.BankBals.Data

Namespace Controllers

    Public Class ViewController
        Inherits BaseController

        '<CompressFilter()> _
        <OutputCache(Duration:=CACHE_LONG)> _
        Function List() As ActionResult
            Return View((New Repository).GetViewItems().ToList())
        End Function

        '<CompressFilter()> _
        '<OutputCache(Duration:=CACHE_LONG)> _
        'Function ListFrame() As ActionResult
        '    Return View("List", "~/Views/Shared/_Frame.vbhtml", Repository.GetViewItems().ToList())
        'End Function

        '<CompressFilter()> _
        <HttpPost()> _
        <OutputCache(Duration:=CACHE_LONG)> _
        Function ListJSON() As ActionResult
            Return Json((New Repository).GetViewItemsAjax())
        End Function

        '<CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function Chart(Optional ByVal ViewID As Integer = 1, Optional ByVal ViewItemID As Integer = 1, Optional ByVal BankID As Integer = -1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.ViewQuery = New Data.ViewQuery(ViewID, ViewItemID)
            ViewBag.IsRatio = _data.IsRatio
            ViewBag.AggItem = _data.AggItem
            Return View(_data.GetChartData(BankID, SetID))
        End Function

        '<CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function Data(Optional ByVal ViewID As Integer = 1, Optional ByVal ViewItemID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.ViewQuery = New Data.ViewQuery(ViewID, ViewItemID)
            ViewBag.View = _data.View
            ViewBag.ViewItem = _data.ViewItem
            ViewBag.AggItem = _data.AggItem
            ViewBag.Sets = _data.Sets
            ViewBag.Dates = _data.Dates
            ViewBag.IsRatio = _data.IsRatio
            Return View(_data.GetData(SetID))
        End Function

        Function GetExcel(Optional ByVal ViewID As Integer = 1, Optional ByVal ViewItemID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim Addr As String = Request.ServerVariables("SERVER_NAME") & ":" & Request.ServerVariables("SERVER_PORT") & IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)
            Dim _data As Data.ViewQuery = New Data.ViewQuery(ViewID, ViewItemID)
            Dim FileName As String = String.Empty
            Dim FilePath As String = _data.GetExportFile(Request.PhysicalApplicationPath & "Content\export\", FileName, Addr, SetID)

            Dim FileResult As System.IO.FileInfo = New System.IO.FileInfo(FilePath)
            Return File(FileResult.FullName, "application/excel", FileName)
        End Function

        '<CompressFilter()> _
        Function GetExcel2(Optional ByVal ViewID As Integer = 1, Optional ByVal ViewItemID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim Result As New ContentResult()
            Dim _data As Data.ViewQuery = New Data.ViewQuery(ViewID, ViewItemID)
            Result.ContentType = "text/csv"
            Result.ContentEncoding = System.Text.Encoding.GetEncoding(1251)
            Result.Content = _data.GetExportTableText(True, SetID)
            Response.AddHeader("content-disposition", "filename=" + _data.View.ViewNameEng & " - " & _data.AggItem.NameEng & ".csv")
            Return Result
        End Function

        '<CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function GetExcelWebService(Optional ByVal ViewID As Integer = 1, Optional ByVal ViewItemID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.ViewQuery = New Data.ViewQuery(ViewID, ViewItemID)
            Dim Result As New ContentResult()
            Result.Content = _data.GetExportTableText(False, SetID)
            Result.ContentEncoding = System.Text.Encoding.UTF8
            Result.ContentType = "text/html"
            Return Result
        End Function

    End Class

End Namespace
