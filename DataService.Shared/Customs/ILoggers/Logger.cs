using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataService.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace DataService.Shared.Customs.ILoggers
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/8/4 15:41:13
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class Logger : ILogger
    {
        public Logger(string categoryName)
        {
            _categoryName = categoryName;
        }

        //class Empty : IDisposable
        //{
        //    public void Dispose()
        //    {
        //    }
        //}

        public IDisposable BeginScope<TState>(TState state)
        {
            var s = state as IDisposable;
            return s;
            //return new Empty();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        private readonly string _categoryName;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            string message;
            if (typeof(TState) == typeof(LogInfo))
            {
                var logInfo = state as LogInfo;
                Debug.Assert(logInfo != null, nameof(logInfo) + " != null");
                logInfo.CategoryName = _categoryName;
                message = formatter(state, exception);
            }
            else
            {
                message = @$"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}][{Thread.CurrentThread.Name}:{Thread.CurrentThread.ManagedThreadId}][{LogExtension.LogLevelToString(logLevel)}][{_categoryName}][-][-][-][EventId={eventId.Id}:{eventId.Name}] {formatter(state, exception)}";
            }
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }

            Console.WriteLine(message);
        }
    }
}