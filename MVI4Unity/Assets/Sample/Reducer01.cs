using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVI4Unity
{
    public class State01 : AStateBase
    {

    }

    public class Reducer01 : Reducer<State01>
    {
        public enum Reducer01FunType
        {
            Func01,
            Func02,
            Func03,
        }

        protected override void RegisterFunc ()
        {
            AddFunc (Reducer01FunType.Func01 , Func01);
            AddAsyncFunc (Reducer01FunType.Func02 , Func02);
            AddCallBack (Reducer01FunType.Func03 , Func03);
        }

        private State01 Func01 (State01 oldState , object @param)
        {
            return new State01 ();
        }

        async private Task<State01> Func02 (State01 oldState , object @param)
        {
            await Task.Run (() => { Thread.Sleep (3000); });
            return new State01 ();
        }

        private void Func03 (State01 oldState , object @param , Action<State01> setNewState)
        {
            setNewState.Invoke (new State01 ());
        }
    }
}