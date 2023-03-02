using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class PoolMgr : SafeSingleton<PoolMgr>
    {
        private readonly Dictionary<Type , IPoolType> _type2PoolType = new Dictionary<Type , IPoolType> ();
        private readonly Dictionary<string , IPoolType> _str2PoolType = new Dictionary<string , IPoolType> ();
        private readonly Dictionary<IPoolType , IPoolStorage> _storageDict = new Dictionary<IPoolType , IPoolStorage> ();

        /// <summary>
        /// 池中弹出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolType"></param>
        /// <returns></returns> 
        public T Pop<T> (IPoolType poolType)
        {
            var storage = GetStorage<T> (poolType);
            if ( storage.GetStoragedCount () > 0 )
            {
                return ( T ) storage.Pop ();
            }
            else
            {
                return ( T ) poolType.Create ();
            }
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolType"></param>
        /// <param name="item"></param>
        public void Push<T> (IPoolType poolType , T item)
        {
            GetStorage<T> (poolType).Push (item);
        }

        /// <summary>
        /// 尝试获取一个对象池类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooltype"></param>
        /// <returns></returns>
        public bool TryGetPoolType<T> (out IPoolType pooltype)
        {
            return _type2PoolType.TryGetValue (typeof (T) , out pooltype);
        }

        /// <summary>
        /// 获取实际存储的仓库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolType"></param>
        /// <returns></returns>
        private IPoolStorage GetStorage<T> (IPoolType poolType)
        {
            if ( !_storageDict.ContainsKey (poolType) )
            {
                _storageDict.Add (poolType , new PoolStorage<T> (poolType));
            }
            return _storageDict [poolType];
        }

        #region 集合对象池，集合实例回收利用，进行Clear而不是重新New，减少GC，比如List，Stack等都是一个道理

        /// <summary>
        /// 针对List的池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public PoolType<List<T>> GetList<T> ()
        {
            if ( !_type2PoolType.ContainsKey (typeof (List<T>)) )
            {
                _type2PoolType [typeof (List<T>)] = new PoolType<List<T>> (
                    onCreate: () =>
                    {
                        return new List<T> (0);
                    } ,
                    onPush: (t) =>
                    {
                        t.Clear ();
                    });
            }
            return _type2PoolType [typeof (List<T>)] as PoolType<List<T>>;
        }

        /// <summary>
        /// 针对Dict的池
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public PoolType<Dictionary<T1 , T2>> GetDict<T1, T2> ()
        {
            if ( !_type2PoolType.ContainsKey (typeof (Dictionary<T1 , T2>)) )
            {
                _type2PoolType [typeof (Dictionary<T1 , T2>)] = new PoolType<Dictionary<T1 , T2>> (
                    onCreate: () =>
                    {
                        return new Dictionary<T1 , T2> ();
                    } ,
                    onPush: (t) =>
                    {
                        t.Clear ();
                    });
            }
            return _type2PoolType [typeof (Dictionary<T1 , T2>)] as PoolType<Dictionary<T1 , T2>>;
        }

        #endregion

        #region AWindow对象池

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowPath"></param>
        /// <returns></returns>
        public AWindow PopAWindow<T> (string windowPath) where T : AWindow
        {
            return GetAWindowPool<T> (windowPath).Pop ();
        }

        /// <summary>
        /// 回收窗口到池里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="window"></param>
        public void PushAWindow<T> (T window) where T : AWindow
        {
            if ( !string.IsNullOrEmpty (window.assetPath) && _str2PoolType.TryGetValue (window.assetPath , out IPoolType poolType) )
            {
                poolType.Push (window);
                return;
            }
            Debug.LogWarning ($"无法回收{window}");
        }

        /// <summary>
        /// 获取窗口对象池
        /// </summary>
        /// <typeparam name="T">该泛型类型被限制为AWindow</typeparam>
        /// <param name="windowPath">窗口预制体路径</param>
        /// <returns></returns>
        public PoolType<T> GetAWindowPool<T> (string windowPath) where T : AWindow
        {
            Type windowType = typeof (T);
            if ( !_str2PoolType.ContainsKey (windowPath) || _str2PoolType [windowPath].GetTag () != windowPath )
            {
                _str2PoolType [windowPath] = new PoolType<T> (
                    onCreate: () =>
                    {
                        return UIWinMgr.Ins.Create<T> (windowPath , null);
                    } ,
                    onPop: PoolRealContainer4AWindow.Ins.Pop ,
                    onPush: PoolRealContainer4AWindow.Ins.Push);
            }
            return _str2PoolType [windowPath] as PoolType<T>;
        }

        /// <summary>
        /// 获取窗口对象池
        /// </summary>
        /// <typeparam name="T">该泛型类型被限制为AWindow</typeparam>
        /// <param name="windowPath">窗口预制体路径</param>
        /// <param name="onCreate">创建方式（替换默认方式</param>
        /// <param name="onPop">弹出监听</param>
        /// <param name="onPush">回收监听</param>
        /// <returns></returns>
        public PoolType<T> GetAWindowPool<T> (string windowPath , Func<T> onCreate = null , Action onPop = null , Action<T> onPush = null) where T : AWindow
        {
            Type windowType = typeof (T);
            onCreate = onCreate ?? ( () => { return UIWinMgr.Ins.Create<T> (windowPath , null); } );
            if ( !_str2PoolType.ContainsKey (windowPath) )
            {
                _str2PoolType [windowPath] = new PoolType<T> (
                    onCreate: onCreate ,
                    onPop: (t) =>
                    {
                        PoolRealContainer4AWindow.Ins.Pop (t);
                        onPop?.Invoke ();
                    } ,
                    onPush: (t) =>
                    {
                        PoolRealContainer4AWindow.Ins.Push (t);
                        onPush?.Invoke (t);
                    });
            }
            return _str2PoolType [windowPath] as PoolType<T>;
        }

        #endregion

        #region WindowNode对象池

        /// <summary>
        /// 获取窗口节点对象池
        /// </summary>
        /// <returns></returns>
        public PoolType<WindowNode> GetWindowNodePool ()
        {
            Type type = typeof (WindowNode);
            if ( !_type2PoolType.ContainsKey (typeof (WindowNode)) )
            {
                _type2PoolType [type] = new PoolType<WindowNode> (
                    onCreate: () => { return new WindowNode (); } ,
                    onPop: default ,
                    onPush: (node) =>
                    {
                        node.childNodeGroup.Clear ();
                    });
            }
            return _type2PoolType [type] as PoolType<WindowNode>;
        }

        /// <summary>
        /// 获取窗口节点对象池
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        public PoolType<WindowNode<A , S , P>> GetWindowNodePool<A, S, P> () where A : AWindow where S : AStateBase
        {
            Type type = typeof (WindowNode<A , S , P>);
            if ( !_type2PoolType.ContainsKey (typeof (WindowNode<A , S , P>)) )
            {
                _type2PoolType [type] = new PoolType<WindowNode<A , S , P>> (
                    onCreate: () => { return new WindowNode<A , S , P> (); } ,
                    onPop: default ,
                    onPush: (node) =>
                    {
                        node.childNodeGroup.Clear ();
                    });
            }
            return _type2PoolType [type] as PoolType<WindowNode<A , S , P>>;
        }

        #endregion
    }
}