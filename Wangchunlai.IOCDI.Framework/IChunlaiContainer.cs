using System;
using System.Collections.Generic;
using System.Text;

namespace Wangchunlai.IOCDI.Framework
{
    public interface IChunlaiContainer
    {
        void Register<TFrom, TTo>(string shortName = null,
            object[] paraList = null,
            LifeTimeType lifeTimeType = LifeTimeType.Transient) where TTo : TFrom;
        TFrom Resolve<TFrom>(string shortName = null);
        IChunlaiContainer NewSubContainer();
    }
}
