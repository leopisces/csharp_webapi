using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Domain.BaseInfo
{
    /// <summary>
    /// 用户角色表扩展
    /// </summary>
    public partial class Base_User
    {
        /// <summary>
        /// 用户角色
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(Claim_RoleId))]
        public Base_ClaimRole ClaimRole { get; set; }
    }

    /// <summary>
    /// 接口请求角色表扩展
    /// </summary>
    public partial class Base_ClaimRole
    {
        /// <summary>
        /// 角色路径关系
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(Base_RoleUrl.Claim_RoleId))]
        public List<Base_RoleUrl> RoleUrl { get; set; }//注意禁止手动赋值 
    }


    /// <summary>
    /// 请求url路径角色关系表扩展
    /// </summary>
    public partial class Base_RoleUrl
    {
        /// <summary>
        /// 用户角色
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(Claim_RoleId))]
        public Base_ClaimRole RoleInfo { get; set; }

        /// <summary>
        /// 请求url路径
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(UrlId))]
        public Base_ReqUrl ReqUrl { get; set; }
    }
}
