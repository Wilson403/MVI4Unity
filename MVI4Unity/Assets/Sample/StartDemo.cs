using UnityEngine;

namespace MVI4Unity
{
    public class StartDemo : MonoBehaviour
    {
        private void Awake ()
        {
            Store<State01> store = SimpleStoreFactory.Ins.CreateStore<State01 , Reducer01> ();
            store.Subscribe (Reducer01.Reducer01MethodType.Func01 , (State01 s) =>
            {
                Debug.LogWarning ("State01");
            });

            store.Subscribe (Reducer01.Reducer01MethodType.Func02 , (s) =>
            {
                Debug.LogWarning ("State02");
            });

            store.Subscribe (Reducer01.Reducer01MethodType.Func03 , (s) =>
            {
                Debug.LogWarning ("State03");
            });

            store.DisPatch (Reducer01.Reducer01MethodType.Func01 , default);
            store.DisPatch (Reducer01.Reducer01MethodType.Func02 , default);
            store.DisPatch (Reducer01.Reducer01MethodType.Func03 , default);

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