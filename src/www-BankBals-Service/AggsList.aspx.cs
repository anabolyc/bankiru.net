using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace www.BankBals.Service {
    public partial class AggsList : System.Web.UI.Page {
        private BankBalsDataContext _db = new BankBalsDataContext();
        public A_AGG Aggregate;
        public List<W_AGG_COMP> Members;

        protected void Page_Load(object sender, EventArgs e) {
            int AggID = int.Parse(Request.QueryString["AggID"]);
            this.Aggregate = _db.A_AGGs.FirstOrDefault(A => A.AggID == AggID);
            this.Members = _db.W_AGG_COMPs.Where(W => W.AggID == AggID).ToList();
        }
    }
}