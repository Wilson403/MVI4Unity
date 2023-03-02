using System;

namespace MVI4Unity
{
    public interface IPoolType
    {
        object Create ();
        string GetTag ();
    }

    public class PoolType<T> : IPoolType
    {
        public readonly Func<T> onCreate;
        public readonly Action<T> onPop;
        public readonly Action<T> onPush;
        public readonly string tag;

        public PoolType (Func<T> onCreate , Action<T> onPop = null , Action<T> onPush = null , string tag = null)
        {
            this.onCreate = onCreate;
            this.onPop = onPop;
            this.onPush = onPush;
            this.tag = tag;
        }

        public T Create ()
        {
            return onCreate.Invoke ();
        }

        public string GetTag ()
        {
            return tag;
        }

        object IPoolType.Create ()
        {
            return Create ();
        }
    }
}