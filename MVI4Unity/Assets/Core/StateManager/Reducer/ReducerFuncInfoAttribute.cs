using System;

namespace MVI4Unity
{
    [AttributeUsage (AttributeTargets.Method , AllowMultiple = false)]
    internal class ReducerFuncInfoAttribute : Attribute
    {
        /// <summary>
        /// 函数标识
        /// </summary>
        public int funcTag;

        /// <summary>
        /// 是否自动执行
        /// </summary>
        public bool autoExecute;

        public ReducerFuncInfoAttribute (int funcTag , bool autoExecute = false)
        {
            this.funcTag = funcTag;
            this.autoExecute = autoExecute;
        }
    }
}