using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace DataService.Shared.Customs.ILoggers
{
    /// <summary>
    /// 描述：LogProvider
    /// 作者：Leopisces
    /// 创建日期：2022/8/4 15:10:52
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class LogProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public void Dispose()
        {

        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName);
        }
    }
}