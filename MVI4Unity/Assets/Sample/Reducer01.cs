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
        }

        protected override void RegisterFunc ()
        {
            AddFunc (Reducer01FunType.Func01 , Func01);
            AddFunc (Reducer01FunType.Func02 , Func02);
        }

        private State01 Func01 (State01 oldState , object @param)
        {
            return default;
        }

        private State01 Func02 (State01 oldState , object @param)
        {
            return default;
        }
    }
}