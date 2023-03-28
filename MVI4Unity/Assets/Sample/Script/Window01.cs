using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVI4Unity.Sample
{
    /*
     * 这里我把AWindow类，State类，以及Reducer类都写进一个类里是为了方便。实际项目里如果功能比较复杂，建议把这些类分开写，方便管理。
     * 比如AWindow类，State类，Reducer类，可以分别写在不同的文件里。
     */

    /// <summary>
    /// 这是一个主界面Window01，绑定了一些组件
    /// 通过预制体的ObjectBindingData组件信息来赋值组件
    /// </summary>
    public class Window01 : AWindow
    {
        [AWindowCom ("container1")]
        public Transform container1;

        [AWindowCom ("container2")]
        public Transform container2;

        [AWindowCom ("btn")]
        public Button btn;

        [AWindowCom ("btn2")]
        public Button btn2;

        [AWindowCom ("btnClose")]
        public Button btnClose;
    }

    /// <summary>
    /// 这是一个主界面Window01的子节点元素
    /// </summary>
    public class WindowItem : AWindow
    {

    }

    /// <summary>
    /// 这是Window01界面需要用到的数据
    /// </summary>
    public class State01 : AStateBase
    {
        public int count;
    }

    /// <summary>
    /// 这是刷新State01的函数集合
    /// </summary>
    public class Reducer01 : Reducer<State01 , Reducer01.Reducer01MethodType>
    {
        public enum Reducer01MethodType
        {
            Init,
            Func01,
            Func02,
            Func03,
        }

        /// <summary>
        /// ReducerMethod特性包含2个参数，第一个对应的是函数枚举，第二个是可选参数，是否在初始化时自动调用1次，默认为false
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [ReducerMethod (( int ) Reducer01MethodType.Init , true)]
        State01 InitState (State01 oldState , object @param)
        {
            State01 state01 = new State01 ();
            state01.count = 10;
            return state01;
        }

        [ReducerMethod (( int ) Reducer01MethodType.Func01)]
        State01 Func01 (State01 oldState , object @param)
        {
            State01 state01 = new State01 ();
            state01.count = oldState.count - 1;
            return state01;
        }

        [ReducerMethod (( int ) Reducer01MethodType.Func02)]
        State01 Func02 (State01 oldState , object @param)
        {
            State01 state01 = new State01 ();
            state01.count = oldState.count + 1;
            return state01;
        }

        [ReducerMethod (( int ) Reducer01MethodType.Func03)]
        void Func03 (State01 oldState , object @param , Action<State01> setNewState)
        {
            setNewState.Invoke (new State01 ());
        }
    }

    /// <summary>
    /// WindowNodeType的参数解释：
    /// fillProps:每次State更新时，会调用这个函数，用来刷新界面
    /// containerCreator:创建容器，这个容器指的是Unity的Transform，从对应的界面类型获取到，用来存放子节点
    /// childNodeCreator:创建子节点
    /// </summary>
    public class Window01Static
    {
        /// <summary>
        /// 这是WindowItem的节点
        /// </summary>
        public static WindowNodeType<WindowItem , State01> item = new WindowNodeType<WindowItem , State01> ("WindownItem" ,
            fillProps: (state , window , store , prop) =>
            {
                Debug.LogWarning ($"{state} = {window} = {store}");
            });

        /// <summary>
        /// 这是Windown01的界面，同时也是根节点
        /// </summary>
        public static WindowNodeType<Window01 , State01> root = new WindowNodeType<Window01 , State01> ("Windown01" ,
            containerCreator: (window) =>
            {
                //添加2个Window01里的容器container1，container2
                List<Transform> containerList = PoolMgr.Ins.GetList<Transform> ().Pop ();
                containerList.Add (window.container1);
                containerList.Add (window.container2);
                return containerList;
            } ,
            childNodeCreator: (state) =>
            {
                List<List<WindowNode>> childNodeGroup = PoolMgr.Ins.GetList<List<WindowNode>> ().Pop ();
                List<WindowNode> childNodeList1 = PoolMgr.Ins.GetList<WindowNode> ().Pop ();
                List<WindowNode> childNodeList2 = PoolMgr.Ins.GetList<WindowNode> ().Pop ();

                //给这2个容器添加子节点
                for ( int i = 0 ; i < state.count ; i++ )
                {
                    childNodeList1.Add (item.CreateWindowNode (state));
                    childNodeList2.Add (item.CreateWindowNode (state));
                }

                childNodeGroup.Add (childNodeList1);
                childNodeGroup.Add (childNodeList2);

                return childNodeGroup;
            } ,
            fillProps: (state , window , store , prop) =>
            {
                if ( state.currentFunTag == ( int ) Reducer01.Reducer01MethodType.Func01 ) 
                {
                    //由Func01引起的变化
                }
                window.btn.GetComponent<RectTransform>()
                window.btn.onClick.AddListener (() => { store.DisPatch (Reducer01.Reducer01MethodType.Func01 , default); });
                window.btn2.onClick.AddListener (() => { store.DisPatch (Reducer01.Reducer01MethodType.Func02 , default); });
                window.btnClose.onClick.AddListener (() => { store.DisPatch (ReducerCommonFunType.Close , default); });
            });
    }
}