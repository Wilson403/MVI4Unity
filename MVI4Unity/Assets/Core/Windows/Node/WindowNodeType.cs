using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public abstract class WindowNodeType
    {
        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract WindowNode CreateWindowNode (AStateBase state);

        /// <summary>
        /// 获取容器列表
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public abstract List<Transform> GetContainerList (AWindow window);

        /// <summary>
        /// 创建该节点类型的窗口
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public abstract AWindow CreateAWindow (Transform container);

        /// <summary>
        /// 获取资源标签
        /// </summary>
        /// <returns></returns>
        public abstract string GetResTag ();

        /// <summary>
        /// 填充属性
        /// </summary>
        /// <param name="window"></param>
        /// <param name="state"></param>
        /// <param name="store"></param>
        public abstract void FillProps (AWindow window , AStateBase state , IStore store);

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public WindowNode GetRoot (AStateBase state)
        {
            return CreateWindowNode (state);
        }
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class WindowNodeType<A, S> : WindowNodeType where A : AWindow where S : AStateBase
    {
        private readonly string _windowAssetPath;
        private readonly PoolType<A> _windowPool;
        private readonly Func<A , List<Transform>> _containerCreator;
        private readonly Func<S , List<List<WindowNode>>> _childNodeCreator;
        private readonly Action<S , A , IStore> _fillProps;

        /// <summary>
        /// 构造节点信息
        /// </summary>
        /// <param name="windowAssetPath"></param>
        /// <param name="fillProps"></param>
        /// <param name="containerCreator"></param>
        /// <param name="childNodeCreator"></param>
        /// <param name="windowPool"></param>
        public WindowNodeType (
            string windowAssetPath ,
            Action<S , A , IStore> fillProps ,
            Func<A , List<Transform>> containerCreator = default ,
            Func<S , List<List<WindowNode>>> childNodeCreator = default ,
            PoolType<A> windowPool = null)
        {
            _windowAssetPath = windowAssetPath;
            _windowPool = windowPool;
            _containerCreator = containerCreator;
            _childNodeCreator = childNodeCreator;
            _fillProps = fillProps;
        }

        public override AWindow CreateAWindow (Transform container)
        {
            AWindow window = ( _windowPool ?? PoolMgr.Ins.GetAWindowPool<A> (_windowAssetPath) ).Pop ();
            window.SetParent (container);
            return window;
        }

        public override WindowNode CreateWindowNode (AStateBase state)
        {
            WindowNode node = PoolMgr.Ins.GetWindowNodePool ().Pop ();
            node.SetWindowNodeType (this);
            node.childNodeGroup.Clear ();
            node.childNodeGroup.AddRange (_childNodeCreator == null ? PoolMgr.Ins.GetList<List<WindowNode>> ().Pop () : _childNodeCreator.Invoke (state as S));
            return node;
        }

        public override List<Transform> GetContainerList (AWindow window)
        {
            if ( _containerCreator == null )
            {
                return PoolMgr.Ins.GetList<Transform> ().Pop ();
            }
            return _containerCreator.Invoke (window as A);
        }

        public override void FillProps (AWindow window , AStateBase state , IStore store)
        {
            window.RemoveAllListeners ();
            _fillProps?.Invoke (state as S , window as A , store);
        }

        public override string GetResTag ()
        {
            return _windowAssetPath;
        }
    }
}