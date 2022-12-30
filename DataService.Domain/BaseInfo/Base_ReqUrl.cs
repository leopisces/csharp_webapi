using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace DataService.Domain.BaseInfo
{
    ///<summary>
    ///接口请求路径表
    ///</summary>
    [SugarTable("Base_ReqUrl")]
    public partial class Base_ReqUrl
    {
        public Base_ReqUrl()
        {


        }
        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int UrlId { get; set; }

        /// <summary>
        /// Desc:Url
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Url { get; set; }

        /// <summary>
        /// Desc:访问次数
        /// Default:
        /// Nullable:True
        /// </summary>  
        public int Count { get; set; }

        /// <summary>
        /// Desc:0:无效 1:有效
        /// Default:
        /// Nullable:True
        /// </summary>  
        public bool Enabled { get; set; }

        /// <summary>
        /// Desc:创建日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? CreaterId { get; set; }

        /// <summary>
        /// Desc:更新日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Desc:更新人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? UpdateId { get; set; }

    }
}
