using System;

namespace MVI4Unity
{
    [AttributeUsage (AttributeTargets.Field)]
    public class AWindowComAttribute : Attribute
    {
        /// <summary>
        /// 对应的标签名称
        /// </summary>
        public readonly string tag;
        public AWindowComAttribute (string tag)
        {
            this.tag = tag;
        }
    }
}