using UnityEngine;

namespace MVI4Unity
{
    public class StartDemo : MonoBehaviour
    {
        private void Awake ()
        {
            //var windowItem = PoolMgr.Ins.PopAWindow<Window01> ("Windown01" , gameObject.transform);
            //PoolMgr.Ins.PushAWindow (windowItem);

            //var pool = PoolMgr.Ins.GetAWindowPool<Window01> ("Windown01" , gameObject.transform , onPop: () => { Debug.LogWarning ("弹出了"); } , onPush: (t) => { Debug.LogWarning ("回收了"); });
            //var windowItem = pool.Pop ();
            //pool.Push (windowItem);
            //pool.Pop ();
        }
    }
}