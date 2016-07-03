using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace www.BankBals.Data {

    public class SourcesData {

        public IQueryable<A_DATE> Dates { get { return _Dates; } }

        private BankBalsDataContext context;
        private IQueryable<A_DATE> _Dates;
        private int _Form;

        public SourcesData(int Form) {
            context = new BankBalsDataContext(Tools.ConnectionString());
            _Form = Form;
            switch (Form) {
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

        public class Data {
            public int BankID;
            public string NameRus;
            public DateTime Date;
            public int? Src;
        }

        public IEnumerable<Data> GetFormData() {
            string SQLText  = " SELECT LIST.BankID, NameRus = LIST.FullNameRus, DATES.Date, Src = CAST(A.Src AS INT) "
                            + ((_Form == 101 || _Form == 102) ? " + (CASE WHEN ISWithTO=1 THEN 10 ELSE 0 END) " : "")
                            + " FROM A_BANKS LIST "
                            + " CROSS JOIN BB_DATES_LIST2({0}) DATES "
                            + " LEFT JOIN DC_BALS_F" + _Form + "_SRC A ON A.BankID = LIST.BankID AND A.Date = DATES.Date "
                            + " INNER JOIN DK_TOPBANKS DK ON DK.BankID = LIST.BankID AND DK.DateID = ( SELECT MAX(DateID) FROM DK_TOPBANKS DK0 WHERE DK.BankID = DK0.BankID) "
                            + " ORDER BY DK.TotalAssets DESC, LIST.BankID, DATES.DateID DESC ";
            return context.ExecuteQuery<Data>(SQLText, _Form);
        }

    }
}
