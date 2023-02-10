using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 子节点信息
    /// </summary>
    public struct ChildNodeVo
    {
        /// <summary>
        /// 子节点们的容器
        /// </summary>
        public Transform container;

        /// <summary>
        /// 全部子节点
        /// </summary>
        public List<WindowNode> allNodeList;
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public abstract class WindowNodeType
    {
        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <returns></returns>
        public abstract WindowNode CreateWindowNode ();

        /// <summary>
        /// 获取子节点信息列表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public abstract List<ChildNodeVo> GetChildNodeList (AStateBase state , AWindow window);

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
        public abstract void FillProps (AWindow window , AStateBase state);

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <returns></returns>
        public WindowNode GetRoot ()
        {
            return CreateWindowNode ();
        }
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class WindowNodeType<A, S> : WindowNodeType where A : AWindow where S : AStateBase
    {
        /// <summary>
        /// 模块信息
        /// </summary>
        public struct ModuleVo
        {
            /// <summary>
            /// 包含的ID列表
            /// </summary>
            public int [] modules;

            /// <summary>
            /// 参数填充委托
            /// </summary>
            public Action<A , S> fillProps;
        }

        /// <summary>
        /// 用于创建子节点的委托
        /// </summary>
        /// <param name="state"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public delegate List<ChildNodeVo> ChildCreator (S state , A window);

        private readonly string _windowAssetPath;
        private readonly PoolType<A> _windowPool;
        private readonly ChildCreator _childCreator;
        private readonly Action<S , A> _fillProps;

        /// <summary>
        /// 构造节点信息
        /// </summary>
        /// <param name="windowAssetPath"></param>
        /// <param name="fillProps"></param>
        /// <param name="childCreator"></param>
        /// <param name="windowPool"></param>
        public WindowNodeType (string windowAssetPath , Action<S , A> fillProps , ChildCreator childCreator = default , PoolType<A> windowPool = null)
        {
            _windowAssetPath = windowAssetPath;
            _windowPool = windowPool;
            _childCreator = childCreator;
            _fillProps = fillProps;
        }

        public override AWindow CreateAWindow (Transform container)
        {
            A window = ( _windowPool ?? PoolMgr.Ins.GetAWindowPool<A> (_windowAssetPath) ).Pop ();
            window.SetParent (container);
            return window;
        }

        public override WindowNode CreateWindowNode ()
        {
            WindowNode node = PoolMgr.Ins.GetWindowNodePool ().Pop ();
            node.windowNodeType = this;
            return node;
        }

        public override List<ChildNodeVo> GetChildNodeList (AStateBase state , AWindow window)
        {
            if ( _childCreator == null )
            {
                return PoolMgr.Ins.GetList<ChildNodeVo> ().Pop ();
            }
            return _childCreator.Invoke (state as S , window as A);
        }

        public override void FillProps (AWindow window , AStateBase state)
        {
            _fillProps?.Invoke (state as S , window as A);
        }

        public override string GetResTag ()
        {
            return _windowAssetPath;
        }
    }
}