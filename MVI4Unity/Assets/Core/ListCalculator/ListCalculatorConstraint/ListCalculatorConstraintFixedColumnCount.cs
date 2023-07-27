using System;

namespace MVI4Unity
{
    /// <summary>
    /// @创建者: 江选辉
    /// @创建日期: 2021/04/28 20.02
    /// @功能描述: 使用列的数量来进行约束
    /// @修改者：
    /// @修改日期：
    /// @修改描述：
    /// </summary>
    public class ListCalculatorConstraintFixedColumnCount : ListCalculatorConstraint
    {
        /// <summary>
        /// 约束的数量
        /// </summary>
        public int constraintCount;

        /// <summary>
        /// 最小行数
        /// </summary>
        public int minRowCount;

        public ListCalculatorConstraintFixedColumnCount (int constraintCount , int minRowCount)
        {
            this.constraintCount = constraintCount;
            this.minRowCount = minRowCount;
        }

        /// <summary>
        /// 重新对元素进行排版
        /// </summary>
        /// <param name="holaList"></param>
        public override ListCalculatorColumnRowCount GetColumnRowCount (ListCalculator holaList)
        {
            // 列数
            int columnCount = constraintCount;

            // 行数
            int rowCount = ( int ) Math.Ceiling (( decimal ) holaList.recDict.Count / columnCount);

            // 被最小行数约束
            if ( rowCount < minRowCount )
            {
                rowCount = minRowCount;
            }

            return new ListCalculatorColumnRowCount (columnCount , rowCount);
        }
    }
}