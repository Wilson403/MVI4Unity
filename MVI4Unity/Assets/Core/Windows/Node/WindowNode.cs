using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class WindowNode : IEquatable<WindowNode>
    {
        /// <summary>
        /// 子节点组
        /// </summary>
        public readonly List<List<WindowNode>> childNodeGroup;

        /// <summary>
        /// 该节点对应类型
        /// </summary>
        private WindowNodeType _windowNodeType;

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
            _windowNodeType = windowNodeType;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destory ()
        {
            //childNodes.Push ();
            if ( _window != null )
            {
                PoolMgr.Ins.PushAWindow (_window);
            }
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
            node._windowNodeType = _windowNodeType;
            node.id = id;
            node._window = _window;
        }

        public bool Equals (WindowNode other)
        {
            //判断依据：节点类型对应的窗口池相同，即都是同一个预制体
            return _windowNodeType.GetResTag ().Equals (other._windowNodeType.GetResTag ());
        }

        public override string ToString ()
        {
            //StringBuilder sb = new StringBuilder ();
            //JsonWriter jr = new JsonWriter (sb)
            //{
            //    PrettyPrint = true ,
            //    IndentValue = 4
            //};
            //JsonMapper.ToJson (this , jr);
            return $"Type:{_windowNodeType.GetType ()}\nresName:{_windowNodeType.GetResTag ()}";
        }

        #region 封装来自WindowNodeType的函数，而不是直接调用

        public AWindow CreateAWindow (Transform container)
        {
            _window = _windowNodeType.CreateAWindow (container);
            return _window;
        }

        public void FillProps (AWindow window , AStateBase state , IStore store)
        {
            _windowNodeType.FillProps (window , state , store);
        }

        public List<Transform> GetContainerList (AWindow window)
        {
            return _windowNodeType.GetContainerList (window);
        }

        #endregion
    }
}