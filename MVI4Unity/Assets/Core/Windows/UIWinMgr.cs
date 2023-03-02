using System;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 界面管理器
    /// </summary>
    public class UIWinMgr : SafeSingleton<UIWinMgr>
    {
        /// <summary>
        /// 创建视图
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public AWindow Create (Type type , GameObject prefab , Transform parent , object data = null)
        {
            GameObject go = UnityEngine.Object.Instantiate (prefab , parent);
            go.name = prefab.name;
            AWindow view = Activator.CreateInstance (type) as AWindow;
            view.SetGameObject (go , data);
            return view;
        }

        /// <summary>
        /// 创建视图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Create<T> (GameObject prefab , Transform parent , object data = null) where T : AWindow
        {
            AWindow view = Create (typeof (T) , prefab , parent , data);
            return view as T;
        }

        /// <summary>
        /// 创建视图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetPath"></param>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Create<T> (string assetPath , Transform parent , object data = null) where T : AWindow
        {
            GameObject prefab = Resources.Load<GameObject> (assetPath);
            if ( prefab == default )
            {
                Debug.LogError ($"assetPath[{assetPath}] 对应资源为空");
            }
            T window = Create<T> (prefab , parent , data);
            window.assetPath = assetPath;
            return window;
        }

        /// <summary>
        /// 打开某个界面
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="parent"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public RootNodeContainer<S , R> Open<S, R> (Transform parent , WindowNodeType component) where S : AStateBase where R : IReducer
        {
            return Create<RootNodeContainer<S , R>> ("RootNodeContainer" , parent , new RootNodeContainerData () { component = component });
        }

        /// <summary>
        /// 打开某个界面
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="parent"></param>
        /// <param name="component"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public RootNodeContainer<S , R> Open<S, R> (Transform parent , WindowNodeType component , object @param) where S : AStateBase where R : IReducer
        {
            return Create<RootNodeContainer<S , R>> ("RootNodeContainer" , parent , new RootNodeContainerData () { component = component , data = param });
        }
    }
}