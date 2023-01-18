using System;
using System.Threading.Tasks;

namespace MVI4Unity
{
    public interface IReducer
    {
        /// <summary>
        /// 获取函数类型
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        ReducerExecuteType GetReducerExecuteType (Enum tag);

        /// <summary>
        /// 执行回调
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <param name="setNewState"></param>
        void ExecuteCallback (Enum tag , object lastState , object @param , Action<object> setNewState);

        /// <summary>
        /// 执行异步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<object> AsyncExecute (Enum tag , object lastState , object @param);

        /// <summary>
        /// 执行同步方法
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lastState"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        object Execute (Enum tag , object lastState , object @param);
    }
}