using System;

namespace MVI4Unity
{
    [AttributeUsage (AttributeTargets.Method , AllowMultiple = false)]
    internal class ReducerMethodAttribute : Attribute
    {
        /// <summary>
        /// 方法标识
        /// </summary>
        public int methodTag;

        /// <summary>
        /// 首次自动执行
        /// </summary>
        public bool firstAutoExecute;

        public ReducerMethodAttribute (int funcTag , bool firstAutoExecute = false)
        {
            this.methodTag = funcTag;
            this.firstAutoExecute = firstAutoExecute;
        }
    }
}