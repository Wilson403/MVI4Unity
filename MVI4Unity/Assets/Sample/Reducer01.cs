using System;

namespace MVI4Unity
{
    public class State01 : AStateBase
    {
        public int count;
    }

    public class Reducer01 : Reducer<State01 , Reducer01.Reducer01MethodType>
    {
        public enum Reducer01MethodType
        {
            Init,
            Func01,
            Func02,
            Func03,
        }

        [ReducerMethod (( int ) Reducer01MethodType.Init , true)]
        State01 InitState (State01 oldState , object @param)
        {
            State01 state01 = new State01 ();
            state01.count = 10;
            return state01;
        }

        [ReducerMethod (( int ) Reducer01MethodType.Func01)]
        State01 Func01 (State01 oldState , object @param)
        {
            State01 state01 = new State01 ();
            state01.count = oldState.count - 1;
            return state01;
        }

        [ReducerMethod (( int ) Reducer01MethodType.Func02)]
        State01 Func02 (State01 oldState , object @param)
        {
            State01 state01 = new State01 ();
            state01.count = oldState.count + 1;
            return state01;
        }

        [ReducerMethod (( int ) Reducer01MethodType.Func03)]
        void Func03 (State01 oldState , object @param , Action<State01> setNewState)
        {
            setNewState.Invoke (new State01 ());
        }
    }
}