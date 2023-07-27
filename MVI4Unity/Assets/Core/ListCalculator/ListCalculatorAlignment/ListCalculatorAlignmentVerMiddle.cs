namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的垂直倾向-中
    /// </summary>
    public class ListCalculatorAlignmentVerMiddle : ListCalculatorAlignmentVer
    {
        public override float GetPositionY (float gridHeight , float eleHeight)
        {
            return ( gridHeight - eleHeight ) / 2;
        }
    }
}