using AutoMapper;
using DataService.Dto.BaseInfo;
using DataService.Shared.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataService.HostApi.AutoMapper
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/8/16 16:41:20
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserProfile()
        {
            CreateMap<ClientInformation, ClientInformationDto>();
            CreateMap<LoginInfo, LoginInfoDto>();
        }
    }
}