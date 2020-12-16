using System;
using System.Collections.Generic;
using System.Text;

namespace Wangchunlai.IOCDI.Framework
{
    class ContainerRegistModel
    {
        public Type TargetType { get; set; }
        public LifeTimeType LifeTime { get; set; }
        /// <summary>
        /// 仅存储单例模式情况下的对象
        /// </summary>
        public object SingletonInstance { get; set; }
    }
    public enum LifeTimeType
    {
        /// <summary>
        /// 瞬态模式
        /// </summary>
        Transient,
        /// <summary>
        /// 单例模式
        /// </summary>
        Singleton,
        /// <summary>
        /// 作用域模式
        /// </summary>
        Scope,
        /// <summary>
        /// 线程模式
        /// </summary>
        PerThread
    }
}
