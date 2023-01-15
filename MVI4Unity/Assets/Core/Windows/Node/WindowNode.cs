using System.Collections.Generic;

namespace MVI4Unity
{
    public class WindowNode
    {
        /// <summary>
        /// 子节点列表
        /// </summary>
        private readonly List<List<WindowNode>> _childNodes = new List<List<WindowNode>> ();
    }
}