using System;

namespace MVI4Unity.Sample
{
    public class OpenViewMgr : SafeSingleton<OpenViewMgr>
    {
        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="content"></param>
        public void ShowTip (string content)
        {
            RootNodeContainer<TipViewState , TipViewReducer> rootView = UIWinMgr.Ins.Open<TipViewState , TipViewReducer> (UIContent.Ins.tipContainer , TipViewStatic.tipCom);
            rootView.DisPatch (TipViewReducerFunType.ShowTip , new TipViewState () { content = content });
        }

        /// <summary>
        /// 显示确认框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="certain"></param>
        public void ShowCertainView (string title , string content , Action certain)
        {
            RootNodeContainer<CertainViewState , CertainReducer> rootView = UIWinMgr.Ins.Open<CertainViewState , CertainReducer> (UIContent.Ins.tipContainer , CertainViewStatic.certainCom);
            rootView.DisPatch (CertainReducerFunType.ShowCertainView , new CertainViewState ()
            {
                content = content ,
                title = title ,
                certain = certain
            });
        }

        /// <summary>
        /// 打开窗口01
        /// </summary>
        public void OpenWindow01 ()
        {
            UIWinMgr.Ins.Open<State01 , Reducer01> (UIContent.Ins.tipContainer , Window01Static.root);
        }
    }
}