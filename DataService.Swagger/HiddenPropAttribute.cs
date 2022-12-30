using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Swagger
{
    /// <summary>
    /// 隐藏类字段，不生成到swagger文档展示
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)] //此特性可以在方法上和类上使用
    public partial class HiddenPropAttribute : Attribute
    {
    }
}
