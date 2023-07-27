namespace MVI4Unity
{
    /// <summary>
    /// 列表的起始角-水平-右
    /// </summary>
    public class ListCalculatorStartCornerHorRight : ListCalculatorStartCornerHor
    {
        public override int ParseColumnIndex (int columnCount , int columnIndex)
        {
            return columnCount - columnIndex - 1;
        }
    }
}
