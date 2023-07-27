using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 列表元素
    /// </summary>
    public class ListCalculatorElementVo
    {
        /// <summary>
        /// 尺寸
        /// </summary>
        public Vector2 size;

        private ListCalculatorElementVo () 
        {

        }

        /// <summary>
        /// 更正参数
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public ListCalculatorElementVo Set (float w , float h)
        {
            size.x = w;
            size.y = h;
            return this;
        }

        /// <summary>
        /// 当前对象池缓存
        /// </summary>
        readonly static List<ListCalculatorElementVo> _storaged = new List<ListCalculatorElementVo> ();

        /// <summary>
        /// 提取实例
        /// </summary>
        /// <returns></returns>
        public static ListCalculatorElementVo Pop ()
        {
            ListCalculatorElementVo inst;

            if ( _storaged.Count == 0 )
            {
                inst = new ListCalculatorElementVo ();
            }

            else
            {
                inst = _storaged [_storaged.Count - 1];
                _storaged.RemoveAt (_storaged.Count - 1);
            }

            return inst;
        }

        /// <summary>
        /// 存储实例
        /// </summary>
        /// <param name="inst"></param>
        public static void Push (ListCalculatorElementVo inst)
        {
            inst.size = Vector2.zero;
            _storaged.Add (inst);
        }
    }
}