using System;
using System.Collections.Generic;
using System.Text;
using static Wangchunlai.IOCDI.Framework.CusAOP.ContainerAopExtend;

namespace Wangchunlai.IOCDI.IService
{
    public interface IUserServiceA
    {
        [LogBefore]
        [LogAfter]
        [Monitor]
        void Login();
        void Login1();
    }
}
