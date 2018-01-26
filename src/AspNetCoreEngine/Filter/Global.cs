using Engine.Core.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEngine.Filter
{
    public static class Global
    {
        /// <summary>
        /// 全局kernel
        /// </summary>
        public static IKernel Kernel { get; set; }
    }
}
