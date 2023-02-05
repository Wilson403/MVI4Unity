using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class WindowNodeDisputeResolver : SafeSingleton<WindowNodeDisputeResolver>
    {
        public void ResolveDispute4List (List<WindowNode> currNodes , List<WindowNode> newNodes)
        {
            currNodes.RemoveAll (x => x.Equals (null));
            newNodes.RemoveAll (x => x.Equals (null));

            Debug.LogWarning (newNodes [0]);
        }
    }
}