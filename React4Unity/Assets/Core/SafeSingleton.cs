namespace React4Unity
{
    /// <summary>
    /// 单件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SafeSingleton<T> where T : new()
    {
        /// <summary>
        /// 单件实例
        /// </summary>
        private static T _singleton = default;

        /// <summary>
        /// 安全锁
        /// </summary>
        private static readonly object _safelock = new object ();

        /// <summary>
        /// 保护构造
        /// </summary>
        protected SafeSingleton () { }

        /// <summary>
        /// 创建单例
        /// </summary>
        /// <returns></returns>
        public static T CreateSingleton ()
        {
            if (_singleton == null)
            {
                lock ( _safelock )
                {
                    if ( _singleton == null )
                    {
                        _singleton = new T ();
                    }
                };
            };
            return _singleton;
        }

        /// <summary>
        /// 单件对象
        /// </summary>
        public static T Ins
        {
            get
            {
                return _singleton != null ? _singleton : CreateSingleton ();
            }
        }
    }
}