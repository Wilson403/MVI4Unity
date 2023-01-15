namespace MVI4Unity
{
    public interface IStore
    {
        /// <summary>
        /// 派发
        /// </summary>
        void DisPatch ();

        /// <summary>
        /// 订阅
        /// </summary>
        void Subscribe ();
    }
}