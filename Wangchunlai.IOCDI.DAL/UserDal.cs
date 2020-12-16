
using System;
using System.Collections.Generic;
using System.Text;
using Wangchunlai.IOCDI.Framework;
using Wangchunlai.IOCDI.IDAL;
using Wangchunlai.IOCDI.IService;

namespace Wangchunlai.IOCDI.DAL
{
    public class UserDal : IUserDal
    {
        private IUserServiceA _IUserServiceA=null;
        private IUserServiceB _IUserServiceB=null;
        public UserDal()
        {

        }
        //[ChunlaiConstructor]
        public UserDal(IUserServiceA iUserServiceA)
        {
            _IUserServiceA = iUserServiceA;
        }
        public UserDal(IUserServiceA iUserServiceA, IUserServiceB iUserServiceB)
        {
            _IUserServiceA = iUserServiceA;
            _IUserServiceB = iUserServiceB;
        }
        public void Login()
        {
            //throw new NotImplementedException();
            //Console.WriteLine("user dal login");
            _IUserServiceA.Login();
            _IUserServiceB.Login();
        }
    }
}
