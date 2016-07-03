using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcContrib.ActionResults;

namespace www.BankBals.Data {

    public class Export : System.Web.Mvc.Controller {
        public static char SEPARATOR = ';';
        private BankBalsDataContext context;

        public Export() {
            context = new BankBalsDataContext(Tools.ConnectionString());
        }

        public class QueryResult {
            public List<string> Header = new List<string>();
            public List<List<string>> Body = new List<List<string>>();

            public QueryResult() {
                Header = new List<string>();
                Body = new List<List<string>>();
            }

            public QueryResult(List<string> header, List<List<string>> body) {
                Header = header;
                Body = body;
            }

            public static QueryResult Query(BankBalsDataContext context, string SQLText) {
                List<string> _Head = new List<string>();
                List<List<string>> _Body = new List<List<string>>();

                using (IDbCommand command = context.Connection.CreateCommand()) {
                    command.CommandText = SQLText;
                    context.Connection.Open();
                    bool headSaved = false;
                    using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (reader.Read()) {
                            if (!headSaved) {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    _Head.Add(reader.GetName(i));
                                headSaved = true;
                            }
                            List<string> line = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++) {
                                line.Add(reader.GetValue(i).ToString());
                            }
                            _Body.Add(line);
                        }
                    }
                }
                return new QueryResult(_Head, _Body);
            }
        }

        public enum Format { 
            TEXT = 1,
            HTML = 2,
            HTMLTABLE = 3
        }

        public string AsText(string SQLText, Format format) {
            StringBuilder Body = new StringBuilder();
            StringBuilder Head = new StringBuilder();

            string newHCell = format == Format.TEXT ? String.Empty : "<th>";
            string endHCell = format == Format.TEXT ? SEPARATOR.ToString() : "</th>";
            string newCell = format == Format.TEXT ? String.Empty : "<td>";
            string endCell = format == Format.TEXT ? SEPARATOR.ToString() : "</td>";
            string newLine = format == Format.TEXT ? String.Empty : "<tr>";
            string endLine = format == Format.TEXT ? Environment.NewLine : "</tr>";

            using (IDbCommand command = context.Connection.CreateCommand()) {
                command.CommandText = SQLText;
                command.CommandTimeout = 0;
                context.Connection.Open();
                bool headSaved = false;
                try {
                    using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (reader.Read()) {
                            if (!headSaved) {
                                for (int i = 0; i < reader.FieldCount; i++) {
                                    Head.Append(newHCell);
                                    Head.Append(reader.GetName(i));
                                    Head.Append(endHCell);
                                }
                                headSaved = true;
                            }
                            Body.Append(newLine);
                            for (int i = 0; i < reader.FieldCount; i++) {
                                Body.Append(newCell);
                                Body.Append(reader.GetValue(i).ToString());
                                Body.Append(endCell);
                            }
                            Body.Append(endLine);
                        }
                    }
                } catch (Exception e) {
                    Head.Append(newHCell);
                    Head.Append("Error");
                    Head.Append(endHCell);
                    Body.Append(newCell);
                    Body.Append(e.Message);
                    Body.Append(endCell);
                }
            }

            switch (format) { 
                case Format.HTML:
                    return "<hmtl> <body><table><thead><tr>" + Head.ToString() + "</tr></thead><tbody>" + Body.ToString() + "</tbody></table></body></html>";
                    
                case Format.TEXT:
                    return Head.ToString() + endLine + Body.ToString();
                    
                case Format.HTMLTABLE:
                    return "<table><thead><tr>" + Head.ToString() + "</tr></thead><tbody>" + Body.ToString() + "</tbody></table>";
                    
                default:
                    return String.Empty;
            }
        }

        public ActionResult AsTEXT(string SQLText) {
			ContentResult Result = new ContentResult();
            Result.Content = AsText(SQLText, Format.TEXT);
			Result.ContentEncoding = Encoding.UTF8;
			Result.ContentType = "text/html";
			return Result;
		}

        public ActionResult AsHTML(string SQLText) {
            ContentResult Result = new ContentResult();
            Result.Content = AsText(SQLText, Format.HTML);
			Result.ContentEncoding = Encoding.UTF8;
			Result.ContentType = "text/html";
			return Result;
		}
        
        public ActionResult AsJSON(string SQLText) {
            return Json(QueryResult.Query(context, SQLText), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult AsXML(string SQLText) {
            return new XmlResult(QueryResult.Query(context, SQLText));
        }
        

        public string GetOneDateData(int DateID, int ViewID) {
            StringBuilder Body = new StringBuilder();
            StringBuilder Head = new StringBuilder();
            Head.Append("<th>Reg.Num.</th> <th>Name</th>");

            Repository R = new Repository();
            A_VIEWS_SET _Set = R.GetSets(ViewID).OrderBy(S => S.SetID).First();
            A_VIEW View = R.GetViews().First(V => V.ViewID == ViewID);
            List<A_VIEWITEMS_ALL> ItemsList = R.GetViewItems(ViewID).ToList();
            foreach (A_VIEWITEMS_ALL VI in ItemsList) {
                Head.Append("<th>" + VI.NameRus + "</th>");
            }

            string SQLText = "SELECT BankID, NameRUS, " + ItemsString(ItemsList, false) 
                + "FROM (" 
                + "    SELECT B.BankID, B.NameRus, V.ViewItemID, VALUE = CASE WHEN V.IsRatio=1 THEN D." + _Set.A_PARAM.Ticker + " / 10000. ELSE D." + _Set.A_PARAM.Ticker + " END " 
                + "    FROM BB_VIEWITEMS_LIST2(" + ViewID + ") V" 
                + "    INNER JOIN DK_F" + View.Form + " D ON V.AggItemID=D.AggItemID AND D.DateID=" + DateID  
                + "    INNER JOIN BB_BANKS_LIST(NULL) B ON B.BankID = D.BankID" 
                + ") A PIVOT (" 
                + "    SUM([VALUE]) FOR ViewItemID IN ( " + ItemsString(ItemsList, true) + " )" 
                + ") P";

            using (IDbCommand command = context.Connection.CreateCommand()) {
                command.CommandText = SQLText;
                context.Connection.Open();
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection)) {
                    while (reader.Read()) {
                        Body.Append("<tr>");
                        for (int i = 0; i <= reader.FieldCount - 1; i++) {
                            Body.Append("<td>" + reader[i].ToString() + "</td>");
                        }
                        Body.Append("</tr>");
                    }
                }
            }
            return "<hmtl> <body><table><tr>" + Head.ToString() + "</tr>" + Body.ToString() + "</table></body></html>";
        }
        
        private string ItemsString(List<A_VIEWITEMS_ALL> ItemsList, bool ShortString = false) {
            StringBuilder Result = new StringBuilder();
            foreach (A_VIEWITEMS_ALL VI in ItemsList) {
                if (ShortString) {
                    Result.Append(", [" + VI.ViewItemID + "]");
                } else {
                    if ((VI.IsRatio ?? false) == true) {
                        Result.Append(", [" + VI.ViewItemID + "] AS [" + VI.NameRus + "1]");
                    } else {
                      Result.Append(", [" + VI.ViewItemID + "] AS [" + VI.NameRus + "0]");
                    }
                }
            }
            return Result.ToString().Substring(2);
        }

		public class HelpDataRow {
			public int LevelID;
			public string Ticker;
			public string Name;
			public string Type;
		}

		public IEnumerable<HelpDataRow> GetHelpData(int Form, int AggItemID) {
            string SQLText = "SELECT LevelID, Ticker, Name, Type = APType FROM BB_HELP({0}, {1})";
            return context.ExecuteQuery<HelpDataRow>(SQLText, Form, AggItemID);
		}

	}

}