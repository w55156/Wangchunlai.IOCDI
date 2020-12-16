using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Wangchunlai.IOCDI.Framework.CusAOP
{
    public static class ContainerAopExtend
    {
        public static object AOP(this object t, Type interfaceType)
        {
            ProxyGenerator generator = new ProxyGenerator();//实例化【代理类生成器】  
            IocInterceptor interceptor = new IocInterceptor();//实例化【拦截器】 
            t = generator.CreateInterfaceProxyWithTarget(interfaceType, t, interceptor);
            return t;
        }
        public abstract class BaseInterceptorAttribute : Attribute
        {
            public abstract Action Do(IInvocation invocation, Action action);
        }
        public class LogBeforeAttribute : BaseInterceptorAttribute
        {
            public override Action Do(IInvocation invocation, Action action)
            {
                return () =>
                {
                    Console.WriteLine($"This is LogBeforeAttribute1 {invocation.Method.Name} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
                    action.Invoke();
                    //Console.WriteLine($"This is LogBeforeAttribute2 {invocation.Method.Name} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}");
                };
            }
        }
        public class LogAfterAttribute : BaseInterceptorAttribute
        {
            public override Action Do(IInvocation invocation, Action action)
            {
                return () =>
                {
                    //Console.WriteLine($"This is LogAfterAttribute1 {DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    action.Invoke();
                    Console.WriteLine($"This is LogAfterAttribute2 {DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff")}");
                    
                };
            }
        }
        public class MonitorAttribute : BaseInterceptorAttribute
        {
            public override Action Do(IInvocation invocation, Action action)
            {
                return () =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    // Do
                    action.Invoke();
                    stopwatch.Stop();
                    Console.WriteLine($"本次方法所花费的时间：{stopwatch.ElapsedMilliseconds}ms");
                };
            }
        }
        public class IocInterceptor : StandardInterceptor
        {
            /// <summary>
            /// 调用前的拦截器
            /// </summary>
            /// <param name="invocation"></param>
            protected override void PreProceed(IInvocation invocation)
            {
                // Console.WriteLine("调用前的拦截器，方法名是：{0}。", invocation.Method.Name);// 方法名   获取当前成员的名称。 
            }
            /// <summary>
            /// 拦截的方法返回时调用的拦截器
            /// </summary>
            /// <param name="invocation"></param>
            protected override void PerformProceed(IInvocation invocation)
            {
                var method = invocation.Method;
                Action action = () => base.PerformProceed(invocation);
                if (method.IsDefined(typeof(BaseInterceptorAttribute), true))
                {
                    foreach (var attribute in method.GetCustomAttributes<BaseInterceptorAttribute>())
                    {
                      action=  attribute.Do(invocation, action);
                    }
                }
                action.Invoke();
                //base.PerformProceed(invocation);
                //Console.WriteLine("拦截的方法返回时调用的拦截器，方法名是：{0}。", invocation.Method.Name);
            }

            /// <summary>
            /// 调用后的拦截器
            /// </summary>
            /// <param name="invocation"></param>
            protected override void PostProceed(IInvocation invocation)
            {
                //Console.WriteLine("调用后的拦截器，方法名是：{0}。", invocation.Method.Name);
            }
        }
    }
}
