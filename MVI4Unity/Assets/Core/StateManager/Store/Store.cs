using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;

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

    public class Store<S> : IStore where S : AStateBase
    {
        private IReducer _reducer;

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
        public delegate Task<S> AsyncReducer (S lastState , object @param);

        /// <summary>
        /// 回调委托
        /// </summary>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <param name="setNewState"></param>
        public delegate void CallbackReducer (S lastState , object @param , Action<S> setNewState);

        /// <summary>
        /// 添加Reducer
        /// </summary>
        /// <param name="reducer"></param>
        public void AddReducer (IReducer reducer) 
        {
            _reducer = reducer;
        }

        async public void DisPatch (Enum tag , object @param)
        {
            ReducerExecuteType funcType = _reducer.GetReducerExecuteType (tag);
            switch ( funcType )
            {
                case ReducerExecuteType.Synchronize:
                    {
                        S lastState = GetCurrentState ();
                        S newState = _reducer.Execute (tag , lastState , param) as S;
                        SetNewState (tag , newState);
                    }
                    break;

                case ReducerExecuteType.Async:
                    {
                        S lastState = GetCurrentState ();
                        S newState = await _reducer.AsyncExecute (tag , lastState , param) as S;
                        SetNewState (tag , newState);
                    }
                    break;

                case ReducerExecuteType.CallBack:
                    {
                        S lastState = GetCurrentState ();
                        _reducer.ExecuteCallback (tag , lastState , param , (newState) =>
                        {
                            SetNewState (tag , newState as S);
                        });
                    }
                    break;

                default:
                    Debug.LogError ($"Not Permission:[{funcType}]");
                    break;
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="callback"></param>
        public void Subscribe (Enum tag , Action<S> callback)
        {
            CallBackWarpper<S> callBackWarpper = new CallBackWarpper<S> ();
            callBackWarpper.callback = callback;
            callBackWarpper.tag = tag;
            _callbackList.Add (callBackWarpper);
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="callback"></param>
        public void Subscribe (Action<S> callback)
        {
            CallBackWarpper<S> callBackWarpper = new CallBackWarpper<S> ();
            callBackWarpper.callback = callback;
            _callbackList.Add (callBackWarpper);
        }

        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="tag"></param>
        private void TriggerCallback (Enum tag)
        {
            for ( int i = 0 ; i < _callbackList.Count ; i++ )
            {
                CallBackWarpper<S> callBackWarpper = _callbackList [i];
                if ( callBackWarpper.tag == null || tag.GetHashCode () == callBackWarpper.tag.GetHashCode () )
                {
                    callBackWarpper.callback?.Invoke (_currentState);
                }
            }
        }

        #region State管理

        const int MAX_STATE_COUNT = 20;
        readonly Queue<S> _cache4State = new Queue<S> (MAX_STATE_COUNT);
        S _currentState;

        /// <summary>
        /// 设置新状态
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="newState"></param>
        private void SetNewState (Enum tag , S newState)
        {
            if ( _cache4State.Count >= MAX_STATE_COUNT )
            {
                _cache4State.Dequeue ();
            }
            _cache4State.Enqueue (newState);
            _currentState = newState;
            TriggerCallback (tag);
        }

        /// <summary>
        /// 获取当前的State
        /// </summary>
        /// <returns></returns>
        private S GetCurrentState ()
        {
            return _currentState;
        }

        #endregion
    }
}