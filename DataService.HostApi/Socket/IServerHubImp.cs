using DataService.Shared.Enums;
using DataService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.HostApi.Socket
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/8/9 16:09:31
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public interface IServerHubImp
    {
        /// <summary>
        /// 获取所有在线用户
        /// </summary>
        /// <returns></returns>
        List<OnLineUser> GetAll();

        /// <summary>
        /// 主动推送发送                         
        /// </summary>
        /// <param name="type"></param>
        /// <param name="conn_id"></param>
        /// <param name="msg"></param>
        Task<bool> SendMessageAsync(MessageType type, string conn_id, object msg);

        /// <summary>
        /// 主动推送发送
        /// </summary>
        /// <param name="type"></param>
        /// <param name="user_id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<bool> SendMessageByUserAsync(MessageType type, string user_id, object msg);

        /// <summary>
        /// 全员推送                         
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        Task<bool> SendMessageToAllAsync(MessageType type, object msg);
    }
}