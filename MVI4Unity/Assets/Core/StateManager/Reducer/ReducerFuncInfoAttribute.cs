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
        /// 是否自动执行
        /// </summary>
        public bool autoExecute;

        public ReducerMethodAttribute (int funcTag , bool autoExecute = false)
        {
            this.methodTag = funcTag;
            this.autoExecute = autoExecute;
        }
    }
}