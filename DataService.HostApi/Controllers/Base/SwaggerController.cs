using DataService.Swagger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DataService.HostApi.Controllers.Base
{
    /// <summary>
    /// swagger
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly SwaggerGenerator _swaggerGenerator;

        private readonly SpireDocHelper _spireDocHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="spireDocHelper"></param>
        /// <param name="swaggerGenerator"></param>
        public SwaggerController(IWebHostEnvironment hostingEnvironment
            , SpireDocHelper spireDocHelper
            , SwaggerGenerator swaggerGenerator
        )
        {
            _webHostEnvironment = hostingEnvironment;
            _spireDocHelper = spireDocHelper;
            _swaggerGenerator = swaggerGenerator;
        }
        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        [HttpGet]
        public FileResult ExportWord(string type, string version)
        {
            var contenttype = string.Empty;
            var model = _swaggerGenerator.GetSwagger(version.ToLower()); //1. 根据指定版本获取指定版本的json对象。
            //ListHelper.SaveJsonRecord<OpenApiDocument>(model, AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../../../" + "/wwwroot/Files/TempFiles/new.json");
            var html = HtmlHelper.GeneritorSwaggerHtml($"{_webHostEnvironment.WebRootPath}/WebAPITemplate.cshtml", model); //2. 根据模板引擎生成html
            var op = _spireDocHelper.SwaggerConversHtml(html, type, out contenttype); //3.将html导出文件类型
            return File(op, contenttype, $"ERPWeb接口文档{type}");
        }
    }
}