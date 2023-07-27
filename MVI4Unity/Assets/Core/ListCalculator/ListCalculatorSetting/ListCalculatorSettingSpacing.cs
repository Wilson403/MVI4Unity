namespace MVI4Unity
{
    /// <summary>
    /// 列表元素的间距
    /// </summary>
    public class ListCalculatorSettingSpacing
    {
        /// <summary>
        /// 列间距
        /// </summary>
        public float column;
        /// <summary>
        /// 行间距
        /// </summary>
        public float row;

        public ListCalculatorSettingSpacing (float column , float row)
        {
            this.column = column;
            this.row = row;
        }
    }
}