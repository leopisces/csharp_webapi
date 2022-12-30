using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Domain.Shared.Json
{
    /// <summary>
    /// ToLowerContractResolver
    /// </summary>
    public class ToLowerContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// ResolvePropertyName
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
