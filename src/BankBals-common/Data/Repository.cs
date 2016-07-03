using System.Linq;

namespace www.BankBals.Data {
    
    public class Repository {

        private BankBalsDataContext context = new BankBalsDataContext(Tools.ConnectionString());

        public IQueryable<A_VIEW> GetViews() {
            return context.A_VIEWs;
        }

        public IQueryable<A_VIEWITEMS_ALL> GetViewItems() {
            return context.A_VIEWITEMS_ALLs;
        }

        public IQueryable<A_VIEWITEMS_ALL> GetViewItems(int ViewID) {
            return context.A_VIEWITEMS_ALLs.Where(A => A.ViewID == ViewID);
        }

        public IQueryable<A_VIEWS_BANKSPIC> GetViewsCompare() {
            return context.A_VIEWS_BANKSPICs.Where(V => !V.IsHidden);
        }

        public IQueryable<A_VIEWITEMS_COMPARE> GetViewItemsCompare() {
            return context.A_VIEWITEMS_COMPAREs;
        }

        public IQueryable<A_VIEWITEMS_COMPARE> GetViewItemsCompare(int ViewID) {
            return context.A_VIEWITEMS_COMPAREs.Where(A => A.ViewID == ViewID);
        }

        public IQueryable GetViewItemsAjax() {
            IQueryable<A_VIEWITEMS_ALL> ViewItems = context.A_VIEWITEMS_ALLs;

            return context.A_VIEWs
                .Where(V => !V.IsHidden)
                .Select(V => new {
                    ViewID = V.ViewID,
                    ViewName = V.ViewNameRus,
                    Form = V.Form,
                    ViewItems = ViewItems.Where(VI => VI.ViewID == V.ViewID).Select(VI => new { 
                        ViewItemID = VI.ViewItemID, 
                        ViewItemName = VI.NameRus,
                        IsHeader = !VI.AggItemID.HasValue, 
                        AggItemID = VI.AggItemID
                    })
                });
        }

        public IQueryable GetBanksItemsAjax(bool LoadComparables) { 
            if (LoadComparables)
                return context.A_BANKS_ALLs.Select(B => new {
                    BankID = B.BankID, 
                    BankName =  B.NameRus,
                    BankIDC1 = B.BankIDC1,
                    BankIDC2 = B.BankIDC2
                }); 
            else
                return context.A_BANKS_ALLs.Select(B => new {
                    BankID = B.BankID, 
                    BankName =  B.NameRus
                }); 
        }

        public IQueryable<A_BANKS_ALL> GetBanks() {
            return context.A_BANKS_ALLs.OrderBy(B => B.NameRus);
        }

        public IQueryable<A_AGG> GetAggsOnly() {
            return context.A_AGGs;
        }

        public IQueryable<A_BANK> GetBanksOnly() {
            return context.A_BANKs;
        }

        public IQueryable<A_AGGITEM> GetAggItems() {
            return context.A_AGGITEMs;
        }

        public A_AGGITEM GetAggItems(string Ticker) {
            return context.A_AGGITEMs.First(A => A.Ticker == Ticker);
        }

        public IQueryable<A_DATE> GetDates() {
            return context.A_DATEs;
        }

        public A_DATE GetLastDate(bool IsQuartal = false) {
            return context.A_DATEs.Where(D => D.IsQuartal == IsQuartal ? true : D.IsQuartal).OrderByDescending(D => D.DateID).First();
        }
 
        public IQueryable<A_2X2CHARTS_VIEW> GetChartsList() {
            return context.A_2X2CHARTS_VIEWs;
        }

        public IQueryable<W_AGG_COMP> GetAggMembers(int AggID) {
            return context.W_AGG_COMPs.Where(W => W.AggID == AggID);
        }

        public IQueryable<A_VIEWS_SET> GetSets() {
            return context.A_VIEWS_SETs;
        }

        public IQueryable<A_VIEWS_SET> GetSets(int ViewID) {
            return context.A_VIEWS_SETs.Where(S => S.ViewID == ViewID);
        }
    }
}
