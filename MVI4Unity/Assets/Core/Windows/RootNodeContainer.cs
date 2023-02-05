using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 根节点的容器
    /// </summary>
    public class RootNodeContainer : AWindow
    {
        private readonly List<WindowNode> _currentNodes = new List<WindowNode> ();

        protected override void OnInit ()
        {
            base.OnInit ();
            if ( data is AWindowData windowData )
            {
                Load (windowData.component.GetRoot ());
                return;
            }
            Debug.LogError ($"数据类型异常[{data}]");
        }

        private void Load (WindowNode root)
        {
            List<WindowNode> newNodes = PoolMgr.Ins.GetList<WindowNode> ().Pop (); //从池里获取一个列表
            newNodes.Add (root);
            WindowNodeDisputeResolver.Ins.ResolveDispute4List (_currentNodes , newNodes);
            newNodes.Push (); //列表用完回收
        }
    }
}