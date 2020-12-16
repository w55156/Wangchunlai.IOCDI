using System;
using System.Collections.Generic;
using System.Text;
using Wangchunlai.IOCDI.Framework;
using Wangchunlai.IOCDI.IService;

namespace Wangchunlai.IOCDI.Service
{
    public class UserServiceB :IUserServiceB
    {
        private IUserServiceA userServiceA;
        private int iIndex;
        //[PropertyInjection]
        public IUserServiceA UserServiceA { get; set; }
        public void Login()
        {
            //throw new NotImplementedException();
            Console.WriteLine("user service B login");
        }
        public UserServiceB([ParameterConstant]string _sIndex,IUserServiceA _iuserServiceA,[ParameterConstant]int _iIndex)
        {//常量参数标记特性演示
            this.userServiceA = _iuserServiceA;
            this.iIndex = _iIndex;

        }
        //[MethodInjection]
        public void Init(IUserServiceA _UserServiceA)
        {
            this.userServiceA = _UserServiceA;
        }
    }
}
