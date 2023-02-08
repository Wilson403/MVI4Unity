using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

namespace MVI4Unity
{
    public class WindowNode : IEquatable<WindowNode>
    {
        /// <summary>
        /// 子节点列表
        /// </summary>
        private readonly List<List<WindowNode>> _childNodes = new List<List<WindowNode>> ();

        /// <summary>
        /// 该节点对应类型
        /// </summary>
        public WindowNodeType windowNodeType;

        /// <summary>
        /// 该节点来源
        /// </summary>
        public WindowNode from;

        /// <summary>
        /// 该节点去向
        /// </summary>
        public WindowNode to;

        public int id;

        /// <summary>
        /// 清除记录
        /// </summary>
        public void ClearRecord ()
        {
            from = null;
            to = null;
        }

        public bool Equals (WindowNode other)
        {
            //判断依据：节点类型对应的窗口池相同，即都是同一个预制体
            return windowNodeType.GetResTag ().Equals (other.windowNodeType.GetResTag ());
        }

        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder ();
            JsonWriter jr = new JsonWriter (sb)
            {
                PrettyPrint = true ,
                IndentValue = 4
            };
            JsonMapper.ToJson (this , jr);
            return sb.ToString ();
        }
    }
}