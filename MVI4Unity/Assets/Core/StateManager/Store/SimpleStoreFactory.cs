using System;
using System.Collections.Generic;

namespace MVI4Unity
{
    public class SimpleStoreFactory : SafeSingleton<SimpleStoreFactory>
    {
        readonly Dictionary<Type , IReducer> _type2Reducer = new Dictionary<Type , IReducer> ();

        IReducer GetReducer<R> () where R : IReducer
        {
            var reducerRealType = typeof (R);
            if ( _type2Reducer.ContainsKey (reducerRealType) )
            {
                return _type2Reducer [reducerRealType];
            }
            R reducer = Activator.CreateInstance<R> ();
            _type2Reducer [reducerRealType] = reducer;
            return reducer;
        }

        public Store<S> CreateStore<S, R> () where S : AStateBase where R : IReducer
        {
            return new Store<S> (GetReducer<R> () as Reducer<S>);
        }
    }
}