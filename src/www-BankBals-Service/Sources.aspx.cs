using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace www.BankBals.Service {
    public partial class Sources : System.Web.UI.Page {
        private SourcesData _data;
        public int FormID;
        public List<A_DATE> Dates;
        public List<SourcesData.Data> SourcesData;

        protected void Page_Load(object sender, EventArgs e) {
            this.FormID = int.Parse(Request.QueryString["FormID"]);
            this._data = new SourcesData(FormID);
            this.Dates = _data.Dates.ToList();
            this.SourcesData = _data.GetFormData().ToList();
        }
    }
}