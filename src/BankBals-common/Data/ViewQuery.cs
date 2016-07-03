using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SharpCompress.Common;
using SharpCompress.Writer;

namespace www.BankBals.Data {
    public class ViewQuery {

        #region Class Definitions

        public class Dictionary {
            public int BankID;
            public string Name;
        }

        public class Data {
            public long? Value0;
            public long? Value1;
        }

        public class Data2 {
            public int DateID;
            public int BankID;
            public string Name;
            public long? Value;
        }

        public class Data3 {
            public DateTime Date;
            public long? Value;
        }

        public class Result2 {
            public string Name;
            public IEnumerable<Data3> Data;
            public Result2(IEnumerable<Data3> data, string name) {
                this.Data = data;
                this.Name = name;
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

        #endregion

        public A_VIEWITEM ViewItem { get { return _ViewItem; } }
        public A_VIEW View { get { return _View; } }
        public A_AGGITEM AggItem { get { return _AggItem; } }
        public IQueryable<A_VIEWS_SET> Sets { get { return _Sets; } }
        public IQueryable<A_DATE> Dates { get { return _Dates; } }
        public bool IsRatio;

        private A_VIEW _View;
        private A_VIEWITEM _ViewItem;
        private A_AGGITEM _AggItem;
        private BankBalsDataContext context;
        private IQueryable<A_VIEWS_SET> _Sets;
        private IQueryable<A_DATE> _Dates;

        private const string DATA_MAIN = "B";

        public ViewQuery(int ViewID, int ViewItemID) {
            context = new BankBalsDataContext(Tools.ConnectionString());
            this._View = context.A_VIEWs.First(V => V.ViewID == ViewID);
            this._ViewItem = context.A_VIEWITEMs.First(VI => VI.ViewID == ViewID && VI.ViewItemID == ViewItemID);
            _Sets = context.A_VIEWS_SETs.Where(S => S.ViewID == ViewID).OrderBy(S => S.OrderNum);
            _AggItem = context.A_AGGITEMs.First(A => A.Form == _View.Form && A.AggItemID == _ViewItem.AggItemID);
            IsRatio = context.A_AGGITEMTYPEs.First(AT => AT.AggItemTypeID == _AggItem.AggItemTypeID).IsRatio;
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
            //if (!Cache.CACHE_ENABLED)
                return _GetData(SetID);
            //else {
            //    string filePath = _filePath(DATA_MAIN, SetID.ToString());
            //    if (!File.Exists(filePath)) {
            //        File.WriteAllText(filePath, JsonConvert.SerializeObject(_GetData(SetID), Formatting.None, new JsonSerializerSettings() { 
            //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //        }));
            //    }
            //    return JsonConvert.DeserializeObject<Result>(File.ReadAllText(filePath));
            //}
        }

        private string _filePath(string Key, params string[] param) {
            string folderPath = Path.Combine(Cache.CacheDirectory, Key);
            if (!Directory.Exists(folderPath)) 
                Directory.CreateDirectory(folderPath);

            string filePath = _View.ViewID + "_" + _ViewItem.ViewItemID;
            foreach (string p in param)
                filePath += "_" + p;
            filePath += ".json";
            return Path.Combine(folderPath, filePath);
        }
        
        private Result _GetData(int SetID) {
            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText0 = " SELECT /*LIST.R, */LIST.BankID, LIST.Name "
                            + " FROM ( "
                            + "	SELECT A.*, B.Name "
                            + "	FROM BB_BANKS_LIST_BYAGG(-6) A "
                            + "	INNER JOIN BB_BANKS_LIST(NULL) B ON A.BankID = B.BankID "
                            + " ) LIST ORDER BY Ord, R, LIST.BankID DESC";

            //_ViewItem.A_PARAM.Ticker
            string SQLText1 = " SELECT Value0 = DATA." + mode.A_PARAM.Ticker + ", Value1 = " + (mode.ParID2 == null ? "NULL" : "DATA." + mode.A_PARAM1.Ticker)
                            + " FROM BB_BANKS_LIST_BYAGG(-6) LIST "
                            + " CROSS JOIN BB_DATES_LIST2({1}) DATES "
                            + " LEFT JOIN DK_F" + _View.Form + " DATA ON DATA.BankID = LIST.BankID AND DATA.DateID=DATES.DateID AND AggItemID = {0} "
                            + " ORDER BY Ord, R, LIST.BankID DESC, DATES.DateID DESC";

            return new Result(mode
                , context.ExecuteQuery<Dictionary>(SQLText0)
                , context.ExecuteQuery<Data>(SQLText1, _ViewItem.AggItemID, _View.Form));

        }

        public string GetExportFile(string FolderName, out string FileName, string ServerHost, int SetID) { 
            FileName = _View.ViewNameEng + " - " + _AggItem.NameEng + ".xlsx";
            string result = string.Empty;
            string ID = System.Guid.NewGuid().ToString();
            string FilePath = ID + ".xlsx";
            //copy folder to export
            Tools.CopyDirectory(FolderName + "..\\_export\\_template", FolderName + ID);
            //make modifications
            using (StreamWriter outfile = new StreamWriter(FolderName + ID + "\\xl\\connections.xml")) {
                outfile.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                outfile.WriteLine("<connections xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><connection id=\"1\" name=\"Connection\" type=\"4\" refreshedVersion=\"4\" background=\"1\" refreshOnLoad=\"1\" saveData=\"1\"><webPr sourceData=\"1\" parsePre=\"1\" consecutive=\"1\" xl2000=\"1\" url=\"http://" + ServerHost + "/View/GetExcelWebService?ViewID=" + _View.ViewID + "&amp;ViewItemID=" + _ViewItem.ViewItemID + "&amp;SetID=" + SetID + "\"/></connection></connections>");
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
            foreach(A_DATE D in _Dates)
                Head.Append(cellStart + D.Date.ToString("d MMM yyyy") + cellEnd);

            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText = " SELECT DATES.DateID, LIST.BankID, Name = LIST.NameRus, Value = DATA." + mode.A_PARAM.Ticker
                            + " FROM BB_BANKS_LIST(NULL) LIST "
                            + " CROSS JOIN BB_DATES_LIST2({1}) DATES "
                            + " LEFT JOIN DK_F" + _View.Form + " DATA ON DATA.BankID = LIST.BankID AND DATA.DateID=DATES.DateID AND AggItemID = {0} "
                            + " ORDER BY BankID, DATES.DateID DESC";

            IEnumerable<Data2> resp = context.ExecuteQuery<Data2>(SQLText, _ViewItem.AggItemID, _View.Form);
            int BankID = 0;
            foreach (Data2 row in resp) {
                if (BankID != row.BankID) {
                    if (BankID != 0)
                        Body.Append(lineEnd);
                    Body.Append(lineStart);
                    BankID = row.BankID;

                    Body.Append(cellStart + row.BankID + cellEnd);
                    Body.Append(cellStart + row.Name + cellEnd);
                }
                Body.Append(cellStart);
                Body.Append(row.Value);
                Body.Append(cellEnd);
            }
            Body.Append(lineEnd);
            
            if (isCSV)
                return lineStart + Head.ToString() + lineEnd + Body.ToString();
            else
                return "<hmtl> <body><table>" + lineStart + Head.ToString() + lineEnd + Body.ToString() + "</table></body></html>";
        }

        public Result2 GetChartData(int BankID, int SetID) {
            string BankName;
            if (BankID < 0)
                BankName = context.A_AGGs.First(A => A.AggID == BankID).FullNameRUS;
            else
                BankName = context.A_BANKs.First(A => A.BankID == BankID).FullNameRUS;

            A_VIEWS_SET mode = _Sets.First(M => M.SetID == SetID);
            string SQLText =  " SELECT Date = DATES.Date, Value = DATA." + mode.A_PARAM.Ticker
                            + " FROM BB_DATES_LIST2({2}) DATES "
                            + " LEFT JOIN DK_F" + _View.Form + " DATA ON DATA.BankID = {1} AND DATA.DateID=DATES.DateID AND AggItemID = {0} "
                            + " ORDER BY DATES.DateID";

            return new Result2(context.ExecuteQuery<Data3>(SQLText, _ViewItem.AggItemID, BankID, _View.Form), BankName);
        }
    }
}


