using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataService.SqlSugarOrm
{
    public class SugarOption
    {
        public List<DBConfig> Databases { get; set; }
        public class DBConfig
        {
            public string DbType { get; set; }
            public string DBName { get; set; }

            public string Master
            {
                get => ConnectStringHelper.TranConnectStr(_Master, DbType);
                set => _Master = value;
            }

            public string[] Slaves
            {
                get => ConnectStringHelper.TranConnectStr(_Slaves, DbType);
                set => _Slaves = value;
            }

            private string _Master;
            private string[] _Slaves;
        }

        internal class ConnectStringHelper
        {
            public static string TranConnectStr(string ConnectStr, string DbType = null)
            {
                if (!string.IsNullOrEmpty(DbType) && (DbType.ToLower() == "mssql" || DbType.ToLower() == "oracle"))
                {
                    return ConnectStr;
                }
                /*
                 * 语句自动拼接
                 *Allow User Variables=True;SslMode=none;Connect Timeout=1600;Persist Security Info=True
                 */
                if (string.IsNullOrEmpty(ConnectStr))
                {
                    return null;
                }
                if (!ConnectStr.Contains("Allow User Variables"))
                {
                    ConnectStr += ";Allow User Variables=True";
                }
                if (!ConnectStr.Contains("SslMode"))
                {
                    ConnectStr += ";SslMode=none";
                }
                if (!ConnectStr.Contains("Connect Timeout"))
                {
                    ConnectStr += ";Connect Timeout=1600";
                }
                if (!ConnectStr.Contains("Persist Security Info"))
                {
                    ConnectStr += ";Persist Security Info=True";
                }
                return ConnectStr;
            }

            public static string[] TranConnectStr(string[] ConnectStrArr, string DbType)
            {
                if (ConnectStrArr == null)
                    return null;
                return ConnectStrArr.Select(c => TranConnectStr(c, DbType)).ToArray();
            }
        }
    }
}
