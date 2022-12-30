using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DataService.Domain.BaseInfo
{
    ///<summary>
    ///接口请求角色表
    ///</summary>
    [SugarTable("Base_ClaimRole")]
    public partial class Base_ClaimRole
    {
           public Base_ClaimRole(){


           }
           /// <summary>
           /// Desc:角色主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Claim_RoleId {get;set;}

           /// <summary>
           /// Desc:名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:描述
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Description {get;set;}

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
