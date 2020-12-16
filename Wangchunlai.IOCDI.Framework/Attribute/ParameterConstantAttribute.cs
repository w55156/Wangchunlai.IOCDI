using System;
using System.Collections.Generic;
using System.Text;

namespace Wangchunlai.IOCDI.Framework
{
    /// <summary>
    /// 带这个属性标识的参数代表：常量参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterConstantAttribute:Attribute
    {
    }
}
