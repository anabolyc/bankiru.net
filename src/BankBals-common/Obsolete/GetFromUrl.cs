using System;
using System.Net;
using System.IO;
/*
namespace www.BankBals.Common {

    public static class GetFromUrl {

        public static Result DownLoadFile0(string FromPathName, string ToPathName) {
            WebClient WebClient = new WebClient();
            try {
                WebClient.DownloadFile(FromPathName, ToPathName);
                return Result.RESULT_OK;
            }
            catch {
                return Result.RESULT_FAIL;
            }
        }

        public static Result DownLoadFile(string FromPathName, string ToPathName) {
            WebClient WebClient = new WebClient();
            try {
                WebClient.DownloadFile(FromPathName, ToPathName);
                return Result.RESULT_OK;
            } catch {
                return Result.RESULT_FAIL;
            }
        }

        public static Result DownLoadFile(string remoteUri, out string remoteText) {
            WebClient WebClient = new WebClient();
            try {
                //WebClient.DownloadFile(remoteUri, ToPathName);
                byte[] data = WebClient.DownloadData(remoteUri);
                remoteText = System.Text.Encoding.ASCII.GetString(data);
                return Result.RESULT_OK;
            } catch {
                remoteText = String.Empty;
                return Result.RESULT_FAIL;
            }
        }

    }
}


*/