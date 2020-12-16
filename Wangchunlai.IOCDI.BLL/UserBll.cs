
using Wangchunlai.IOCDI.Framework;
using Wangchunlai.IOCDI.IBLL;
using Wangchunlai.IOCDI.IDAL;

namespace Wangchunlai.IOCDI.BLL
{
    public class UserBll : IUserBll
    {
        private IUserDal _iUserDal=null;
       /// <summary>
       /// 属性注入演示
       /// </summary>
        [PropertyInjection]
        public IUserDal UserDal { get; set; }
        public IUserDal UserDalMysql { get; set; }
        /// <summary>
        /// 属性注入演示
        /// 参数别名标记演示
        /// </summary>
        [PropertyInjection]
        [ParameterShortName("mysql")]
        public IUserDal UserDalMysql2 { get; set; }
        /// <summary>
        /// 构造函数参数别名演示
        /// </summary>
        /// <param name="iUserDal"></param>
        public UserBll([ParameterShortName("mysql")]IUserDal iUserDal)
        {
            //_IUserDal = iUserDal;
            this.UserDalMysql= iUserDal;
        }
        public void Login()
        {
            //throw new NotImplementedException();
            //Console.WriteLine("use bll login");
            //IUserDal.Login();
        }
    }
}
