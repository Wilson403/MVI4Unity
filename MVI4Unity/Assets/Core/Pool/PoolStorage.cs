using System.Collections.Generic;

namespace MVI4Unity
{
    public class PoolStorage<T> : IPoolStorage
    {
        private readonly List<object> _storage = new List<object> ();
        private readonly PoolType<T> _poolType;

        public PoolStorage (IPoolType poolType)
        {
            _poolType = poolType as PoolType<T>;
        }

        public int GetStoragedCount ()
        {
            return _storage.Count;
        }

        public object Pop ()
        {
            object item = _storage [_storage.Count - 1];
            _storage.RemoveAt (_storage.Count - 1);
            _poolType.onPop?.Invoke (( T ) item);
            if ( item is IPoolItem poolItem )
            {
                poolItem.OnPop ();
            }
            return item;
        }

        public void Push (object item)
        {
            if ( item == null )
            {
                return;
            }

            if ( _storage.Contains (item) )
            {
                return;
            }

            //如果超出缓存的最大限制，不回收了
            if ( item is IPoolEleCountLimit limit && limit.GetPoolEleMaxCount () >= 0 && _storage.Count >= limit.GetPoolEleMaxCount () )
            {
                limit.OnPushFail ();
                return;
            }

            _storage.Add (item);
            _poolType.onPush?.Invoke (( T ) item);

            //执行回收单元自带的回调
            if ( item is IPoolItem poolItem )
            {
                poolItem.OnPush ();
            }
        }
    }
}