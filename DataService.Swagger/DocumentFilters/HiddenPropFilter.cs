using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace DataService.Swagger.DocumentFilters
{
    /// <summary>
    /// 根据标签隐藏类的属性
    /// </summary>
    public class HiddenPropFilter : ISchemaFilter
    {
        /// <summary>
        /// 实现
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties.Count > 0)
            {
                var props = context.Type.GetProperties().ToList();
                props.ForEach(p =>
                {
                    if (p.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenPropAttribute)))
                    {
                        schema.Properties.Remove(p.Name);
                    }
                });
            }
        }
    }
}