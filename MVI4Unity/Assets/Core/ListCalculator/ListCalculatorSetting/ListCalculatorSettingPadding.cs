namespace MVI4Unity
{
    /// <summary>
    /// 列表内边距
    /// </summary>
    public class ListCalculatorSettingPadding
    {
        /// <summary>
        /// 上
        /// </summary>
        public float top;
        /// <summary>
        /// 右
        /// </summary>
        public float right;
        /// <summary>
        /// 下
        /// </summary>
        public float bottom;
        /// <summary>
        /// 左
        /// </summary>
        public float left;

        public ListCalculatorSettingPadding (float top , float right , float bottom , float left)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;
        }
    }
}