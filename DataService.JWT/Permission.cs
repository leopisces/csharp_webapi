
namespace DataService.JWT
{
    /// <summary>
    /// 用户或角色或其他凭据实体
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Name{ get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url{ get; set; }
    }
}
