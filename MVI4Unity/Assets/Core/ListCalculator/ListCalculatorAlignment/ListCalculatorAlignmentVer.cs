namespace MVI4Unity
{
    /// <summary>
    /// 元素排版的垂直倾向
    /// </summary>
    public abstract class ListCalculatorAlignmentVer
    {
        /// <summary>
        /// 计算出 y 的应当坐标
        /// </summary>
        /// <param name="gridHeight"></param>
        /// <param name="eleHeight"></param>
        /// <returns></returns>
        public abstract float GetPositionY (float gridHeight , float eleHeight);
    }
}