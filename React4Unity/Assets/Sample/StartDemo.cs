using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace React4Unity
{
    public class StartDemo : MonoBehaviour
    {
        public Button btn1;
        object obj1;
        int a = 1;

        private void Awake ()
        {
            List<List<string>> lists = new List<List<string>> ();
            for ( int i = 0 ; i < 10 ; i++ )
            {
                lists.Add (PoolMgr.Ins.PopList4String ()); 
            }

            for ( int i = 0 ; i < 5 ; i++ )
            {
                PoolMgr.Ins.PushList4String (lists [i]);
            }
        }

        private void Update ()
        {

        }
    }

    public class Test1
    {
        public int a = 1;
    }
}