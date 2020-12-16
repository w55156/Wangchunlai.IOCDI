using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;
using System.Threading;
using Wangchunlai.IOCDI.Framework.CusAOP;

namespace Wangchunlai.IOCDI.Framework
{
    /// <summary>
    /// 用来生成对象
    /// 第三方　业务无关性
    /// </summary>
    public class ChunlaiContainer : IChunlaiContainer
    {
        /// <summary>
        /// 容器（字典）
        /// </summary>
        private Dictionary<string, ContainerRegistModel> ContainerDictionary = new Dictionary<string, ContainerRegistModel>();
        /// <summary>
        /// 参数列表（字典）
        /// </summary>
        private Dictionary<string, object[]> ParaListDictionary = new Dictionary<string, object[]>();
        private Dictionary<string, object> ScopeDictionaty = new Dictionary<string, object>();
        /// <summary>
        /// 获取Ｋey
        /// </summary>
        /// <param name="fullName">TFrom的Type的fullName</param>
        /// <param name="shortName">实现类的shortName</param>
        /// <returns></returns>
        private string GetKey(string fullName, string shortName) => $"{fullName}___{shortName}";
        private string GetShortName(ICustomAttributeProvider provider)
        {
            if (provider.IsDefined(typeof(ParameterShortNameAttribute), true))
            {
                var attribute = (ParameterShortNameAttribute)provider.GetCustomAttributes(typeof(ParameterShortNameAttribute), true)[0];
                return attribute.ShortName;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 注册实现类
        /// </summary>
        /// <typeparam name="TFrom">抽象接口</typeparam>
        /// <typeparam name="TTo">具体实现类</typeparam>
        /// <param name="shortName"></param>
        public void Register<TFrom, TTo>(string shortName = null, object[] paraList = null, LifeTimeType lifeTimeType = LifeTimeType.Transient) where TTo : TFrom
        {
            this.ContainerDictionary.Add(this.GetKey(typeof(TFrom).FullName, shortName), new ContainerRegistModel()
            {
                TargetType = typeof(TTo),
                LifeTime = lifeTimeType
            });
            if (paraList != null && paraList.Length > 0)
            {
                this.ParaListDictionary.Add(this.GetKey(typeof(TFrom).FullName, shortName), paraList);
            }
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="TFrom">抽象接口</typeparam>
        /// <param name="shortName">具体实现类的shortName</param>
        /// <returns></returns>
        public TFrom Resolve<TFrom>(string shortName = null)
        {
            return (TFrom)this.ResolveObject(typeof(TFrom), shortName);
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="abstractType">抽象接口的Type</param>
        /// <param name="shortName">具体实现类的shortName</param>
        /// <returns></returns>
        private object ResolveObject(Type abstractType, string shortName = null)
        {
            string key = this.GetKey(abstractType.FullName, shortName);
            var containerRegistModel = this.ContainerDictionary[key];
            Type type = containerRegistModel.TargetType;

            #region 生命周期管理
            switch (containerRegistModel.LifeTime)
            {
                case LifeTimeType.Transient:
                    Console.WriteLine("do nothing before");
                    break;
                case LifeTimeType.Singleton:
                    if (containerRegistModel.SingletonInstance == null)
                    {
                        break;
                    }
                    else
                    {
                        return containerRegistModel.SingletonInstance;
                    }
                case LifeTimeType.Scope:
                    if (this.ScopeDictionaty.ContainsKey(key))
                    {
                        return this.ScopeDictionaty[key];
                    }
                    else
                    {
                        break;
                    }
                case LifeTimeType.PerThread:
                    object threadObj = CusCallContext.GetData($"{key}{Thread.CurrentThread.ManagedThreadId}");
                    if (threadObj == null)
                    {
                        break;
                    }
                    else
                    {
                        return threadObj;
                    }
                default:
                    break;
            }
            #endregion

            #region 选择合适的构造函数
            ConstructorInfo ctor = null;
            // 1 标记特性优先
            ctor = type.GetConstructors().FirstOrDefault(c => c.IsDefined(typeof(ConstructorInjectionAttribute), true));
            if (ctor == null)
            {
                // 2 参数个数最多
                ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
            }
            #endregion

            #region 准备构造函数的参数
            List<object> paraList = new List<object>();
            object[] paraConstant = this.ParaListDictionary.ContainsKey(key) ? this.ParaListDictionary[key] : null;
            int idx = 0;
            int parameterLen = ctor.GetParameters().Where(c => c.IsDefined(typeof(ParameterConstantAttribute), true)).Count();
            if (paraConstant != null && paraConstant.Length != parameterLen)
            {
                // 传入的常量参数值与定义的常量参数个数不一致
                throw new Exception(message: $"传入的常量参数值个数与定义的常量参数个数不一致@{key}");
            }

            foreach (var para in ctor.GetParameters())
            {
                Type paraType = para.ParameterType;//获取参数的类型
                if (para.IsDefined(typeof(ParameterConstantAttribute), true))
                {
                    object objItem = paraConstant[idx];
                    Type objItemType = objItem.GetType();
                    if (paraType != objItemType)
                    {
                        //传入的参数值类型与定义的参数类型不一致
                        throw new Exception("传入的参数值类型与定义的参数类型不一致");
                    }
                    paraList.Add(objItem);
                    idx++;
                }
                else
                {
                    string paraShortName = this.GetShortName(para);
                    object paraInstance = this.ResolveObject(paraType, paraShortName);
                    paraList.Add(paraInstance);
                }
            }
            object oInstance = Activator.CreateInstance(type, paraList.ToArray());
            #endregion


            #region 属性注入
            foreach (var prop in type.GetProperties().Where(p => p.IsDefined(typeof(PropertyInjectionAttribute), true)))
            {
                Type propType = prop.PropertyType;
                object propInstance = this.ResolveObject(propType, this.GetShortName(prop));
                prop.SetValue(oInstance, propInstance);
            }
            #endregion

            #region 方法注入
            foreach (var method in type.GetMethods().Where(p => p.IsDefined(typeof(MethodInjectionAttribute), true)))
            {
                List<object> methodParaList = new List<object>();
                foreach (var methodPara in method.GetParameters())
                {
                    Type methodParaType = methodPara.ParameterType;//获取方法的参数类型
                    object methodParaInstance = this.ResolveObject(methodParaType);
                    methodParaList.Add(methodParaInstance);
                }
                method.Invoke(oInstance, methodParaList.ToArray());
            }
            #endregion

            #region 生命周期管理
            switch (containerRegistModel.LifeTime)
            {
                case LifeTimeType.Transient:
                    Console.WriteLine("do nothing after");
                    break;
                case LifeTimeType.Singleton:
                    containerRegistModel.SingletonInstance = oInstance;
                    break;
                case LifeTimeType.Scope:
                    this.ScopeDictionaty[key] = oInstance;
                    break;
                case LifeTimeType.PerThread:
                    CusCallContext.SetData($"{key}{Thread.CurrentThread.ManagedThreadId}", oInstance);
                    break;
                default:
                    break;
            }
            #endregion

            return oInstance.AOP(abstractType);
        }
        public ChunlaiContainer()
        {

        }
        private ChunlaiContainer(Dictionary<string, ContainerRegistModel> containerdic,
            Dictionary<string, object[]> paralistDic,
            Dictionary<string, object> scopeDic)
        {
            this.ContainerDictionary = containerdic;
            this.ParaListDictionary = paralistDic;
            this.ScopeDictionaty = scopeDic;
        }
        public IChunlaiContainer NewSubContainer()
        {
            return new ChunlaiContainer(this.ContainerDictionary,
                this.ParaListDictionary,
                new Dictionary<string, object>());
        }
    }
}
