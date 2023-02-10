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

        protected override void OnInit ()
        {
            base.OnInit ();
            Store<S> store = SimpleStoreFactory.Ins.CreateStore<S , R> ();
            if ( data is AWindowData windowData )
            {
                _windowData = windowData;
                store.Subscribe ((state) =>
                {
                    List<WindowNode> newNodes = PoolMgr.Ins.GetList<WindowNode> ().Pop (); //从池里获取一个列表
                    newNodes.Add (_windowData.component.GetRoot ());
                    WindowNodeDisputeResolver.Ins.ResolveDispute4List (GameObject.transform , state , store , _currentNodes , newNodes);
                    newNodes.Push (); //列表用完回收
                });
                store.InitState ();
                return;
            }
            Debug.LogError ($"数据类型异常[{data}]");
        }
    }
}