using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    /// <summary>
    /// 列表元素记录
    /// </summary>
    public class ListCalculatorElementRecordVo
    {
        /// <summary>
        /// 源数据
        /// </summary>
        public ListCalculatorElementVo ele;

        /// <summary>
        /// 左下角的位置
        /// </summary>
        public Vector2 leftBottomPos;

        /// <summary>
        /// 是否应当激活
        /// </summary>
        public bool active;

        private ListCalculatorElementRecordVo () { }

        /// <summary>
        /// 更正参数
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public ListCalculatorElementRecordVo Set (ListCalculatorElementVo ele)
        {
            this.ele = ele;
            return this;
        }

        /// <summary>
        /// 当前对象池缓存
        /// </summary>
        readonly static List<ListCalculatorElementRecordVo> _storaged = new List<ListCalculatorElementRecordVo> ();

        /// <summary>
        /// 提取实例
        /// </summary>
        /// <returns></returns>
        public static ListCalculatorElementRecordVo Pop ()
        {
            ListCalculatorElementRecordVo inst;

            if ( _storaged.Count == 0 )
            {
                inst = new ListCalculatorElementRecordVo ();
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
        public static void Push (ListCalculatorElementRecordVo inst)
        {
            ListCalculatorElementVo.Push (inst.ele);
            inst.leftBottomPos = Vector2.zero;
            inst.active = false;
            inst.ele = default;
            _storaged.Add (inst);
        }
    }
}