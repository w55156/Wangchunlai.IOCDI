using Castle.DynamicProxy;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wangchunlai.IOCDI.BLL;
using Wangchunlai.IOCDI.DAL;
using Wangchunlai.IOCDI.Framework;
using Wangchunlai.IOCDI.Framework.CusAOP;
using Wangchunlai.IOCDI.IBLL;
using Wangchunlai.IOCDI.IDAL;
using Wangchunlai.IOCDI.IService;
using Wangchunlai.IOCDI.Service;

namespace Wangchunlai.IOCDI.ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IChunlaiContainer container = new ChunlaiContainer();
            #region
            {
                // 需求：上层仅依赖抽象就能完成对象的获取
                // 常规IOC容器：（第三方，与业务无关）　容器对象－注册－生成
                //container.Register<IUserServiceA, UserServiceA>();
                //container.Register<IUserServiceB, UserServiceB>(paraList:new object[] { "jack", 3 });
                //container.Register<IUserServiceC, UserServiceC>();
                //container.Register<IUserServiceD, UserServiceD>();
                //container.Register<IUserServiceE, UserServiceE>();
                //container.Register<IUserDal, UserDal>();
                //container.Register<IUserDal, UserDalMysql>("mysql");// 分开注册 单接口多实现
                //container.Register<IUserBll, UserBll>();

                //userBll.Login(); // 方法注入　初始化对象
                //IUserServiceB userServiceB = container.Resolve<IUserServiceB>();
                //IUserDal userDal = container.Resolve<IUserDal>("mysql");// 分开创建实例 单接口多实现

                //IUserBll userBll = container.Resolve<IUserBll>();
            }
            #endregion

            {
                // IOC容器　支持 多构造函数注入（选择超集）　不限层级注入
                // 其他注入方式（属性注入、方法注入）　
                // 单接口多实现
                // 如果传的参数是int 如 3
                // 为什么不用net core自带的　IServices 只支持构造函数注入
                // 对象生命周期管理
                // 瞬态模式（默认的，太简单了） Transient
                //container.Register<IUserServiceA, UserServiceA>();
                //IUserServiceA a1 = container.Resolve<IUserServiceA>();
                //IUserServiceA a2 = container.Resolve<IUserServiceA>();
                //Console.WriteLine(object.ReferenceEquals(a1, a2));
            }

            {// 单例模式　Singleton (容器级别，同一个容器)
                //container.Register<IUserServiceA, UserServiceA>(lifeTimeType:LifeTimeType.Singleton);
                //IUserServiceA a1 = container.Resolve<IUserServiceA>();
                //IUserServiceA a2 = container.Resolve<IUserServiceA>();
                //Console.WriteLine(object.ReferenceEquals(a1,a2));
            }

            {// 作用域模式　Scope（HTTP请求级别，同一个请求。子容器单例。）
                //container.Register<IUserServiceA, UserServiceA>(lifeTimeType: LifeTimeType.Scope);
                //IUserServiceA a1 = container.Resolve<IUserServiceA>();
                //IUserServiceA a2 = container.Resolve<IUserServiceA>();
                //Console.WriteLine(object.ReferenceEquals(a1, a2));

                //IChunlaiContainer container1 = container.NewSubContainer();
                //IUserServiceA a11 = container1.Resolve<IUserServiceA>();
                //IUserServiceA a12 = container1.Resolve<IUserServiceA>();
                //Console.WriteLine(object.ReferenceEquals(a11, a12));

                //IChunlaiContainer container2 = container.NewSubContainer();
                //IUserServiceA a21 = container2.Resolve<IUserServiceA>();
                //IUserServiceA a22 = container2.Resolve<IUserServiceA>();
                //Console.WriteLine(object.ReferenceEquals(a21, a22));


                //Console.WriteLine(object.ReferenceEquals(a11, a22));

            }

            {// 线程模式
                //container.Register<IUserServiceA, UserServiceA>(lifeTimeType: LifeTimeType.PerThread);
                //IUserServiceA a1 = container.Resolve<IUserServiceA>();
                //IUserServiceA a2 = container.Resolve<IUserServiceA>();
                //IUserServiceA a3 = null;
                //IUserServiceA a4 = null;
                //IUserServiceA a5 = null;
                //Task.Run(() =>
                //{
                //    Console.WriteLine($"This is {Thread.CurrentThread.ManagedThreadId} a3");
                //    a3 = container.Resolve<IUserServiceA>();
                //});
                //Task.Run(() =>
                //{
                //    Console.WriteLine($"This is {Thread.CurrentThread.ManagedThreadId} a4");
                //    a4 = container.Resolve<IUserServiceA>();
                //}).ContinueWith(t =>
                //{
                //    Console.WriteLine($"This is {Thread.CurrentThread.ManagedThreadId} a5");
                //    a5 = container.Resolve<IUserServiceA>();
                //});

                //Thread.Sleep(1000);

                //Console.WriteLine(object.ReferenceEquals(a1, a2));
                //Console.WriteLine(object.ReferenceEquals(a1, a3));
                //Console.WriteLine(object.ReferenceEquals(a1, a4));
                //Console.WriteLine(object.ReferenceEquals(a1, a5));

                //Console.WriteLine(object.ReferenceEquals(a3, a4));
                //Console.WriteLine(object.ReferenceEquals(a3, a5));
                //Console.WriteLine(object.ReferenceEquals(a4, a5));

            }

            {
                //AOP
                //类型注入，类里面需要定义虚方法
                //接口注入
                //ProxyGenerator generator = new ProxyGenerator();//实例化【代理类生成器】  
                //Interceptor interceptor = new Interceptor();//实例化【拦截器】  

                //使用【代理类生成器】创建Person对象，而不是使用new关键字来实例化  
                //TestInterceptor test = generator.CreateClassProxy<TestInterceptor>(interceptor);
                //Console.WriteLine("当前类型:{0},父类型:{1}", test.GetType(), test.GetType().BaseType);
                //Console.WriteLine();
                //test.MethodInterceptor();
                //Console.WriteLine();
                //test.NoInterceptor();
                //Console.WriteLine();
                //Console.ReadLine();
                container.Register<IUserServiceA, UserServiceA>(lifeTimeType:LifeTimeType.Singleton);
                IUserServiceA a1 = container.Resolve<IUserServiceA>();
                a1.Login();
            }
            Console.ReadKey();
        }
    }
}
