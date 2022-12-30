using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Swagger
{
    /// <summary>
    /// 隐藏接口，不生成到swagger文档展示
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)] //此特性可以在方法上和类上使用
    public partial class HiddenApiAttribute : Attribute
    {
    }



}
