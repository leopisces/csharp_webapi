using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{

    /// <summary>
    /// 验证错误返回
    /// </summary>
    public class BaseValidationError
    {
        /// <summary>
        /// 字段
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseValidationError(string field, string message)
        {
            this.Field = field != string.Empty ? field : null;
            this.Msg = message;
        }
    }
}
