using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Customs.ILoggers
{
    /// <summary>
    /// 描述：日志信息
    /// 作者：Leopisces
    /// 创建日期：2022/8/4 15:12:56
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// CategoryName
        /// </summary>
        public string CategoryName { set; get; }
        /// <summary>
        /// ThreadName
        /// </summary>
        public string ThreadName { get; set; }
        /// <summary>
        /// ThreadId
        /// </summary>
        public int ThreadId { get; set; }
        /// <summary>
        /// 类文件地址
        /// </summary>
        public string ClassFile { set; get; }
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// TraceId
        /// </summary>
        public string TraceId { set; get; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { set; get; }
        /// <summary>
        /// MemberName
        /// </summary>
        public string MemberName { set; get; }
        /// <summary>
        /// Tags
        /// </summary>
        public string[] Tags { set; get; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel LogLevel { set; get; }
    }
}