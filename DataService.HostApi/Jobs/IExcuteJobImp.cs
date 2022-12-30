using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.HostApi.Jobs
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/8/9 15:33:07
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public interface IExcuteJobImp
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}