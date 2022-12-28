namespace React4Unity
{
    public interface IPoolStorage
    {
        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <returns></returns>
        object Pop ();

        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <param name="item"></param>
        void Push (object item);

        /// <summary>
        /// 获取存储数量
        /// </summary>
        /// <returns></returns>
        int GetStoragedCount ();
    }
}