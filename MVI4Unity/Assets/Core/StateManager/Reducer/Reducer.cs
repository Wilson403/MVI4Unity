using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace MVI4Unity
{
    public enum ReducerExecuteType
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

    /// <summary>
    /// Reducer: 无状态设计，相当于一个函数容器，同类型的Reducer整个生命周期只需创建一次
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="E"></typeparam>
    public abstract class Reducer<S, E> : IReducer where S : AStateBase where E : Enum
    {
        private readonly Dictionary<string , Store<S>.Reducer> _tag2Func = new Dictionary<string , Store<S>.Reducer> ();
        private readonly Dictionary<string , Store<S>.AsyncReducer> _tag2AsyncFunc = new Dictionary<string , Store<S>.AsyncReducer> ();
        private readonly Dictionary<string , Store<S>.CallbackReducer> _tag2Callback = new Dictionary<string , Store<S>.CallbackReducer> ();

        public Reducer ()
        {
            RegisterFunc ();
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        private void RegisterFunc ()
        {
            MethodInfo [] methods = GetType ().GetMethods (BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for ( int i = 0 ; i < methods.Length ; i++ )
            {
                MethodInfo method = methods [i];
                ReducerFuncInfoAttribute attr = method.GetCustomAttribute<ReducerFuncInfoAttribute> ();
                if ( attr != null )
                {
                    Enum tag = Enum.ToObject (typeof (E) , attr.funcTag) as Enum;

                    if ( UtilGeneral.Ins.Method2Delegate (method , this , out Store<S>.Reducer reducer) )
                        AddFunc (tag , reducer);

                    else if ( UtilGeneral.Ins.Method2Delegate (method , this , out Store<S>.AsyncReducer asyncReducer) )
                        AddAsyncFunc (tag , asyncReducer);

                    else if ( UtilGeneral.Ins.Method2Delegate (method , this , out Store<S>.CallbackReducer callback) )
                        AddCallBack (tag , callback);

                    else
                        Debug.LogError ($"[{attr.funcTag}] Register fail");
                }
            }
        }

        private string GetEnumName (Enum tag)
        {
            return tag.ToString ();
        }

        /// <summary>
        /// 添加同步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="reducer"></param>
        private void AddFunc (Enum tag , Store<S>.Reducer reducer)
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
        /// 添加异步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="asyncReducer"></param>
        private void AddAsyncFunc (Enum tag , Store<S>.AsyncReducer asyncReducer)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2AsyncFunc.ContainsKey (@enum) )
            {
                Debug.LogError ($"Repeat key: [{@enum}]");
                return;
            }
            _tag2AsyncFunc [@enum] = asyncReducer;
        }

        /// <summary>
        /// 添加回调方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="asyncReducer"></param>
        private void AddCallBack (Enum tag , Store<S>.CallbackReducer asyncReducer)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Callback.ContainsKey (@enum) )
            {
                Debug.LogError ($"Repeat key: [{@enum}]");
                return;
            }
            _tag2Callback [@enum] = asyncReducer;
        }

        public void ExecuteCallback (Enum tag , object lastState , object param , Action<object> setNewState)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Callback.TryGetValue (@enum , out Store<S>.CallbackReducer func) )
            {
                func?.Invoke (lastState as S , param , setNewState);
            }
        }

        async public Task<object> AsyncExecute (Enum tag , object lastState , object param)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2AsyncFunc.TryGetValue (@enum , out Store<S>.AsyncReducer func) )
            {
                return await func?.Invoke (lastState as S , param);
            }
            return null;
        }

        public object Execute (Enum tag , object lastState , object param)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Func.TryGetValue (@enum , out Store<S>.Reducer func) )
            {
                return func?.Invoke (lastState as S , param);
            }
            return null;
        }

        public ReducerExecuteType GetReducerExecuteType (Enum tag)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Func.ContainsKey (@enum) )
            {
                return ReducerExecuteType.Synchronize;
            }
            else if ( _tag2AsyncFunc.ContainsKey (@enum) )
            {
                return ReducerExecuteType.Async;
            }
            else if ( _tag2Callback.ContainsKey (@enum) )
            {
                return ReducerExecuteType.CallBack;
            }
            return ReducerExecuteType.None;
        }
    }
}