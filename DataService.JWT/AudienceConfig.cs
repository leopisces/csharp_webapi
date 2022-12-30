using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.JWT
{
    public class AudienceConfig
    {
        /// <summary>
        /// 有效期(小时)
        /// </summary>
        public int ValidateHour { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public string Identification { get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }
    }
}
