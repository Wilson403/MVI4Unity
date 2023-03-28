using UnityEngine;

namespace MVI4Unity.Sample
{
    public class StartDemo : MonoBehaviour
    {
        private void Awake ()
        {
            OpenViewMgr.Ins.ShowTip ("欢迎进入演示场景");
        }
    }
}