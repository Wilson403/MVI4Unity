using UnityEngine;

namespace React4Unity
{
    public class AView
    {
        /// <summary>
        /// 关联的GameObject对象
        /// </summary>
        public GameObject GameObject { get; private set; }

        /// <summary>
        /// 关联对象的Transform
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// 关联对象的RectTransform
        /// </summary>
        public RectTransform RectTransform { get; private set; }

        /// <summary>
        /// 视图的名称
        /// </summary>
        public string ViewName { get; private set; }

        public void SetGameObject (GameObject gameObject)
        {
            GameObject = gameObject;
            Transform = gameObject.transform;
            RectTransform = gameObject.GetComponent<RectTransform> ();
            ViewName = gameObject.name;
        }
    }
}