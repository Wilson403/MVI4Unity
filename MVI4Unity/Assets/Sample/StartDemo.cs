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
                containerCreator: (window) =>
                {
                    List<Transform> containerList = PoolMgr.Ins.GetList<Transform> ().Pop ();
                    containerList.Add (window.container1);
                    containerList.Add (window.container2);
                    return containerList;
                } ,
                childNodeCreator: (state) =>
                {
                    List<List<WindowNode>> childNodeGroup = PoolMgr.Ins.GetList<List<WindowNode>> ().Pop ();
                    List<WindowNode> childNodeList1 = PoolMgr.Ins.GetList<WindowNode> ().Pop ();
                    List<WindowNode> childNodeList2 = PoolMgr.Ins.GetList<WindowNode> ().Pop ();

                    for ( int i = 0 ; i < state.count ; i++ )
                    {
                        childNodeList1.Add (item.CreateWindowNode (state));
                        childNodeList2.Add (item.CreateWindowNode (state));
                    }

                    childNodeGroup.Add (childNodeList1);
                    childNodeGroup.Add (childNodeList2);

                    return childNodeGroup;
                } ,
                fillProps: (state , window , store) =>
                {
                    window.btn.onClick.RemoveAllListeners ();
                    window.btn.onClick.AddListener (() => { store.DisPatch (Reducer01.Reducer01MethodType.Func01 , default); });
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