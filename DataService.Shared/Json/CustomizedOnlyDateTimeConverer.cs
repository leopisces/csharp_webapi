using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Json
{
    /// <summary>
    /// 描述：日期格式化(HH:mm)
    /// 作者：Leopisces
    /// 创建日期：2022/8/12 12:50:45
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public class CustomizedOnlyDateTimeConverer : DateTimeConverterBase
    {
        private static IsoDateTimeConverter dtConvertor = new IsoDateTimeConverter { DateTimeFormat = "HH:mm" };

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            dtConvertor.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return dtConvertor.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
