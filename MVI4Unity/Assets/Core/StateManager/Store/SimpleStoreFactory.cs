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
            if ( !_type2Reducer.ContainsKey (reducerRealType) )
            {
                _type2Reducer [reducerRealType] = Activator.CreateInstance<R> ();
            }
            return _type2Reducer [reducerRealType];
        }

        public Store<S> CreateStore<S, R> () where S : AStateBase where R : IReducer
        {
            Store<S> store = new Store<S> ();
            store.AddReducer (GetReducer<R> ());
            return store;
        }
    }
}