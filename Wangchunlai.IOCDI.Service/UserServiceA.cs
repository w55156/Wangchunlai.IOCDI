using System;
using System.Collections.Generic;
using System.Text;
using Wangchunlai.IOCDI.IService;
using static Wangchunlai.IOCDI.Framework.CusAOP.ContainerAopExtend;

namespace Wangchunlai.IOCDI.Service
{
    public class UserServiceA :IUserServiceA
    {
        public UserServiceA()
        {
            Console.WriteLine($"{this.GetType().Name}被构造……");
        }
        public void Login()
        {
            Console.WriteLine($"{this.GetType().Name}登录方法。");
        }
        public void Login1()
        {
            Console.WriteLine($"{this.GetType().Name}登录方法1。");
        }
    }
}
