using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class StartDemo : MonoBehaviour
    {
        private void Awake ()
        {
            WindowNodeType<WindowItem , State01> item = new WindowNodeType<WindowItem , State01> ("WindownItem" ,
                fillProps: (state , window , store) =>
                {

                });

            WindowNodeType<Window01 , State01> root = new WindowNodeType<Window01 , State01> ("Windown01" ,
                childCreator: (state , window) =>
                {
                    List<ChildNodeVo> childNodeVos = PoolMgr.Ins.GetList<ChildNodeVo> ().Pop ();
                    List<WindowNode> windowNodes = PoolMgr.Ins.GetList<WindowNode> ().Pop ();

                    for ( int i = 0 ; i < state.count ; i++ )
                    {
                        windowNodes.Add (item.CreateWindowNode ());
                    }

                    childNodeVos.Add (new ChildNodeVo ()
                    {
                        container = window.container1 ,
                        allNodeList = windowNodes
                    });

                    childNodeVos.Add (new ChildNodeVo ()
                    {
                        container = window.container2 ,
                        allNodeList = windowNodes
                    });

                    return childNodeVos;
                } ,
                fillProps: (state , window , store) =>
                {
                    Debug.LogWarning ($"{state.currentFunTag}");
                    window.btn.onClick.AddListener (() => { store.DisPatch (Reducer01.Reducer01MethodType.Func02, default); });
                });

            UIWinMgr.Ins.CreateRootNodeContainer<State01 , Reducer01> (transform , new AWindowData ()
            {
                component = root ,
            });

            #region Pool Test

            //var windowItem = PoolMgr.Ins.PopAWindow<Window01> ("Windown01" , gameObject.transform);
            //PoolMgr.Ins.PushAWindow (windowItem);

            //var pool = PoolMgr.Ins.GetAWindowPool<Window01> ("Windown01" , gameObject.transform , onPop: () => { Debug.LogWarning ("弹出了"); } , onPush: (t) => { Debug.LogWarning ("回收了"); });
            //var windowItem = pool.Pop ();
            //pool.Push (windowItem);
            //pool.Pop ();

            #endregion
        }
    }
}