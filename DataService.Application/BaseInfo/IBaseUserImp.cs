using DataService.Application.Contracts.BaseInfo;
using DataService.Domain.BaseInfo;
using DataService.SqlSugarOrm;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Application.BaseInfo
{
    /// <summary>
    /// 描述：BaseUserImp
    /// 作者：Leopisces
    /// 创建日期：2022-08-12 23:34:25
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class BaseUserImp : BaseRepository<Base_User>, IBaseUserImp
    {
        public BaseUserImp(IOptions<SugarOption> options, ILogger<BaseUserImp> logger) : base(options, logger)
        {
        }

        protected override SqlDatabaseEnum InitDB()
        {
            return SqlDatabaseEnum.ALIYUNDB;
        }
    }
}
