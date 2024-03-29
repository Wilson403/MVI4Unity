﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MVI4Unity
{

    public class Store<S> : IStore where S : AStateBase
    {
        private IReducer _reducer;

        /// <summary>
        /// 回调集合
        /// </summary>
        private readonly List<Action<S>> _callbackList = new List<Action<S>> ();

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
        /// <param name="callback"></param>
        public void Subscribe (Action<S> callback)
        {
            _callbackList.Add (callback);
        }

        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="tag"></param>
        private void TriggerCallback (Enum tag)
        {
            for ( int i = 0 ; i < _callbackList.Count ; i++ )
            {
                Action<S> callback = _callbackList [i];
                _currentState ??= Activator.CreateInstance<S> ();
                _currentState.currentFunTag = tag != default ? tag.GetHashCode () : _currentState.currentFunTag;
                callback?.Invoke (_currentState);
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

        /// <summary>
        /// 是否初始化了状态
        /// </summary>
        private bool _isInitState = false;

        /// <summary>
        /// 初始化状态
        /// </summary>
        public void InitState ()
        {
            if ( _isInitState )
            {
                return;
            }
            _isInitState = true;

            bool isForceInitState = true;
            //执行初始函数来初始化状态实例
            foreach ( var item in _reducer.GetFirstAutoExecuteDict () )
            {
                DisPatch (item.Key , default);
                if ( item.Value == ReducerExecuteType.Synchronize )
                {
                    isForceInitState = false;
                }
            }

            //如果一个初始函数为能及时提供状态刷新，则需要强制刷新一次状态
            if ( isForceInitState )
            {
                SetNewState (default , Activator.CreateInstance<S> ());
                return;
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy ()
        {
            var state = _currentState ?? Activator.CreateInstance<S> ();
            state.shouldDestroy = true;
            SetNewState (default , state);
        }

        #endregion
    }
}