using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 根节点的容器
    /// </summary>
    public class RootNodeContainer<S, R> : AWindow where S : AStateBase where R : IReducer
    {
        /// <summary>
        /// 当前节点列表
        /// </summary>
        private readonly List<WindowNode> _currentNodes = new List<WindowNode> ();

        protected override void OnInit ()
        {
            base.OnInit ();
            Store<S> store = SimpleStoreFactory.Ins.CreateStore<S , R> ();
            if ( data is AWindowData windowData )
            {
                store.Subscribe ((state) =>
                {
                    List<WindowNode> newNodeList = PoolMgr.Ins.GetList<WindowNode> ().Pop (); //从池里获取一个列表
                    newNodeList.Add (windowData.component.GetRoot (state));
                    WindowNodeDisputeResolver.Ins.ResolveDispute4List (GameObject.transform , state , store , _currentNodes , newNodeList);
                    newNodeList.Push (); //列表用完回收                    
                });
                store.InitState ();
                return;
            }
            Debug.LogError ($"数据类型异常[{data}]");
        }
    }
}