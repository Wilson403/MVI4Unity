using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class WindowNode : IEquatable<WindowNode>, IPoolItem
    {
        /// <summary>
        /// 子节点组
        /// </summary>
        public List<List<WindowNode>> childNodeGroup;

        /// <summary>
        /// 该节点对应类型
        /// </summary>
        protected WindowNodeType windowNodeType;

        /// <summary>
        /// 该节点来源
        /// </summary>
        public WindowNode from;

        /// <summary>
        /// 该节点去向
        /// </summary>
        public WindowNode to;

        public int id;

        private AWindow _window;

        /// <summary>
        /// 对应的窗口
        /// </summary>
        public AWindow Window
        {
            get
            {
                return _window;
            }
        }

        public WindowNode ()
        {
            childNodeGroup = PoolMgr.Ins.GetList<List<WindowNode>> ().Pop ();
        }

        /// <summary>
        /// 清除记录
        /// </summary>
        public void ClearRecord ()
        {
            from = null;
            to = null;
        }

        /// <summary>
        /// 设置节点类型
        /// </summary>
        /// <param name="windowNodeType"></param>
        public void SetWindowNodeType (WindowNodeType windowNodeType)
        {
            this.windowNodeType = windowNodeType;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destory ()
        {
            PoolMgr.Ins.GetWindowNodePool ().Push (this);
        }

        /// <summary>
        /// 拷贝节点空壳
        /// </summary>
        public WindowNode CloneNodeShallow ()
        {
            WindowNode node = PoolMgr.Ins.GetWindowNodePool ().Pop ();
            for ( int i = 0 ; i < childNodeGroup.Count ; i++ )
            {
                node.childNodeGroup.Add (PoolMgr.Ins.GetList<WindowNode> ().Pop ());
            }
            CloneNodeProp (node);
            return node;
        }

        /// <summary>
        /// 拷贝节点参数
        /// </summary>
        /// <param name="node"></param>
        public void CloneNodeProp (WindowNode node)
        {
            node.windowNodeType = windowNodeType;
            node.id = id;
            node._window = _window;
        }

        public void OnPush ()
        {
            //回收对应的实体窗口
            if ( _window != null )
            {
                PoolMgr.Ins.PushAWindow (_window);
            }

            //回收每一个List<WindowNode>
            for ( int i = 0 ; i < childNodeGroup.Count ; i++ )
            {
                childNodeGroup [i].Push ();
            }

            //最后回收List<List<WindowNode>>
            childNodeGroup.Push ();
            childNodeGroup = null;
        }

        public void OnPop ()
        {
            childNodeGroup = childNodeGroup ?? PoolMgr.Ins.GetList<List<WindowNode>> ().Pop ();
        }

        public bool Equals (WindowNode other)
        {
            //判断依据：节点类型对应的窗口池相同，即都是同一个预制体
            return windowNodeType.GetResTag ().Equals (other.windowNodeType.GetResTag ());
        }

        public override string ToString ()
        {
            return $"Type:{windowNodeType.GetType ()}\nresName:{windowNodeType.GetResTag ()}";
        }

        #region 封装来自WindowNodeType的函数，而不是直接调用

        public AWindow CreateAWindow (Transform container)
        {
            _window = windowNodeType.CreateAWindow (container);
            return _window;
        }

        public virtual void FillProps (AWindow window , AStateBase state , IStore store)
        {
            windowNodeType.FillProps (window , state , store);
        }

        public List<Transform> GetContainerList (AWindow window)
        {
            return windowNodeType.GetContainerList (window);
        }

        #endregion
    }

    /// <summary>
    /// 带有属性的节点
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="P"></typeparam>
    public class WindowNode<A, S, P> : WindowNode where A : AWindow where S : AStateBase
    {
        public P prop;

        public override void FillProps (AWindow window , AStateBase state , IStore store)
        {
            if ( windowNodeType is WindowNodeType<A , S , P> realWindowNodeType )
            {
                realWindowNodeType.FillProps (window , state , store , prop);
                return;
            }
            base.FillProps (window , state , store);
        }
    }
}