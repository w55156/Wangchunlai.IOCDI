
using System;
using System.Collections.Generic;
using System.Text;
using Wangchunlai.IOCDI.Framework;
using Wangchunlai.IOCDI.IDAL;
using Wangchunlai.IOCDI.IService;

namespace Wangchunlai.IOCDI.DAL
{
    public class UserDalMysql : IUserDal
    {
        private IUserServiceA _IUserServiceA=null;
        public UserDalMysql()
        {

        }
        //[ChunlaiConstructor]
        public UserDalMysql(IUserServiceA iUserServiceA)
        {
            _IUserServiceA = iUserServiceA;
        }

        public void Login()
        {
            //throw new NotImplementedException();
            //Console.WriteLine("user dal login");
            _IUserServiceA.Login();
        }
    }
}
