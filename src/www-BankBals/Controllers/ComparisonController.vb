Imports www.BankBals.Classes
Imports www.BankBals.Data

Namespace Controllers

    <CompressFilter()> _
    Public Class ComparisonController
        Inherits BaseController

        <OutputCache(Duration:=CACHE_LONG)> _
        Function Index() As ActionResult
            Return View((New Repository).GetBanks.ToList())
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function Compare(Optional ByVal ViewID As Integer = 1, Optional ByVal BankID0 As Integer = -1, Optional ByVal BankID1 As Integer = -1, Optional ByVal BankID2 As Integer = -1) As ActionResult
            Dim R As Repository = New Repository()

            Dim BL As IQueryable(Of A_BANKS_ALL) = R.GetBanks()
            Dim B As A_BANKS_ALL = BL.First(Function(B0) B0.BankID = BankID0)
            Dim B1 As A_BANKS_ALL = BL.First(Function(B0) B0.BankID = BankID1)
            Dim B2 As A_BANKS_ALL = BL.First(Function(B0) B0.BankID = BankID2)


            ViewBag.Views = R.GetViewsCompare()
            ViewBag.ViewItems = R.GetViewItemsCompare(ViewID)
            ViewBag.ViewID = ViewID
            ViewBag.Bank = B
            ViewBag.BankC1 = B1
            ViewBag.BankC2 = B2
            ViewBag.BankC3 = BL.First(Function(B0) B0.BankID = -3)
            ViewBag.BankC4 = BL.First(Function(B0) B0.BankID = -4)

            Dim isF102 As Boolean = R.GetViewsCompare().First(Function(V) V.ViewID = ViewID).Form = 102
            Dim dateid As Integer = R.GetLastDate(isF102).DateID
            Dim CV(4)() As CompQuery.Result
            CV(0) = CompQuery.GetCompareData(ViewID, B.BankID, dateid)
            CV(1) = CompQuery.GetCompareData(ViewID, B1.BankID, dateid)
            CV(2) = CompQuery.GetCompareData(ViewID, B2.BankID, dateid)
            CV(3) = CompQuery.GetCompareData(ViewID, -3, dateid)
            CV(4) = CompQuery.GetCompareData(ViewID, -4, dateid)

            Return View(CV)
        End Function

        <OutputCache(Duration:=CACHE_SHORT)> _
        Function CompareBubbles(Optional ByVal BankID As Integer = -1) As ActionResult
            Dim R As Repository = New Repository()
            Dim BL As IQueryable(Of A_BANKS_ALL) = R.GetBanks()

            ViewBag.Views = R.GetViewsCompare()
            ViewBag.Bank = BL.First(Function(B0) B0.BankID = BankID)
            Return View((New Repository()).GetChartsList().ToList())
        End Function

        <HttpPost()> _
        <OutputCache(Duration:=CACHE_SHORT)> _
        Function CompareBubblesJSON(Optional ByVal BankID As Integer = -1, Optional ByVal ChartID As Integer = 1) As ActionResult
            'Dim SQLText = "EXEC GET_2X2CHARTDATA " & BankID & ", " & ChartID & ", " & LANGUAGE
            'Dim export As New Export
            'Return export.AsJSON(SQLText)
            Return Json(CompQuery.GetCompareDataJson(ChartID, BankID))
        End Function

    End Class
End Namespace
