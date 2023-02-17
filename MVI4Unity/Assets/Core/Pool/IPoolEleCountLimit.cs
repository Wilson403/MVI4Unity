namespace MVI4Unity
{
    public interface IPoolEleCountLimit
    {
        /// <summary>
        /// 获取池中元素数量的最大限制
        /// </summary>
        int GetPoolEleMaxCount ();

        /// <summary>
        /// 回收失败时的处理
        /// </summary>
        void OnPushFail ();
    }
}