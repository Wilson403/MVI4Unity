using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class StartDemo : MonoBehaviour
    {
        private void Awake ()
        {
            WindowNodeType<WindowItem , State01> item = new WindowNodeType<WindowItem , State01> ("WindownItem" ,
                childCreator: default,
                fillProps: (state , window , store) =>
                {
                    store.Subscribe ((s) =>
                    {

                    });
                });

            WindowNodeType<Window01 , State01> root = new WindowNodeType<Window01 , State01> ("Windown01" ,
                childCreator: (state , window) =>
                {
                    List<ChildNodeVo> childNodeVos = PoolMgr.Ins.GetList<ChildNodeVo> ().Pop ();
                    List<WindowNode> windowNodes = PoolMgr.Ins.GetList<WindowNode> ().Pop ();

                    for ( int i = 0 ; i < 10 ; i++ )
                    {
                        windowNodes.Add (item.CreateWindowNode ());
                    }

                    childNodeVos.Add (new ChildNodeVo ()
                    {
                        container = window.GameObject.transform ,
                        allNodeList = windowNodes
                    });
                    return childNodeVos;
                } ,
                fillProps: (state , window , store) =>
                {
                    store.Subscribe ((s) =>
                    {

                    });
                });

            UIWinMgr.Ins.CreateRootNodeContainer (transform , new AWindowData ()
            {
                component = root
            });

            //Store<State01> store = SimpleStoreFactory.Ins.CreateStore<State01 , Reducer01> ();
            //store.Subscribe (Reducer01.Reducer01MethodType.Func01 , (State01 s) =>
            //{
            //    Debug.LogWarning ("State01");
            //});

            //store.Subscribe (Reducer01.Reducer01MethodType.Func02 , (s) =>
            //{
            //    Debug.LogWarning ("State02");
            //});

            //store.Subscribe (Reducer01.Reducer01MethodType.Func03 , (s) =>
            //{
            //    Debug.LogWarning ("State03");
            //});

            //store.DisPatch (Reducer01.Reducer01MethodType.Func01 , default);
            //store.DisPatch (Reducer01.Reducer01MethodType.Func02 , default);
            //store.DisPatch (Reducer01.Reducer01MethodType.Func03 , default);

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