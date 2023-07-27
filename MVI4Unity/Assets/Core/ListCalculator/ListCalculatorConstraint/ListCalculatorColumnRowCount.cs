namespace MVI4Unity
{
    /// <summary>
    /// 行列数
    /// </summary>
    public class ListCalculatorColumnRowCount
    {
        /// <summary>
        /// 列数
        /// </summary>
        public readonly int columnCount;

        /// <summary>
        /// 行数
        /// </summary>
        public readonly int rowCount;

        public ListCalculatorColumnRowCount (int columnCount , int rowCount)
        {
            this.columnCount = columnCount;
            this.rowCount = rowCount;
        }
    }
}