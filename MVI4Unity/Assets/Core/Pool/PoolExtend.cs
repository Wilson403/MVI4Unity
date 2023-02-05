using System.Collections.Generic;

namespace MVI4Unity
{
    /// <summary>
    /// 针对Pool的扩展
    /// </summary>
    public static class PoolExtend
    {
        public static T Pop<T> (this PoolType<T> poolType)
        {
            return PoolMgr.Ins.Pop<T> (poolType);
        }

        public static void Push<T> (this PoolType<T> poolType , T item)
        {
            PoolMgr.Ins.Push<T> (poolType , item);
        }

        public static void Push<T> (this List<T> poolType)
        {
            PoolMgr.Ins.GetList<T> ().Push (poolType);
        }
    }
}