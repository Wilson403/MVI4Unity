﻿using System;
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
        /// 获取AWindow池
        /// </summary>
        public abstract IPoolType GetAWindowPool ();

        /// <summary>
        /// 创建该节点类型的窗口
        /// </summary>
        /// <returns></returns>
        public AWindow CreateAWindow ()
        {
            return PoolMgr.Ins.Pop<AWindow> (GetAWindowPool ());
        }

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
        public delegate List<ChildNodeVo> ChildCreator (AStateBase state , AWindow window);

        /// <summary>
        /// 用于创建子节点的委托
        /// </summary>
        /// <param name="state"></param>
        /// <param name="window"></param>
        /// <param name="store"></param>
        public delegate void FillProps (AStateBase state , AWindow window , Store<S> store);

        private readonly string _windowAssetPath;
        private readonly Transform _container;
        private readonly PoolType<A> _windowPool;
        private readonly ChildCreator _childCreator;
        private readonly FillProps _fillProps;

        /// <summary>
        /// 构造节点信息
        /// </summary>
        /// <param name="windowAssetPath">窗口路径</param>
        /// <param name="container">容器，窗口父节点</param>
        /// <param name="childCreator">子节点创建器</param>
        /// <param name="fillProps"></param>
        /// <param name="windowPool">可选，自定义窗口对象池</param>
        public WindowNodeType (string windowAssetPath , Transform container , ChildCreator childCreator , FillProps fillProps , PoolType<A> windowPool = null)
        {
            _windowAssetPath = windowAssetPath;
            _container = container;
            _windowPool = windowPool;
            _childCreator = childCreator;
            _fillProps = fillProps;
        }

        public override IPoolType GetAWindowPool ()
        {
            return _windowPool ?? PoolMgr.Ins.GetAWindowPool<A> (_windowAssetPath , _container);
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
            return _childCreator.Invoke (state , window);
        }
    }
}