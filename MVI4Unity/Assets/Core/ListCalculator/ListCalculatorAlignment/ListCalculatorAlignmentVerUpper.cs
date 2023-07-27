namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的垂直倾向-上
    /// </summary>
    public class ListCalculatorAlignmentVerUpper : ListCalculatorAlignmentVer
    {
        public override float GetPositionY (float gridHeight , float eleHeight)
        {
            return gridHeight - eleHeight;
        }
    }
}