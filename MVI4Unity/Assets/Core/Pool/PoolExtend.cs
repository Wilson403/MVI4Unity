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

        public static void Push<T1, T2> (this Dictionary<T1 , T2> poolType)
        {
            PoolMgr.Ins.GetDict<T1 , T2> ().Push (poolType);
        }
    }
}