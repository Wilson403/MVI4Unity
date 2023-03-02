using System;

namespace MVI4Unity
{
    public interface IStore
    {
        /// <summary>
        /// 派发
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="param"></param>
        void DisPatch (Enum tag , object @param = null);
    }
}