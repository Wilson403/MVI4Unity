using System.Collections.Generic;
using System.Text;
using LitJson;

namespace MVI4Unity
{
    public class WindowNode
    {
        /// <summary>
        /// 子节点列表
        /// </summary>
        private readonly List<List<WindowNode>> _childNodes = new List<List<WindowNode>> ();

        /// <summary>
        /// 该节点对应类型
        /// </summary>
        public WindowNodeType windowNodeType;

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