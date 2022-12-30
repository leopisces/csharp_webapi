using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace DataService.Swagger.DocumentFilters
{
    /// <summary>
    /// Swagger注释帮助类
    /// </summary>
    public class SwaggerDocTag : IDocumentFilter
    {
        /// <summary>
        /// 从API文档中读取控制器描述
        /// </summary>
        /// <returns>所有控制器描述</returns>
        public List<OpenApiTag> GetControllerDesc(List<string> docNames)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var xmlpath = Path.Combine(basePath, GetType().Assembly.GetName().Name + ".xml");
            var controllerDescDict = new List<OpenApiTag>();
            if (File.Exists(xmlpath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlpath);
                string type = string.Empty, path = string.Empty, controllerName = string.Empty;

                string[] arrPath;
                int length = -1, cCount = "Controller".Length;
                XmlNode summaryNode = null;
                foreach (XmlNode node in xmldoc.SelectNodes("//member"))
                {
                    type = node.Attributes["name"].Value;
                    if (type.StartsWith("T:"))
                    {
                        //控制器
                        arrPath = type.Split('.');
                        length = arrPath.Length;
                        controllerName = arrPath[length - 1];
                        if (controllerName.EndsWith("Controller"))
                        {
                            //获取控制器注释
                            summaryNode = node.SelectSingleNode("summary");
                            string key = controllerName.Remove(controllerName.Length - cCount, cCount);
                            if (summaryNode != null && !string.IsNullOrEmpty(summaryNode.InnerText) && !controllerDescDict.Exists(it => it.Name == key))
                            {
                                controllerDescDict.Add(new OpenApiTag { Name = key, Description = summaryNode.InnerText.Trim() });
                            }
                        }
                    }
                }
            }
            return controllerDescDict.Where(it => docNames.Contains(it.Name)).ToList();
        }

        /// <summary>
        /// 添加附加注释
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var names = new List<string>();
            swaggerDoc.Paths.Keys.ToList().ForEach(item =>
            {
                if (item.Split("/").Length > 3)
                {
                    names.Add(item.Split("/")[3]);
                }
            });
            swaggerDoc.Tags = GetControllerDesc(names);
        }
    }
}
