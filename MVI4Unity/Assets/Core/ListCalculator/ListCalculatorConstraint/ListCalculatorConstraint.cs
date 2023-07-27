namespace MVI4Unity
{
    /// <summary>
    /// 列表的约束
    /// </summary>
    public abstract class ListCalculatorConstraint
    {
        /// <summary>
        /// 重新对元素进行排版
        /// </summary>
        /// <param name="holaList"></param>
        public abstract ListCalculatorColumnRowCount GetColumnRowCount (ListCalculator holaList);
    }
}