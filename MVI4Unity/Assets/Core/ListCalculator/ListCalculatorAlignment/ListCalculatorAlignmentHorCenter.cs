namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的水平倾向-中
    /// </summary>
    public abstract class ListCalculatorAlignmentHorCenter : ListCalculatorAlignmentHor
    {
        public override float GetPositionX (float gridWidth , float eleWidth)
        {
            return ( gridWidth - eleWidth ) / 2;
        }
    }
}