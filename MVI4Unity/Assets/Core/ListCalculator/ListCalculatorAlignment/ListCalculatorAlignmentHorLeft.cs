namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的水平倾向-左
    /// </summary>
    public abstract class ListCalculatorAlignmentHorLeft : ListCalculatorAlignmentHor
    {
        public override float GetPositionX (float gridWidth , float eleWidth)
        {
            return 0;
        }
    }
}