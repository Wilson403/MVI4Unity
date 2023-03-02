using Newtonsoft.Json;

namespace MVI4Unity
{
    public abstract class AStateBase
    {
        /// <summary>
        /// 当前触发状态变更的函数标签
        /// </summary>
        public int currentFunTag;

        /// <summary>
        /// 无效的函数标签
        /// 请避免使用这个值来作为枚举值
        /// </summary>
        const int INVAILD_FUN_TAG = -999999999;

        /// <summary>
        /// 是否销毁
        /// </summary>
        public bool shouldDestroy;

        public AStateBase ()
        {
            shouldDestroy = false;
            currentFunTag = INVAILD_FUN_TAG;
        }

        public T Clone<T> () where T : AStateBase
        {
            return JsonConvert.DeserializeObject<T> (JsonConvert.SerializeObject (this));
        }
    }
}