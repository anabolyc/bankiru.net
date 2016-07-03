using System;
using System.Data.SqlClient;
using System.IO;

namespace www.BankBals {
    public static class Tools {
#if DEBUG
        public const string SQL_DB_BANKBALS = "Bankbals2";
        public const string SQL_DB_ASP = "aspnetdb";
        public const string SQL_ADDRESS = ".\\sqlexpress";
        public const string SQL_USERNAME = "webdeveloper";
        public const string SQL_PASSWORD = "***";
#else
        public const string SQL_ADDRESS = "***";
        public const string SQL_DB_BANKBALS = "anabolyc_bb";
        public const string SQL_DB_ASP = "anabolyc_aspnetdb";
        public const string SQL_USERNAME = "anabo_bankbals";
        public const string SQL_PASSWORD = "***";
#endif   

        public static string ConnectionString(string InitialCatalog = SQL_DB_BANKBALS) { 
            SqlConnectionStringBuilder cssb = new SqlConnectionStringBuilder();
            cssb.DataSource = SQL_ADDRESS;
            if (InitialCatalog != String.Empty)
                cssb.InitialCatalog = InitialCatalog;
            cssb.MultipleActiveResultSets = true;
            cssb.UserID = SQL_USERNAME;
            cssb.Password = SQL_PASSWORD;
            return cssb.ConnectionString;
        }

        public static void CopyDirectory(string sourcePath, string destPath) {
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            foreach (string fileItem in Directory.GetFiles(sourcePath))
                File.Copy(fileItem, Path.Combine(destPath, Path.GetFileName(fileItem)));

            foreach (string folderItem in Directory.GetDirectories(sourcePath))
                CopyDirectory(folderItem, Path.Combine(destPath, Path.GetFileName(folderItem)));
        } 
    }
}
