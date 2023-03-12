using System;
using UnityEngine.UI;

namespace MVI4Unity.Sample
{
    /*
     * 这里我把AWindow类，State类，以及Reducer类都写进一个类里是为了方便。实际项目里如果功能比较复杂，建议把这些类分开写，方便管理。
     * 比如AWindow类，State类，Reducer类，可以分别写在不同的文件里。
     */

    public class CertainView : AWindow
    {
        [AWindowCom ("btnOK")]
        public Button btnOk;

        [AWindowCom ("btnCancel")]
        public Button btnCancel;

        [AWindowCom ("textTitle")]
        public Text textTitle;

        [AWindowCom ("textContent")]
        public Text textContent;
    }

    public class CertainViewState : AStateBase
    {
        public Action certain;
        public string content;
        public string title;
    }

    public enum CertainReducerFunType
    {
        ShowCertainView,
        InvokeCertainFun
    }

    public class CertainReducer : Reducer<CertainViewState , CertainReducerFunType>
    {
        [ReducerMethod (( int ) CertainReducerFunType.ShowCertainView)]
        CertainViewState ShowCertainView (CertainViewState oldState , object @param)
        {
            CertainViewState state = @param as CertainViewState;
            oldState.certain = state.certain;
            oldState.content = state.content;
            oldState.title = state.title;
            return oldState;
        }

        [ReducerMethod (( int ) CertainReducerFunType.InvokeCertainFun)]
        CertainViewState InvokeCertainFun (CertainViewState oldState , object @param)
        {
            if ( oldState != null )
            {
                oldState?.certain?.Invoke ();
            }
            return oldState;
        }
    }

    public class CertainViewStatic
    {
        /// <summary>
        /// 确认界面组件
        /// </summary>
        public static WindowNodeType<CertainView , CertainViewState> certainCom = new WindowNodeType<CertainView , CertainViewState> ("CertainView" ,
            fillProps: (state , window , store , prop) =>
            {
                window.btnCancel.onClick.AddListener (() =>
                {
                    store.DisPatch (ReducerCommonFunType.Close);
                });

                window.btnOk.onClick.AddListener (() =>
                {
                    store.DisPatch (CertainReducerFunType.InvokeCertainFun);
                    store.DisPatch (ReducerCommonFunType.Close);
                });

                window.textContent.text = state.content;
                window.textTitle.text = state.title;
            });
    }
}