Imports www.BankBals.Classes
Imports www.BankBals.Data

Namespace Controllers

    Public Class BankController
        Inherits BaseController

        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_LONG)> _
        Function List() As ActionResult
            Return View((New Repository).GetBanks.ToList())
        End Function

        <HttpPost()> _
        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_LONG)> _
        Function ListJSON(Optional ByVal LoadComparables As Boolean = False) As ActionResult
            Return Json((New Repository).GetBanksItemsAjax(LoadComparables))
        End Function

        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function Chart(Optional ByVal ViewID As Integer = 1, Optional ByVal ViewItemID As Integer = 1, Optional ByVal BankID As Integer = -1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            ViewBag.Bank = _data.Bank
            Return View(_data.GetChartData(ViewItemID, SetID))
        End Function

        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function Data(Optional ByVal BankID As Integer = -1, Optional ByVal ViewID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            ViewBag.Views = _data.Views
            ViewBag.View = _data.View
            ViewBag.Sets = _data.Sets
            ViewBag.Dates = _data.Dates
            ViewBag.Bank = _data.Bank
            Return View(_data.GetData(SetID))
        End Function

        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        <HttpPost()> _
        Function DataJSONAggItem(ByVal BankID As Integer, ByVal ViewID As Integer, ByVal ViewItemTicker As String, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            Dim AggItem As A_AGGITEM = (New Repository()).GetAggItems(ViewItemTicker)
            Return Json(_data.GetAggItemData(AggItem, SetID))
        End Function

        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        <HttpPost()> _
        Function MoreDataJSON(ByVal BankID As Integer, ByVal ViewID As Integer, ByVal ViewItem As Integer, Optional ByVal SetID As Integer = 1, Optional ByVal IsAggItemID As Boolean = True) As ActionResult
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            If IsAggItemID Then
                Return Json(_data.GetMoreData1(ViewItem, SetID))
            Else
                Return Json(_data.GetMoreData2(ViewItem, SetID))
            End If
        End Function

        Function GetExcel(Optional ByVal BankID As Integer = -1, Optional ByVal ViewID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim Addr As String = Request.ServerVariables("SERVER_NAME") & ":" & Request.ServerVariables("SERVER_PORT") & IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            Dim FileName As String = String.Empty
            Dim FilePath As String = _data.GetExportFile(Request.PhysicalApplicationPath & "Content\export\", FileName, Addr, SetID)

            Dim FileResult As System.IO.FileInfo = New System.IO.FileInfo(FilePath)
            Return File(FileResult.FullName, "application/excel", FileName)
        End Function

        <CompressFilter()> _
        Function GetExcel2(Optional ByVal BankID As Integer = -1, Optional ByVal ViewID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim Result As New ContentResult()
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            Result.ContentType = "text/csv"
            Result.ContentEncoding = System.Text.Encoding.GetEncoding(1251)
            Result.Content = _data.GetExportTableText(True, SetID)
            Response.AddHeader("content-disposition", "filename=" + _data.Bank.NameEng & " - " & _data.View.ViewNameEng & ".csv")
            Return Result
        End Function

        <CompressFilter()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function GetExcelWebService(Optional ByVal BankID As Integer = -1, Optional ByVal ViewID As Integer = 1, Optional ByVal SetID As Integer = 1) As ActionResult
            Dim _data As Data.BankQuery = New Data.BankQuery(BankID, ViewID)
            Dim Result As New ContentResult()
            Result.Content = _data.GetExportTableText(False, SetID)
            Result.ContentEncoding = System.Text.Encoding.UTF8
            Result.ContentType = "text/html"
            Return Result
        End Function

    End Class
End Namespace
