using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Newtonsoft.Json;

namespace www.BankBals {

    public static class Cache {

        public const bool CACHE_ENABLED = false;

        public static string AssemblyDirectory {
            get {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string CacheDirectory {
            get {
                return Path.Combine(Cache.AssemblyDirectory, "Cache");
            }
        }

    }
}

    /*
        private static NLog.Logger logger = LogManager.GetLogger("CacheManager");
        private const int THREAD_TIMEOUT_MS = 60000;
        private const int CHECK_TIMEOUT_MS = 60000;
        private static string RootFolder = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, "Cache");
        //private static string ExportFolder = Path.Combine(HttpRuntime.AppDomainAppPath, "Content", "export");
        private static string KeyfileLocation = Path.Combine(RootFolder, ".cache.lock");
        private static string rebuildingFilePath = Path.Combine(RootFolder, ".rebuilding.lock");

        public class CachedJsonManager : System.Web.Mvc.Controller {

            private object _syncLock = new object();
            public OneBank.CompareData GetCompData(int BankID, int ViewID) {
                OneBank B = GlobalVar.CachedQuery.GetBanksList().GetBankByID(BankID);
                if (GlobalVar.USE_CASHEMANAGER) {
                    var filePath = GetCompDataJSONFileName(BankID, ViewID);
                    if (!System.IO.File.Exists(filePath)) {
                        System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(B.GetCompareData(ViewID), Formatting.None));
                    }
                    return JsonConvert.DeserializeObject<OneBank.CompareData>(System.IO.File.ReadAllText(filePath));
                } else {
                    return B.GetCompareData(ViewID);
                }
            }

            public ActionResult GetBankDataJSON(int BankID, int ViewID) {
                Models.BankData B = new Models.BankData(BankID, ViewID);
                if (GlobalVar.USE_CASHEMANAGER) {
                    var filePath = GetBankDataJSONFileName(BankID, ViewID);
                    if (!System.IO.File.Exists(filePath)) {
                        System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(B.GetData(), Formatting.None));
                    }
                    return Content(System.IO.File.ReadAllText(filePath), "application/json");
                } else {
                    return Content(JsonConvert.SerializeObject(B.GetData(), Formatting.None), "application/json");
                }
            }

            private string GetBankDataJSONFileName(int BankID, int ViewID) {
                var folderPath = Path.Combine(CacheManager.RootFolder, "Bank");
                if (!Directory.Exists(folderPath)) {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = ViewID + "_" + BankID + ".json";
                return Path.Combine(folderPath, filePath);
            }

            private string GetCompDataJSONFileName(int BankID, int ViewID) {
                var folderPath = Path.Combine(CacheManager.RootFolder, "Comp");
                if (!Directory.Exists(folderPath)) {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = ViewID + "_" + BankID + ".json";
                return Path.Combine(folderPath, filePath);
            }

            public ActionResult GetViewDataJSON(int ViewID, int ViewItemID) {
                Models.ViewsList VL = GlobalVar.CachedQuery.GetViewsList();
                Models.ViewData V = new Models.ViewData(VL.GetViewByID(ViewID), VL.GetViewByID(ViewID).GetViewItemByID(ViewItemID));
                if (GlobalVar.USE_CASHEMANAGER) {
                    var filePath = GetViewDataJSONFileName(ViewID, ViewItemID);
                    if (!System.IO.File.Exists(filePath)) {
                        System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(V.GetData(), Formatting.None));
                    }
                    return Content(System.IO.File.ReadAllText(filePath), "application/json");
                } else {
                    return Content(JsonConvert.SerializeObject(V.GetData(), Formatting.None), "application/json");
                }
            }

            private string GetViewDataJSONFileName(int ViewID, int ViewItemID) {
                var folderPath = Path.Combine(CacheManager.RootFolder, "View");
                if (!Directory.Exists(folderPath)) {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = ViewID + "_" + ViewItemID + ".json";
                return Path.Combine(folderPath, filePath);
            }

            public static void ClearCache() {
                string folderPath = null;
                folderPath = Path.Combine(CacheManager.RootFolder, "View");
                if (System.IO.Directory.Exists(folderPath)) {
                    foreach (string F in System.IO.Directory.GetFiles(folderPath)) {
                        System.IO.File.Delete(F);
                    }
                }

                folderPath = Path.Combine(CacheManager.RootFolder, "Bank");
                if (System.IO.Directory.Exists(folderPath)) {
                    foreach (string F in System.IO.Directory.GetFiles(folderPath)) {
                        System.IO.File.Delete(F);
                    }
                }

                folderPath = Path.Combine(CacheManager.RootFolder, "Comp");
                if (System.IO.Directory.Exists(folderPath)) {
                    foreach (string F in System.IO.Directory.GetFiles(folderPath)) {
                        System.IO.File.Delete(F);
                    }
                }

                folderPath = CacheManager.ExportFolder;
                if (System.IO.Directory.Exists(folderPath)) {
                    foreach (string F in System.IO.Directory.GetFiles(folderPath)) {
                        System.IO.File.Delete(F);
                    }
                }

                //If IO.File.Exists(KeyfileLocation) Then IO.File.Delete(KeyfileLocation)

            }

        }

        public class CacheUpdater {
            private string SQLText = "SELECT CHECKSUM_AGG(BINARY_CHECKSUM(*)) FROM [X_DATASTATE] WHERE Subscription != 0x0";

            private Thread Workerprocess;
            public CacheUpdater() {
                Workerprocess = new Thread(new ThreadStart(WorkBody));
                Workerprocess.Start();
                logger.Info("CacheUpdater Started");
            }

            private bool NeedUpdate() {
                bool result = false;
                www.BankBals.Classes.DB.DBRequest Request = new www.BankBals.Classes.DB.DBRequest(SQLText);
                www.BankBals.Classes.DB.DBResponse Response = (new DB()).DBQuery(Request);

                if (Response.Code == DB.ErrorCode.OK) {
                    if (Response.Result.Length > 0) {
                        if (Response.Result[0].ToString() != GetKey()) {
                            SetKey(Response.Result[0].ToString());
                            result = true;
                        }
                    }
                }
                return result;
            }

            private void WorkBody() {
                bool FirstTimeCheck = true;
                do {
                    bool lastRebuildNotFinished = false;
                    if (FirstTimeCheck) {
                        lastRebuildNotFinished = File.Exists(CacheManager.rebuildingFilePath);
                        FirstTimeCheck = false;
                    }
                    if (NeedUpdate() || lastRebuildNotFinished) {
                        logger.Info("CacheUpdater: Need to update Cache!");
                        GlobalVar.CM.RebuildCache();
                    }
                    Thread.Sleep(CacheManager.CHECK_TIMEOUT_MS);
                } while (true);
            }

            private string GetKey() {
                string functionReturnValue = null;
                if (System.IO.File.Exists(CacheManager.KeyfileLocation)) {
                    functionReturnValue = System.IO.File.ReadAllText(CacheManager.KeyfileLocation);
                } else {
                    functionReturnValue = string.Empty;
                }
                return functionReturnValue;
            }

            private void SetKey(string value) {
                System.IO.File.WriteAllText(CacheManager.KeyfileLocation, value);
            }
        }

        public class CacheRefresher {

            private Thread Workerprocess;
            private bool StopProcess = false;
            BanksList BL = GlobalVar.CachedQuery.GetBanksList();
            ViewsList VL = GlobalVar.CachedQuery.GetViewsList();
            public struct StateRow {
                public char[] RowValues;
                public string AsString() {
                    return new string(RowValues);
                }
            }
            private StateRow[] StateB;

            private StateRow[] StateV;
            // === P U B L I C ===================
            public CacheRefresher() {
                logger.Info("CacheRefresher started");
                InitState();
            }

            public void StopCache() {
                logger.Info("CacheRefresher StopCache");
                StopWork();
            }

            public void ClearCache() {
                logger.Info("CacheRefresher ClearCache");
                StopWork();
                CachedJsonManager.ClearCache();
                InitState();
            }

            public void RebuildCache() {
                logger.Info("CacheRefresher RebuildCache");
                StopWork();
                CachedJsonManager.ClearCache();
                InitState();
                StartWork();
            }

            protected override void Finalize() {
                StopWork();
                logger.Info("CacheRefresher Finalizing");
#if DEBUG
                ClearCache();
#endif
            }

            public string[] GetCurrnetState() {
                string[] result = new string[3];
                foreach (CacheManager.CacheRefresher.StateRow StateRow_loopVariable in StateB) {
                    StateRow = StateRow_loopVariable;
                    result[0] += StateRow.AsString() + System.Environment.NewLine;
                }
                foreach (CacheManager.CacheRefresher.StateRow StateRow_loopVariable in StateV) {
                    StateRow = StateRow_loopVariable;
                    result[1] += StateRow.AsString() + System.Environment.NewLine;
                }
                return result;
            }

            // === P R I V A T E ===================
            private void InitState() {
                StateB = new StateRow[VL.ViewsList.Length + 1];
                for (int j = 0; j <= VL.ViewsList.Length - 1; j++) {
                    StateB[j].RowValues = new char[BL.BanksList.Length + 1];
                    for (int i = 0; i <= BL.BanksList.Length - 1; i++) {
                        StateB[j].RowValues[i] = '.';
                    }
                }

                StateV = new StateRow[VL.ViewsList.Length + 1];
                for (int j = 0; j <= VL.ViewsList.Length - 1; j++) {
                    StateV[j].RowValues = new char[VL.ViewsList[j].ViewItems.Length + 1];
                    for (int i = 0; i <= VL.ViewsList[j].ViewItems.Length - 1; i++) {
                        StateV[j].RowValues[i] = '.';
                    }
                }
                StopProcess = false;
            }

            private void WorkBody() {
                File.WriteAllText(CacheManager.rebuildingFilePath, string.Empty);

                int vi = 0;
                foreach (OneView V in VL.ViewsList) {
                    int bi = 0;
                    foreach (OneBank B in BL.BanksList) {
                        GlobalVar.CachedJson.GetBankDataJSON(B.BankID, V.ViewID);
                        logger.Info("Cached: GetBankDataJSON({0}, {1})", B.BankID, V.ViewID);
                        StateB[vi].RowValues[bi] = "*";
                        bi += 1;
                        if (StopProcess) {
                            return;
                        }
                    }
                    vi += 1;
                }

                int vj = 0;
                foreach (OneView V in VL.ViewsList) {
                    int vij = 0;
                    foreach (ViewItem VItm in V.ViewItems) {
                        if (VItm.AggItemID != 0) {
                            GlobalVar.CachedJson.GetViewDataJSON(V.ViewID, VItm.ViewItemID);
                            logger.Info("Cached: GetViewDataJSON({0}, {1})", V.ViewID, VItm.ViewItemID);
                        }
                        StateV[vj].RowValues[vij] = "*";
                        vij += 1;
                        if (StopProcess)
                            return;
                    }
                    vj += 1;
                }

                File.Delete(CacheManager.rebuildingFilePath);
            }

            private void StartWork() {
                Workerprocess = new Thread(new ThreadStart(WorkBody));
                Workerprocess.Start();
                logger.Info("CacheRefresher started Worker Thread");
            }

            private void StopWork() {
                logger.Info("CacheRefresher StopWork");
                if ((Workerprocess != null)) {
                    if (Workerprocess.IsAlive) {
                        StopProcess = true;
                        var endtime = DateTime.Now.AddMilliseconds(CacheManager.THREAD_TIMEOUT_MS);
                        while (true) {
                            if (DateTime.Now > endtime | !Workerprocess.IsAlive) {
                                break; // TODO: might not be correct. Was : Exit While
                            }
                        }
                    }
                }
            }

        }
    */