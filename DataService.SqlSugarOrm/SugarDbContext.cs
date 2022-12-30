using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataService.SqlSugarOrm
{
    /// <summary>
    /// 基础类
    /// </summary>
    public abstract class SugarDbContext
    {
        protected readonly ILogger logger;
        protected readonly SugarOption sugarOption;
        protected SqlDatabaseEnum DatabaseEnum
        {
            get
            {
                return InitDB();
            }
        }
        protected abstract SqlDatabaseEnum InitDB();

        protected SqlSugarClient Instance //注意当前方法的类不能是静态的 public static class这么写是错误的
        {
            get
            {
                return DBClient();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SugarDbContext(IOptions<SugarOption> options, ILogger logger)
        {
            this.sugarOption = options.Value;
            this.logger = logger;
        }

        /// <summary>
        /// 返回一个SqlSugarClient
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="AutoClose"></param>
        /// <returns></returns>
        protected SqlSugarClient DBClient(bool AutoClose = true)
        {
            //拼装
            string _sqlConType_name = DatabaseEnum.ToString().ToLower();
            var obj = sugarOption.Databases.Where(c => c.DBName.ToLower() == _sqlConType_name).FirstOrDefault();
            if (obj == null)
            {
                throw new Exception("not found sugar configure node in appsettiing.json");
            }

            try
            {
                var dbtype = (obj.DbType.ToLower()) switch
                {
                    "mysql" => DbType.MySql,
                    "mssql" => DbType.SqlServer,
                    "oracle" => DbType.Oracle,
                    _ => throw new Exception("sugarsql type error , appsetting.json"),
                };
                SqlSugarClient db = new SqlSugarClient(
                    new ConnectionConfig()
                    {
                        ConnectionString = obj.Master,
                        DbType = dbtype,//设置数据库类型
                        IsAutoCloseConnection = AutoClose,//自动释放数据务，如果存在事务，在事务结束后释放
                        InitKeyType = InitKeyType.Attribute, //从实体特性中读取主键自增列信息
                        ConfigureExternalServices = new ConfigureExternalServices()
                        {
                            SqlFuncServices = ExpMethods(dbtype)   //扩展方法
                        },
                    }
                );
                db.Ado.CommandTimeOut = 20000;//设置超时时间

                //创建SLAVE
                if (obj.Slaves != null && obj.Slaves.Count() > 0)
                {
                    db.CurrentConnectionConfig.SlaveConnectionConfigs = new List<SlaveConnectionConfig>();
                    foreach (var slave in obj.Slaves)
                    {
                        db.CurrentConnectionConfig.SlaveConnectionConfigs.Add(new SlaveConnectionConfig() { ConnectionString = slave, HitRate = 30 });
                    }
                }

                //用来打印Sql方便你调式    
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //logger.LogDebug(sql);
                    //logger.LogDebug("OnLogExecuting:\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                db.Aop.OnLogExecuted = (sql, pars) =>
                {
                    logger.LogDebug(sql);
                    logger.LogDebug("OnLogExecuted:\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                db.Aop.OnError = ex =>
                {
                    logger.LogError("Excute sql faild!", ex.Message);
                };

                return db;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 扩展Lambda
        /// </summary>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        private List<SqlFuncExternal> ExpMethods(SqlSugar.DbType dbtype)
        {
            var expMethods = new List<SqlFuncExternal>();
            expMethods.Add(new SqlFuncExternal()
            {
                UniqueMethodName = "Round",
                MethodValue = (expInfo, dbType, expContext) =>
                {
                    if (dbtype == SqlSugar.DbType.MySql)
                        return string.Format("Round({0},{1})", expInfo.Args[0].MemberName, expInfo.Args[1].MemberName);
                    else
                        throw new Exception("未实现");
                }
            });
            return expMethods;
        }

        /// <summary>
        /// 四舍五入保留i位数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static decimal? Round<T>(T str, int i)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }
    }
}
