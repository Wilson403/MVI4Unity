namespace MVI4Unity
{
    /// <summary>
    /// 列表的起始角-水平-左
    /// </summary>
    public class ListCalculatorStartCornerHorLeft : ListCalculatorStartCornerHor
    {
        public override int ParseColumnIndex (int columnCount , int columnIndex)
        {
            return columnIndex;
        }
    }
}
