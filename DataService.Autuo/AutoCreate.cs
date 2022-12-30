using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using DataService.SqlSugarOrm;
using Microsoft.Extensions.Configuration;
using DataService.Auto;
using Microsoft.Extensions.Logging;
using RazorEngine;
using RazorEngine.Templating;
using DataService.Shared.Helpers;

namespace Lp.AutoCreate
{
    public class AutoCreate : SugarDbContext
    {
        private readonly IConfiguration _configuration;
        public AutoCreate(IOptions<SugarOption> options, IConfiguration configuration, ILogger<AutoCreate> logger) : base(options, logger)
        {
            _configuration = configuration;
        }

        #region 自动生成业务层，数据层，model层
        /// <summary>
        /// 自动生成model
        /// </summary>
        public AutoCreate CreateModel()
        {
            using (var Db = Instance)
            {
                var autoConfig = _configuration.GetSection("AutoConfig").Get<AutoConfig>();
                var path = Directory.GetCurrentDirectory() + "../../../../../";
                var space = autoConfig.Namespaces["Model"];
                foreach (var p in autoConfig.TableNames)
                {
                    foreach (var item in Db.DbMaintenance.GetTableInfoList().Where(it => it.Name == p))
                    {
                        /*实体名*/
                        string entityName = item.Name;
                        Db.MappingTables.Add(entityName, item.Name);
                        foreach (var col in Db.DbMaintenance.GetColumnInfosByTableName(item.Name))
                        {
                            /*类的属性*/
                            Db.MappingColumns.Add(col.DbColumnName, col.DbColumnName, entityName);
                        }
                    }
                    Db.DbFirst.Where(it => it == p).IsCreateAttribute().CreateClassFile(path + "/DataService.Domain/" + space.Split('.')[2], space);
                    Console.WriteLine($"{p.Replace("_", "")}.cs已生成");
                }
            }
            return this;
        }

        /// <summary>
        /// 自动生成业务接口层文件
        /// </summary>
        /// <returns></returns>
        public AutoCreate CreateInterface()
        {
            using (var Db = Instance)
            {
                var autoConfig = _configuration.GetSection("AutoConfig").Get<AutoConfig>();
                var path = Directory.GetCurrentDirectory() + "../../../../../";
                var space = autoConfig.Namespaces["Interface"];
                foreach (var p in autoConfig.TableNames)
                {
                    var templatePath = path + "DataService.Autuo/Template/Interface.txt";

                    string template = File.ReadAllText(templatePath); //从文件中读出模板内容
                    string templateKey = Guid.NewGuid().ToString(); //取个名字
                    var savePath = path + "DataService.Application.Contracts/" + space.Split('.')[3] + "/I" + p.Replace("_", "") + "Imp.cs";
                    var model = new Parameter
                    {
                        Namespace = space,
                        TableName = p,
                        ModelSpace = autoConfig.Namespaces["Model"],
                        TableNameNo_= p.Replace("_", "")
                    };
                    var result = Engine.Razor.RunCompile(template, templateKey, model.GetType(), model);
                    FileHelper.CreateFile(savePath, result, System.Text.Encoding.UTF8);
                    Console.WriteLine($"I{p.Replace("_", "")}Imp.cs已生成");
                }


            }
            return this;
        }

        /// <summary>
        /// 自动生成业务实现层文件
        /// </summary>
        /// <returns></returns>
        public AutoCreate CreateImp()
        {
            using (var Db = Instance)
            {
                var autoConfig = _configuration.GetSection("AutoConfig").Get<AutoConfig>();
                var path = Directory.GetCurrentDirectory() + "../../../../../";
                var space = autoConfig.Namespaces["Imp"];
                foreach (var p in autoConfig.TableNames)
                {
                    var templatePath = path + "DataService.Autuo/Template/Imp.txt";

                    string template = File.ReadAllText(templatePath); //从文件中读出模板内容
                    string templateKey = Guid.NewGuid().ToString(); //取个名字
                    var savePath = path + "DataService.Application/" + space.Split('.')[2] + "/I" + p.Replace("_", "") + "Imp.cs";
                    var model = new Parameter
                    {
                        Namespace = space,
                        TableName = p,
                        ModelSpace = autoConfig.Namespaces["Model"],
                        TableNameNo_ = p.Replace("_", ""),
                        InterfaceSpace= autoConfig.Namespaces["Interface"]
                    };
                    var result = Engine.Razor.RunCompile(template, templateKey, model.GetType(), model);
                    FileHelper.CreateFile(savePath, result, System.Text.Encoding.UTF8);
                    Console.WriteLine($"{p.Replace("_", "")}Imp.cs已生成");
                }


            }
            return this;
        }
        #endregion

        #region 表相关
        /// <summary>
        /// 自动生成建表Sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public AutoCreate CreateTableSql(string tableName)
        {
            var sql = $"CREATE TABLE {tableName} (\n";
            using (var Db = Instance)
            {
                var ts = Db.Ado.SqlQuery<TableStructure>(@"
                    SELECT
	                    b.comments AS Comments,
	                    A .column_name AS ColumnName,
	                    CASE
		                    WHEN A .data_type = 'NUMBER' THEN
			                    A .data_type || '(' || TO_CHAR (A .data_precision) || ',0)'
		                    WHEN A .data_type = 'DATE' THEN
			                    A .data_type
		                    WHEN A .data_type = 'VARCHAR' THEN
			                    A .data_type || '(' || A .data_length || ')'
		                    ELSE
			                    A .data_type || '(' || A .data_length || ')'
		                    END AS DataType,
                     A .nullable AS Nullable
                    FROM
	                    user_tab_columns A,
	                    user_col_comments b
                    WHERE
	                    A .TABLE_NAME = @table_name
                    AND b.table_name = @table_name
                    AND A .column_name = b.column_name
                    ORDER BY A.column_id
                ", new { table_name = tableName });
                ts.ForEach(it =>
                {
                    sql += $" {it.ColumnName} {it.DataType} {(it.Nullable == "Y" ? "NULL" : "NOT NULL")} , -- {it.Comments} \n";
                });
                sql += $" CONSTRAINT PK_{tableName} PRIMARY KEY (I_ID)\n)";
            }
            Console.WriteLine(sql);
            return this;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>

        protected override SqlDatabaseEnum InitDB()
        {
            return SqlDatabaseEnum.ALIYUNDB;
        }

        public class TableStructure
        {
            public string ColumnName { get; set; }
            public string Comments { get; set; }
            public string DataType { get; set; }
            public string Nullable { get; set; }

        }
        #endregion

    }
}