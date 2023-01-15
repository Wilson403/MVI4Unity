using System;
using System.Collections.Generic;

namespace MVI4Unity
{
    /// <summary>
    /// 回调包装
    /// </summary>
    /// <typeparam name="S"></typeparam>
    public class CallBackWarpper<S> where S : AStateBase
    {
        public Enum tag;
        public Action<S> callback;
    }

    public abstract class Store<S> : IStore where S : AStateBase
    {
        private readonly Reducer _reducer;

        /// <summary>
        /// 回调集合
        /// </summary>
        private readonly List<CallBackWarpper<S>> _callbackList = new List<CallBackWarpper<S>> ();

        /// <summary>
        /// 同步委托
        /// </summary>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public delegate S Reducer (S lastState , object @param);

        /// <summary>
        /// 异步委托
        /// </summary>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public delegate S AsyncReducer (S lastState , object @param);

        public Store (Reducer reducer)
        {
            _reducer = reducer;
        }

        public void DisPatch ()
        {
            throw new NotImplementedException ();
        }

        public void Subscribe ()
        {
            throw new NotImplementedException ();
        }
    }
}