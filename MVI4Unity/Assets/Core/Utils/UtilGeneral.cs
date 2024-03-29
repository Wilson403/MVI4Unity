﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MVI4Unity
{
    public class UtilGeneral : SafeSingleton<UtilGeneral>
    {
        /// <summary>
        /// 方法是否能够匹配委托类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool IsMethodCompatibleWithDelegate<T> (MethodInfo method) where T : class
        {
            MethodInfo delegateSignature = typeof (T).GetMethod ("Invoke");
            IEnumerable<Type> delegateParameters = delegateSignature.GetParameters ().Select (x => x.ParameterType);
            IEnumerable<Type> methodParameters = method.GetParameters ().Select (x => x.ParameterType);

            //如果返回类型和函数签名都一致，说明委托类型与方法可以匹配上
            return delegateSignature.ReturnType == method.ReturnType && delegateParameters.SequenceEqual (methodParameters);
        }

        /// <summary>
        /// 将Method转换为Delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="inst"></param>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public bool Method2Delegate<T> (MethodInfo method , object inst , out T @delegate) where T : class
        {
            if ( IsMethodCompatibleWithDelegate<T> (method) )
            {
                @delegate = Delegate.CreateDelegate (typeof (T) , inst , method) as T;
                return @delegate != null;
            }
            @delegate = null;
            return false;
        }

        /// <summary>
        ///  获取类的基类
        /// /summary>
        /// <param name="type"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetBaseClasses (Type type , bool includeSelf = false)
        {
            if ( type is object && type.BaseType is object )
            {
                if ( includeSelf )
                {
                    yield return type;
                }
                Type current = type.BaseType;
                while ( current is object )
                {
                    yield return current;
                    current = current.BaseType;
                }
            }
        }
    }
}