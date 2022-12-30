using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DataService.Domain.BaseInfo
{
    ///<summary>
    ///请求url路径角色关系表
    ///</summary>
    [SugarTable("Base_RoleUrl")]
    public partial class Base_RoleUrl
    {
           public Base_RoleUrl(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Role_UrlId {get;set;}

           /// <summary>
           /// Desc:请求url角色id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Claim_RoleId {get;set;}

           /// <summary>
           /// Desc:api路径id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int UrlId {get;set;}

           /// <summary>
           /// Desc:创建日期
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateDate {get;set;}

           /// <summary>
           /// Desc:创建人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? CreaterId {get;set;}

           /// <summary>
           /// Desc:更新日期
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? UpdateDate {get;set;}

           /// <summary>
           /// Desc:更新人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? UpdateId {get;set;}

    }
}
