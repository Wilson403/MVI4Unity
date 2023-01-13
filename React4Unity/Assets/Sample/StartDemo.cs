using Hot;
using UnityEngine;

namespace MVI4Unity
{
    public class StartDemo : MonoBehaviour
    {
        private void Awake ()
        {
            UIWinMgr.Ins.Create<Window01> ("Windown01" , gameObject.transform);
        }
    }
}