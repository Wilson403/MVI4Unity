using System.Threading.Tasks;
using UnityEngine.UI;

namespace MVI4Unity.Sample
{
    /*
     * 这里我把AWindow类，State类，以及Reducer类都写进一个类里是为了方便。实际项目里如果功能比较复杂，建议把这些类分开写，方便管理。
     * 比如AWindow类，State类，Reducer类，可以分别写在不同的文件里。
     */

    public enum TipViewReducerFunType
    {
        ShowTip,
        AutoDestroy
    }

    public class TipView : AWindow
    {
        [AWindowCom ("text")]
        public Text text;
    }

    public class TipViewState : AStateBase
    {
        public string content;
    }

    public class TipViewReducer : Reducer<TipViewState , TipViewReducerFunType>
    {
        [ReducerMethod (( int ) TipViewReducerFunType.ShowTip)]
        TipViewState ShowTip (TipViewState oldState , object @param)
        {
            return @param as TipViewState;
        }

        [ReducerMethod (( int ) TipViewReducerFunType.AutoDestroy , true)]
        async Task<TipViewState> AutoDestroy (TipViewState oldState , object @param)
        {
            await Task.Delay (1500);
            TipViewState state = new TipViewState ();
            state.shouldDestroy = true;
            return state;
        }
    }

    public class TipViewStatic
    {
        /// <summary>
        /// 提示组件
        /// </summary>
        public static WindowNodeType<TipView , TipViewState> tipCom = new WindowNodeType<TipView , TipViewState> ("TipView" ,
            fillProps: (state , window , store , prop) =>
            {
                window.text.text = state.content;
            });
    }
}