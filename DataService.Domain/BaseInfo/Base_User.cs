using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DataService.Domain.BaseInfo
{
    ///<summary>
    ///用户信息表
    ///</summary>
    [SugarTable("Base_User")]
    public partial class Base_User
    {
           public Base_User(){


           }
           /// <summary>
           /// Desc:主键
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public int UserId {get;set;}

           /// <summary>
           /// Desc:姓名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:账号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Account {get;set;}

           /// <summary>
           /// Desc:密码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Password {get;set;}

           /// <summary>
           /// Desc:手机号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Phone {get;set;}

           /// <summary>
           /// Desc:0:禁用 1:启用
           /// Default:0
           /// Nullable:True
           /// </summary>           
           public int? State {get;set;}

           /// <summary>
           /// Desc:请求url的角色，用于实现访问接口权限管理
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Claim_RoleId {get;set;}

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
