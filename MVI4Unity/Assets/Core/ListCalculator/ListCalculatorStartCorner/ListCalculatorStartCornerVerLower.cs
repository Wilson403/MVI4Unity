namespace MVI4Unity
{
    /// <summary>
    /// 列表的起始角-垂直-下
    /// </summary>
    public class ListCalculatorStartCornerVerLower : ListCalculatorStartCornerVer
    {
        public override int ParseRowIndex (int rowCount , int rowIndex)
        {
            return rowIndex;
        }
    }
}
