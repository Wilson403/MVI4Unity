﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVI4Unity
{
    public class State01 : AStateBase
    {

    }

    public class Reducer01 : Reducer<State01 , Reducer01.Reducer01FunType>
    {
        public enum Reducer01FunType
        {
            Func01,
            Func02,
            Func03,
        }

        [ReducerFuncInfo (( int ) Reducer01FunType.Func01 , true)]
        private State01 Func01 (State01 oldState , object @param)
        {
            return new State01 ();
        }

        [ReducerFuncInfo (( int ) Reducer01FunType.Func02)]
        async private Task<State01> Func02 (State01 oldState , object @param)
        {
            await Task.Run (() => { Thread.Sleep (3000); });
            return new State01 ();
        }

        [ReducerFuncInfo (( int ) Reducer01FunType.Func03)]
        private void Func03 (State01 oldState , object @param , Action<State01> setNewState)
        {
            setNewState.Invoke (new State01 ());
        }
    }
}