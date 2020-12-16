using System;
using System.Collections.Generic;
using System.Text;

namespace Wangchunlai.IOCDI.Framework
{
    /// <summary>
    /// 带这个属性标识的参数代表：有别名的参数或属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter|AttributeTargets.Property)]
    public class ParameterShortNameAttribute:Attribute
    {
        public string ShortName { get; private set; }
        public ParameterShortNameAttribute(string shortName)
        {
            this.ShortName = shortName;
        }
    }
}
