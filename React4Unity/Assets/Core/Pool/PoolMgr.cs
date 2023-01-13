using System;
using System.Collections.Generic;

namespace MVI4Unity
{
    public class PoolMgr : SafeSingleton<PoolMgr>
    {
        private readonly Dictionary<Type , IPoolType> _poolTypeDict = new Dictionary<Type , IPoolType> ();
        private readonly Dictionary<IPoolType , IPoolStorage> _storageDict = new Dictionary<IPoolType , IPoolStorage> ();

        public PoolMgr ()
        {
            _poolTypeDict [typeof (List<string>)] = new PoolType<List<string>> (onCreate: () => { return new List<string> (); } , onPush: (t) => { t.Clear (); });
        }

        /// <summary>
        /// 池中弹出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolType"></param>
        /// <returns></returns> 
        public T Pop<T> (PoolType<T> poolType)
        {
            var storage = GetStorage (poolType);
            if ( storage.GetStoragedCount () > 0 )
            {
                return ( T ) storage.Pop ();
            }
            else
            {
                return poolType.Create ();
            }
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolType"></param>
        /// <param name="item"></param>
        public void Push<T> (PoolType<T> poolType , T item)
        {
            GetStorage (poolType).Push (item);
        }

        /// <summary>
        /// 尝试获取一个对象池类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooltype"></param>
        /// <returns></returns>
        public bool TryGetPoolType<T> (out IPoolType pooltype)
        {
            return _poolTypeDict.TryGetValue (typeof (T) , out pooltype);
        }

        /// <summary>
        /// 获取实际存储的仓库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolType"></param>
        /// <returns></returns>
        private IPoolStorage GetStorage<T> (PoolType<T> poolType)
        {
            if ( !_storageDict.ContainsKey (poolType) )
            {
                _storageDict.Add (poolType , new PoolStorage<T> (poolType));
            }
            return _storageDict [poolType];
        }
    }
}