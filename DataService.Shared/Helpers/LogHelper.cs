using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 描述：log4net日志类
    /// 作者：Leopisces
    /// 创建日期：2022/7/24 16:31:53
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.1      
    /// 修改人：Leopisces
    /// 修改日期：2022/8/4 20:18:53
    /// 修改内容：精简方法调用,和自定义的ILogger结合起来用,效果还不错
    /// ==============================================================
    public class LogHelper
    {
        private static readonly ILog log;
        /// <summary>
        /// 保留日志大小G
        /// </summary>
        private static readonly int logSize;
        /// <summary>
        /// 日志上次清理时间
        /// </summary>
        private static DateTime lastDelete = DateTime.MinValue;

        static LogHelper()
        {
            string appPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log4net.Config";
            log4net.Config.XmlConfigurator.Configure(new FileInfo(appPath));
            log = LogManager.GetLogger("log");
            logSize = ConfigurationManager.AppSettings["log_size"] == null ? 1 : Convert.ToInt32(ConfigurationManager.AppSettings["log_size"]);

            //init
            log.Debug("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            log.Debug("系统启动");
            log.Debug("作者: Leopisces");
            log.Debug($"日期: {DateTime.Now}");
            log.Debug("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="info">错误信息</param>
        /// <param name="ex">异常</param>
        public static void Log(LogLevel level, string info, Exception ex = null)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    if (log.IsDebugEnabled)
                    {
                        if (ex == null)
                        {
                            log.Debug(info);
                        }
                        else
                        {
                            log.Debug(info, ex);
                        }
                        DeleteLog();
                    }
                    break;
                case LogLevel.Information:
                    if (log.IsInfoEnabled)
                    {
                        if (ex == null)
                        {
                            log.Info(info);
                        }
                        else
                        {
                            log.Info(info, ex);
                        }
                        DeleteLog();
                    }
                    break;
                case LogLevel.Warning:
                    if (log.IsWarnEnabled)
                    {
                        if (ex == null)
                        {
                            log.Warn(info);
                        }
                        else
                        {
                            log.Warn(info, ex);
                        }
                        DeleteLog();
                    }
                    break;
                case LogLevel.Error:
                    if (log.IsErrorEnabled)
                    {
                        if (ex == null)
                        {
                            log.Error(info);
                        }
                        else
                        {
                            log.Error(info, ex);
                        }
                        DeleteLog();
                    }
                    break;
                default:
                    if (log.IsFatalEnabled)
                    {
                        if (ex == null)
                        {
                            log.Fatal(info);
                        }
                        else
                        {
                            log.Fatal(info, ex);
                        }
                        DeleteLog();
                    }
                    break;
            }
        }

        /// <summary>
        /// 定期删除日志
        /// </summary>
        private static void DeleteLog()
        {
            try
            {
                lastDelete = DateTime.Now;
                if (DateTime.Now - lastDelete < new TimeSpan(7, 0, 0, 0))
                {
                    return;
                }

                lastDelete = DateTime.Now;
                string programPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log";

                var directorys = new DirectoryInfo(programPath).GetDirectories().ToList();
                var fileList = new List<FileInfo>();

                directorys.ForEach(obj =>
                {
                    fileList.AddRange(obj.GetFiles());
                });

                fileList.Sort((x, y) =>
                {
                    return (int)(y.LastWriteTime - x.LastWriteTime).TotalSeconds;
                });

                long totalSize = 0;
                for (int i = 0; i < fileList.Count; i++)
                {
                    var item = fileList[i];
                    if (totalSize > logSize * 1024 * 1024 * 1024 && i > 4)
                    {
                        item.Delete();
                    }
                    else
                    {
                        totalSize += item.Length;
                    }
                }


                directorys.ForEach(obj =>
                {
                    if (obj.GetFiles().Length == 0)
                    {
                        Directory.Delete(obj.FullName, true);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Fatal("删除旧日志失败", ex);
            }
        }
    }
}
