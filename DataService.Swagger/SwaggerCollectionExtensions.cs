using DataService.Shared.Base;
using DataService.Swagger.DocumentFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataService.Swagger
{
    public static class SwaggerCollectionExtensions
    {
        /// <summary>
        /// 注入自定义的swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="apiInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IApiInfo apiInfo)
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(c => c.FullName);

                //遍历GroupVersion所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(GroupVersion).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();

                    options.SwaggerDoc(f.Name.ToLower(), new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version ?? apiInfo.Version,
                        Description = info?.Description
                    });
                });

                //options.SwaggerDoc(apiInfo.ApiName, new OpenApiInfo
                //{
                //    Title = apiInfo.Title,
                //    Version = apiInfo.Version,
                //    Description = apiInfo.Description
                //});

                //判断接口归于哪个分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    return docName == apiInfo.ApiName ? true : apiDescription.GroupName == docName;
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlpath = Path.Combine(basePath, apiInfo.ApplicationAssembly.GetName().Name + ".xml");

                if (File.Exists(xmlpath))
                {
                    options.IncludeXmlComments(xmlpath, includeControllerXmlComments: true);
                }
                var modelPath = Path.Combine(basePath, "DataService.Dto.xml");
                if (File.Exists(modelPath))
                {
                    options.IncludeXmlComments(modelPath);
                }
                //TOKEN
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });
                //添加安全要求
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
                //options.DocumentFilter<HiddenFilter>();
                //options.DocumentFilter<SwaggerDocTag>();
                //options.SchemaFilter<HiddenPropFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IApiInfo apiInfo)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentName}/{version}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                //遍历GroupVersion所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(GroupVersion).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    c.SwaggerEndpoint($"/{f.Name.ToLower()}/{info.Version}/swagger.json", $"{info.Title} {info.Version}");

                });

                c.DocumentTitle = apiInfo.ApiName + "接口文档";

                //css 注入
                c.InjectStylesheet("/css/swaggerdoc.css");
                c.InjectStylesheet("/css/app.min.css");
                //js 注入
                c.InjectJavascript("/js/jquery.js");
                c.InjectJavascript("/js/swaggerdoc.js");
                c.InjectJavascript("/js/app.min.js");

                //c.SwaggerEndpoint($"/{apiInfo.ApiName}/{apiInfo.Version}/swagger.json", $"{apiInfo.Title} {apiInfo.Version}");
                //SWAGGER UI折叠
                c.DocExpansion(DocExpansion.None);
                //不显示model
                c.DefaultModelExpandDepth(-1);
            });

            return app;
        }
    }
}