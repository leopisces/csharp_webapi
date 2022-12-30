namespace DataService.Shared.Base
{
    /// <summary>
    /// 返回数据
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ResponseResult() { 
        
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ResponseResult(int code, string msg = null, object data = null)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="data"></param>
        public ResponseResult(ErrorCodes errcode, object data = null)
        {
            this.Code = (int)errcode;
            this.Msg = errcode.ToString().ToLower();
            this.Data = data;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errorMsg"></param>
        /// <param name="data"></param>
        public ResponseResult(ErrorCodes errcode, string errorMsg = null, object data = null)
        {
            this.Code = (int)errcode;
            this.Msg = errorMsg;
            this.Data = data;
        }
    }
}
