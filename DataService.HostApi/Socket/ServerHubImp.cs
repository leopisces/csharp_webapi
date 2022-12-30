using DataService.Redis;
using DataService.Shared.Enums;
using DataService.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.HostApi.Socket
{
    /// <summary>
    /// 描述：ServerHub
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 16:08:56
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class ServerHubImp : Hub, IServerHubImp
    {
        /// <summary>
        /// redis字典信息缓存key
        /// </summary>
        private readonly string _hubKey = "message_notice";
        private readonly IRedisService _redisService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IHubContext<ServerHubImp> _hubContext;
        private readonly ILogger<ServerHubImp> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="hubContext"></param>
        /// <param name="redisService"></param>
        /// <param name="logger"></param>
        public ServerHubImp(IHttpContextAccessor accessor
            , IHubContext<ServerHubImp> hubContext
            , IRedisService redisService
            , ILogger<ServerHubImp> logger
        )
        {
            _accessor = accessor;
            _hubContext = hubContext;
            _redisService = redisService;
            _logger = logger;
        }

        #region redis

        /// <summary>
        /// 获取所有在线连接
        /// </summary>
        /// <returns></returns>
        public List<OnLineUser> GetAll()
        {
            return _redisService.HashValues<OnLineUser>(_hubKey);
        }

        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <returns></returns>
        public OnLineUser GetByConnId(string conn_id)
        {
            return _redisService.HashGet<OnLineUser>(_hubKey, conn_id);
        }

        /// <summary>
        /// 获取在线连接
        /// </summary>
        /// <returns></returns>
        public List<OnLineUser> GetByUserId(string user_id)
        {
            var list = GetAll();
            if (list == null)
            {
                return null;
            }
            return list.Where(x => x.user_id == user_id).ToList();
        }

        /// <summary>
        /// 保存连接信息
        /// </summary>
        /// <param name="info"></param>
        public void SetOnline(OnLineUser info)
        {
            _redisService.HashSet<OnLineUser>(_hubKey, info.conn_id, info);
        }

        /// <summary>
        /// 移除连接
        /// </summary>
        /// <param name="conn_id"></param>
        public void DeleteByConnId(string conn_id)
        {
            _redisService.HashDelete(_hubKey, conn_id);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="conn_id"></param>
        /// <returns></returns>
        public bool IsExist(string conn_id)
        {
            return _redisService.HashExists(_hubKey, conn_id);
        }
        #endregion


        /// <summary>
        /// 当连接成功时执行
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            string conn_id = Context.ConnectionId;

            //Console.WriteLine($"SignalR已连接:{conn_id}");

            //验证Token
            var token = _accessor.HttpContext.Request.Query["access_token"].ToString();
            if (string.IsNullOrWhiteSpace(token))
            {
                return base.OnConnectedAsync();
            }

            var jwtArr = token.Split('.');
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));

            var user_id = payLoad["UserID"].ToString();
            //Console.WriteLine("SignalR已连接，用户名：" + user_id);

            SetOnline(new OnLineUser() { user_id = user_id, conn_id = conn_id });

            Clients.Client(conn_id).SendAsync(MessageType.ConnectSuc.ToString(), new
            {
                conn_id,
                user_id,
                msg = "连接成功"
            });

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 当连接断开时的处理
        /// </summary>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string conn_id = Context.ConnectionId;

            //Console.WriteLine($"SignalR断开连接:{conn_id}");

            DeleteByConnId(conn_id);

            return base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// 指定连接推送                         
        /// </summary>
        /// <param name="type"></param>
        /// <param name="conn_id"></param>
        /// <param name="msg"></param>
        public async Task<bool> SendMessageAsync(MessageType type, string conn_id, object msg)
        {
            try
            {
                var user = GetByConnId(conn_id);
                if (user == null)
                {
                    return false;
                }

                //给当前连接返回消息 .Clients可以发多个连接ID
                await _hubContext.Clients.Client(conn_id).SendAsync(type.ToString(), new { conn_id, user.user_id, msg });

                //Console.WriteLine($"发送成功:{conn_id}:{type.ToString()}:{msg}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"发送失败:{ex.ToString()}");
                return false;
            }
        }

        /// <summary>
        /// 指定用户推送                         
        /// </summary>
        /// <param name="type"></param>
        /// <param name="user_id"></param>
        /// <param name="msg"></param>
        public async Task<bool> SendMessageByUserAsync(MessageType type, string user_id, object msg)
        {
            try
            {
                var users = GetByUserId(user_id);

                if (users == null || users.Count < 1)
                {
                    return false;
                }
                foreach (var user in users)
                {
                    //给当前连接返回消息 .Clients可以发多个连接ID
                    await _hubContext.Clients.Client(user.conn_id).SendAsync(type.ToString(), new { user.conn_id, user.user_id, msg });
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"发送失败:{ex.ToString()}");
                return false;
            }
        }

        /// <summary>
        /// 全员推送                         
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public async Task<bool> SendMessageToAllAsync(MessageType type, object msg)
        {
            try
            {
                var users = GetAll();

                if (users == null || users.Count < 1)
                {
                    return false;
                }
                foreach (var user in users)
                {
                    //给当前连接返回消息 .Clients可以发多个连接ID
                    await _hubContext.Clients.Client(user.conn_id).SendAsync(type.ToString(), new { user.conn_id, user.user_id, msg });
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"发送失败:{ex.ToString()}");
                return false;
            }
        }
    }
}