using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public enum ReducerFuncType
    {
        None = 0,

        /// <summary>
        /// 同步
        /// </summary>
        Synchronize = 1,

        /// <summary>
        /// 异步
        /// </summary>
        Async = 2,

        /// <summary>
        /// 回调
        /// </summary>
        CallBack = 3
    }

    public abstract class Reducer<S> : IReducer where S : AStateBase
    {
        private readonly Dictionary<string , Store<S>.Reducer> _tag2Func = new Dictionary<string , Store<S>.Reducer> ();
        private readonly Dictionary<string , Store<S>.AsyncReducer> _tag2AsyncFunc = new Dictionary<string , Store<S>.AsyncReducer> ();

        public Reducer ()
        {
            RegisterFunc ();
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        protected abstract void RegisterFunc ();

        protected string GetEnumName (Enum tag)
        {
            return tag.ToString ();
        }

        /// <summary>
        /// 添加同步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="reducer"></param>
        protected void AddFunc (Enum tag , Store<S>.Reducer reducer)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Func.ContainsKey (@enum) )
            {
                Debug.LogError ($"Repeat key: [{@enum}]");
                return;
            }
            _tag2Func [@enum] = reducer;
        }

        /// <summary>
        /// 执行同步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public S Execute (Enum tag , S lastState , object @param)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Func.TryGetValue (@enum , out Store<S>.Reducer func) )
            {
                return func?.Invoke (lastState , param);
            }
            return null;
        }

        /// <summary>
        /// 获取函数类型
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ReducerFuncType GetReducerFuncType (Enum tag)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Func.ContainsKey (@enum) )
            {
                return ReducerFuncType.Synchronize;
            }
            return ReducerFuncType.None;
        }
    }
}