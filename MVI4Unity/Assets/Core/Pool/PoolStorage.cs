using System.Collections.Generic;

namespace MVI4Unity
{
    public class PoolStorage<T> : IPoolStorage
    {
        private readonly List<object> _storage = new List<object> ();
        private readonly PoolType<T> _poolType;

        public PoolStorage (PoolType<T> poolType)
        {
            _poolType = poolType;
        }

        public int GetStoragedCount ()
        {
            return _storage.Count;
        }

        public object Pop ()
        {
            var item = _storage [_storage.Count - 1];
            _storage.RemoveAt (_storage.Count - 1);
            _poolType.onPop?.Invoke ();
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

            _storage.Add (item);
            _poolType.onPush?.Invoke (( T ) item);
        }
    }
}