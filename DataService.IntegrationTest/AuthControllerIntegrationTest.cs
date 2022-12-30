using DataService.HostApi;
using DataService.IntegrationTest.Base;
using DataService.Shared.Attributes;
using DataService.Shared.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DataService.IntegrationTest
{
    /// <summary>
    /// Auth控制器集成测试
    /// 测试接口方法，模拟服务端和调用方
    /// </summary>
    [TestCaseOrderer("DataService.IntegrationTest.PriorityOrderer", "DataService.IntegrationTest")]
    public class AuthControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public AuthControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private readonly HttpClient _client;

        /// <summary>
        /// 登录并返回登录相关信息
        /// </summary>
        /// <returns></returns>
        [Theory(DisplayName = "登录验证")]
        [InlineData("lp", "1234")]
        //[TestPriority(2)]
        public async Task LoginSuccess(string account, string pwd)
        {
            string url = $"/api/auth/login?account={account}&password={pwd}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult>(content);
            var data = JsonConvert.DeserializeObject<LoginInfo>(result.Data.ToString());
            Assert.True(data.Status);
        }

        /// <summary>
        /// 登录失败
        /// </summary>
        /// <returns></returns>
        [Theory(DisplayName = "登录失败验证")]
        [InlineData("lp", "12345")]
        //[TestPriority(1)]
        public async Task LoginFaild(string account, string pwd)
        {
            string url = $"/api/auth/login?account={account}&password={pwd}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult>(content);
            Assert.Equal(401, result.Code);
        }

    }
}
