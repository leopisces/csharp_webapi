using ERPCore.Helpers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;

namespace DataService.Swagger.DocumentFilters
{

    /// <summary>
    /// 隐藏过滤器
    /// </summary>
    public class HiddenFilter : IDocumentFilter 
    {
        /// <summary>
        /// 实现
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //ListHelper.SaveJsonRecord<OpenApiDocument>(swaggerDoc, AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../../../" + "/wwwroot/Files/TempFiles/new.json");
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                // 隐藏api接口
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute))
                            || method.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute)))
                    {
                        string key = "/" + apiDescription.RelativePath;
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        swaggerDoc.Paths.Remove(key);
                    }
                }
            }
        }
    }

}
