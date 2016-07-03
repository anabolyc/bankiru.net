using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpCompress.Common;
using SharpCompress.Writer;

namespace www.BankBals.Data {

    public class BankQuery {

        public class Dictionary {
            public int ViewItemID;
            public int LevelDepth;
            public bool IsHeader;
            public int? AggItemID;
            public bool? IsRatio;
            public bool ISWithSub;
            public string Ticker;
            public string Name;
        }

        public class Data {
            public long? Value0;
            public long? Value1;
        }

        public class Data2 {
            public bool? IsRatio;
            public string Ticker;
            public string Name;
            public long? Value;
        }

        public class Data3 {
            public DateTime Date;
            public long? Value;
        }

        public class Result2 {
            public A_AGGITEM AggItem;
            public bool IsRatio;
            public IEnumerable<Data3> Data;
            public Result2(IEnumerable<Data3> data, A_AGGITEM aggItem, bool isRatio) {
                this.Data = data;
                this.AggItem = aggItem;
                this.IsRatio = isRatio;
            }
        }

        public class Result {
            public A_VIEWS_SET Set;
            public IEnumerable<Dictionary> Dictionary;
            public IEnumerable<Data> Data;
            public Result(A_VIEWS_SET set, IEnumerable<Dictionary> dictionary, IEnumerable<Data> data) {
                this.Set = set;
                this.Dictionary = dictionary;
                this.Data = data;
            }
        }

        public A_BANKS_ALL Bank { get { return _Bank; } }
        public A_VIEW View { get { return _View; } }
        public IEnumerable<A_VIEW> Views { get { return context.A_VIEWs.Where(V => !V.IsHidden).AsEnumerable<A_VIEW>(); } }
        public IQueryable<A_VIEWS_SET> Sets { get { return _Sets; } }
        public IQueryable<A_DATE> Dates { get { return _Dates; } }

        private A_VIEW _View;
        private A_BANKS_ALL _Bank;
        private BankBalsDataContext context;
        private IQueryable<A_VIEWS_SET> _Sets;
        private IQueryable<A_DATE> _Dates;
        private IQueryable<A_PARAM> _Params;

        public BankQuery(int BankID, int ViewID) {
            context = new BankBalsDataContext(Tools.ConnectionString());
            _View = context.A_VIEWs.First(V => V.ViewID == ViewID);
            _Bank = context.A_BANKS_ALLs.First(B => B.BankID == BankID);
            _Sets = context.A_VIEWS_SETs.Where(S => S.ViewID == ViewID).OrderBy(S => S.OrderNum);
            _Params = context.A_PARAMs.Where(P => (_View.Form == 101 && P.F101) || (_View.Form == 102 && P.F102) || (_View.Form == 123 && P.F123) || (_View.Form == 134 && P.F134) || (_View.Form == 135 && P.F135));
            switch (_View.Form) {
                case 101:
                    _Dates = context.A_DATEs.Where(D => D.IsVisible == true && D.F101 == true).OrderByDescending(D => D.DateID);
                    break;
                case 102:
                    _Dates = context.A_DATEs.Where(D => D.IsVisible == true && D.F102 == true).OrderByDescending(D => D.DateID);
                    break;
                case 123:
                    _Dates = context.A_DATEs.Where(D => D.IsVisible == true && D.F123 == true).OrderByDescending(D => D.DateID);
                    break;
                case 134:
                    _Dates = context.A_DATEs.Where(D => D.IsVisible == true && D.F134 == true).OrderByDescending(D => D.DateID);
                    break;
                case 135:
                    _Dates = context.A_DATEs.Where(D => D.IsVisible == true && D.F135 == true).OrderByDescending(D => D.DateID);
                    break;
                default:
                    _Dates = context.A_DATEs.Where(D => D.IsVisible == true).OrderByDescending(D => D.DateID);
                    break;
            }
            
        }

        public Result GetData(int SetID) {
            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText0 = " SELECT A.ViewItemID, A.LevelDepth, IsHeader = CAST(CASE WHEN A.AggItemID IS NULL THEN 1 ELSE 0 END AS BIT), A.AggItemID, A.IsWithSub, AT.IsRatio, AGG.Ticker, Name = ISNULL(AGG.NameRus, A.HeaderNameRus) "
                            + " FROM A_VIEWITEMS A "
                            + " LEFT JOIN A_F" + _View.Form + "_AGGITEMS AGG ON A.AggItemID=AGG.AggItemID "
                            + " LEFT JOIN A_AGGITEMTYPES AT ON AT.AggItemTypeID = AGG.AggItemTypeID "
                            + " WHERE A.ViewID={0}"
                            + " ORDER BY A.ViewItemID, A.LevelDepth";
            // stupid check which columns we have for that form
            string valuePriColumn = String.Empty;
            if (_Params.Count() == 1) {
                valuePriColumn = "Data." + mode.A_PARAM.Ticker;
            } else {
                valuePriColumn = "CASE ";
                foreach (A_PARAM item in _Params)
                    if (mode.A_PARAM.ParID != item.ParID)
                        valuePriColumn += " WHEN DEF.ParID = " + item.ParID + " THEN DATA." + item.Ticker;
                valuePriColumn += " ELSE Data." + mode.A_PARAM.Ticker + " END";
            }
            string valueSecColumn = (mode.ParID2 == null ? "NULL" : "DATA." + mode.A_PARAM1.Ticker);
            string SQLText1 = " SELECT Value0 = " + valuePriColumn + ", Value1 = " + valueSecColumn
                            + " FROM A_VIEWITEMS DEF "
                            + " CROSS JOIN BB_DATES_LIST2({2}) DATES "
                            + " LEFT JOIN DK_F" + _View.Form + " DATA ON DATA.BankID = {0} AND DATA.DateID=DATES.DateID AND DATA.AggItemID = DEF.AggItemID "
                            + " WHERE DEF.ViewID = {1} "
                            + " ORDER BY DEF.ViewItemID, DEF.LevelDepth, DATES.DateID DESC";

            return new Result(mode,
                context.ExecuteQuery<Dictionary>(SQLText0, _View.ViewID),
                context.ExecuteQuery<Data>(SQLText1, _Bank.BankID, _View.ViewID, _View.Form));
        }

        public string GetExportFile(string FolderName, out string FileName, string ServerHost, int SetID) {
            FileName = _Bank.NameEng + " - " + _View.ViewNameEng + ".xlsx";
            string result = string.Empty;
            string ID = System.Guid.NewGuid().ToString();
            string FilePath = ID + ".xlsx";
            //copy folder to export
            Tools.CopyDirectory(FolderName + "..\\_export\\_template", FolderName + ID);
            //make modifications
            using (StreamWriter outfile = new StreamWriter(FolderName + ID + "\\xl\\connections.xml")) {
                outfile.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                outfile.WriteLine("<connections xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><connection id=\"1\" name=\"Connection\" type=\"4\" refreshedVersion=\"4\" background=\"1\" refreshOnLoad=\"1\" saveData=\"1\"><webPr sourceData=\"1\" parsePre=\"1\" consecutive=\"1\" xl2000=\"1\" url=\"http://" + ServerHost + "/Bank/GetExcelWebService?BankID=" + _Bank.BankID + "&amp;ViewID=" + _View.ViewID + "&amp;SetID=" + SetID + "\"/></connection></connections>");
            }
            
            //pack and remove folder
            using (FileStream zip = File.OpenWrite(FolderName + FilePath)) 
            using (IWriter zipWriter = WriterFactory.Open(zip, ArchiveType.Zip, CompressionType.Deflate)) 
                zipWriter.WriteAll(FolderName + ID, "*", SearchOption.AllDirectories);

            try {
                Directory.Delete(FolderName + ID, true);
            } catch { }

            return FolderName + FilePath;
        }

        public string GetExportTableText(bool isCSV, int SetID) {
            string lineStart = isCSV ? string.Empty : "<tr>";
            string lineEnd = isCSV ? Environment.NewLine : "</tr>";
            string cellStart = isCSV ? String.Empty : "<td>";
            string cellEnd = isCSV ? ";" : "</td>";

            StringBuilder Body = new StringBuilder();
            StringBuilder Head = new StringBuilder();

            Head.Append(cellStart + "Reg.Num." + cellEnd + cellStart + "Name" + cellEnd);
            foreach (A_DATE D in _Dates)
                Head.Append(cellStart + D.Date.ToString("d MMM yyyy") + cellEnd);

            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText = "  SELECT A.LevelDepth, AT.IsRatio, AGG.Ticker, Name = AGG.NameRus, A.AggItemID, DATES.DateID, Value= DATA0." + mode.A_PARAM.Ticker
                            + " FROM A_VIEWITEMS A"
                            + " LEFT JOIN A_F" + _View.Form + "_AGGITEMS AGG ON A.AggItemID=AGG.AggItemID"
                            + " LEFT JOIN A_AGGITEMTYPES AT ON AT.AggItemTypeID = AGG.AggItemTypeID"
                            + " CROSS JOIN BB_DATES_LIST2({2}) DATES"
                            + " LEFT JOIN DK_F" + _View.Form + " DATA0 ON DATA0.AggItemID = A.AggItemID AND DATA0.DateID=DATES.DateID AND DATA0.BankID = {1}"
                            + " WHERE A.ViewID = {0}"
                            + " ORDER BY OrderNum, A.AggItemID, DATES.DateID DESC";

            IEnumerable<Data2> resp = context.ExecuteQuery<Data2>(SQLText, _View.ViewID, _Bank.BankID, _View.Form);
            string Ticker = String.Empty;
            foreach (Data2 row in resp) {
                if (Ticker != row.Ticker) {
                    if (Ticker != String.Empty)
                        Body.Append(lineEnd);
                    Body.Append(lineStart);
                    Ticker = row.Ticker;

                    Body.Append(cellStart + row.Ticker + cellEnd);
                    Body.Append(cellStart + row.Name + cellEnd);
                }
                Body.Append(cellStart);
                Body.Append((row.IsRatio ?? false) ? row.Value / 100.0 : row.Value);
                Body.Append(cellEnd);
            }
            Body.Append(lineEnd);

            if (isCSV)
                return lineStart + Head.ToString() + lineEnd + Body.ToString();
            else
                return "<hmtl> <body><table>" + lineStart + Head.ToString() + lineEnd + Body.ToString() + "</table></body></html>";
        }

        public Result2 GetChartData(int ViewItemID, int SetID) {
            A_VIEWITEM ViewItem = context.A_VIEWITEMs.First(V => V.ViewID == _View.ViewID && V.ViewItemID == ViewItemID);
            A_AGGITEM AggItem = context.A_AGGITEMs.First(A => A.Form == _View.Form && A.AggItemID == ViewItem.AggItemID);
            bool IsRatio = context.A_AGGITEMTYPEs.First(AT => AT.AggItemTypeID == AggItem.AggItemTypeID).IsRatio;

            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText = " SELECT Date = DATES.Date, Value = DATA." + mode.A_PARAM.Ticker
                            + " FROM BB_DATES_LIST2({2}) DATES "
                            + " LEFT JOIN DK_F" + _View.Form + " DATA ON DATA.BankID = {1} AND DATA.DateID=DATES.DateID AND AggItemID = {0} "
                            + " ORDER BY DATES.DateID";

            return new Result2(context.ExecuteQuery<Data3>(SQLText, AggItem.AggItemID, _Bank.BankID, _View.Form), AggItem, IsRatio);
        }
        
        #pragma warning disable 0649
        private class Result3 {
            public int? AggItemID;
            public int? ID;
            public string Ticker;
            public string Name;
            public int DateID;
            public long? Value0;
            public long? Value1;
        }
        #pragma warning restore 0649

        public object GetAggItemData(A_AGGITEM AggItem, int SetID) {
            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText1 = " SELECT AggItemID, ID, Ticker, Name, DateID, Value0 = DATA." + mode.A_PARAM.Ticker + ", Value1 = " + (mode.ParID2 == null ? "NULL" : "DATA." + mode.A_PARAM1.Ticker)
                            + " FROM BB_AGGITEM" + _View.Form + "_DATA({0}, {1}, DEFAULT) DATA";

            IQueryable<Result3> query = context.ExecuteQuery<Result3>(SQLText1, _Bank.BankID, AggItem.AggItemID).ToList().AsQueryable();
            return new {
                Items = query.Select(item => new {
                    ID = item.ID,
                    AggItemID = item.AggItemID,
                    Ticker = item.Ticker,
                    Name = item.Name,
                    Expandable = item.AggItemID.HasValue,
                }).Distinct(),
                Values = query.Select(item => new long?[] { item.Value0, item.Value1 })
            };
        }

        public object GetMoreData1(int AggItemID, int SetID) {
            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText = " SELECT Value0 = DATA." + mode.A_PARAM.Ticker + ", Value1 = " + (mode.ParID2 == null ? "NULL" : "DATA." + mode.A_PARAM1.Ticker)
                           + " FROM BB_DATA101_EX1({0}, {1}, {2}) DATA";
            return new {
                Items = context.A_F101_TOCURITEMs
                .Where(A => A.TOCurItemID != 0)
                .OrderBy(A => A.TOCurItemID)
                .Select(A => new {
                    IsRatio = A.IsRatio,
                    Name = A.NameRus
                }),
                Values = context.ExecuteQuery<Data>(SQLText, _View.ViewID, _Bank.BankID, AggItemID).Select(item => new long?[] { item.Value0, item.Value1 })
            };
        }

        public object GetMoreData2(int ID, int SetID) {
            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText = " SELECT TOCurItemID, DateID, Value0 = DATA." + mode.A_PARAM.Ticker + ", Value1 = " + (mode.ParID2 == null ? "NULL" : "DATA." + mode.A_PARAM1.Ticker)
                           + " FROM BB_DATA101_EX2({0}, {1}, {2}) DATA";
            return new {
                Items = context.A_F101_TOCURITEMs
                .Where(A => A.TOCurItemID != 0)
                .OrderBy(A => A.TOCurItemID)
                .Select(A => new {
                    IsRatio = A.IsRatio,
                    Name = A.NameRus
                }),
                Values = context.ExecuteQuery<Data>(SQLText, _View.ViewID, _Bank.BankID, ID).Select(item => new long?[] { item.Value0, item.Value1 })
            };
        }
    }

    public class CompQuery {
        public A_BANKS_ALL Bank { get { return _Bank; } }
        public A_VIEWS_BANKSPIC View { get { return _View; } }
        public IEnumerable<A_VIEW> Views { get { return context.A_VIEWs.Where(V => !V.IsHidden).AsEnumerable<A_VIEW>(); } }
        public A_DATE Date { get { return _Date; } }

        private A_VIEWS_BANKSPIC _View;
        private A_BANKS_ALL _Bank;
        private BankBalsDataContext context;
        private A_DATE _Date;

        public CompQuery(int BankID, int ViewID) {
            context = new BankBalsDataContext(Tools.ConnectionString());
            _View = context.A_VIEWS_BANKSPICs.First(V => V.ViewID == ViewID);
            _Bank = context.A_BANKS_ALLs.First(B => B.BankID == BankID);
            _Date = context.A_DATEs.Max();
        }

        public class Result {
            public long? Value;
            public long? Rank;
            public long? Chg;
        }

        private static string SQLText1 = "SELECT Value, Rank = ValueRank, Chg = ValueChg FROM BB_DATA_COMPARE({0}, {1}, {2})";
        
        public IEnumerable<Result> GetCompareData() {
            return context.ExecuteQuery<Result>(SQLText1, _View.ViewID, _Bank.BankID, _Date.DateID);
        }

        public static Result[] GetCompareData(int ViewID, int BankID, int DateID) {
            return (new BankBalsDataContext(Tools.ConnectionString())).ExecuteQuery<Result>(SQLText1, ViewID, BankID, DateID).ToArray();
        }

        public class CompDataRow {
            public long RN;
            public int BankID;
            public string BankName;
            public long? TotAss;
            public decimal? X;
            public decimal? Y;
        }

        public static IEnumerable<CompDataRow> GetCompareDataJson(int ChartID, int BankID) {
            string SQLText = "EXEC GET_2X2CHARTDATA {0}, {1}, DEFAULT";
            return (new BankBalsDataContext(Tools.ConnectionString())).ExecuteQuery<CompDataRow>(SQLText, BankID, ChartID);
        }

        
   }

}