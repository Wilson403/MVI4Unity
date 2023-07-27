namespace MVI4Unity
{
    /// <summary>
    /// 列表的起始角-垂直-上
    /// </summary>
    public class ListCalculatorStartCornerVerUpper : ListCalculatorStartCornerVer
    {
        public override int ParseRowIndex (int rowCount , int rowIndex)
        {
            return rowCount - rowIndex - 1;
        }
    }
}
