using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 视图对象
    /// </summary>
    public abstract class AWindow
    {
        private GameObject _gameObject;
        private Transform _transform;
        private RectTransform _rectTransform;

        /// <summary>
        /// 设置实际游戏对象
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="data"></param>
        internal void SetGameObject (GameObject gameObject , object data = null)
        {
            _gameObject = gameObject;
            _transform = gameObject.transform;
            _rectTransform = gameObject.GetComponent<RectTransform> ();
        }

        /// <summary>
        /// 设置激活
        /// </summary>
        /// <param name="value"></param>
        public void SetActive (bool value)
        {
            _gameObject.SetActive (value);
            if ( value )
            {
                Enable ();
                return;
            }
            Disable ();
        }

        /// <summary>
        /// 是否销毁了
        /// </summary>
        /// <returns></returns>
        public bool IsDestroyed () 
        {
            return _gameObject == null;
        }

        /// <summary>
        /// 获取视图名称
        /// </summary>
        /// <returns></returns>
        public string GetViewName () 
        {
            return _gameObject.name;
        }

        #region 生命周期

        /// <summary>
        /// 启用
        /// </summary>
        protected virtual void Enable ()
        {

        }

        /// <summary>
        /// 禁用
        /// </summary>
        protected virtual void Disable ()
        {

        }

        /// <summary>
        /// 打开
        /// </summary>
        protected virtual void Open ()
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        protected virtual void Close ()
        {

        }

        /// <summary>
        /// 获得焦点
        /// </summary>
        protected virtual void OnFocus ()
        {

        }

        #endregion
    }
}