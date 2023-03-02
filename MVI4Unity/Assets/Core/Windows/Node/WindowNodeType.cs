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
        /// 创建一个节点
        /// </summary>
        /// <param name="state"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract WindowNode CreateWindowNode (AStateBase state , object @param);

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

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <param name="state"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public WindowNode GetRoot (AStateBase state , object @param)
        {
            return CreateWindowNode (state , @param);
        }
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="P"></typeparam>
    public class WindowNodeType<A, S, P> : WindowNodeType where A : AWindow where S : AStateBase
    {
        protected readonly string windowAssetPath;
        protected readonly PoolType<A> windowPool;
        protected readonly Func<A , List<Transform>> containerCreator;
        protected readonly Func<S , List<List<WindowNode>>> childNodeCreator;
        protected readonly Action<S , A , IStore , P> fillProps;

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
            Action<S , A , IStore , P> fillProps ,
            Func<A , List<Transform>> containerCreator = default ,
            Func<S , List<List<WindowNode>>> childNodeCreator = default ,
            PoolType<A> windowPool = null)
        {
            this.windowAssetPath = windowAssetPath;
            this.windowPool = windowPool;
            this.containerCreator = containerCreator;
            this.childNodeCreator = childNodeCreator;
            this.fillProps = fillProps;
        }

        public override AWindow CreateAWindow (Transform container)
        {
            AWindow window = ( windowPool ?? PoolMgr.Ins.GetAWindowPool<A> (windowAssetPath) ).Pop ();
            window.SetParent (container);
            window.GameObject.transform.localScale = Vector3.one;
            window.GameObject.transform.localPosition = new Vector3 (window.GameObject.transform.localScale.x , window.GameObject.transform.localScale.y , 0);
            return window;
        }

        public override WindowNode CreateWindowNode (AStateBase state)
        {
            return CreateWindowNode (state , default);
        }

        public override WindowNode CreateWindowNode (AStateBase state , object @param)
        {
            return CreateWindowNode (state , ( P ) param);
        }

        public WindowNode<A , S , P> CreateWindowNode (AStateBase state , P @param)
        {
            WindowNode<A , S , P> node = PoolMgr.Ins.GetWindowNodePool<A , S , P> ().Pop ();
            node.prop = param;
            node.SetWindowNodeType (this);
            node.childNodeGroup.Clear ();
            node.childNodeGroup.AddRange (childNodeCreator == null ? PoolMgr.Ins.GetList<List<WindowNode>> ().Pop () : childNodeCreator.Invoke (state as S));
            return node;
        }

        public override List<Transform> GetContainerList (AWindow window)
        {
            if ( containerCreator == null )
            {
                return PoolMgr.Ins.GetList<Transform> ().Pop ();
            }
            return containerCreator.Invoke (window as A);
        }

        public override void FillProps (AWindow window , AStateBase state , IStore store)
        {
            window.RemoveAllListeners ();
            fillProps?.Invoke (state as S , window as A , store , default);
        }

        public void FillProps (AWindow window , AStateBase state , IStore store , P @param)
        {
            window.RemoveAllListeners ();
            fillProps?.Invoke (state as S , window as A , store , param);
        }

        public override string GetResTag ()
        {
            return windowAssetPath;
        }
    }

    /// <summary>
    /// 只带默认参数的节点类型
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class WindowNodeType<A, S> : WindowNodeType<A , S , int> where A : AWindow where S : AStateBase
    {
        public WindowNodeType (string windowAssetPath ,
            Action<S , A , IStore , int> fillProps ,
            Func<A , List<Transform>> containerCreator = null ,
            Func<S , List<List<WindowNode>>> childNodeCreator = null ,
            PoolType<A> windowPool = null) : base (windowAssetPath , fillProps , containerCreator , childNodeCreator , windowPool)
        {

        }
    }
}