using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using System.Linq;

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
        protected object data;

        public GameObject GameObject
        {
            get
            {
                return _gameObject;
            }
        }

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
            this.data = data;
            FillComponent ();
            OnInit ();
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
        /// 设置父对象
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent (Transform parent)
        {
            _transform.SetParent (parent);
            _transform.localScale = Vector3.one;
            _transform.localPosition = Vector3.zero;
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

        #region 预制体组件查找

        /// <summary>
        /// 访问物体的集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected GameObject [] EleArr (string key)
        {
            UnityEngine.Object [] oriArr = ObjectBindingData.Find (GameObject , key);
            GameObject [] result = new GameObject [oriArr.Length];
            for ( int i = 0 ; i < oriArr.Length ; i++ )
            {
                result [i] = oriArr [i] as GameObject;
            }
            return result;
        }

        /// <summary>
        /// 通过标识，直接访问视图节点上绑定的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected GameObject Ele (string key)
        {
            var objList = EleArr (key);
            if ( objList.Length == 0 )
            {
                return default;
            }
            return objList [0];
        }

        /// <summary>
        /// 通过标识，直接访问视图节点上绑定的组件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        protected Component Ele (string key , Type t)
        {
            GameObject gameObj = Ele (key);
            if ( gameObj == default )
            {
                return default;
            }
            Component com = gameObj.GetComponent (t);
            if ( com == null )
            {
                Debug.LogError ($"[{GameObject.name}] 没有在[{gameObj.name}]上找到[{t}] key[{key}]");
                return default;
            }
            return com;
        }

        /// <summary>
        /// 访问组件的集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object [] EleArr (string key , Type type)
        {
            return ObjectBindingData.Find (GameObject , key);
        }

        /// <summary>
        /// 访问组件的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T [] EleArr<T> (string key) where T : Component
        {
            GameObject [] objArr = EleArr (key);
            T [] result = new T [objArr.Length];

            for ( int i = 0 ; i < objArr.Length ; i++ )
            {
                result [i] = objArr [i].GetComponent<T> ();
            }
            return result;
        }

        /// <summary>
        /// 类型到属性对象列表的映射
        /// </summary>
        static readonly Dictionary<Type , List<FieldInfo>> _fieldInfoCache = new Dictionary<Type , List<FieldInfo>> (0);

        /// <summary>
        /// 获取身上的属性对象列表
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static List<FieldInfo> GetFieldInfo (object o)
        {
            var selfType = o.GetType ();
            if ( !_fieldInfoCache.ContainsKey (selfType) )
            {
                List<FieldInfo> propInfoList = new List<FieldInfo> ();

                //获取包含父类的所有字段
                Type [] types = UtilGeneral.Ins.GetBaseClasses (selfType , true).ToArray ();
                for ( int i = 0 ; i < types.Length ; i++ )
                {
                    if ( types [i] == typeof (AWindow) )
                    {
                        break;
                    };
                    propInfoList.AddRange (types [i].GetFields (BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public));
                }
                _fieldInfoCache [selfType] = propInfoList;
            }
            return _fieldInfoCache [selfType];
        }

        /// <summary>
        /// 填充组件
        /// </summary>
        private void FillComponent ()
        {
            List<FieldInfo> propInfoList = GetFieldInfo (this);
            for ( int propInfoIdx = 0 ; propInfoIdx < propInfoList.Count ; propInfoIdx++ )
            {
                FieldInfo propInfo = propInfoList [propInfoIdx];
                // 尝试读取该字段上面的信息特性
                var cfgArray = propInfo.GetCustomAttributes (typeof (AWindowComAttribute) , false);
                AWindowComAttribute cfg = null;
                if ( cfgArray != null && cfgArray.Length != 0 )
                {
                    cfg = cfgArray [0] as AWindowComAttribute;
                }

                if ( cfg != null )
                {
                    // 如果是数组类型
                    if ( propInfo.FieldType.IsArray )
                    {
                        var eleType = propInfo.FieldType.GetElementType ();
                        if ( eleType == GameObject.GetType () )
                        {
                            propInfo.SetValue (this , EleArr (cfg.tag));
                        }
                        else
                        {
                            object [] objArr = EleArr (cfg.tag , eleType);
                            Array arr = Array.CreateInstance (eleType , objArr.Length);
                            for ( int i = 0 ; i < objArr.Length ; i++ )
                            {
                                arr.SetValue (objArr [i] , i);
                            }
                            propInfo.SetValue (this , arr);
                        }
                    }
                    // 不是数组类型
                    else
                    {
                        if ( propInfo.FieldType == GameObject.GetType () )
                        {
                            // 直接设置为物体
                            propInfo.SetValue (this , Ele (cfg.tag));
                        }
                        else
                        {
                            // 直接设置为对应的组件
                            propInfo.SetValue (this , Ele (cfg.tag , propInfo.FieldType));
                        }
                    }
                }
            }
        }
        #endregion

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
        /// 初始化
        /// </summary>
        protected virtual void OnInit ()
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        protected virtual void OnClose ()
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