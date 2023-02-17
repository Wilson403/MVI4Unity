using System;
using System.Collections.Generic;
using MVI4Unity;
using UnityEngine;
using Zero;

public class PoolRealContainer4AWindow : SingletonMonoBehaviour<PoolRealContainer4AWindow>
{

    private readonly Dictionary<Type , List<GameObject>> _cacheTransform = new Dictionary<Type , List<GameObject>> ();

    /// <summary>
    /// 回收实际的Gameobject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="window"></param>
    public void Push<T> (T window) where T : AWindow
    {
        Type type = typeof (T);
        CertainInitCacheDictKey (type);
        window.RemoveAllListeners ();
        window.SetParent (transform);
        window.SetActive (false);
        _cacheTransform [type].Add (window.GameObject);
    }

    /// <summary>
    /// 弹出一个实际的GameObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="window"></param>
    public void Pop<T> (T window) where T : AWindow
    {
        Type type = typeof (T);
        CertainInitCacheDictKey (type);
        window.SetActive (true);
        if ( _cacheTransform [type].Contains (window.GameObject) )
        {
            _cacheTransform [type].Remove (window.GameObject);
        }
    }

    /// <summary>
    /// 确保字典Key存在
    /// </summary>
    /// <param name="type"></param>
    void CertainInitCacheDictKey (Type type)
    {
        if ( !_cacheTransform.ContainsKey (type) )
        {
            _cacheTransform [type] = new List<GameObject> (0);
        }
    }
}