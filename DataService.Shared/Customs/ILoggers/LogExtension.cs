using Microsoft.Extensions.Logging;
using DataService.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DataService.Shared.Customs.ILoggers
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/8/4 15:13:29 
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public static class LogExtension
    {
        public static void LogPlus(this ILogger logger
            , LogLevel type
            , string message
            , Exception exception = null
            , string traceId = null
            , string userId = null
            , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
            , [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = ""
            , [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            , params string[] tags
        )
        {
            var logInfo = new LogInfo()
            {
                ClassFile = Path.GetFileName(sourceFilePath),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                LineNumber = sourceLineNumber,
                Message = message,
                TraceId = traceId,
                UserId = userId,
                MemberName = memberName,
                Tags = tags,
                LogLevel = type
            };
            logger.Log(type, eventId: EmptyEventId, logInfo, exception, Formatter);
        }

        private static string Formatter(LogInfo logInfo, Exception exception)
        {
            // honeycomb-log
            // [2099-10-19 19:07:45.456][threadName][INFO][类:行数][_traceId:realTraceId][_userId:realUserId][tag:custom] 业务的日志/异常堆栈

            const string empty = "-";

            var traceMessage = string.IsNullOrEmpty(logInfo.TraceId) ? empty : $"_traceId:{logInfo.TraceId}";

            var userMessage = string.IsNullOrEmpty(logInfo.UserId) ? empty : $"_userId:{logInfo.UserId}";

            var logLevelMessage = LogLevelToString(logInfo.LogLevel);

            var logInfoMessage = string.IsNullOrEmpty(logInfo.Message) ? exception?.Message : logInfo.Message;

            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.sss}][{logInfo.ThreadName}:{logInfo.ThreadId}][{logLevelMessage}][{logInfo.ClassFile}:{logInfo.CategoryName}.{logInfo.MemberName}:{logInfo.LineNumber}][{traceMessage}][{userMessage}][tags:{string.Join(";", logInfo.Tags)}] {logInfoMessage} {exception?.ToString()}";
        }

        public static string LogLevelToString(LogLevel logLevel)
            => logLevel switch
            {
                LogLevel.Trace => "TRACE",
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARNING",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "CRITICAL",
                LogLevel.None => "NONE",
                _ => "NONE"
            };

        private static readonly EventId EmptyEventId = new EventId();
    }
}