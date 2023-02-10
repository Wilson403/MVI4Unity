using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 根节点的容器
    /// </summary>
    public class RootNodeContainer<S, R> : AWindow where S : AStateBase where R : IReducer
    {
        private readonly List<WindowNode> _currentNodes = new List<WindowNode> ();
        private AWindowData _windowData;
        private Store<S> _store;

        protected override void OnInit ()
        {
            base.OnInit ();
            _store = SimpleStoreFactory.Ins.CreateStore<S , R> ();
            if ( data is AWindowData windowData )
            {
                _windowData = windowData;
                Load (_windowData.component.GetRoot () , Activator.CreateInstance<S> ());
                _store.Subscribe ((s) =>
                {
                    Load (_windowData.component.GetRoot () , s);
                });
                return;
            }
            Debug.LogError ($"数据类型异常[{data}]");
        }

        private void Load (WindowNode root , AStateBase state)
        {
            List<WindowNode> newNodes = PoolMgr.Ins.GetList<WindowNode> ().Pop (); //从池里获取一个列表
            newNodes.Add (root);
            WindowNodeDisputeResolver.Ins.ResolveDispute4List (GameObject.transform , state , _currentNodes , newNodes);
            newNodes.Push (); //列表用完回收
        }
    }
}