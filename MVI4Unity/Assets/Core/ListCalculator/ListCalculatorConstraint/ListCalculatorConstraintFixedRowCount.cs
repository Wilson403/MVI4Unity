using System;

namespace MVI4Unity
{
    /// <summary>
    /// @创建者: 江选辉
    /// @创建日期: 2021/04/28 20.02
    /// @功能描述: 使用行的数量进行约束
    /// @修改者：
    /// @修改日期：
    /// @修改描述：
    /// </summary>
    public class ListCalculatorConstraintFixedRowCount : ListCalculatorConstraint
    {
        /// <summary>
        /// 约束的数量
        /// </summary>
        public int constraintCount;
        /// <summary>
        /// 最小列数
        /// </summary>
        public int minColumnCount;

        public ListCalculatorConstraintFixedRowCount (int constraintCount , int minColumnCount)
        {
            this.constraintCount = constraintCount;
            this.minColumnCount = minColumnCount;
        }

        public override ListCalculatorColumnRowCount GetColumnRowCount (ListCalculator holaList)
        {
            // 行数
            int rowCount = constraintCount;

            // 列数
            int columnCount = ( int ) Math.Ceiling (( decimal ) holaList.recDict.Count / rowCount);

            // 被最小列数约束
            if ( columnCount < minColumnCount )
            {
                columnCount = minColumnCount;
            }

            return new ListCalculatorColumnRowCount (columnCount: columnCount , rowCount: rowCount);
        }
    }
}