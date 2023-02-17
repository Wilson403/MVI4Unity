using System;

namespace MVI4Unity
{
    public interface IPoolType
    {
        object Create ();
    }

    public class PoolType<T> : IPoolType
    {
        public readonly Func<T> onCreate;
        public readonly Action<T> onPop;
        public readonly Action<T> onPush;

        public PoolType (Func<T> onCreate , Action<T> onPop = null , Action<T> onPush = null)
        {
            this.onCreate = onCreate;
            this.onPop = onPop;
            this.onPush = onPush;
        }

        public T Create ()
        {
            return onCreate.Invoke ();
        }

        object IPoolType.Create ()
        {
            return Create ();
        }
    }
}