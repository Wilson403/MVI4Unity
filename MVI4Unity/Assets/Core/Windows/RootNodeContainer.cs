using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class RootNodeContainerData
    {
        public WindowNodeType component;
        public object data;
    }

    /// <summary>
    /// 根节点的容器
    /// </summary>
    public class RootNodeContainer<S, R> : AWindow where S : AStateBase where R : IReducer
    {
        /// <summary>
        /// 当前节点列表
        /// </summary>
        private readonly List<WindowNode> _currentNodes = new List<WindowNode> ();
        private Store<S> _store;
        private readonly Queue<S> _stateQueue = new Queue<S> ();
        private RootNodeContainerData _rootNodeContainerData;
        public event Action<AWindow> onDestroy;

        protected override void OnInit ()
        {
            base.OnInit ();
            _store = SimpleStoreFactory.Ins.CreateStore<S , R> ();
            _rootNodeContainerData = data as RootNodeContainerData;
            _store.Subscribe ((state) =>
            {
                if ( IsDestroyed () )
                {
                    return;
                }
                _stateQueue.Enqueue (state);
                ResolveDisputeQuene ();
            });
            _store.InitState ();
        }

        /// <summary>
        /// 是否等待
        /// </summary>
        bool wait = false;

        /// <summary>
        /// 解决冲突列表
        /// </summary>
        void ResolveDisputeQuene ()
        {
            if ( _stateQueue.Count > 0 && !wait )
            {
                wait = true;
                ResolveDispute (_stateQueue.Dequeue () , _rootNodeContainerData.component);
                wait = false;
                ResolveDisputeQuene ();
            }
        }

        /// <summary>
        /// 解决冲突
        /// </summary>
        /// <param name="state"></param>
        /// <param name="component"></param>
        void ResolveDispute (S state , WindowNodeType component)
        {
            List<WindowNode> newNodeList = PoolMgr.Ins.GetList<WindowNode> ().Pop (); //从池里获取一个列表
            WindowNode node = state.shouldDestroy ? default : _rootNodeContainerData.data != null ? component.GetRoot (state , _rootNodeContainerData.data) : component.GetRoot (state);
            newNodeList.Add (node);
            WindowNodeDisputeResolver.Ins.ResolveDispute4List (GameObject.transform , state , _store , _currentNodes , newNodeList);
            newNodeList.Push (); //列表用完回收           

            if ( state.shouldDestroy )
            {
                Destroy ();
                onDestroy?.Invoke (this);
                return;
            }

            if ( node != null && node.Window != null )
            {
                node.Window.SetDefaultTransProp ();
            }
        }

        /// <summary>
        /// 派发
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="param"></param>
        public void DisPatch (Enum tag , object @param = null)
        {
            _store?.DisPatch (tag , param);
        }
    }
}