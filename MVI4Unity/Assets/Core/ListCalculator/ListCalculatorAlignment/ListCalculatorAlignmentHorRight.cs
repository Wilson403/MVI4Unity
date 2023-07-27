namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的水平倾向-右
    /// </summary>
    public abstract class ListCalculatorAlignmentHorRight : ListCalculatorAlignmentHor
    {
        public override float GetPositionX (float gridWidth , float eleWidth)
        {
            return gridWidth - eleWidth;
        }
    }
}