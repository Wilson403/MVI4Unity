namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的水平倾向
    /// </summary>
    public abstract class ListCalculatorAlignmentHor
    {
        /// <summary>
        /// 计算出 x 的应当坐标
        /// </summary>
        /// <param name="gridWidth"></param>
        /// <param name="eleWidt"></param>
        /// <returns></returns>
        public abstract float GetPositionX (float gridWidth , float eleWidt);
    }
}