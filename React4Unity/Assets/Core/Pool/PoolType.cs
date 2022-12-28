using System;

namespace React4Unity
{
    public interface IPoolType
    {

    }

    public class PoolType<T> : IPoolType
    {
        public readonly Func<T> onCreate;
        public readonly Action onPop;
        public readonly Action<T> onPush;

        public PoolType (Func<T> onCreate , Action onPop = null , Action<T> onPush = null)
        {
            this.onCreate = onCreate;
            this.onPop = onPop;
            this.onPush = onPush;
        }

        public T Create ()
        {
            return onCreate.Invoke ();
        }
    }
}