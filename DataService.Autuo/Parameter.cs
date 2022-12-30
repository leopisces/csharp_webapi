using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Auto
{
    public class Parameter
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableNameNo_ { get; set; }
        /// <summary>
        /// 实体的命名空间
        /// </summary>
        public string ModelSpace { get; set; }
        /// <summary>
        /// 接口层的命名空间
        /// </summary>
        public string InterfaceSpace { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
