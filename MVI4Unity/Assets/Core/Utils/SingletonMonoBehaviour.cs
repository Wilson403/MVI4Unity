using UnityEngine;

namespace Zero
{
    /// <summary>
    /// MonoBehaviour的单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        private static T _ins;

        /// <summary>
        /// 获得单例
        /// </summary>
        public static T Ins
        {
            get
            {
                CreateIns ();
                return _ins;
            }
        }

        public static void CreateIns () 
        {
            if ( null == _ins )
            {
                _ins = GameObject.FindObjectOfType<T> ();

                if ( null == _ins )
                {
                    GameObject go = new GameObject ();
                    go.name = typeof (T).Name;
                    go.AddComponent<T> ();
                    _ins = go.GetComponent<T> ();
                }
                if ( _ins.transform.parent == null )
                {
                    DontDestroyOnLoad (_ins.gameObject);
                }
            }
        }

        /// <summary>
        /// 销毁当前单例
        /// </summary>
        public static void DestroyIns()
        {
            if (null != _ins)
            {                
                GameObject.Destroy(_ins.gameObject);
                _ins = null;
            }
        }
    }
}