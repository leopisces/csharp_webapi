using Microsoft.OpenApi.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Swagger
{
    /// <summary>
    /// 描述：HtmlHelper
    /// 作者：Leopisces
    /// 创建日期：2022/8/8 11:38:20
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class HtmlHelper
    {
        /// <summary>
        /// 将数据遍历静态页面中
        /// </summary>
        /// <param name="templatePath">静态页面地址</param>
        /// <param name="model">获取到的文件数据</param>
        /// <returns></returns>
        public static string GeneritorSwaggerHtml(string templatePath, OpenApiDocument model)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            var result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), model.GetType(), model);
            return result;
        }
    }
}
