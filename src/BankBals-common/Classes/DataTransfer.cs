using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;

namespace www.BankBals.Classes {

    [ComVisible(true)]
    public class DataTransfer {

        private const string SQL_DATE_FORMAT = "yyyy-MM-dd";

        private OleDbConnection _dBaseConnection;
        public string ConnectionStringFrom {
            set {
                this._dBaseConnection = new OleDbConnection(value);
            }
        }

        private SqlConnection _myConnection;
        public string ConnectionStringTo {
            set {
                _myConnection = new SqlConnection(value);
            }
        }

        private Exception _lastException;
        public Exception LastException {
            get {
                return _lastException;
            }
        }

        private string _ColumnsList = "*";
        public string ColumnsList {
            set {
                this._ColumnsList = value;
            }
        }

        private string _TableName = String.Empty;
        public string TableName {
            set {
                this._TableName = value;
            }
        }

        private string _WhereClause = String.Empty;
        public string WhereClause {
            set {
                this._WhereClause = value;
            }
        }

        private string _DestinationTable = String.Empty;
        public string DestinationTable {
            set {
                this._DestinationTable = value;
            }
        }

        private bool _AssignDate = false;
        private DateTime _Date;
        public DateTime AssignDate {
            set {
                this._AssignDate = true;
                this._Date = value;
            }
        }

        private DataTable _DataTable = new DataTable();
        public bool DownloadData() {
            try {
                _dBaseConnection.Open();

                OleDbCommand command = new OleDbCommand("SELECT " + this._ColumnsList + " FROM [" + this._TableName + "] " + this._WhereClause, _dBaseConnection);

                try {
                    _DataTable.Load(command.ExecuteReader());
                } catch (Exception e) {
                    this._lastException = e;
                    return false;
                }

                _dBaseConnection.Close();
                return true;
            } catch (Exception e) {
                this._lastException = e;
                return false;
            }
        }

        public bool UploadData() {

            if (_AssignDate) {
                DataColumn DataCol = _DataTable.Columns.Add("Date", Type.GetType("System.DateTime"), "'" + _Date.ToString(SQL_DATE_FORMAT) + "'");
                DataCol.SetOrdinal(0);
            }

            try {
                DataTableReader reader = _DataTable.CreateDataReader();
                _myConnection.Open();

                SqlBulkCopy sqlcpy = new SqlBulkCopy(_myConnection, SqlBulkCopyOptions.FireTriggers, null);
                sqlcpy.BulkCopyTimeout = 0;
                sqlcpy.DestinationTableName = _DestinationTable;
                try {
                    sqlcpy.WriteToServer(_DataTable);
                } catch (Exception e) {
                    this._lastException = e;
                    return false;
                }
                _myConnection.Close();
                reader.Close();
                return true;
            } catch (Exception e) {
                this._lastException = e;
                return false;
            }
        }
    }
}