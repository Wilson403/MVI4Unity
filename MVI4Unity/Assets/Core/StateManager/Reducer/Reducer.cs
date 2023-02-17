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
        CallBack = 3,
    }

    /// <summary>
    /// 内部通用函数
    /// </summary>
    public enum ReducerCommonFunType
    {
        Close = 999999999,
    }

    /// <summary>
    /// Reducer: 无状态设计，相当于一个函数容器，同类型的Reducer整个生命周期只需创建一次
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="E"></typeparam>
    public abstract class Reducer<S, E> : IReducer where S : AStateBase where E : Enum
    {
        private readonly Dictionary<string , Store<S>.Reducer> _tag2Method = new Dictionary<string , Store<S>.Reducer> ();
        private readonly Dictionary<string , Store<S>.AsyncReducer> _tag2AsyncMethod = new Dictionary<string , Store<S>.AsyncReducer> ();
        private readonly Dictionary<string , Store<S>.CallbackReducer> _tag2Callback = new Dictionary<string , Store<S>.CallbackReducer> ();
        private readonly List<Enum> _firstAutoExecuteList = new List<Enum> ();

        public Reducer ()
        {
            RegisterFunc ();
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        private void RegisterFunc ()
        {
            //通过反射的形式添加派生类的函数
            MethodInfo [] methods = GetType ().GetMethods (BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for ( int i = 0 ; i < methods.Length ; i++ )
            {
                MethodInfo method = methods [i];
                ReducerMethodAttribute attr = method.GetCustomAttribute<ReducerMethodAttribute> ();
                if ( attr != null )
                {
                    Enum tag = Enum.ToObject (typeof (E) , attr.methodTag) as Enum;

                    if ( attr.firstAutoExecute )
                        GetFirstAutoExecuteList ().Add (tag);

                    if ( UtilGeneral.Ins.Method2Delegate (method , this , out Store<S>.Reducer reducer) )
                        AddMethod (tag , reducer);

                    else if ( UtilGeneral.Ins.Method2Delegate (method , this , out Store<S>.AsyncReducer asyncReducer) )
                        AddAsyncMethod (tag , asyncReducer);

                    else if ( UtilGeneral.Ins.Method2Delegate (method , this , out Store<S>.CallbackReducer callback) )
                        AddCallBack (tag , callback);

                    else
                        Debug.LogError ($"[{attr.methodTag}] Register fail");
                }
            }

            //添加内部的函数
            AddMethod (ReducerCommonFunType.Close , Close);
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
        private void AddMethod (Enum tag , Store<S>.Reducer reducer)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Method.ContainsKey (@enum) )
            {
                Debug.LogError ($"Repeat key: [{@enum}]");
                return;
            }
            _tag2Method [@enum] = reducer;
        }

        /// <summary>
        /// 添加异步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="asyncReducer"></param>
        private void AddAsyncMethod (Enum tag , Store<S>.AsyncReducer asyncReducer)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2AsyncMethod.ContainsKey (@enum) )
            {
                Debug.LogError ($"Repeat key: [{@enum}]");
                return;
            }
            _tag2AsyncMethod [@enum] = asyncReducer;
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
            if ( _tag2Callback.TryGetValue (@enum , out Store<S>.CallbackReducer method) )
            {
                method?.Invoke (lastState as S , param , setNewState);
            }
        }

        async public Task<object> AsyncExecute (Enum tag , object lastState , object param)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2AsyncMethod.TryGetValue (@enum , out Store<S>.AsyncReducer method) )
            {
                return await method?.Invoke (lastState as S , param);
            }
            return null;
        }

        public object Execute (Enum tag , object lastState , object param)
        {
            string @enum = GetEnumName (tag);
            if ( _tag2Method.TryGetValue (@enum , out Store<S>.Reducer method) )
            {
                return method?.Invoke (lastState as S , param);
            }
            return null;
        }

        public List<Enum> GetFirstAutoExecuteList ()
        {
            return _firstAutoExecuteList;
        }

        public ReducerExecuteType GetReducerExecuteType (Enum tag)
        {
            var @enum = GetEnumName (tag);
            if ( _tag2Method.ContainsKey (@enum) )
            {
                return ReducerExecuteType.Synchronize;
            }
            else if ( _tag2AsyncMethod.ContainsKey (@enum) )
            {
                return ReducerExecuteType.Async;
            }
            else if ( _tag2Callback.ContainsKey (@enum) )
            {
                return ReducerExecuteType.CallBack;
            }
            return ReducerExecuteType.None;
        }

        #region 通用状态变更函数的注册

        [ReducerMethod (( int ) ReducerCommonFunType.Close)]
#pragma warning disable IDE0051
        S Close (S oldState , object @param)
        {
            oldState.shouldDestroy = true;
            return oldState;
        }

        #endregion
    }
}