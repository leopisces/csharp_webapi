using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Attributes
{
    /// <summary>
    /// 测试的方法排序
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        public TestPriorityAttribute(int priority) => Priority = priority;
    }
}
