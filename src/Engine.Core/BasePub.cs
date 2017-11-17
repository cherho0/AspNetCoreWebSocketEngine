using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core
{
    /// <summary>
    /// 集线器基类
    /// </summary>
    public abstract class BasePub
    {
        /// <summary>
        /// 收到消息
        /// </summary>
        protected abstract void RecvMsg();
    }
}
