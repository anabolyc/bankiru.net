using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using BankBals_common.Classes;

namespace www.BankBals.Updater {
    class Program {
        // проверить коннект к дб в самом начале.
        // проверить что есть драйвер для dbf
        private const string HOST = "http://cbr.ru";
        private const string SQL_DATE_FORMAT = "yyyy-MM-dd";
        private static DateTime DEFAULT_DATE = new DateTime(1970, 1, 1);
        //private const string JetDbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=dBase IV;Data Source=";
        private const string JetDbConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=dBase IV;Data Source=";

        public static Logger Log = new Logger();
        public static System.Data.SqlClient.SqlConnectionStringBuilder ConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder();
        public static ProgramOptions options;

        public struct ProgramOptions {
            public bool FullLoad;
            public bool ForceLoad;
            public string uploadfile;
        }

        static int Main(string[] args) {
            bool runable = false;

            ParseCmdLineParams(args, out runable);
            if (runable) {
                if (options.uploadfile != null) {
                    getContentManual(options.uploadfile);
                } else {
                    getContent(HOST + "/credit/forms.asp");
                }
                return Log.GetResult();
            } else {
                Log.Write("Nothing to do! Use --help to see keys", "Main");
                return 0;
            }

        }

        private static void PrintHelp() {
            string filepath = Common.GetAppFolder() + "\\help.txt";
            if (File.Exists(filepath)) {
                string[] helps = File.ReadAllLines(filepath);
                foreach (string s in helps)
                    Console.WriteLine(s);
            } else {
                Console.WriteLine("Help file is missing: './help.txt'");
            }
        }

        private static void ParseCmdLineParams(string[] args, out bool runable) {
            runable = false;
            int i = 0;

            ConnectionString.DataSource = "(local)";
            ConnectionString.IntegratedSecurity = true;
            ConnectionString.InitialCatalog = "BankBals";

            foreach (string arg in args) {
                if (arg == "--help") {
                    PrintHelp();
                    runable = false;
                    return;
                }
                //SQL Server
                if ((arg == "--server") && (i < args.Length - 1))
                    ConnectionString.DataSource = args[i + 1];
                if ((arg == "--username") && (i < args.Length - 1))
                    ConnectionString.UserID = args[i + 1];
                if ((arg == "--password") && (i < args.Length - 1))
                    ConnectionString.Password = args[i + 1];
                if ((arg == "--database") && (i < args.Length - 1))
                    ConnectionString.InitialCatalog = args[i + 1];
                
                //options
                if ((arg == "--upload-file") && (i < args.Length)) {
                    options.uploadfile = args[i + 1];
                    runable = true;
                }
                if (arg == "--auto") {
                    runable = true;
                }
                if (arg == "--full-load") {
                    options.FullLoad = true;
                    runable = true;
                }
                if (arg == "--force-load") {
                    options.ForceLoad = true;
                    runable = true;
                }
                i++;
            }

            Log.Write("Using server: " + ConnectionString.DataSource, "ParseCmdLineParams");
            if ((ConnectionString.UserID != String.Empty) && (ConnectionString.Password != String.Empty)) {
                ConnectionString.IntegratedSecurity = false;
                Log.Write("IntegratedSecurity set to False, UserID = " + ConnectionString.UserID + ", password = ******", "ParseCmdLineParams");
            } else {
                ConnectionString.IntegratedSecurity = true;
                Log.Write("IntegratedSecurity set to True", "ParseCmdLineParams");
            }

        }

        private static void getContentManual(string ArchiveName) {
            string s = String.Empty;
            bool dbupdated = false;

            if (File.Exists(ArchiveName)) {
                Packer packer = ArchiveName.ToLower().Contains("rar") ? Packer.RAR : Packer.ZIP;
                if (ArchiveName.Like("%101-%.___%"))
                    GetFormManual(101, ArchiveName, out dbupdated, packer);
                if (ArchiveName.Like("%102-%.___%"))
                    GetFormManual(102, ArchiveName, out dbupdated, packer);
                if (ArchiveName.Like("%134-%.___%"))
                    GetFormManual(134, ArchiveName, out dbupdated, packer);
                if (ArchiveName.Like("%135-%.___%"))
                    GetFormManual(135, ArchiveName, out dbupdated, packer);
            } else {
                Log.Write("Не найден файл!", "getContentManual", true);
            }

        }

        private static void getContent(string link) {
            string s = String.Empty;
            bool F101Loaded = false;
            bool F102Loaded = false;
            bool F134Loaded = false;
            bool F135Loaded = false;

            bool dbupdated = false;

            if (GetFromUrl.DownLoadFile(link, out s) == Result.RESULT_OK) {
                string[] myDelims = new string[] { "</option>" };
                string[] lines = s.ToLower().Split(myDelims, StringSplitOptions.None);
                

                if (options.FullLoad) {
                    //lines.Reverse(); @@
                    //foreach (string line in lines) {
                    for (var i = 0; i < lines.Length - 1; i++) {
                        string l = lines[i].ReplaceAll("\"'").Split(new string[] { "<option" }, StringSplitOptions.None)[1];
                        Packer packer = l.ToLower().Contains("rar") ? Packer.RAR : Packer.ZIP;
                        if (l.Like("%101-%.___%"))
                            GetForm(101, l, out dbupdated, packer);
                        if (l.Like("%102-%.___%"))
                            GetForm(102, l, out dbupdated, packer);
                        if (l.Like("%134-%.___%"))
                            GetForm(134, l, out dbupdated, packer);
                        if (l.Like("%135-%.___%"))
                            GetForm(135, l, out dbupdated, packer);
                    }
                } else {
                    for (var i = 0; i < lines.Length - 1; i++) {
                        string l = lines[i].ReplaceAll("\"'").Split(new string[] { "<option" }, StringSplitOptions.None)[1];
                        Packer packer = l.ToLower().Contains("rar") ? Packer.RAR : Packer.ZIP;
                        if (l.Like("%101-%.___%"))
                            if (!F101Loaded) {
                                GetForm(101, l, out dbupdated, packer);
                                F101Loaded = true;
                            }
                        
                        if (l.Like("%102-%.___%"))
                            if (!F102Loaded) {
                                GetForm(102, l, out dbupdated, packer);
                                F102Loaded = true;
                            }
                        
                        if (l.Like("%134-%.___%"))
                            if (!F101Loaded) {
                                GetForm(134, l, out dbupdated, packer);
                                F134Loaded = true;
                            }
                        
                        if (l.Like("%135-%.___%")) 
                            if (!F135Loaded) {
                                GetForm(135, l, out dbupdated, packer);
                                F135Loaded = true;
                            }
                    }
                }
            } else {
                Log.Write("Не удалось скачать файл!", "getContent", true);
            }

        }

        private static void GetFormManual(int FormID, string ArchiveName, out bool DbUpdated, Packer packer) {
            bool DataLoaded = false;

            string AFileName = Path.GetFileName(ArchiveName).Split('.')[0];
            DateTime StartDate = new DateTime(int.Parse(AFileName.Substring(4, 4)), int.Parse(AFileName.Substring(8, 2)), 1);

            string DirectoryPath = Common.GetTmpPath() + "\\cbrbanks\\";
            if (Directory.Exists(DirectoryPath)) {
                Directory.Delete(DirectoryPath, true);
            }
            Directory.CreateDirectory(DirectoryPath);

            if (Common.Unpack(ArchiveName, DirectoryPath, packer) != Result.RESULT_OK) {
                Log.Write("Failed to unpack file: " + ArchiveName, "GetForm", true);
                DbUpdated = false;
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(DirectoryPath);
            FileInfo[] filesList = dir.GetFiles("*.dbf");
            foreach (FileInfo file in filesList) {
                string fileName = file.Name.ToUpper();
                if (FormID == 101) {
                    if (fileName.Like("_______B.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, NULL AS VR, NULL AS VV, NULL AS VITG, NULL AS ORA, NULL AS OVA, NULL AS OITGA, NULL AS ORP, NULL AS OVP, NULL AS OITGP, NULL AS IR, NULL AS IV, ITOGO AS IITG", out DataLoaded, "WHERE (ITOGO " + Common.BetweenString() + ")");
                    } else if (fileName.Like("______B1.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, VR, VV, VITG, ORA, OVA, OITGA, ORP, OVP, OITGP, IR, IV, IITG", out DataLoaded, "WHERE (IITG " + Common.BetweenString() + ") AND (OITGA " + Common.BetweenString() + ") AND (OITGP " + Common.BetweenString() + ") AND (VITG " + Common.BetweenString() + ")");
                    } else if (fileName.Like("_______N.DBF")) {
                        //Banks List = DO NOTHING 
                    } else if (fileName.Like("______N1.DBF")) {
                        //Banks List = DO NOTHING 
                    } else if (fileName.Like("_______S.DBF")) {
                        //Control Sum = DO NOTHING 
                    } else if (fileName.Like("______S1.DBF")) {
                        //Control Sum = DO NOTHING 
                    } else if (fileName.Like("NAMES.DBF")) {
                        //Names = DO NOTHING 
                    } else {
                        Log.Write("Unknown file: " + fileName, "UploadFile", true);
                    }
                } else if (FormID == 102) {
                    if (fileName.Like("SPRAV1.DBF")) {
                        //Names = DO NOTHING 
                    } else if (fileName.Like("______P.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, NULL AS R, NULL AS V, ITOGO AS ITOGO", StartDate, out DataLoaded);
                    } else if (fileName.Right(7).ToUpper() == "_P1.DBF" ) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, SIM_R AS R, SIM_V AS V, SIM_ITOGO AS ITOGO", StartDate, out DataLoaded);
                    } else if (fileName.Like("______NP.DBF")) {
                        //Banks List = DO NOTHING 
                    } else if (fileName.Like("_____NP1.DBF")) {
                        //Banks List = DO NOTHING 
                    } else if (fileName.Like("_____SP1.DBF")) {
                        //Control Sum = DO NOTHING 
                    } else if (fileName.Like("______SP.DBF")) {
                        //Control Sum = DO NOTHING 
                    } else {
                        Log.Write("Unknown file: " + fileName, "UploadFile", true);
                    }
                } else if (FormID == 134) {
                    if (fileName.Like("_______134D.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F134_DATA_TEMP", "REGN AS BankID, C1 AS [ID], C3 AS [Value]", StartDate, out DataLoaded);
                    } else if (fileName.Like("_______134N.DBF")) {
                        //Names = DO NOTHING 
                    } else if (fileName.Like("_______134B.DBF")) {
                        //Banks List = DO NOTHING 
                    } else {
                        Log.Write("Unknown file: " + fileName, "UploadFile", true);
                    }
                } else if (FormID == 135) {
                    if (fileName.Like("_______135_1.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_ACC", "REGN AS BankID, C1_1 AS [ID], C2_1 AS [Value]", StartDate, out DataLoaded);
                    } else if (fileName.Like("_______135_2.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_FIG", "REGN AS BankID, C1_2 AS [ID], C2_2 AS [Value]", StartDate, out DataLoaded);
                    } else if (fileName.Like("_______135_3.DBF")) {
                        if (StartDate <= new DateTime(2011, 1, 1)) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_3 AS [RowNum], C2_3 AS [ID], C3_3 AS [Xnum], C4_3 AS [XDate]", StartDate, out DataLoaded);
                        } else {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_NORM", "REGN AS BankID, C1_3 AS [ID], C2_3 AS [Value]", StartDate, out DataLoaded);
                        }
                    } else if (fileName.Like("_______135_4.DBF")) {
                        if (StartDate <= new DateTime(2011, 1, 1)) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_4 AS [Holiday]", StartDate, out  DataLoaded);
                        } else {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_4 AS [RowNum], C2_4 AS [ID], C3_4 AS [Xnum], C4_4 AS [XDate]", StartDate, out DataLoaded);
                        }
                    } else if (fileName.Like("_______135_5.DBF")) {
                        ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_5 AS [Holiday]", StartDate, out DataLoaded);
                    } else if (fileName.Like("_______135B.DBF")) {
                        //Banks List = DO NOTHING 
                    } else {
                        Log.Write("Unknown file: " + fileName, "UploadFile", true);
                    }
                } else {
                    Log.Write("Unknown form!!", fileName, true);
                }
            }
            try {
                Directory.Delete(DirectoryPath, true);
            }
            catch { }
            DbUpdated = DataLoaded;
        }

        private static void GetForm(int FormID, string SourceString, out bool DbUpdated, Packer packer) {
            string link = String.Empty;
            DateTime StartDate = DEFAULT_DATE;
            bool DataLoaded = false;
            DateTime NewAgeDate = DEFAULT_DATE;
            DBClass DBC = new DBClass(Log, ConnectionString.ConnectionString);
            DBClass.DBResponse Response = DBC.DBQuery(Log, new DBClass.DBRequest("SELECT NewAgeDate FROM NEWAGE_DATE"));
            DbUpdated = false;

            if (Response.Code == 0)
                NewAgeDate = DateTime.Parse(Response.Result[0].Split(DBClass.SEPARATOR.ToCharArray())[1]);

            if (SourceString.Like("%value=%__.__.____")) {
                link = (SourceString.Split(new string[] {"value="}, StringSplitOptions.None)[1]).Split('>')[0];
                string datestring = SourceString.Right(10);
                StartDate = new DateTime(int.Parse(datestring.Right(4)), int.Parse(datestring.Substring(3, 2)), 1);
            } else {
                Log.Write("Unexpected string format: " + SourceString, "GetForm", true);
            }

            if (NewAgeDate > StartDate) {
                return;
            }

            string DirectoryPath = Common.GetTmpPath() + "\\cbrbanks\\";
            if (Directory.Exists(DirectoryPath)) {
                Directory.Delete(DirectoryPath, true);
            }
            Directory.CreateDirectory(DirectoryPath);
            link = HOST + link;

            string ArchiveName = link.Split('/')[link.Split('/').Length - 1];
            if (GetFromUrl.DownLoadFile(link, DirectoryPath + ArchiveName) == Result.RESULT_FAIL) {
                Log.Write("Не удалось скачать архив с отчетностями", "GetForm", true);
                return;
            } else {
                if (Common.Unpack(DirectoryPath + ArchiveName, DirectoryPath, packer) != Result.RESULT_OK) {
                    Log.Write("Failed to unpack file: " + ArchiveName, "GetForm", true);
                    return;
                }

                DirectoryInfo dir = new DirectoryInfo(DirectoryPath);
                FileInfo[] filesList = dir.GetFiles("*.dbf");
                foreach (FileInfo file in filesList) {
                    string fileName = file.Name.ToUpper();
                    if (FormID == 101) {
                        if (fileName.Like("_______B.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, NULL AS VR, NULL AS VV, NULL AS VITG, NULL AS ORA, NULL AS OVA, NULL AS OITGA, NULL AS ORP, NULL AS OVP, NULL AS OITGP, NULL AS IR, NULL AS IV, ITOGO AS IITG", out DataLoaded, "WHERE (ITOGO " + Common.BetweenString() + ")");
                        } else if (fileName.Like("______B1.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F101_DATA_TEMP", "DT, REGN, NUM_SC, VR, VV, VITG, ORA, OVA, OITGA, ORP, OVP, OITGP, IR, IV, IITG", out DataLoaded, "WHERE (IITG " + Common.BetweenString() + ") AND (OITGA " + Common.BetweenString() + ") AND (OITGP " + Common.BetweenString() + ") AND (VITG " + Common.BetweenString() + ")");
                        } else if (fileName.Like("_______N.DBF")) {
                            //Banks List = DO NOTHING
                        } else if (fileName.Like("______N1.DBF")) {
                            //Banks List = DO NOTHING
                        } else if (fileName.Like("_______S.DBF")) {
                            //Control Sum = DO NOTHING
                        } else if (fileName.Like("______S1.DBF")) {
                            //Control Sum = DO NOTHING
                        } else if (fileName.Like("NAMES.DBF")) {
                            //Names = DO NOTHING
                        } else {
                            Log.Write("Unknown file: " + fileName, "UploadFile", true);
                        }
                    } else if (FormID == 102) {
                        if (fileName.Like("SPRAV1.DBF")) {
                            //Names = DO NOTHING
                        } else if (fileName.Like("______P.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, NULL AS R, NULL AS V, ITOGO AS ITOGO", StartDate, out DataLoaded);
                        } else if (fileName.Like("______P1.DBF") && !fileName.Like("_____NP1.DBF") && !fileName.Like("_____SP1.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F102_DATA_TEMP", "REGN AS BankID, CODE AS ID, SIM_R AS R, SIM_V AS V, SIM_ITOGO AS ITOGO", StartDate, out DataLoaded);
                        } else if (fileName.Like("______NP.DBF")) {
                            //Banks List = DO NOTHING
                        } else if (fileName.Like("_____NP1.DBF")) {
                            //Banks List = DO NOTHING
                        } else if (fileName.Like("_____SP1.DBF")) {
                            //Control Sum = DO NOTHING
                        } else if (fileName.Like("______SP.DBF")) {
                            //Control Sum = DO NOTHING
                        } else {
                            Log.Write("Unknown file: " + fileName, "UploadFile", true);
                        }
                    } else if (FormID == 134) {
                        if (fileName.Like("_______134D.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F134_DATA_TEMP", "REGN AS BankID, C1 AS [ID], C3 AS [Value]", StartDate, out DataLoaded);
                        } else if (fileName.Like("_______134N.DBF")) {
                            //Names = DO NOTHING
                        } else if (fileName.Like("_______134B.DBF")) {
                            //Banks List = DO NOTHING
                        } else {
                            Log.Write("Unknown file: " + fileName, "UploadFile", true);
                        }
                    } else if (FormID == 135) {
                        if (fileName.Like("_______135_1.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_ACC", "REGN AS BankID, C1_1 AS [ID], C2_1 AS [Value]", StartDate, out DataLoaded);
                        } else if (fileName.Like("_______135_2.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_FIG", "REGN AS BankID, C1_2 AS [ID], C2_2 AS [Value]", StartDate, out  DataLoaded);
                        } else if (fileName.Like("_______135_3.DBF")) {
                            if (StartDate <= new DateTime(2011, 1, 1)) {
                                ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_3 AS [RowNum], C2_3 AS [ID], C3_3 AS [Xnum], C4_3 AS [XDate]", StartDate, out DataLoaded);
                            } else {
                                ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_NORM", "REGN AS BankID, C1_3 AS [ID], C2_3 AS [Value]", StartDate, out DataLoaded);
                            }
                        } else if (fileName.Like("_______135_4.DBF")) {
                            if (StartDate <= new DateTime(2011, 1, 1)) {
                                ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_4 AS [Holiday]", StartDate, out DataLoaded);
                            } else {
                                ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_FOUL", "REGN AS BankID, C1_4 AS [RowNum], C2_4 AS [ID], C3_4 AS [Xnum], C4_4 AS [XDate]", StartDate, out DataLoaded);
                            }
                        } else if (fileName.Like("_______135_5.DBF")) {
                            ImportDBFtoSQLServer(ArchiveName, DirectoryPath, file.Name, "DI_CBR_BALS_F135_DATA_CAL", "REGN AS BankID, C1_5 AS [Holiday]", StartDate, out DataLoaded);
                        } else if (fileName.Like("_______135B.DBF")) {
                            //Banks List = DO NOTHING
                        } else {
                            Log.Write("Unknown file: " + fileName, "UploadFile", true);
                        }
                    } else {
                        Log.Write("Unknown form!!", fileName, true);
                    }
                }
                /* @@
                if(DataLoaded) {
                    DBQuery("EXEC " + SQL_DB_DEV + "MAKE_SOURCELIST_F" + FormID);
                    DBQuery("EXEC " + SQL_DB_WEB + "CALC_DK_F" + FormID + "_DATE '" + Format(StartDate, SQL_DATE_FORMAT) + "'");
                    DBQuery("EXEC " + SQL_DB_DEV + "FILL_A_DATE");
                    DBQuery("EXEC " + SQL_DB_WEB + "CALC_DK_F" + FormID + "_PIVOT_DATE '" + Format(StartDate, SQL_DATE_FORMAT) + "'");
                    DBQuery("EXEC " + SQL_DB_WEB + "CALC_DK_AGG_ALL");
                    DbUpdated = true;
                }
                 * */
            }

            Directory.Delete(DirectoryPath, true);
        }

        private static bool ShouldLoad(string ArchiveName, string FileName, int RowsCount) {
            DBClass DBC = new DBClass(Log, ConnectionString.ConnectionString);
            DBClass.DBResponse Response = DBC.DBQuery(Log, new DBClass.DBRequest("SELECT 1 FROM W_LOAD_FILES WHERE ArchiveName='" + ArchiveName + "' AND FileName='" + FileName + "' AND RowsCount=" + RowsCount));

            //@@
            if (Response.Code == 0)
                return Response.Result.Length == 0;
            else
                return true;
        }

        private static string DBFConnectionString(string FolderPath) {
            return JetDbConnectionString + FolderPath;
        } 

        private static DataTable GetDBFTable(string FolderPath, string FilePath, string ColumnsList, string WhereClause, out bool success) {
            DataTable dt = new DataTable();
            Log.Write("Opening DBF-database: " + FilePath, "GetDBFTable");

            OleDbConnection dBaseConnection = new OleDbConnection(DBFConnectionString(FolderPath));
            Log.Write("Connecting: " + FilePath, "GetDBFTable");
            try {
                success = true;
                dBaseConnection.Open();
                Log.Write("Connected!", "GetDBFTable");
                string TableName = FilePath.Split('.')[0];
                string FilePath0 = String.Empty;
                if (TableName.Length > 8) {
                    FilePath0 = FilePath.Right(8 + ".dbf".Length);
                    File.Copy(FolderPath + FilePath, FolderPath + FilePath0);
                    TableName = TableName.Right(8);
                }

                Log.Write("SELECTING data: " + FilePath, "GetDBFTable");
                OleDbCommand command = new OleDbCommand("SELECT " + ColumnsList + " FROM [" + TableName + "] " + WhereClause, dBaseConnection);

                try {
                    dt.Load(command.ExecuteReader());
                }
                catch (Exception exc) {
                    success = false;
                    Log.Write("Failed to load data from file: " + FilePath + ": " + exc.Message, "GetDBFTable", true);
                }

                dBaseConnection.Close();
                if (FilePath0 != string.Empty) {
                    File.Delete(FolderPath + FilePath0);
                }
                return dt;
            }
            catch (Exception e) {
                success = false;
                Log.Write(e.Message, "GetDBFTable", false, true);
            }
            return dt;
        }

        private static void ImportDBFtoSQLServer(string ArchiveName, string FolderPath, string FilePath, string DestinationTable, string ColumnsList, DateTime AssignDate, out bool DbUpdated) {
            ImportDBFtoSQLServer(ArchiveName, FolderPath, FilePath, DestinationTable, ColumnsList, AssignDate, out DbUpdated, String.Empty);
        }

        private static void ImportDBFtoSQLServer(string ArchiveName, string FolderPath, string FilePath, string DestinationTable, string ColumnsList, out bool DbUpdated, string WhereClause) {
            ImportDBFtoSQLServer(ArchiveName, FolderPath, FilePath, DestinationTable, ColumnsList, DEFAULT_DATE, out DbUpdated, WhereClause);
        }

        private static void ImportDBFtoSQLServer(string ArchiveName, string FolderPath, string FilePath, string DestinationTable, string ColumnsList, DateTime AssignDate, out bool DbUpdated, string WhereClause) {
            DataTable DT = new DataTable();
            DBClass DBC = new DBClass(Log, ConnectionString.ConnectionString);
            DbUpdated = false;
            bool success;
            DT = GetDBFTable(FolderPath, FilePath, ColumnsList, WhereClause, out success);
            if (success) {
                if (AssignDate != DEFAULT_DATE) {
                    Type DateType = Type.GetType("System.DateTime");
                    DataColumn DataCol = DT.Columns.Add("Date", DateType, "'" + AssignDate.ToString(SQL_DATE_FORMAT) + "'");
                    DataCol.SetOrdinal(0);
                }
                if (options.ForceLoad || ShouldLoad(ArchiveName, FilePath, DT.Rows.Count)) {
                    DbUpdated = true;
                    SqlConnection myConnection = new SqlConnection(ConnectionString.ConnectionString);
                    DataTableReader reader = DT.CreateDataReader();
                    myConnection.Open();

                    Log.Write("BULK INSERT started...", "ImportDBFtoSQLServer");
                    SqlBulkCopy sqlcpy = new SqlBulkCopy(ConnectionString.ConnectionString, SqlBulkCopyOptions.FireTriggers);
                    sqlcpy.BulkCopyTimeout = 0;
                    sqlcpy.DestinationTableName = DestinationTable;
                    try {
                        sqlcpy.WriteToServer(DT);
                        DBC.DBQuery(Log, new DBClass.DBRequest("DELETE FROM W_LOAD_FILES WHERE ArchiveName = '" + ArchiveName + "' AND FileName = '" + FilePath + "';INSERT INTO W_LOAD_FILES (ArchiveName, FileName, RowsCount) VALUES ('" + ArchiveName + "', '" + FilePath + "', " + DT.Rows.Count + ")"));
                    } catch (Exception exc) {
                        Log.Write("Failed to BULK: " + FilePath + ": " + exc.Message, "ImportDBFtoSQLServer", true);
                    }
                    myConnection.Close();
                    reader.Close();
                }
            }
        }

    }
}
